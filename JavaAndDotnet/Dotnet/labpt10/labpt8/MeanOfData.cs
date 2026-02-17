using System;
using System.Formats.Asn1;
using System.IO;
using System.Threading;

namespace ExampleApp
{
    /// <summary>
    /// Reads lines [idStart..idEnd] from the CSV, sums age and bone‐fracture columns,
    /// computes local means, and calls back to TaskClass.meanFromThread().
    /// </summary>
    public class MeanOfData
    {
        private readonly TaskClass _task;
        private readonly int _index;
        private readonly int _idStart;
        private readonly int _idEnd;
        private readonly string _inputFile;

        public MeanOfData(TaskClass task, int index, int idStart, int idEnd, string inputFile)
        {
            _task = task;
            _index = index;
            _idStart = idStart;
            _idEnd = idEnd;
            _inputFile = inputFile;
        }

        /// <summary>
        /// Entry point for ThreadStart.
        /// </summary>
        public void Run()
        {
            long startTime = Environment.TickCount64;

            try
            {
                using (var reader = new StreamReader(_inputFile))
                {
                    // Skip lines until idStart
                    string line;
                    int currentId = -1;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Parse the first column: ID
                        var parts = line.Split(',');
                        if (!int.TryParse(parts[0], out currentId)) continue;

                        if (currentId >= _idStart) break;
                    }

                    // Now sum over [_idStart.._idEnd]
                    double sumAge = 0.0;
                    double sumBones = 0.0;
                    int count = 0;

                    while (line != null)
                    {
                        var values = line.Split(',');
                        if (!int.TryParse(values[0], out int id)) break;
                        if (id > _idEnd) break;

                        // values[3] = age, values[4] = boneFractures
                        if (double.TryParse(values[3], out double age) &&
                            double.TryParse(values[4], out double bones))
                        {
                            sumAge += age;
                            sumBones += bones;
                            count++;
                        }

                        if (count >= (_idEnd - _idStart + 1)) break;
                        line = reader.ReadLine();
                    }

                    double meanAge = (count > 0) ? (sumAge / count) : 0.0;
                    double meanBones = (count > 0) ? (sumBones / count) : 0.0;
                    long endTime = Environment.TickCount64;

                    _task.MeanFromThread(meanAge, meanBones, endTime - startTime, _index);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"MeanOfData (thread {_index}) error: {ex.Message}");
            }
        }
    }
}

using System;
using System.Formats.Asn1;
using System.IO;
using System.Threading;

namespace ExampleApp
{
    /// <summary>
    /// After global means (meanX, meanY) are known, this class reads lines [idStart..idEnd] from CSV,
    /// accumulates (x−meanX)², (y−meanY)², (x−meanX)*(y−meanY) for its slice, then calls back to TaskClass.addFromThread().
    /// </summary>
    public class PearsonParameters
    {
        private readonly TaskClass _task;
        private readonly int _index;
        private readonly int _idStart;
        private readonly int _idEnd;
        private readonly double _meanX;
        private readonly double _meanY;
        private readonly string _inputFile;

        public PearsonParameters(double meanX, double meanY,
                                 int idStart, int idEnd,
        string inputFile,
                                 TaskClass task, int index)
        {
            _meanX = meanX;
            _meanY = meanY;
            _idStart = idStart;
            _idEnd = idEnd;
            _inputFile = inputFile;
            _task = task;
            _index = index;
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
                        var parts = line.Split(',');
                        if (!int.TryParse(parts[0], out currentId)) continue;
                        if (currentId >= _idStart) break;
                    }

                    // Now accumulate until idEnd
                    double paramXX = 0.0;
                    double paramYY = 0.0;
                    double paramXY = 0.0;
                    int count = 0;

                    while (line != null)
                    {
                        var values = line.Split(',');
                        if (!int.TryParse(values[0], out int id)) break;
                        if (id > _idEnd) break;

                        if (double.TryParse(values[3], out double age) &&
                            double.TryParse(values[4], out double bones))
                        {
                            double dx = age - _meanX;
                            double dy = bones - _meanY;
                            paramXX += dx * dx;
                            paramYY += dy * dy;
                            paramXY += dx * dy;
                            count++;
                        }

                        if (count >= (_idEnd - _idStart + 1)) break;
                        line = reader.ReadLine();
                    }

                    long endTime = Environment.TickCount64;
                    _task.AddFromThread(paramXY, paramXX, paramYY, endTime - startTime, _index);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"PearsonParameters (thread {_index}) error: {ex.Message}");
            }
        }
    }
}

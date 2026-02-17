using System;
using System.Threading;

namespace ExampleApp
{
    /// <summary>
    /// Koordynuje dwie fazy obliczeń (faza 1: obliczanie średnich, faza 2: obliczanie sum do korelacji),
    /// a do każdej zębatki (każdy wątek w fazie 1 lub 2) wysyła aktualizację postępu przez IProgress<double>.
    /// </summary>
    public class TaskClass
    {
        private readonly int _numberOfThreads;
        private readonly int _counterOfLines = 100_000;
        private readonly string _inputFile;

        private double _meanX, _meanY;      // globalne średnie
        private double _sumXY, _sumXX, _sumYY; // globalne sumy do korelacji

        private readonly Thread[] _threads;  // tablica wątków
        private readonly long[] _times;      // czasy poszczególnych wątków (ostatnia faza)

        private readonly object _meanLock = new object();
        private readonly object _sumLock = new object();

        private readonly IProgress<double> _progress; // raportowanie postępu (0–100%)

        private int _completedTasks; // ile „podzadań” (threads w fazie1 lub fazie2) już skończyło
        private readonly int _totalTasks; // = 2 * numberOfThreads

        private long _overallStartTime;
        private long _overallEndTime;

        /// <summary>
        /// Ostateczna wartość korelacji Pearsona (po wykonaniu 2 faz).
        /// </summary>
        public double PearsonCorrelation { get; private set; }

        /// <summary>
        /// Całkowity czas obliczeń (ms) od startu fazy 1 do końca fazy 2.
        /// </summary>
        public long TotalComputingTime => _overallEndTime - _overallStartTime;

        /// <summary>
        /// Konstruktor uruchamia obliczenia w dwóch fazach. 
        /// Parametr progress (IProgress<double>) będzie wzywany z wartościami od 0 do 100
        /// w miarę ukończonych wątków (faza1 + faza2).
        /// </summary>
        public TaskClass(int numberOfThreads, string inputFile, IProgress<double> progress)
        {
            if (numberOfThreads <= 0)
                throw new ArgumentException("Number of threads must be ≥ 1", nameof(numberOfThreads));

            _numberOfThreads = numberOfThreads;
            _inputFile = inputFile;
            _threads = new Thread[_numberOfThreads];
            _times = new long[_numberOfThreads];

            _meanX = 0.0;
            _meanY = 0.0;
            _sumXY = 0.0;
            _sumXX = 0.0;
            _sumYY = 0.0;

            _progress = progress ?? new Progress<double>(); // jeśli null, create dummy

            _completedTasks = 0;
            _totalTasks = 2 * _numberOfThreads; // faza1: _numberOfThreads wątków + faza2: _numberOfThreads wątków

            _overallStartTime = Environment.TickCount64;

            // =======================
            // Faza 1: liczenie średnich
            // =======================
            int chunkSize = _counterOfLines / _numberOfThreads;
            for (int i = 0; i < _numberOfThreads; i++)
            {
                int startId = i * chunkSize;
                int endId = (i == _numberOfThreads - 1)
                            ? (_counterOfLines - 1)
                            : (startId + chunkSize - 1);

                var worker = new MeanOfData(this, i, startId, endId, _inputFile);
                _threads[i] = new Thread(new ThreadStart(worker.Run));
                _threads[i].Start();
            }

            // Czekamy, aż wszystkie wątki fazy 1 zwrócą się z MeanFromThread
            for (int i = 0; i < _numberOfThreads; i++)
            {
                _threads[i].Join();
            }

            // =======================
            // Faza 2: liczenie sum do korelacji
            // =======================
            for (int i = 0; i < _numberOfThreads; i++)
            {
                int startId = i * chunkSize;
                int endId = (i == _numberOfThreads - 1)
                            ? (_counterOfLines - 1)
                            : (startId + chunkSize - 1);

                var worker = new PearsonParameters(_meanX, _meanY, startId, endId, _inputFile, this, i);
                _threads[i] = new Thread(new ThreadStart(worker.Run));
                _threads[i].Start();
            }

            // Czekamy, aż wszystkie wątki fazy 2 zwrócą się z AddFromThread
            for (int i = 0; i < _numberOfThreads; i++)
            {
                _threads[i].Join();
            }

            // Po zsumowaniu fazy2 wyliczamy ostateczną korelację:
            if (_sumXX > 0 && _sumYY > 0)
            {
                PearsonCorrelation = _sumXY / (Math.Sqrt(_sumXX) * Math.Sqrt(_sumYY));
            }
            else
            {
                PearsonCorrelation = 0.0;
            }

            _overallEndTime = Environment.TickCount64;
        }

        /// <summary>
        /// Wywoływane przez każdy wątek MeanOfData po obliczeniu lokalnych średnich.
        /// Tutaj agregujemy wątki w fazie1 i raportujemy postęp.
        /// </summary>
        public void MeanFromThread(double threadMeanX, double threadMeanY, long elapsedMs, int index)
        {
            lock (_meanLock)
            {
                if (_meanX == 0.0 && _meanY == 0.0)
                {
                    _meanX = threadMeanX;
                    _meanY = threadMeanY;
                }
                else
                {
                    _meanX = (_meanX + threadMeanX) / 2.0;
                    _meanY = (_meanY + threadMeanY) / 2.0;
                }

                _times[index] = elapsedMs;
            }

            // Zwiększamy licznik zakończonych wątków (faza1 lub faza2):
            int done = Interlocked.Increment(ref _completedTasks);
            double percent = (done * 100.0) / _totalTasks;
            _progress.Report(percent);
        }

        /// <summary>
        /// Wywoływane przez każdy wątek PearsonParameters po obliczeniu lokalnych sum.
        /// Tutaj agregujemy wątki w fazie2 i raportujemy postęp.
        /// </summary>
        public void AddFromThread(double threadSumXY, double threadSumXX, double threadSumYY, long elapsedMs, int index)
        {
            lock (_sumLock)
            {
                _sumXY += threadSumXY;
                _sumXX += threadSumXX;
                _sumYY += threadSumYY;
                _times[index] = elapsedMs;
            }

            int done = Interlocked.Increment(ref _completedTasks);
            double percent = (done * 100.0) / _totalTasks;
            _progress.Report(percent);
        }

        /// <summary>
        /// Zwraca tablicę z czasami ms wątków (ostatnia faza).
        /// </summary>
        public long[] GetPerThreadTimes() => (long[])_times.Clone();
    }
}

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;    // dla BackgroundWorker

namespace ExampleApp
{
    public partial class MainWindow : Window
    {
        private const string CsvFileName = "data.csv";

        public MainWindow()
        {
            InitializeComponent();

            // Domyślnie ustawiamy, żeby Combobox pokazywał „Delegaty”
            MethodComboBox.SelectedIndex = 1;

            // Pasek postępu domyślnie ukrywamy
            MainProgressBar.Visibility = Visibility.Collapsed;
            MainProgressBar.IsIndeterminate = false;
            MainProgressBar.Minimum = 0;
            MainProgressBar.Maximum = 100;
            MainProgressBar.Value = 0;
        }

        private async void GenerateDataButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateDataButton.IsEnabled = false;
            ComputeButton.IsEnabled = false;
            OutputTextBox.Clear();

            // Pokażemy ProgressBar w trybie indeterminate,
            // bo generowanie danych ma stałą liczbę rekordów, nie rzucamy procentów z GeneratorDanych
            MainProgressBar.Visibility = Visibility.Visible;
            MainProgressBar.IsIndeterminate = true;

            StatusTextBlock.Text = "Generowanie danych (może potrwać)...";

            try
            {
                await Task.Run(() =>
                {
                    if (File.Exists(CsvFileName))
                        File.Delete(CsvFileName);

                    GeneratorDanych.GenerateData();
                });

                StatusTextBlock.Text = $"Plik \"{CsvFileName}\" wygenerowany.";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Błąd podczas generowania: {ex.Message}";
            }
            finally
            {
                // Ukrywamy ProgressBar i odblokowujemy przyciski
                MainProgressBar.IsIndeterminate = false;
                MainProgressBar.Visibility = Visibility.Collapsed;

                GenerateDataButton.IsEnabled = true;
                ComputeButton.IsEnabled = true;
            }
        }


        private void ComputeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(CsvFileName))
            {
                MessageBox.Show($"Plik \"{CsvFileName}\" nie istnieje. Najpierw wygeneruj dane.",
                                "Brak pliku", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(ThreadsTextBox.Text.Trim(), out int nThreads) || nThreads <= 0)
            {
                MessageBox.Show("Podaj poprawną, dodatnią liczbę wątków.",
                                "Niepoprawne dane", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Blokujemy UI i przygotowujemy ProgressBar:
            GenerateDataButton.IsEnabled = false;
            ComputeButton.IsEnabled = false;
            OutputTextBox.Clear();

            MainProgressBar.Value = 0;
            MainProgressBar.Visibility = Visibility.Visible;
            MainProgressBar.IsIndeterminate = false; // od teraz to będzie "determinate"

            StatusTextBlock.Text = "Start obliczeń…";

            switch (MethodComboBox.SelectedIndex)
            {
                case 0: // Task<T>
                    StatusTextBlock.Text = "Start obliczeń (Task<T>)…";
                    RunWithTaskFactory(nThreads);
                    break;
                case 1: // async-await
                    StatusTextBlock.Text = "Start obliczeń (async-await)…";
                    RunWithAsyncAwait(nThreads);
                    break;
                case 2: // BackgroundWorker
                    StatusTextBlock.Text = "Start obliczeń (BackgroundWorker)…";
                    RunWithBackgroundWorker(nThreads);
                    break;
                default:
                    MessageBox.Show("Nieznana metoda.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    UnblockUIAndHideProgress();
                    break;
            }
        }

        /// <summary>
        /// Odfunkcyjnia UI i ukrywa ProgressBar.
        /// </summary>
        private void UnblockUIAndHideProgress()
        {
            MainProgressBar.IsIndeterminate = false;
            MainProgressBar.Visibility = Visibility.Collapsed;

            GenerateDataButton.IsEnabled = true;
            ComputeButton.IsEnabled = true;
        }

        /// <summary>
        /// Uruchamia TaskClass synchronnie, przekazując reporter postępu (IProgress<double>),
        /// zwraca ComputeResult po zakończeniu.
        /// </summary>
        private ComputeResult ComputePearsonSynchronously(int nThreads, IProgress<double> progress)
        {
            var task = new TaskClass(nThreads, CsvFileName, progress);
            return new ComputeResult
            {
                PearsonCorrelation = task.PearsonCorrelation,
                TotalTimeMs = task.TotalComputingTime,
                PerThreadTimes = task.GetPerThreadTimes()
            };
        }

        private void RunWithTaskFactory(int nThreads)
        {
            // Tworzymy reporter postępu, który w UI ustawi wartość ProgressBar.Value
            var progress = new Progress<double>(percent =>
            {
                // Wywoływane w wątku kontekstu synchronizacji UI
                MainProgressBar.Value = percent;
            });

            // Pełne obliczenia wrzucamy do Task.Run, przekazując reporter do ComputePearsonSynchronously
            Task<ComputeResult> task = Task.Run(() =>
            {
                return ComputePearsonSynchronously(nThreads, progress);
            });

            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Dispatcher.Invoke(() =>
                    {
                        StatusTextBlock.Text = $"Błąd w Task<T>: {t.Exception.GetBaseException().Message}";
                        UnblockUIAndHideProgress();
                    });
                }
                else
                {
                    ComputeResult result = t.Result;
                    Dispatcher.Invoke(() =>
                    {
                        MainProgressBar.Value = 100; // upewniamy się, że 100% jest widoczne
                        DisplayResult("Task<T>", nThreads, result);
                        StatusTextBlock.Text = "Obliczenia (Task<T>) zakończone.";
                        UnblockUIAndHideProgress();
                    });
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        
        private async void RunWithAsyncAwait(int nThreads)
        {
            // 1) Reporter postępu:
            var progress = new Progress<double>(percent =>
            {
                MainProgressBar.Value = percent;
            });

            try
            {
                // 2) Czekamy aż Task.Run skończy (ComputePearsonSynchronously z progress) …
                ComputeResult result = await Task.Run(() =>
                {
                    return ComputePearsonSynchronously(nThreads, progress);
                });

                // 3) Po zakończeniu wątek UI wyświetla wynik i ustawia pasek na 100%:
                MainProgressBar.Value = 100;
                DisplayResult("async-await", nThreads, result);
                StatusTextBlock.Text = "Obliczenia (async-await) zakończone.";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Błąd w async-await: {ex.GetBaseException().Message}";
            }
            finally
            {
                UnblockUIAndHideProgress();
            }
        }

        private void RunWithBackgroundWorker(int nThreads)
        {
            // 1) Zrobimy BackgroundWorker i dopiszemy tam IProgress<double> ręcznie,
            //    bo BackgroundWorker miał domyślną obsługę progress, ale w naszym TaskClass
            //    używamy IProgress<double>. Zatem stworzymy progress, który będzie publikował
            //    zmiany w writerze do BackgroundWorker.ProgressChanged.

            var bw = new BackgroundWorker
            {
                WorkerReportsProgress = true,  // dzięki temu możemy w DoWork raportować procent
                WorkerSupportsCancellation = false
            };

            // Reporter postępu, który wprost wywoła odpowiednią metodę BackgroundWorker.ReportProgress.
            IProgress<double> progress = new Progress<double>(percent =>
            {
                // rounds to int because ReportProgress przyjmuje int
                int intPerc = (int)Math.Round(percent);
                bw.ReportProgress(intPerc);
            });

            // 2) W DoWork wykonujemy ComputePearsonSynchronously, przekazując reporter:
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    int threads = (int)args.Argument;
                    ComputeResult result = ComputePearsonSynchronously(threads, progress);
                    args.Result = result;
                }
                catch (Exception ex)
                {
                    args.Result = ex; // żeby potem w RunWorkerCompleted wykryć wyjątek
                }
            };

            // 3) Gdy progress się zmieni, aktualizujemy ProgressBar:
            bw.ProgressChanged += (sender, args) =>
            {
                MainProgressBar.Value = args.ProgressPercentage;
            };

            // 4) Gdy wszystko się zakończy finalnie:
            bw.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error != null)
                {
                    StatusTextBlock.Text = $"Błąd w BackgroundWorker: {args.Error.GetBaseException().Message}";
                }
                else if (args.Result is Exception ex)
                {
                    StatusTextBlock.Text = $"Błąd w BackgroundWorker: {ex.GetBaseException().Message}";
                }
                else if (args.Result is ComputeResult result)
                {
                    MainProgressBar.Value = 100;
                    DisplayResult("BackgroundWorker", nThreads, result);
                    StatusTextBlock.Text = "Obliczenia (BackgroundWorker) zakończone.";
                }

                UnblockUIAndHideProgress();
            };

            // 5) Uruchomienie:
            bw.RunWorkerAsync(nThreads);
        }


        private void DisplayResult(string methodName, int nThreads, ComputeResult result)
        {
            OutputTextBox.Clear();
            OutputTextBox.AppendText($"Metoda: {methodName}\r\n");
            OutputTextBox.AppendText($"Liczba wątków: {nThreads}\r\n");
            OutputTextBox.AppendText($"Pearson correlation: {result.PearsonCorrelation:F6}\r\n");
            OutputTextBox.AppendText($"Total elapsed time (ms): {result.TotalTimeMs}\r\n\r\n");
            OutputTextBox.AppendText("Per-thread times (ms):\r\n");
            for (int i = 0; i < result.PerThreadTimes.Length; i++)
            {
                OutputTextBox.AppendText($"  Thread {i}: {result.PerThreadTimes[i]} ms\r\n");
            }
        }

    }
}

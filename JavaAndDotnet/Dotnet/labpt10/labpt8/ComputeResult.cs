namespace ExampleApp
{
    /// <summary>
    /// Przechowuje rezultaty obliczeń: wartość korelacji oraz czasy poszczególnych wątków.
    /// </summary>
    public class ComputeResult
    {
        /// <summary>Wynik korelacji Pearsona.</summary>
        public double PearsonCorrelation { get; set; }

        /// <summary>Całkowity czas obliczeń (ms), od rozpoczęcia fazy 1 do zakończenia fazy 2.</summary>
        public long TotalTimeMs { get; set; }

        /// <summary>Czasy (ms) per-thread w ostatniej fazie (PearsonParameters lub MeanOfData w zależności od implementacji).</summary>
        public long[] PerThreadTimes { get; set; }
    }
}

namespace Typerary.Shared
{
    public class PracticeResult
    {
        private bool hasResult = false;
        private SortedDictionary<int, PracticeSectionResult> practiceSectionResults;
        private int collectCount;
        private int wrongCount;
        private double accuracy;

        public PracticeResult()
        {
            practiceSectionResults = new SortedDictionary<int, PracticeSectionResult>();
            hasResult = false;
        }

        public void AddSectionResult(int key, PracticeSectionResult result)
        {
            practiceSectionResults.Add(key, result);
            hasResult = true;
        }

        public void SetResultValues()
        {
            foreach (var result in practiceSectionResults.Values)
            {
                collectCount += result.CollectCount;
                wrongCount += result.WrongCount;
            }
            accuracy = 100.0 * collectCount / (collectCount + wrongCount);
            Console.WriteLine($"Result Set : Collect => {collectCount} / wrongCount => {wrongCount} / Accuracy => {accuracy.ToString("0.00")}");
        }

        public int GetCollectCount() => collectCount;

        public int GetWrongCount() => wrongCount;

        public double GetAccuracy() => accuracy;
    }
}

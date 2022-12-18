namespace Typerary.Shared
{
    public class PracticeResult
    {
        private SortedDictionary<int, PracticeSectionResult> _practiceSectionResults;

        public PracticeResult()
        {
            _practiceSectionResults = new SortedDictionary<int, PracticeSectionResult>();
        }

        public void AddSectionResult(int key, PracticeSectionResult result)
        {
            _practiceSectionResults.Add(key, result);
            Console.WriteLine($"{key}, {result}");
        }
    }
}

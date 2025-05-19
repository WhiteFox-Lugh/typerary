namespace Typerary.Shared
{
    public class PracticeResult
    {
        private readonly SortedDictionary<int, PracticeSectionResult> _practiceSectionResults = new();
        private int _collectCount;
        private int _wrongCount;
        private double _accuracy;

        public void AddSectionResult(int key, PracticeSectionResult result)
        {
            _practiceSectionResults.Add(key, result);
        }

        public void SetResultValues()
        {
            foreach (var result in _practiceSectionResults.Values)
            {
                _collectCount += result.CollectCount;
                _wrongCount += result.WrongCount;
            }
            _accuracy = 100.0 * _collectCount / (_collectCount + _wrongCount);
        }

        public int GetCollectCount() => _collectCount;

        public int GetWrongCount() => _wrongCount;

        public double GetAccuracy() => _accuracy;

        public SortedDictionary<int, PracticeSectionResult> GetSectionResults() => new(_practiceSectionResults);
    }
}

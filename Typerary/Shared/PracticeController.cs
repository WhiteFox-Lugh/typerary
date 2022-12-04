namespace Typerary.Shared
{
    public class PracticeController
    {
        private readonly BookContent[] _bookContents;
        private readonly List<string> _taskSentences = new();
        private readonly List<string> _judgeSentences = new();
        private int currentTaskSentenceIndex = 0;

        public BookContent[] Content
        {
            get
            {
                var ret = new BookContent[_bookContents.Length];
                Array.Copy(_bookContents, ret, _bookContents.Length);
                return ret;
            }
        }

        public SortedDictionary<int, PracticeSectionResult> PracticeSectionResults { init; get; }

        public PracticeController(Book book)
        {
            _bookContents = book.Content;
            SetTaskSentences();
            PracticeSectionResults = new();
            ResetTaskIndex();
        }

        public void ResetTaskIndex() => currentTaskSentenceIndex = 0;

        public string GetFirstTaskSentence() => _taskSentences[0];

        public bool HasNextTaskSentence() => currentTaskSentenceIndex + 1 < _taskSentences.Count;

        public string? GetNextTaskSentence() => HasNextTaskSentence() ? _taskSentences[currentTaskSentenceIndex + 1] : null;

        public void IncrementTaskSentenceIndex()
        {
            ++currentTaskSentenceIndex;
            Console.WriteLine("Index Incremented");
        }

        private void ClearTaskAndJudgeSentences()
        {
            _taskSentences.Clear();
            _judgeSentences.Clear();
        }

        public void ClearPracticeSectionResults() => PracticeSectionResults.Clear();

        public void SendAndScoringInputSentence(string sentence)
        {
            var currentIndex = currentTaskSentenceIndex;
            var currentJudgeSentence = _judgeSentences[currentIndex];
            var sectionResult = new PracticeSectionResult(currentJudgeSentence, sentence);
            sectionResult.SetDiffMarkUpSentence();
            PracticeSectionResults.Add(currentIndex, sectionResult);
        }

        public void SetTaskSentences(int sectionNumber = 0, int sentenceNumber = 0)
        {
            ClearTaskAndJudgeSentences();
            for (var sectionIdx = sectionNumber; sectionIdx < Content.Length; ++sectionIdx)
            {
                var sentences = Content[sectionIdx].Sentences;
                for (var sentenceIdx = (sectionIdx == sectionNumber) ? sentenceNumber : 0; sentenceIdx < sentences.Length; ++sentenceIdx)
                {
                    var taskSentence = sentences[sentenceIdx].OriginSentence;
                    _taskSentences.Add(taskSentence);
                    var judgeSentence = sentences[sentenceIdx].JudgeSentence;
                    _judgeSentences.Add(judgeSentence);
                }
            }
        }
    }
}

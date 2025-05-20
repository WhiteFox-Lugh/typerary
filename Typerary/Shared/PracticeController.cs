namespace Typerary.Shared
{
    public class PracticeController
    {
        private readonly List<string?> _taskSentences = new();
        private readonly List<string?> _judgeSentences = new();
        private int _currentTaskSentenceIndex;
        private BookContent[] _bookContents;

        public PracticeResult CurrentPracticeResult { get; private set; }

        private BookContent[] Content
        {
            get
            {
                var ret = new BookContent[_bookContents.Length];
                Array.Copy(_bookContents, ret, _bookContents.Length);
                return ret;
            }
        }

        public bool IsFinished { get; private set; }

        public void Initialize(Book book)
        {
            if (book is null or { Content: null }) { throw new ArgumentNullException(nameof(book)); }
            _bookContents = book.Content;
        }

        public void Reset()
        {
            SetTaskSentences(sectionNumber: 0, sentenceNumber: 0);
            _currentTaskSentenceIndex = 0;
            CurrentPracticeResult = new();
            IsFinished = false;
        }

        public string? GetFirstTaskSentence() => _taskSentences[0];

        public bool IsFirstSentence() => _currentTaskSentenceIndex == 0;

        public bool HasNextTaskSentence() => _currentTaskSentenceIndex + 1 < _taskSentences.Count;

        public string? GetNextTaskSentence() => HasNextTaskSentence() ? _taskSentences[_currentTaskSentenceIndex + 1] : null;

        public void IncrementTaskSentenceIndex() => ++_currentTaskSentenceIndex;

        public int GetSectionCount() => _taskSentences.Count();

        public int GetCurrentSectionNumber() => _currentTaskSentenceIndex + 1;

        public void SendAndScoringInputSentence(string sentence)
        {
            var currentIndex = _currentTaskSentenceIndex;
            if (currentIndex >= _taskSentences.Count) { return; }

            var currentJudgeSentence = _judgeSentences[currentIndex];
            var sectionResult = new PracticeSectionResult(currentJudgeSentence, sentence);
            sectionResult.SetDiffMarkUpSentence();
            CurrentPracticeResult.AddSectionResult(currentIndex, sectionResult);
        }

        private void SetTaskSentences(int sectionNumber, int sentenceNumber)
        {
            _taskSentences.Clear();
            _judgeSentences.Clear();

            for (var sectionIdx = sectionNumber; sectionIdx < Content.Length; ++sectionIdx)
            {
                var sentences = Content[sectionIdx].Sentences;
                if (sentences != null)
                {
                    for (var sentenceIdx = (sectionIdx == sectionNumber) ? sentenceNumber : 0;
                         sentenceIdx < sentences.Length;
                         ++sentenceIdx)
                    {
                        var taskSentence = sentences[sentenceIdx].OriginSentence;
                        _taskSentences.Add(taskSentence);
                        var judgeSentence = sentences[sentenceIdx].JudgeSentence;
                        _judgeSentences.Add(judgeSentence);
                    }
                }
            }
        }

        public void DoFinishProcess()
        {
            if (IsFinished) { return; }
            IsFinished = true;

            CurrentPracticeResult.SetResultValues();
        }
    }
}

namespace Typerary.Shared
{
    public class PracticeController
    {
        private readonly BookContent[]? _bookContents;
        private readonly List<string?> _taskSentences = new();
        private readonly List<string?> _judgeSentences = new();
        private int _currentTaskSentenceIndex;

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
        
        public PracticeController(Book book)
        {
            _bookContents = book.Content;
            Init();
        }

        public void Init()
        {
            SetTaskSentences();
            _currentTaskSentenceIndex = 0;
            IsFinished = false;
        }

        public string? GetFirstTaskSentence() => _taskSentences[0];

        public bool IsFirstSentence() => _currentTaskSentenceIndex == 0;

        public bool HasNextTaskSentence() => _currentTaskSentenceIndex + 1 < _taskSentences.Count;

        public string? GetNextTaskSentence() => HasNextTaskSentence() ? _taskSentences[_currentTaskSentenceIndex + 1] : null;

        public void IncrementTaskSentenceIndex() => ++_currentTaskSentenceIndex;

        private void ResetPracticeResult() => CurrentPracticeResult = new();

        public int GetSectionCount() => _taskSentences.Count();

        public int GetCurrentSectionNumber() => _currentTaskSentenceIndex + 1;

        public void SendAndScoringInputSentence(string sentence)
        {
            var currentIndex = _currentTaskSentenceIndex;
            if (currentIndex >= _taskSentences.Count) { return; }
            // 最初のセクション打ち終わったらリセットする
            else if (currentIndex == 0) { ResetPracticeResult(); }

            var currentJudgeSentence = _judgeSentences[currentIndex];
            var sectionResult = new PracticeSectionResult(currentJudgeSentence, sentence);
            sectionResult.SetDiffMarkUpSentence();
            CurrentPracticeResult.AddSectionResult(currentIndex, sectionResult);
        }

        private void SetTaskSentences(int sectionNumber = 0, int sentenceNumber = 0)
        {
            _taskSentences.Clear();
            _judgeSentences.Clear();

            for (var sectionIdx = sectionNumber; sectionIdx < Content.Length; ++sectionIdx)
            {
                var sentences = Content[sectionIdx].Sentences;
                if (sentences != null)
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

        public void DoFinishProcess()
        {
            if (IsFinished) { return; }
            IsFinished = true;

            CurrentPracticeResult.SetResultValues();
        }
    }
}

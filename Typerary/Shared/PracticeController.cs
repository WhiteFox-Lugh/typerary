namespace Typerary.Shared
{
    public class PracticeController
    {
        private readonly BookContent[] _bookContents;
        private readonly List<string> _taskSentences = new();
        private readonly List<string> _judgeSentences = new();
        private int currentTaskSentenceIndex = 0;

        public PracticeResult CurrentPracticeResult { get; private set; }

        public BookContent[] Content
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
            SetTaskSentences();
            ResetTaskIndex();
            ResetPracticeResult();
            IsFinished = false;
        }

        public void ResetTaskIndex() => currentTaskSentenceIndex = 0;

        public string GetFirstTaskSentence() => _taskSentences[0];

        public bool HasNextTaskSentence() => currentTaskSentenceIndex + 1 < _taskSentences.Count;

        public string? GetNextTaskSentence() => HasNextTaskSentence() ? _taskSentences[currentTaskSentenceIndex + 1] : null;

        public void IncrementTaskSentenceIndex() => ++currentTaskSentenceIndex;

        private void ResetPracticeResult() => CurrentPracticeResult = new();

        private void ClearTaskAndJudgeSentences()
        {
            _taskSentences.Clear();
            _judgeSentences.Clear();
        }


        public void SendAndScoringInputSentence(string sentence)
        {
            var currentIndex = currentTaskSentenceIndex;
            if (currentIndex >= _taskSentences.Count) { return; }

            var currentJudgeSentence = _judgeSentences[currentIndex];
            var sectionResult = new PracticeSectionResult(currentJudgeSentence, sentence);
            sectionResult.SetDiffMarkUpSentence();
            CurrentPracticeResult.AddSectionResult(currentIndex, sectionResult);
        }

        public void SetTaskSentences(int sectionNumber = 0, int sentenceNumber = 0)
        {
            IsFinished = false;
            ClearTaskAndJudgeSentences();
            ResetPracticeResult();
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

        public void DoFinishProcess()
        {
            if (IsFinished) { return; }
            IsFinished = true;

            CurrentPracticeResult.SetResultValues();
        }
    }
}

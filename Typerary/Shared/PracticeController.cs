namespace Typerary.Shared
{
    public class PracticeController
    {
        private BookContent[] _bookContents;
        private readonly List<string> _taskSentences = new();
        private readonly List<string> _judgeSentences = new();
        private readonly List<string> _inputSentences = new();

        public string BookTitle { get; private set; }
        public BookContent[] Content {
            get {
                var ret = new BookContent[_bookContents.Length];
                Array.Copy(_bookContents, ret, _bookContents.Length);
                return ret;
            }
        }

        public List<string> TaskSentences { get => new(_taskSentences); }
        public List<string> InputSentences { get => new(_inputSentences); }

        public PracticeController(Book book)
        {
            BookTitle = book.Title;
            _bookContents = book.Content;
            SetTaskSentences();
        }

        private void ClearTaskSentences() => _taskSentences.Clear();

        private void ClearJudgeSentences() => _judgeSentences.Clear();

        private void ClearInputSentences() => _inputSentences.Clear();

        public void SetTaskSentences(int sectionNumber = 0, int sentenceNumber = 0)
        {
            ClearTaskSentences();
            ClearJudgeSentences();
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

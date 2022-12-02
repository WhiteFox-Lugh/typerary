namespace Typerary.Shared
{
    public class PracticeController
    {
        private readonly BookContent[] _bookContents;
        private readonly List<string> _taskSentences = new();
        private readonly List<string> _judgeSentences = new();
        private readonly List<string> _inputSentences = new();
        private static int currentTaskSentenceIndex = 0;

        public string BookTitle { init; get; }
        public BookContent[] Content
        {
            get
            {
                var ret = new BookContent[_bookContents.Length];
                Array.Copy(_bookContents, ret, _bookContents.Length);
                return ret;
            }
        }

        public PracticeController(Book book)
        {
            BookTitle = book.Title;
            _bookContents = book.Content;
            SetTaskSentences();
            currentTaskSentenceIndex = 0;
        }

        public string GetFirstTaskSentence() => _taskSentences[0];

        public (bool hasNext, string sentence) GetNextTaskSentence()
        {
            if (currentTaskSentenceIndex + 1 >= _taskSentences.Count) return (false, "");
            currentTaskSentenceIndex++;
            return (true, _taskSentences[currentTaskSentenceIndex]);
        }

        private void ClearTaskSentences() => _taskSentences.Clear();

        private void ClearJudgeSentences() => _judgeSentences.Clear();

        public void ClearInputSentences() => _inputSentences.Clear();

        public void AddInputSentence(string sentence) => _inputSentences.Add(sentence);

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

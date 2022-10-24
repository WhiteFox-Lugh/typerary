namespace Typerary.Shared
{
    public class PracticeController
    {
        private static BookContent[] content;
        public static List<string> TaskSentences { get; } = new();
        public static List<string> JudgeSentences { get; } = new();
        public static List<string> InputSentences { get; } = new();

        public PracticeController(Book book)
        {
            if (book is null) throw new InvalidDataException("book is null");
            if (book.Content is null) throw new InvalidDataException("book content is null");
            content = book.Content;
            SetTaskSentences();
        }

        private void ClearTaskSentences() => TaskSentences.Clear();

        private void ClearJudgeSentences() => JudgeSentences.Clear();

        private void ClearInputSentences() => InputSentences.Clear();

        public void SetTaskSentences(int sectionNumber = 0, int sentenceNumber = 0)
        {
            ClearTaskSentences();
            ClearJudgeSentences();
            for (var sectionIdx = sectionNumber; sectionIdx < content.Length; ++sectionIdx)
            {
                var sentences = content[sectionIdx].Sentences;
                for (var sentenceIdx = (sectionIdx == sectionNumber) ? sentenceNumber : 0; sentenceIdx < sentences.Length; ++sentenceIdx)
                {
                    var taskSentence = sentences[sentenceIdx].OriginSentence;
                    TaskSentences.Add(taskSentence);
                    var judgeSentence = sentences[sentenceIdx].JudgeSentence;
                    JudgeSentences.Add(judgeSentence);
                }
            }
        }
    }
}

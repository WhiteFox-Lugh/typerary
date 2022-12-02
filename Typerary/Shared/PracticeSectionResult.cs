using DiffMatchPatch;

namespace Typerary.Shared
{
    public class PracticeSectionResult
    {
        private static diff_match_patch dmp;
        private string OriginalSentence { init; get; }
        private string InputSentence { init; get; }
        private string DiffMarkUpSentence { init; get; }

        public PracticeSectionResult(string originalSentence, string inputSentence)
        {
            dmp ??= new();
            OriginalSentence = originalSentence;
            InputSentence = inputSentence;

            var diffs = GenerateDiff();
            DiffMarkUpSentence = dmp.diff_prettyHtml(diffs).ToString();

            Console.WriteLine(originalSentence);
            Console.WriteLine(inputSentence);
            Console.WriteLine(DiffMarkUpSentence);
        }

        private List<Diff> GenerateDiff() => dmp.diff_main(OriginalSentence, InputSentence);
    }
}

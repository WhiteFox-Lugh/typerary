using DiffMatchPatch;
// Todo: デバッグ用なので後で消す
using System.Threading;

namespace Typerary.Shared
{
    public class PracticeSectionResult
    {
        private static diff_match_patch dmp;
        public string JudgeSentence { init; get; }
        public string InputSentence { init; get; }
        public string DiffMarkUpSentence { get; private set; }

        public PracticeSectionResult(string judgeSentence, string inputSentence)
        {
            dmp ??= new();
            JudgeSentence = judgeSentence;
            InputSentence = inputSentence;
        }

        // private async Task<List<Diff>> GenerateDiff()
        private List<Diff> GenerateDiff()
        {
            //Console.WriteLine("Start GenerateDiff()");
            //var task = Task.Run(() => dmp.diff_main(OriginalSentence, InputSentence));
            //Console.WriteLine("Before dmp.diff_main()");
            //task.Start();
            //Console.WriteLine("After dmp.diff_main()");
            //var result = await task;
            //Console.WriteLine("End GenerateDiff()");
            //return result;

            // diff 取得に1秒かかると仮定
            Thread.Sleep(1000);

            // InputSentence -> JudgeSentence への diff
            return dmp.diff_main(InputSentence, JudgeSentence);
        }

        // public async Task SetDiffMarkUpSentence()
        public void SetDiffMarkUpSentence()
        {
            //Console.WriteLine("Start SetDiffMarkUpSentence()");
            //var diffTask = Task.Run(() => GenerateDiff());
            //Console.WriteLine("Before GenerateDiff()");
            //var diffs = await diffTask;
            //Console.WriteLine("After GenerateDiff()");
            var diffs = GenerateDiff();
            DiffMarkUpSentence = dmp.diff_prettyHtml(diffs).ToString();

            Console.WriteLine(JudgeSentence);
            Console.WriteLine(InputSentence);
            Console.WriteLine(DiffMarkUpSentence);
        }
    }
}

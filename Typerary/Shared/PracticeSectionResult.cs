using DiffMatchPatch;
using Ganss.XSS;
using Shaman.Runtime;
using System.Web;
using System.Text;
// Todo: デバッグ用なので後で消す
using System.Threading;

namespace Typerary.Shared
{

    public class PracticeSectionResult
    {
        private static diff_match_patch dmp;
        private readonly Func<ValueString, string> insertHtmlStyle = (ValueString text) => $"<ins style=\"background:#e6ffe6;\">{text}</ins>";
        private readonly Func<ValueString, string> deleteHtmlStyle = (ValueString text) => $"<del style=\"background:#ffe6e6;\">{text}</del>";
        private readonly Func<ValueString, string> replaceHtmlStyle = (ValueString text) => $"<span style=\"background:#e6e6ff\">{text}</span>";
        private readonly Func<ValueString, string> equalHtmlStyle = (ValueString text) => $"<span>{text}</span>";

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

        private string ConvertDiffsToHtmlString(List<Diff> diffs)
        {
            var stringBuilder = ReseekableStringBuilder.AcquirePooledStringBuilder();
            for (var i = 0; i < diffs.Count; i++)
            {
                var ithOperation = diffs[i].operation;
                var ithText = diffs[i].text;
                stringBuilder.Append(
                    ithOperation switch
                    {
                        Operation.EQUAL => equalHtmlStyle.Invoke(ithText),
                        Operation.INSERT => insertHtmlStyle.Invoke(ithText),
                        Operation.DELETE when (i + 1 < diffs.Count) && (diffs[i + 1].operation is Operation.INSERT) => replaceHtmlStyle.Invoke(ithText),
                        Operation.DELETE => deleteHtmlStyle.Invoke(ithText),
                        _ => ""
                    });
                if (ithOperation is Operation.DELETE && i + 1 < diffs.Count && diffs[i + 1].operation is Operation.INSERT)
                {
                    i++;
                }
            }

            var text = stringBuilder.ToString();
            var sanitizer = new HtmlSanitizer();
            var ret = sanitizer.Sanitize(text);
            ReseekableStringBuilder.Release(stringBuilder);
            return ret;
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
            DiffMarkUpSentence = ConvertDiffsToHtmlString(diffs);

            Console.WriteLine(JudgeSentence);
            Console.WriteLine(InputSentence);
            Console.WriteLine(DiffMarkUpSentence);
        }
    }
}

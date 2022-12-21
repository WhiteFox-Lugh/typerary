using DiffMatchPatch;
using Ganss.XSS;
using Shaman.Runtime;

namespace Typerary.Shared
{

    public class PracticeSectionResult
    {
        public enum DiffOperation
        {
            Equal,
            Insert,
            Delete,
            Replace
        }
        public class TyperaryDiff
        {
            public readonly DiffOperation operation;
            public readonly ValueString input;
            public readonly ValueString? correctString;

            public TyperaryDiff(DiffOperation op, ValueString inputText, ValueString? correct = null)
            {
                operation = op;
                input = inputText;
                correctString = correct;
            }
        }

        private static diff_match_patch dmp;
        private readonly Func<ValueString, string> insertHtmlStyle = (ValueString text) => $"<ins style=\"background:#e6ffe6; font-weight:bold\">{text}</ins>";
        private readonly Func<ValueString, string> deleteHtmlStyle = (ValueString text) => $"<del style=\"background:#ffe6e6; font-weight:bold\">{text}</del>";
        private readonly Func<ValueString, ValueString?, string> replaceHtmlStyle = (ValueString wrongText, ValueString? collectText) => $"<span style=\"background:#e6e6ff; font-weight:bold\"><del>{wrongText}</del> &rarr; {collectText}</span>";
        private readonly Func<ValueString, string> equalHtmlStyle = (ValueString text) => $"<span style=\"color:#666666\">{text}</span>";
        public int CollectCount { get; private set; } = 10;
        public int WrongCount { get; private set; } = 1;

        public string JudgeSentence { init; get; }
        public string InputSentence { init; get; }
        public string DiffMarkUpSentence { get; private set; }

        public PracticeSectionResult(string judgeSentence, string inputSentence)
        {
            dmp ??= new();
            JudgeSentence = judgeSentence;
            InputSentence = inputSentence;
        }

        private List<Diff> GenerateDiff() => dmp.diff_main(InputSentence, JudgeSentence);

        private List<TyperaryDiff> ConvertDiffToTyperaryDiff(List<Diff> diffs)
        {
            var ret = new List<TyperaryDiff>();

            for (var i = 0; i < diffs.Count; ++i)
            {
                var ithOperation = diffs[i].operation;
                var ithText = diffs[i].text;
                var isReplace = (i + 1 < diffs.Count) && (diffs[i + 1].operation is Operation.INSERT);
                ret.Add(ithOperation switch
                {
                    Operation.EQUAL => new TyperaryDiff(DiffOperation.Equal, ithText),
                    Operation.INSERT => new TyperaryDiff(DiffOperation.Insert, ithText),
                    Operation.DELETE when isReplace => new TyperaryDiff(DiffOperation.Replace, ithText, diffs[i + 1].text),
                    Operation.DELETE => new TyperaryDiff(DiffOperation.Delete, ithText),
                    _ => throw new NotImplementedException("Fatal Error: Diff -> TyperaryDiff のパターン網羅漏れがあります"),
                });
            }
            return ret;
        }

        private string ConvertDiffsToHtmlString(List<TyperaryDiff> diffs)
        {
            var stringBuilder = ReseekableStringBuilder.AcquirePooledStringBuilder();
            foreach (var diff in diffs)
            {
                var htmlText = diff.operation switch
                {
                    DiffOperation.Equal => equalHtmlStyle.Invoke(diff.input),
                    DiffOperation.Insert => insertHtmlStyle.Invoke(diff.input),
                    DiffOperation.Delete => deleteHtmlStyle.Invoke(diff.input),
                    DiffOperation.Replace => replaceHtmlStyle.Invoke(diff.input, diff.correctString),
                    _ => ""
                };
                stringBuilder.Append(htmlText);
            }

            var text = stringBuilder.ToString();
            var sanitizer = new HtmlSanitizer();
            var ret = sanitizer.Sanitize(text);
            ReseekableStringBuilder.Release(stringBuilder);
            return ret;
        }

        public void SetDiffMarkUpSentence()
        {
            var diffs = GenerateDiff();
            var convertedDiff = ConvertDiffToTyperaryDiff(diffs);
            DiffMarkUpSentence = ConvertDiffsToHtmlString(convertedDiff);
        }
    }
}

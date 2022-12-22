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
            public readonly DiffOperation Operation;
            public readonly ValueString Input;
            public readonly ValueString? CorrectString;
            public readonly int CorrectCount;
            public readonly int WrongCount;

            public TyperaryDiff(DiffOperation op, ValueString inputText, ValueString? correct = null)
            {
                Operation = op;
                Input = inputText;
                CorrectString = correct;
                CorrectCount = op switch
                {
                    DiffOperation.Equal => Input.Length,
                    _ => 0
                };

                WrongCount = op switch
                {
                    DiffOperation.Delete or DiffOperation.Insert => Input.Length,
                    DiffOperation.Replace => Math.Max(Input.Length, CorrectString.Value.Length),
                    _ => 0
                };
            }
        }

        private static diff_match_patch dmp;
        private readonly Func<ValueString, string> insertHtmlStyle = (ValueString text) => $"<ins style=\"background:#e6ffe6; font-weight:bold\">{text}</ins>";
        private readonly Func<ValueString, string> deleteHtmlStyle = (ValueString text) => $"<del style=\"background:#ffe6e6; font-weight:bold\">{text}</del>";
        private readonly Func<ValueString, ValueString?, string> replaceHtmlStyle = (ValueString wrongText, ValueString? collectText) => $"<span style=\"background:#e6e6ff; font-weight:bold\"><del>{wrongText}</del> &rarr; {collectText}</span>";
        private readonly Func<ValueString, string> equalHtmlStyle = (ValueString text) => $"<span style=\"color:#666666\">{text}</span>";
        public int CollectCount { get; private set; } = 0;
        public int WrongCount { get; private set; } = 0;
        public int WrongDeleteCount { get; private set; } = 0;
        public int WrongInsertCount { get; private set; } = 0;
        public int WrongReplaceCount { get; private set; } = 0;

        public string JudgeSentence { init; get; }
        public string InputSentence { init; get; }
        public string DiffMarkUpSentence { get; private set; }

        public PracticeSectionResult(string judgeSentence, string inputSentence)
        {
            dmp ??= new();
            JudgeSentence = judgeSentence;
            InputSentence = inputSentence;
            CollectCount = 0;
            WrongCount = 0;
            WrongDeleteCount = 0;
            WrongInsertCount = 0;
            WrongReplaceCount = 0;
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
                if (ithOperation is Operation.DELETE && isReplace)
                {
                    i++;
                }
            }
            return ret;
        }

        private void SetCorrectAndWrongCounts(List<TyperaryDiff> diffs)
        {
            foreach (var diff in diffs)
            {
                switch (diff.Operation)
                {
                    case DiffOperation.Equal:
                        CollectCount += diff.CorrectCount;
                        break;
                    case DiffOperation.Insert:
                        WrongCount += diff.WrongCount;
                        WrongInsertCount += diff.WrongCount;
                        break;
                    case DiffOperation.Delete:
                        WrongCount += diff.WrongCount;
                        WrongDeleteCount += diff.WrongCount;
                        break;
                    case DiffOperation.Replace:
                        WrongCount += diff.WrongCount;
                        WrongReplaceCount += diff.WrongCount;
                        break;
                }
            }
        }

        private string ConvertDiffsToHtmlString(List<TyperaryDiff> diffs)
        {
            var stringBuilder = ReseekableStringBuilder.AcquirePooledStringBuilder();
            foreach (var diff in diffs)
            {
                var htmlText = diff.Operation switch
                {
                    DiffOperation.Equal => equalHtmlStyle.Invoke(diff.Input),
                    DiffOperation.Insert => insertHtmlStyle.Invoke(diff.Input),
                    DiffOperation.Delete => deleteHtmlStyle.Invoke(diff.Input),
                    DiffOperation.Replace => replaceHtmlStyle.Invoke(diff.Input, diff.CorrectString),
                    _ => ""
                };
                stringBuilder.Append(htmlText);
            }

            var text = stringBuilder.ToString();
            var sanitizer = new HtmlSanitizer();
            var sanitizedText = sanitizer.Sanitize(text);
            var ret = sanitizedText.Replace("\n", "&#x23CE;");
            ReseekableStringBuilder.Release(stringBuilder);
            return ret;
        }

        public void SetDiffMarkUpSentence()
        {
            var diffs = GenerateDiff();
            var convertedDiff = ConvertDiffToTyperaryDiff(diffs);
            SetCorrectAndWrongCounts(convertedDiff);
            DiffMarkUpSentence = ConvertDiffsToHtmlString(convertedDiff);
        }
    }
}

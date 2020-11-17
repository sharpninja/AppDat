using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace AppDat.Core.Entities
{
    public record StringRegexConstraint : IStringConstraint
    {
        public string Pattern { get; init; }

        public bool Evaluate(string value)
        {
            var regex = new Regex(Pattern);

            return regex.IsMatch(value);
        }

        public override string ToString()
        {
            return $"{nameof(StringRegexConstraint)} - Pattern: {Pattern}";
        }
    }
}

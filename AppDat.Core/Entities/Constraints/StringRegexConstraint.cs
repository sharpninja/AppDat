namespace AppDat.Core.Entities
{
    public record StringRegexConstraint : IStringConstraint
    {
        string Pattern;
    }
}

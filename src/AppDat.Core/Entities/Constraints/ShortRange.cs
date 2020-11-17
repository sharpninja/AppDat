namespace AppDat.Core.Entities
{
    public record ShortRange : IFieldRangeConstraint<short>
    {
        public short MinValue { get; init; } = short.MinValue;
        public short MaxValue { get; init; } = short.MaxValue;
    }
}

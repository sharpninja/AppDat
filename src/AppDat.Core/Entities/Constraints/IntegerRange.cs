namespace AppDat.Core.Entities
{
    public record IntegerRange : IFieldRangeConstraint<int>
    {
        public int MinValue { get; init; } = int.MinValue;
        public int MaxValue { get; init; } = int.MaxValue;
    }
}

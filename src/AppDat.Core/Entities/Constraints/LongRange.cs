namespace AppDat.Core.Entities
{
    public record LongRange : IFieldRangeConstraint<long>
    {
        public long MinValue { get; init; } = long.MinValue;
        public long MaxValue { get; init; } = long.MaxValue;
    }
}

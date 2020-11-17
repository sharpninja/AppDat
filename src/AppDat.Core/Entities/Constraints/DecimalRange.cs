namespace AppDat.Core.Entities
{
    public record DecimalRange : IFieldRangeConstraint<decimal>
    {
        public decimal MinValue { get; init; } = decimal.MinValue;
        public decimal MaxValue { get; init; } = decimal.MaxValue;
    }
}

namespace AppDat.Core.Entities
{
    public record DoubleRange : IFieldRangeConstraint<double>
    {
        public double MinValue { get; init; } = double.MinValue;
        public double MaxValue { get; init; } = double.MaxValue;
    }
}

namespace AppDat.Core.Entities
{
    public record FloatRange : IFieldRangeConstraint<float>
    {
        public float MinValue { get; init; } = float.MinValue;
        public float MaxValue { get; init; } = float.MaxValue;
    }
}

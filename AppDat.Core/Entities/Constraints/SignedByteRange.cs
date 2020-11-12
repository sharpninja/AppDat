namespace AppDat.Core.Entities
{
    public record SignedByteRange : IFieldRangeConstraint<sbyte>
    {
        public sbyte MinValue { get; init; } = sbyte.MinValue;
        public sbyte MaxValue { get; init; } = sbyte.MaxValue;
    }
}

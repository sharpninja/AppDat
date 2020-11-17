namespace AppDat.Core.Entities
{
    public record UnsignedShortRange : IFieldRangeConstraint<ushort>
    {
        public ushort MinValue { get; init; } = ushort.MinValue;
        public ushort MaxValue { get; init; } = ushort.MaxValue;
    }
}

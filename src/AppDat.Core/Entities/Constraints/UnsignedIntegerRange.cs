namespace AppDat.Core.Entities
{
    public record UnsignedIntegerRange : IFieldRangeConstraint<uint>
    {
        public uint MinValue { get; init; } = uint.MinValue;
        public uint MaxValue { get; init; } = uint.MaxValue;
    }
}

namespace AppDat.Core.Entities
{
    public record UnsignedLongRange : IFieldRangeConstraint<ulong>
    {
        public ulong MinValue { get; init; } = ulong.MinValue;
        public ulong MaxValue { get; init; } = ulong.MaxValue;
    }
}

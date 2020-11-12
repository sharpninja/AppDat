namespace AppDat.Core.Entities
{
    public record StringLengthConstraint : IStringConstraint
    {
        public uint MinLength { get; init; } = 0;
        public uint MaxLength { get; init; } = uint.MaxValue;
    }
}

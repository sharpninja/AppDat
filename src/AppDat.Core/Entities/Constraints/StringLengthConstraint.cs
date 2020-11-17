namespace AppDat.Core.Entities
{
    public record StringLengthConstraint : IMinMaxLengthConstraint<char[], uint>
    {
        public uint MinLength { get; init; } = 0;
        public uint MaxLength { get; init; } = uint.MaxValue;

        public bool Evaluate(string value)
        {
            return !(value.Length < MinLength ||
                value.Length > MaxLength);
        }

        public override string ToString()
        {
            return $"{nameof(StringLengthConstraint)} - MinLength: {MinLength}, MaxLength: {MaxLength}";
        }
    }
}

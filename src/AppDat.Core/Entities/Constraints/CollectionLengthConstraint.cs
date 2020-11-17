using System.Collections;

namespace AppDat.Core.Entities
{
    public record CollectionLengthConstraint<TCollection> : 
        IMinMaxLengthConstraint<TCollection, int>
        where TCollection : ICollection
    {
        public int MinLength { get; init; } = 0;
        public int MaxLength { get; init; } = int.MaxValue;

        public bool Evaluate(string value)
        {
            return !(value.Length < MinLength ||
                value.Length > MaxLength);
        }

        public override string ToString()
        {
            return $"{nameof(CollectionLengthConstraint<TCollection>)} - MinLength: {MinLength}, MaxLength: {MaxLength}";
        }
    }
}

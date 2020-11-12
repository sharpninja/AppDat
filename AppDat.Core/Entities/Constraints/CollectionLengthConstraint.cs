using System.Collections;

namespace AppDat.Core.Entities
{
    public record CollectionLengthConstraint<TCollection> : IConstraint<TCollection>
        where TCollection : ICollection
    {
        public ulong MinLength { get; init; } = 0;
        public ulong MaxLength { get; init; } = ulong.MaxValue;
    }
}

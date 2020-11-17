using System.Collections;

namespace AppDat.Core.Entities
{
    public interface IMinMaxLengthConstraint<TCollection, TSize> : 
        IConstraint<TCollection> 
        where TCollection : ICollection
    {
        TSize MinLength { get; init; }
        TSize MaxLength { get; init; }
    }
}

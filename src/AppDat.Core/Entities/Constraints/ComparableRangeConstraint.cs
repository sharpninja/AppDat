using System;

namespace AppDat.Core.Entities
{
    public record ComparableRangeConstraint<TType> : IFieldRangeConstraint<TType>
    {
        public TType MinValue { get; init; }
        public TType MaxValue { get; init; }

        public bool Evaluate(IComparable<TType> comparable)
        {
            return !(comparable.CompareTo(MinValue) < 0 || 
                comparable.CompareTo(MaxValue) > 0);
        }

        public override string ToString()
        {
            return $"{nameof(ComparableRangeConstraint<TType>)} - MinValue: {MinValue}, MaxValue: {MaxValue}";
        }
    }
}

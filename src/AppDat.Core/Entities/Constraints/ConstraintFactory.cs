using AppDat.Core.Entities;

using System;
using System.Collections;

namespace AppDat.Rdbms.Core.Entities.Constraints
{
    public static class ConstraintFactory
    {
        public static IFieldRangeConstraint<TType>? GetFieldRangeConstraint<TType>(IComparable<TType> min, IComparable<TType> max)
        {
            return (min, max) switch
            {
                (IComparable<TType> i, IComparable<TType> a) => new ComparableRangeConstraint<TType> { MinValue = (TType)i, MaxValue = (TType)a },
                //(int i, int a) => (IFieldRangeConstraint<TType>)new IntegerRange { MinValue = i, MaxValue = a },
                //(uint i, uint a) => (IFieldRangeConstraint<TType>)new UnsignedIntegerRange { MinValue = i, MaxValue = a },
                //(long i, long a) => (IFieldRangeConstraint<TType>)new LongRange { MinValue = i, MaxValue = a },
                //(ulong i, ulong a) => (IFieldRangeConstraint<TType>)new UnsignedLongRange { MinValue = i, MaxValue = a },
                //(short i, short a) => (IFieldRangeConstraint<TType>)new ShortRange { MinValue = i, MaxValue = a },
                //(ushort i, ushort a) => (IFieldRangeConstraint<TType>)new UnsignedShortRange { MinValue = i, MaxValue = a },
                //(decimal i, decimal a) => (IFieldRangeConstraint<TType>)new DecimalRange { MinValue = i, MaxValue = a },
                //(float i, float a) => (IFieldRangeConstraint<TType>)new FloatRange { MinValue = i, MaxValue = a },
                //(double i, double a) => (IFieldRangeConstraint<TType>)new DoubleRange { MinValue = i, MaxValue = a },
                //(TimeSpan i, TimeSpan a) => (IFieldRangeConstraint<TType>)new TimeSpanRange { MinValue = i, MaxValue = a },
                //(DateTime i, DateTime a) => (IFieldRangeConstraint<TType>)new DateTimeRange { MinValue = i, MaxValue = a },
                //(DateTimeOffset i, DateTimeOffset a) => (IFieldRangeConstraint<TType>)new DateTimeOffsetRange { MinValue = i, MaxValue = a },
                _ => null
            };
        }

        public static IMinMaxLengthConstraint<TType, TSize>? GetMinMaxLengthConstraint<TType, TSize>(TSize min, TSize max)
            where TType : ICollection
        {
            return (default(TType), min, max) switch
            {
                (char[], uint i, uint a) => (IMinMaxLengthConstraint<TType, TSize>)new StringLengthConstraint { MinLength = i, MaxLength = a },
                (ICollection, int i, int a) => (IMinMaxLengthConstraint<TType, TSize>)new CollectionLengthConstraint<TType> { MinLength = i, MaxLength = a },
                _ => null
            };
        }

        public static IStringConstraint? GetStringConstraint(string criteria)
        {
            return new StringRegexConstraint { Pattern = criteria };
        }
    }
}

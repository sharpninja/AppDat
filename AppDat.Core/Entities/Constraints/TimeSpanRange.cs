using System;

namespace AppDat.Core.Entities
{
    public record TimeSpanRange : IFieldRangeConstraint<TimeSpan>
    {
        public TimeSpan MinValue { get; init; } = TimeSpan.MinValue;
        public TimeSpan MaxValue { get; init; } = TimeSpan.MaxValue;
    }
}

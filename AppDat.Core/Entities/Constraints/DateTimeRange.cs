using System;

namespace AppDat.Core.Entities
{
    public record DateTimeRange : IFieldRangeConstraint<DateTime>
    {
        public DateTime MinValue { get; init; } = DateTime.MinValue;
        public DateTime MaxValue { get; init; } = DateTime.MaxValue;
    }
}

using System;

namespace AppDat.Core.Entities
{
    public record DateTimeOffsetRange : IFieldRangeConstraint<DateTimeOffset>
    {
        public DateTimeOffset MinValue { get; init; } = DateTimeOffset.MinValue;
        public DateTimeOffset MaxValue { get; init; } = DateTimeOffset.MaxValue;
    }
}

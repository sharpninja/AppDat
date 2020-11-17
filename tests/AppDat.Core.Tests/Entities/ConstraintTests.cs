using AppDat.Core.Entities;
using AppDat.Rdbms.Core.Entities.Constraints;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace AppDat.Core.Tests.Entities
{
    public class ConstraintTests
    {
        [Theory]
        [InlineData((short)10, (short)0, (short)255, true)]
        [InlineData((short)0, (short)10, (short)255, false)]
        [InlineData((short)0, (short)255, (short)10, false)]
        public void ShortRangeConstraintTests(short value, IComparable<short> min, IComparable<short> max, bool expected)
        {
            var constraint = ConstraintFactory.GetFieldRangeConstraint(min, max);

            Assert.NotNull(constraint);

            var container = new ConstraintContainer<IFieldRangeConstraint<short>, short>(constraint);

            Assert.NotNull(container);
            Assert.NotNull(container.Constraint);
            Assert.Equal(constraint, container.Constraint);

            var result = container.Evaluate(value);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData((ushort)10, (ushort)0, (ushort)255, true)]
        [InlineData((ushort)0, (ushort)10, (ushort)255, false)]
        [InlineData((ushort)0, (ushort)255, (ushort)10, false)]
        public void UshortRangeConstraintTests(ushort value, IComparable<ushort> min, IComparable<ushort> max, bool expected)
        {
            var constraint = ConstraintFactory.GetFieldRangeConstraint(min, max);

            Assert.NotNull(constraint);

            var container = new ConstraintContainer<IFieldRangeConstraint<ushort>, ushort>(constraint);

            Assert.NotNull(container);
            Assert.NotNull(container.Constraint);
            Assert.Equal(constraint, container.Constraint);

            var result = container.Evaluate(value);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10, 0, 255, true)]
        [InlineData(0, 10, 255, false)]
        [InlineData(0, 255, 10, false)]
        public void IntRangeConstraintTests(int value, IComparable<int> min, IComparable<int> max, bool expected)
        {
            var constraint = ConstraintFactory.GetFieldRangeConstraint(min, max);

            Assert.NotNull(constraint);

            var container = new ConstraintContainer<IFieldRangeConstraint<int>, int>(constraint);

            Assert.NotNull(container);
            Assert.NotNull(container.Constraint);
            Assert.Equal(constraint, container.Constraint);

            var result = container.Evaluate(value);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10u, 0u, 255u, true)]
        [InlineData(0u, 10u, 255u, false)]
        [InlineData(0u, 255u, 10u, false)]
        public void UintRangeConstraintTests(uint value, IComparable<uint> min, IComparable<uint> max, bool expected)
        {
            var constraint = ConstraintFactory.GetFieldRangeConstraint(min, max);

            Assert.NotNull(constraint);

            var container = new ConstraintContainer<IFieldRangeConstraint<uint>, uint>(constraint);

            Assert.NotNull(container);
            Assert.NotNull(container.Constraint);
            Assert.Equal(constraint, container.Constraint);

            var result = container.Evaluate(value);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10l, 0l, 255l, true)]
        [InlineData(0l, 10l, 255l, false)]
        [InlineData(0l, 255l, 10l, false)]
        public void LongRangeConstraintTests(long value, IComparable<long> min, IComparable<long> max, bool expected)
        {
            var constraint = ConstraintFactory.GetFieldRangeConstraint(min, max);

            Assert.NotNull(constraint);

            var container = new ConstraintContainer<IFieldRangeConstraint<long>, long>(constraint);

            Assert.NotNull(container);
            Assert.NotNull(container.Constraint);
            Assert.Equal(constraint, container.Constraint);

            var result = container.Evaluate(value);

            Assert.Equal(expected, result);
        }

        public void DecimalRangeConstraintTests(decimal value, IComparable<decimal> min, IComparable<decimal> max, bool expected)
        {
            var constraint = ConstraintFactory.GetFieldRangeConstraint(min, max);

            Assert.NotNull(constraint);

            var container = new ConstraintContainer<IFieldRangeConstraint<decimal>, decimal>(constraint);

            Assert.NotNull(container);
            Assert.NotNull(container.Constraint);
            Assert.Equal(constraint, container.Constraint);

            var result = container.Evaluate(value);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10f, 0f, 255f, true)]
        [InlineData(0f, 10f, 255f, false)]
        [InlineData(0f, 255f, 10f, false)]
        public void FloatRangeConstraintTests(float value, IComparable<float> min, IComparable<float> max, bool expected)
        {
            var constraint = ConstraintFactory.GetFieldRangeConstraint(min, max);

            Assert.NotNull(constraint);

            var container = new ConstraintContainer<IFieldRangeConstraint<float>, float>(constraint);

            Assert.NotNull(container);
            Assert.NotNull(container.Constraint);
            Assert.Equal(constraint, container.Constraint);

            var result = container.Evaluate(value);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10.0, 0.0, 255.0, true)]
        [InlineData(0.0, 10.0, 255.0, false)]
        [InlineData(0.0, 255.0, 10.0, false)]
        public void UlongRangeConstraintTests(double value, IComparable<double> min, IComparable<double> max, bool expected)
        {
            var constraint = ConstraintFactory.GetFieldRangeConstraint(min, max);

            Assert.NotNull(constraint);

            var container = new ConstraintContainer<IFieldRangeConstraint<double>, double>(constraint);

            Assert.NotNull(container);
            Assert.NotNull(container.Constraint);
            Assert.Equal(constraint, container.Constraint);

            var result = container.Evaluate(value);

            Assert.Equal(expected, result);
        }

        [Theory]
        [ClassData(typeof(DateTimeSet))]
        public void DateTimeRangeConstraintTests(DateTime value, IComparable<DateTime> min, IComparable<DateTime> max, bool expected)
        {
            var constraint = ConstraintFactory.GetFieldRangeConstraint(min, max);

            Assert.NotNull(constraint);

            var container = new ConstraintContainer<IFieldRangeConstraint<DateTime>, DateTime>(constraint);

            Assert.NotNull(container);
            Assert.NotNull(container.Constraint);
            Assert.Equal(constraint, container.Constraint);

            var result = container.Evaluate(value);

            Assert.Equal(expected, result);
        }

        [Theory]
        [ClassData(typeof(TimeSpanSet))]
        public void TimeSpanRangeConstraintTests(TimeSpan value, IComparable<TimeSpan> min, IComparable<TimeSpan> max, bool expected)
        {
            var constraint = ConstraintFactory.GetFieldRangeConstraint(min, max);

            Assert.NotNull(constraint);

            var container = new ConstraintContainer<IFieldRangeConstraint<TimeSpan>, TimeSpan>(constraint);

            Assert.NotNull(container);
            Assert.NotNull(container.Constraint);
            Assert.Equal(constraint, container.Constraint);

            var result = container.Evaluate(value);

            Assert.Equal(expected, result);
        }

        [Theory]
        [ClassData(typeof(DateTimeOffsetSet))]
        public void DateTimeOffsetRangeConstraintTests(DateTimeOffset value, IComparable<DateTimeOffset> min, IComparable<DateTimeOffset> max, bool expected)
        {
            var constraint = ConstraintFactory.GetFieldRangeConstraint(min, max);

            Assert.NotNull(constraint);

            var container = new ConstraintContainer<IFieldRangeConstraint<DateTimeOffset>, DateTimeOffset>(constraint);

            Assert.NotNull(container);
            Assert.NotNull(container.Constraint);
            Assert.Equal(constraint, container.Constraint);

            var result = container.Evaluate(value);

            Assert.Equal(expected, result);
        }
    }

    public class TimeSpanSet : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var values = new object[][] {
                new object[] { TimeSpan.FromSeconds(1), TimeSpan.MinValue, TimeSpan.MaxValue, true },
                new object[] { TimeSpan.MinValue, TimeSpan.FromSeconds(1), TimeSpan.MaxValue, false },
                new object[] { TimeSpan.MinValue, TimeSpan.MaxValue, TimeSpan.FromSeconds(1), false },
            };

            foreach (var array in values)
            {
                yield return array;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class DateTimeSet : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var values = new object[][] {
                new object[] { DateTime.Now, DateTime.MinValue, DateTime.MaxValue, true },
                new object[] { DateTime.MinValue, DateTime.Now, DateTime.MaxValue, false },
                new object[] { DateTime.MinValue, DateTime.MaxValue, DateTime.Now, false },
            };

            foreach (var array in values)
            {
                yield return array;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class DateTimeOffsetSet : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var values = new object[][] {
                new object[] { DateTimeOffset.Now, DateTimeOffset.MinValue, DateTimeOffset.MaxValue, true },
                new object[] { DateTimeOffset.MinValue, DateTimeOffset.Now, DateTimeOffset.MaxValue, false },
                new object[] { DateTimeOffset.MinValue, DateTimeOffset.MaxValue, DateTimeOffset.Now, false },
            };

            foreach (var array in values)
            {
                yield return array;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

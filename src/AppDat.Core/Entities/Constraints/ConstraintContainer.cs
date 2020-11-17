using AppDat.Core.Entities;

using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace AppDat.Rdbms.Core.Entities.Constraints
{
    public class ConstraintContainer<TConstraint, TType> : IConstraint<TType>
        where TConstraint : IConstraint<TType>
    {
        private bool? isValid;

        public ConstraintContainer(TConstraint constraint) => Constraint = constraint;

        public TConstraint Constraint { get; init; }

        public bool? IsValid
        {
            get
            {
                if (IsValid is null)
                {
                    throw new NotEvaluatedException();
                }

                return isValid;
            }
            internal set => isValid = value;
        }

        public override string ToString()
        {
            return Constraint?.ToString() ?? "No constraints.";
        }

        public bool Evaluate(TType? value)
        {
            isValid = Constraint switch
            {
                IFieldRangeConstraint<TType> rangeConstraint => ConstraintContainer<TConstraint, TType>.EvaluateNumber(value, rangeConstraint),
                IMinMaxLengthConstraint<ICollection, int> minMaxConstraint => ConstraintContainer<TConstraint, TType>.EvaluateMinMax(value, minMaxConstraint),
                IStringConstraint stringConstraint => EvaluateString(value?.ToString(), stringConstraint),
                _ => null
            };

            return isValid ?? throw new NotEvaluatedException();
        }

        private bool? EvaluateString(string? value, IStringConstraint stringConstraint)
        {
            return value is null ? null : stringConstraint switch
            {
                StringLengthConstraint lengthConstraint => value.Length < lengthConstraint.MaxLength && value.Length > lengthConstraint.MinLength,
                StringRegexConstraint regexConstraint => ConstraintContainer<TConstraint, TType>.EvaluateRegex(value, regexConstraint.Pattern),
                _ => null
            };
        }

        private static bool? EvaluateRegex(string? value, string pattern)
        {
            var regex = new Regex(pattern);

            return value is null ? null : regex.IsMatch(value);
        }

        private static bool? EvaluateMinMax<TSize>(TType? value, IMinMaxLengthConstraint<ICollection, TSize> minMaxConstraint)
        {
            return value is null ? null : value switch
            {
                ICollection c => minMaxConstraint switch {
                    IMinMaxLengthConstraint<ICollection, int> minMax => minMax.MinLength < c.Count && minMax.MaxLength > c.Count,
                    _ => null
                },
                _ => null
            };
        }

        private static bool? EvaluateNumber(TType? value, IFieldRangeConstraint<TType> rangeConstraint)
        {
            return value is null ? null : value switch
            {
                IComparable<TType> i => ((i.CompareTo(rangeConstraint.MinValue) > 0) && (i.CompareTo(rangeConstraint.MaxValue) < 0)),
                //int i => ((rangeConstraint.MinValue is int min && min < i) && (rangeConstraint.MaxValue is int max && max > i)),
                //uint i => ((rangeConstraint.MinValue is uint min && min < i) && (rangeConstraint.MaxValue is uint max && max > i)),
                //long i => ((rangeConstraint.MinValue is long min && min < i) && (rangeConstraint.MaxValue is long max && max > i)),
                //ulong i => ((rangeConstraint.MinValue is ulong min && min < i) && (rangeConstraint.MaxValue is ulong max && max > i)),
                //short i => ((rangeConstraint.MinValue is short min && min < i) && (rangeConstraint.MaxValue is short max && max > i)),
                //ushort i => ((rangeConstraint.MinValue is ushort min && min < i) && (rangeConstraint.MaxValue is ushort max && max > i)),
                //byte i => ((rangeConstraint.MinValue is byte min && min < i) && (rangeConstraint.MaxValue is byte max && max > i)),
                //sbyte i => ((rangeConstraint.MinValue is sbyte min && min < i) && (rangeConstraint.MaxValue is sbyte max && max > i)),
                //decimal i => ((rangeConstraint.MinValue is decimal min && min < i) && (rangeConstraint.MaxValue is decimal max && max > i)),
                //float i => ((rangeConstraint.MinValue is float min && min < i) && (rangeConstraint.MaxValue is float max && max > i)),
                //double i => ((rangeConstraint.MinValue is double min && min < i) && (rangeConstraint.MaxValue is double max && max > i)),
                //TimeSpan i => ((rangeConstraint.MinValue is TimeSpan min && min < i) && (rangeConstraint.MaxValue is TimeSpan max && max > i)),
                //DateTime i => ((rangeConstraint.MinValue is DateTime min && min < i) && (rangeConstraint.MaxValue is DateTime max && max > i)),
                //DateTimeOffset i => ((rangeConstraint.MinValue is DateTimeOffset min && min < i) && (rangeConstraint.MaxValue is DateTimeOffset max && max > i)),
                _ => null
            };
        }
    }
}

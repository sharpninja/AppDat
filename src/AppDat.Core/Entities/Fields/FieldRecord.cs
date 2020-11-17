using AppDat.Rdbms.Core.Entities.Constraints;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization;

namespace AppDat.Core.Entities
{
    [DataContract, Serializable]
    public record FieldRecord<TType>
    {
        [DataMember(Name = "a")]
        public Guid FieldUid { get; init; }
        [DataMember(Name = "b")]
        public string FieldName { get; init; }
        [DataMember(Name = "c")]
        internal string? TypeName
        {
            get => FieldType?.FullName;
            set
            {
                if (value is not null)
                {
                    FieldType = Type.GetType(value);
                }
            }
        }
        [IgnoreDataMember]
        public Type? FieldType { get; private set; } = typeof(TType);
        [DataMember(Name = "d")]
        public bool FieldIsNullable { get; init; } = true;

        [DataMember(Name = "e")]
        public IConstraint<TType>[]? Constraints { get; init; }
        [DataMember(Name = "f")]
        public string? Description { get; init; }
        [DataMember(Name = "g")]
        public string? DisplayName { get; init; }
        
        [IgnoreDataMember]
        public Func<TType?> DefaultValue { get; init; } = () => default(TType);

        public FieldRecord<TTarget> As<TTarget>()
        {
            return new ()
            {
                FieldUid = FieldUid,
                FieldName = FieldName,
                FieldType = FieldType,
                FieldIsNullable = FieldIsNullable,
                Constraints = Constraints?.Cast<IConstraint<TTarget>>()?.ToArray(),
                Description = Description,
                DefaultValue = () => (TTarget)Convert.ChangeType(DefaultValue(), typeof(TTarget))
            };
        }

        public FieldRecord<TType> AddPatternConstraint(string pattern)
        {
            var constraint = ConstraintFactory.GetStringConstraint(pattern);

            if (constraint is not null)
            {
                var constraints = new List<IConstraint<TType>>(Constraints ?? Array.Empty<IConstraint<TType>>())
                {
                    (IConstraint<TType>)constraint
                };

                return this with { Constraints = constraints.ToArray() };
            }

            return this;
        }

        public FieldRecord<TType> AddMinMaxConstraint<TSize>(TSize min, TSize max)
        {
            var constraint = (typeof(TType) == typeof(string)) 
                ? (IConstraint<ICollection>?)ConstraintFactory.GetMinMaxLengthConstraint<char[], TSize>(min, max)
                : ConstraintFactory.GetMinMaxLengthConstraint<ICollection, TSize>(min, max);

            if (constraint is not null)
            {
                var constraints = new List<IConstraint<ICollection>>(
                    Constraints?.Cast<IConstraint<ICollection>>() ?? 
                    Array.Empty<IConstraint<ICollection>>())
                {
                    constraint
                };

                return this with { Constraints = constraints.Cast<IConstraint<TType>>().ToArray() };
            }

            return this;
        }

        public FieldRecord<TType> AddRangeConstraint<TConstraint>(IComparable<TType> min, IComparable<TType> max)
            where TConstraint : IFieldRangeConstraint<TType>            
        {
            var constraint = ConstraintFactory.GetFieldRangeConstraint(min, max);

            if (constraint is not null)
            {
                var constraints = new List<IConstraint<TType>>(Constraints ?? Array.Empty<IConstraint<TType>>())
                {
                    constraint
                };

                return this with { Constraints = constraints.ToArray() };
            }

            return this;
        }
    }
}

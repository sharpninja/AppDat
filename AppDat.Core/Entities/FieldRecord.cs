using System;
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
        public bool FieldIsNullable { get; init; }

        [DataMember(Name = "e")]
        public IConstraint<TType>[]? Constraints { get; init; }
        [DataMember(Name = "f")]
        public string? Description { get; init; }
        [DataMember(Name = "g")]
        public string? DisplayName { get; init; }
        
        [IgnoreDataMember]
        public Func<TType?> DefaultValue { get; init; } = () => default;

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

        public static implicit operator FieldRecord<TType>(FieldRecord<object?> d) => d;
        public static explicit operator FieldRecord<object?>(FieldRecord<TType> b) => (FieldRecord<object?>)b;
    }
}

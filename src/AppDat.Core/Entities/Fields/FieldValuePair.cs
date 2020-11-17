using System;
using System.Runtime.Serialization;

namespace AppDat.Core.Entities
{
    [DataContract, Serializable]
    public record FieldValuePair<TType>
    {
        public FieldValuePair(TType? value, FieldRecord<TType> fieldRecord)
        {
            Value = value;
            Definition = fieldRecord;
        }

        [DataMember(Name = "a")]
        public FieldRecord<TType> Definition { get; init; }

        [DataMember(Name = "b")]
        public TType? Value { get; init; }

        public FieldValuePair<TTarget> As<TTarget>()
        {
            object? value = Value;
            return new((TTarget)value, Definition.As<TTarget>());
        }
    }
}

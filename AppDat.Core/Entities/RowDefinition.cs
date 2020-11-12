using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AppDat.Core.Entities
{
    [DataContract, Serializable]
    public record RowDefinition
    {
        [DataMember(Name = "a")]
        public FieldSet PrimaryKey { get; init; }
        [DataMember(Name = "b")]
        public FieldSet Fields { get; init; }
    }
}

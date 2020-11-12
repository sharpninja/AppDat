using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace AppDat.Core.Entities
{
    [DataContract, Serializable]
    public record FieldSet : IEnumerable<FieldRecord<object?>>
    {
        public FieldSet(params FieldRecord<object?>[] fields)
        {
            Fields = fields;
        }

        [JsonConstructor]
        public FieldSet(IEnumerable<FieldRecord<object?>> fields)
        {
            Fields = fields.ToArray();
        }

        public static FieldSet Default => new()
        {
            Fields = new []
                    {
                        (new FieldRecord<Guid>
                        {
                            DisplayName = "Primary Key",
                            FieldIsNullable = false,
                            FieldUid = Guid.NewGuid(),
                            FieldName = "PrimaryKey",
                            DefaultValue = () => Guid.NewGuid()
                        }).As<object?>()
                    }
        };

        [DataMember(Name = "a")]
        public FieldRecord<object?>[] Fields { get; init; }

        public IEnumerator<FieldRecord<object?>> GetEnumerator()
        {
            return Fields.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

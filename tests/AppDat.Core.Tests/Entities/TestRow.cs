using AppDat.Core.Entities;

using Newtonsoft.Json;

using System;
using System.Collections.Immutable;
using System.Runtime.Serialization;

namespace AppDat.Core.Tests.Entities
{
    [Serializable]
    public class TestRow : Row
    {
        [JsonConstructor]
        public TestRow(Guid rowUid, FieldSet primaryKey, IImmutableDictionary<Guid, RowField<object?>> fields)
            : base(rowUid, primaryKey, fields)
        {
        }

        public TestRow(RowDefinition rowDefinition) : base(rowDefinition)
        {
        }

        public TestRow(FieldSet primaryKey, ValueSet values) : base(primaryKey, values)
        {
        }

        public TestRow(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static TestRow CreateWithError()
        {
            FieldRecord<Guid> keyRecord = new() { FieldUid = Guid.NewGuid(), FieldName = "KeyUid", DisplayName = "Key", FieldIsNullable = false };
            FieldRecord<Guid> keyRecord1 = new() { FieldUid = Guid.NewGuid(), FieldName = "KeyUid1", DisplayName = "Key1", FieldIsNullable = false };
            FieldRecord<string> stringRecord = new() { FieldUid = Guid.NewGuid(), FieldName = "StringValue", DisplayName = "String Value" };
            FieldSet primaryKey = new(keyRecord.As<object?>(), keyRecord1.As<object?>());
            FieldValuePair<Guid> keyField = new(Guid.NewGuid(), keyRecord);
            FieldValuePair<string> stringField = new(nameof(TestRow), stringRecord);
            var keyObject = keyField.As<object?>();
            var stringObject = stringField.As<object?>();
            ValueSet values = new(keyObject, stringObject);
            return new(primaryKey, values);
        }

        public static TestRow CreateWithoutError()
        {
            FieldRecord<Guid> keyRecord = new() { FieldUid = Guid.NewGuid(), FieldName = "KeyUid", DisplayName = "Key", FieldIsNullable = false };
            FieldRecord<string> stringRecord = new() { FieldUid = Guid.NewGuid(), FieldName = "StringValue", DisplayName = "String Value" };
            FieldSet primaryKey = new(keyRecord.As<object?>());
            FieldValuePair<Guid> keyField = new(Guid.NewGuid(), keyRecord);
            FieldValuePair<string> stringField = new(nameof(TestRow), stringRecord);
            var keyObject = keyField.As<object?>();
            var stringObject = stringField.As<object?>();
            ValueSet values = new(keyObject, stringObject);
            return new(primaryKey, values);
        }
    }
}

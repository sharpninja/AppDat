using AppDat.Core.Entities;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Xunit;

namespace AppDat.Core.Tests.Entities
{
    public class RowTests
    {
        private static FieldRecord<Guid> KeyField => 
            new()
            { FieldUid = Guid.NewGuid(), FieldName = "KeyUid", DisplayName = "Key" };


        [Fact]
        public void GetObjectData_StateUnderTest_ExpectedBehavior()
        {
            TestRow? deserialized = null;
            using var row = TestRow.Create();
            var stream = new MemoryStream();
            //var stream = new StringBuilder();
            //var sw = new StringWriter(stream);

            //using (JsonTextWriter writer = new (sw))
            using (BsonWriter writer = new(stream))
            {
                JsonSerializer serializer = new ();
                serializer.Serialize(writer, row);

                Assert.True(stream.Length > 0);

                stream.Seek(0, SeekOrigin.Begin);

                //var json = stream.ToString();

                //using (JsonTextReader reader = new(new StringReader(json)))
                using (BsonReader reader = new(stream))
                {
                    deserialized = serializer.Deserialize<TestRow>(reader);
                }
            }

            // Assert
            Assert.NotNull(deserialized);
            Assert.Equal(row.RowUid, deserialized.RowUid);
        }

        [Fact]
        public void Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            using var row = TestRow.Create();
            var key = row.GetFieldUid("StringValue");
            var newValue = nameof(Row);

            row.Set(key, newValue);

            // Act
            var result = row.Get<string?>(key);

            // Assert
            Assert.Equal(newValue, result);
        }

        [Fact]
        public void Set_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            using var row = TestRow.Create();

            var key = row.GetFieldUid("StringValue");
            var newValue = nameof(Row);

            // Act
            var result = row.Set(
                key,
                newValue);

            // Assert
            Assert.Equal(newValue, result);
        }

        [Fact]
        public void Validate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            using var row = TestRow.Create();

            // Act
            var result = row.Validate();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateBlank_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            RowDefinition rowDefinition = new();

            // Act
            using var result = Row.CreateBlank(rowDefinition);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            FieldSet primaryKey = new();
            ValueSet values = new();

            // Act
            using var result = Row.Create(
                primaryKey,
                values);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Dispose_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            using var row = TestRow.Create();

            // Act
            row.Dispose();

            // Assert
            Assert.True(row.DisposedValue);
        }
    }

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

        public static TestRow Create()
        {
            var keyRecord = new FieldRecord<Guid> { FieldUid = Guid.Empty, FieldName = "KeyUid", DisplayName = "Key" };
            var stringRecord = new FieldRecord<string> { FieldUid = Guid.NewGuid(), FieldName = "StringValue", DisplayName = "String Value" };
            FieldSet primaryKey = new(keyRecord.As<object?>());
            var keyField = new FieldValuePair<Guid>(Guid.NewGuid(), keyRecord);
            var stringField = new FieldValuePair<string>(nameof(TestRow), stringRecord);
            var keyObject = keyField.As<object?>();
            var stringObject = stringField.As<object?>();
            ValueSet values = new(keyObject, stringObject);
            return new(primaryKey, values);
        }
    }
}

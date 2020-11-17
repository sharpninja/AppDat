using AppDat.Core.Entities;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

using System;
using System.IO;
using System.Linq;
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
        public void Serialize_ExpectedBehavior()
        {
            TestRow? deserialized = null;
            using var row = TestRow.CreateWithoutError();
            var stream = new MemoryStream();

            using (BsonWriter writer = new(stream))
            {
                JsonSerializer serializer = new();
                serializer.Serialize(writer, row);

                Assert.True(stream.Length > 0);

                stream.Seek(0, SeekOrigin.Begin);

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
        public void GetFieldValue_ExpectedBehavior()
        {
            // Arrange
            using var row = TestRow.CreateWithoutError();
            var key = row.GetFieldUid("StringValue");
            var newValue = nameof(Row);

            row.Set(key, newValue);

            // Act
            var result = row.Get<string?>(key);

            // Assert
            Assert.Equal(newValue, result);
        }

        [Fact]
        public void SetFieldValue_ExpectedBehavior()
        {
            // Arrange
            using var row = TestRow.CreateWithoutError();

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
        public void ValidateRowWithError_ExpectedBehavior()
        {
            // Arrange
            using var row = TestRow.CreateWithError();

            // Act
            var result = row.Validate();

            // Assert
            Assert.NotNull(result);

            // Expecing Validation Errors.
            Assert.NotEmpty(result);
        }

        [Fact]
        public void ValidateRowWithoutError_ExpectedBehavior()
        {
            // Arrange
            using var row = TestRow.CreateWithoutError();

            // Act
            var result = row.Validate();

            // Assert
            Assert.NotNull(result);

            // Not Expecing Validation Errors.
            Assert.Empty(result);
        }

        [Fact]
        public void CreateBlankRow_ExpectedBehavior()
        {
            // Arrange
            RowDefinition rowDefinition = new();

            // Act
            using var result = Row.CreateBlank(rowDefinition);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateRow_ExpectedBehavior()
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
        public void DisposeRow_ExpectedBehavior()
        {
            // Arrange
            using var row = TestRow.CreateWithoutError();

            // Act
            row.Dispose();

            // Assert
            Assert.True(row.DisposedValue);
        }
    }
}

using System;

namespace AppDat.Core.Entities
{
    [Serializable]
    public record FieldValidationError
    {
        public string Message { get; init; }
    }
}

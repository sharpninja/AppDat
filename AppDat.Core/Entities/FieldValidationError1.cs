using System;

namespace AppDat.Core.Entities
{
    [Serializable]
    public record FieldValidationError<TType> : FieldValidationError
    {
        public FieldValuePair<TType> FieldValue { get; set; }

        private FieldValidationError(FieldValuePair<TType> pair, string message)
        {
            FieldValue = pair;
            Message = message;
        }

        public static FieldValidationError<TType> Create(FieldValuePair<TType> pair, string message) => new(pair, message);
    }
}

using System;
using System.Collections.Generic;

namespace AppDat.Core.Entities
{
    internal static class RecordFactory
    {
        internal static FieldRecord<TType> GetFieldRecord<TType>(Guid fieldUid, string fieldName, bool fieldIsNullable, 
            IConstraint<TType>[]? constraints, string? description, string? displayName)
        {
            return new() 
            {
                FieldUid = fieldUid,
                FieldName = fieldName,
                FieldIsNullable = fieldIsNullable,
                Constraints = constraints,
                Description = description,
                DisplayName = displayName
            };
        }
    }
}

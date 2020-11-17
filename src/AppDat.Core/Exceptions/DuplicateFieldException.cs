using System;

namespace AppDat.Core.Entities
{
    [Serializable]
    internal class DuplicateFieldException : Exception
    {
        public DuplicateFieldException(RowDefinition rowDefinition)
        {
            RowDefinition = rowDefinition;
        }

        public DuplicateFieldException(ValueSet values)
        {
            Values = values;
        }

        public ValueSet Values { get; }
        public RowDefinition RowDefinition { get; }
    }
}

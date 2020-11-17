using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AppDat.Core.Entities
{
    public record ValueSet : IEnumerable<FieldValuePair<object?>>
    {
        public ValueSet(params FieldValuePair<object?>[] fieldValues)
        {
            Fields = fieldValues;
        }

        public FieldValuePair<object?>[] Fields { get; init; }

        public IEnumerator<FieldValuePair<object?>> GetEnumerator()
        {
            return Fields.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

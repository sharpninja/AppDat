using System;

namespace AppDat.Core.Entities
{
    [Serializable]
    internal class ChangeAbortedException<TType> : Exception
    {
        internal ChangeAbortedException(RowField<TType> field)
        {
            Field = field;
        }

        public RowField<TType> Field { get; }
    }
}

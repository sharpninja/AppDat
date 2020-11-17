
using System;

namespace AppDat.Rdbms.Core.Entities.Constraints
{
    [Serializable]
    internal class NotEvaluatedException : Exception
    {
        public NotEvaluatedException()
        {
        }

        public NotEvaluatedException(string? message) : base(message)
        {
        }
    }
}

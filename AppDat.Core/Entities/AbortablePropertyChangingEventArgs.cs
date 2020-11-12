using System;
using System.ComponentModel;

namespace AppDat.Core.Entities
{
    [Serializable]
    public class AbortablePropertyChangingEventArgs : PropertyChangingEventArgs
    {
        public AbortablePropertyChangingEventArgs(string? propertyName) : base(propertyName)
        {
        }

        public bool Abort { get; set; } = false;
    }
}

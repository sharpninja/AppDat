using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AppDat.Core.Entities
{
    [DataContract, Serializable]
    public class RowField<TType> : 
        INotifyPropertyChanged, 
        INotifyPropertyChanging, 
        ISerializable
    {
        private ConcurrentStack<TType?> _history = new();

        public RowField(FieldRecord<TType> record)
        {
            Pair = new(default, record);
        }

        public RowField(FieldValuePair<TType> pair)
        {
            Pair = pair;
        }

        protected RowField(SerializationInfo info, StreamingContext context)
        {
            var value = info.GetValue(nameof(Pair), typeof(FieldValuePair<TType>));
            
            Pair = (FieldValuePair<TType>)(value ?? 
                throw new SerializationException($"Error deserializing `RowField<{typeof(TType).FullName}>`"));

            var history = info.GetValue(nameof(History), typeof(ImmutableArray<TType?>));
            if (history is ImmutableArray<TType?> array)
            {
                _history = new ConcurrentStack<TType?>(array);
            }
        }

        internal RowField(
            Guid fieldUid,
            string fieldName,
            bool fieldIsNullable,
            IConstraint<TType>[]? constraints,
            string? description,
            string? displayName)
        {
            FieldRecord<TType> record = 
                RecordFactory.GetFieldRecord<TType>(
                    fieldUid,
                    fieldName,
                    fieldIsNullable,
                    constraints,
                    description,
                    displayName);

            Pair = new(default, record);
        }

        internal RowField(
            TType value,
            Guid fieldUid,
            string fieldName,
            bool fieldIsNullable,
            IConstraint<TType>[]? constraints,
            string? description,
            string? displayName)
        {
            FieldRecord<TType> record = 
                RecordFactory.GetFieldRecord<TType>(
                    fieldUid,
                    fieldName,
                    fieldIsNullable,
                    constraints,
                    description,
                    displayName);

            Pair = new(value, record);
        }

        [IgnoreDataMember]
        public TType? Value { 
            get => Pair.Value;
            set
            {
                var v = Pair.Value;

                if (value?.Equals(v) ?? (v is null)) return;

                SetValue(Value, true);
            }
        }

        [DataMember(Name = "a")]
        internal FieldValuePair<TType> Pair { get; private set; }

        [IgnoreDataMember]
        public FieldRecord<TType> Definition => Pair.Definition;

        [DataMember(Name = "b")]
        internal TType?[] History
        {
            get => _history.ToArray();
            init
            {
                _history.PushRange(value);
            }
        }

        [IgnoreDataMember]
        internal bool CanUndo => _history.Count > 0;

        internal TType? Undo()
        {
            var value = _history.TryPop(out TType? v) 
                ? v 
                : throw new ApplicationException("Error getting value from History.");

            SetValue(value, false);

            return value;
        }

        private void SetValue(TType? value, bool track = true)
        {
            if (Changing(nameof(Value)).Abort)
            {
                throw new ChangeAbortedException<TType>(this);
            }

            if (track) _history.Push(value);

            Pair = Pair with { Value = value };

            Changed(nameof(Value));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Pair), Pair);
            info.AddValue(nameof(History), History);
        }

        public FieldValidationError[] Validate()
        {
            return Array.Empty<FieldValidationError>();
        }

        private void Changed([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new (name));
        }

        private AbortablePropertyChangingEventArgs Changing([CallerMemberName] string? name = null)
        {
            var args = new AbortablePropertyChangingEventArgs(name);

            PropertyChanging?.Invoke(this, args);

            return args;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        internal RowField<TTarget> As<TTarget>()
        {
            return new(Pair.As<TTarget>());
        }
    }
}

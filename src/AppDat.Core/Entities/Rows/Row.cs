using Newtonsoft.Json;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AppDat.Core.Entities
{
    [CollectionDataContract, Serializable]
    public class Row :
        INotifyPropertyChanged,
        INotifyPropertyChanging,
        ISerializable,
        IDisposable
    {
        private readonly ConcurrentDictionary<Guid, RowField<object?>> _dictionary;
        private bool _disposedValue;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        internal protected Row(Guid rowUid, FieldSet primaryKey, IImmutableDictionary<Guid, RowField<object?>> fields)
        {
            RowUid = rowUid;
            PrimaryKey = primaryKey;

            _dictionary = new(fields);
        }

        internal protected Row(RowDefinition rowDefinition)
        {
            PrimaryKey = rowDefinition.PrimaryKey ?? FieldSet.Default;

            _dictionary = new ();

            if (rowDefinition.Fields is not null &&
                rowDefinition.Fields.Fields.Length > 0)
            {
                foreach (var field in rowDefinition.Fields)
                {
                    var rowField = CreateRowField(field);

                    if (!_dictionary.TryAdd(field.FieldUid, rowField))
                    {
                        throw new DuplicateFieldException(rowDefinition);
                    }

                    rowField.PropertyChanging += RowField_PropertyChanging;
                    rowField.PropertyChanged += RowField_PropertyChanged;
                }
            }
        }

        internal protected Row(FieldSet primaryKey, ValueSet values)
        {
            PrimaryKey = primaryKey ?? FieldSet.Default;

            _dictionary = new ();

            if (values is not null &&
                values.Fields.Length > 0)
            {
                foreach (var value in values)
                {
                    var rowField = CreateRowField(value);

                    if (!_dictionary.TryAdd(value.Definition.FieldUid, rowField))
                    {
                        throw new DuplicateFieldException(values);
                    }

                    rowField.PropertyChanging += RowField_PropertyChanging;
                    rowField.PropertyChanged += RowField_PropertyChanged;
                }
            }
        }

        public Row(SerializationInfo info, StreamingContext context)
        {
            var uid = info.GetValue(nameof(RowUid), typeof(Guid));

            if(uid is Guid guid)
            {
                RowUid = guid;
            }
            else
            {
                throw new SerializationException("Missing `RowUid` in `Row`.");
            }

            var pk = info.GetValue(nameof(PrimaryKey), typeof(FieldSet));

            if (pk is FieldSet primaryKey)
            {
                PrimaryKey = primaryKey;
            }
            else
            {
                throw new SerializationException("Missing `PrimaryKey` in `Row`.");
            }

            var values = info.GetValue(nameof(_dictionary), typeof(KeyValuePair<Guid, RowField<object?>>[]));

            if (values is KeyValuePair<Guid, RowField<object?>>[] dictionary)
            {
                _dictionary = new (dictionary.ToDictionary(pair => pair.Key, pair => pair.Value));

                foreach (var value in _dictionary.Values)
                {
                    value.PropertyChanging += RowField_PropertyChanging;
                    value.PropertyChanged += RowField_PropertyChanged;
                }
            }
            else
            {
                throw new SerializationException("Missing data in `Row`.");
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (DisposedValue)
            {
                throw new ObjectDisposedException(nameof(Row));
            }

            info.AddValue(nameof(RowUid), RowUid);
            info.AddValue(nameof(PrimaryKey), PrimaryKey);
            info.AddValue(nameof(_dictionary), _dictionary.ToArray());
        }

        private static RowField<TType> CreateRowField<TType>(FieldRecord<TType> fieldRecord) => new(fieldRecord);
        private static RowField<TType> CreateRowField<TType>(FieldValuePair<TType> fieldPair) => new(fieldPair);

        [DataMember(Name = "RowUid")]
        public Guid RowUid { get; } = Guid.NewGuid();

        [DataMember(Name = "PrimaryKey")]
        public FieldSet PrimaryKey { get; protected set; }

        [IgnoreDataMember]
        public IImmutableDictionary<Guid, RowField<object?>> Fields
        {
            get => _dictionary.ToImmutableDictionary();
        }

        public TType? Get<TType>(Guid key)
        {
            if (_dictionary.TryGetValue(key, out var value))
            {
                return (TType?)value.Pair.Value;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public TType? Set<TType>(Guid key, TType? newValue)
        {
            if (_dictionary.ContainsKey(key))
            {
                var pair = _dictionary.TryGetValue(key, out var value) 
                    ? value.Pair 
                    : default;

                if(pair?.As<TType>() is FieldValuePair<TType> fieldPair)
                {
                    value = CreateRowField(fieldPair with { Value = newValue }).As<object?>();

                    _dictionary[key] = value;

                    return value.Pair.As<TType?>().Value;
                }
            }
            
            throw new KeyNotFoundException();
        }

        public Guid GetFieldUid(string fieldName)
            => Fields.FirstOrDefault(field => field.Value.Definition.FieldName == fieldName).Key;

        [IgnoreDataMember]
        public bool DisposedValue => _disposedValue;

        public IDictionary<Guid, FieldValidationError[]> Validate()
        {
            if (DisposedValue)
            {
                throw new ObjectDisposedException(nameof(Row));
            }

            var result = new ConcurrentDictionary<Guid, FieldValidationError[]>();

            foreach(var field in PrimaryKey)
            {
                if (!_dictionary.ContainsKey(field.FieldUid))
                {
                    var existing = result.TryGetValue(field.FieldUid, out var value) 
                        ? value.ToList() 
                        : new List<FieldValidationError>();

                    existing.Add(new FieldValidationError
                    {
                        Message = $"PrimaryKey Field [{field.FieldName}] not in Row."
                    });

                    result.AddOrUpdate(
                        field.FieldUid, 
                        existing.ToArray(), 
                        (k, value) => existing.ToArray());
                } 
                else
                {
                    var keyValue = _dictionary.TryGetValue(field.FieldUid, out var value) ? value : null;

                    if(keyValue is null || keyValue.Pair.Value is null)
                    {
                        FieldValidationError error = new()
                        {
                            Message = $"Primary Key value [{keyValue.Pair.Definition.FieldName}] is null."
                        };

                        result.AddOrUpdate(
                            field.FieldUid,
                            new[] { error },
                            (k, v) => new[] { error });
                    }
                }
            }

            foreach (var rowField in _dictionary.Values.ToList())
            {
                var fieldResult = rowField.Validate();

                if (fieldResult.Length > 0)
                {
                    var existing = result.TryGetValue(rowField.Definition.FieldUid, out var value)
                        ? value.ToList()
                        : new List<FieldValidationError>();

                    existing.AddRange(fieldResult);

                    result.AddOrUpdate(
                        rowField.Definition.FieldUid,
                        existing.ToArray(),
                        (k, v) => existing.ToArray());
                }
            }

            return result;
        }

        public static Row CreateBlank(RowDefinition rowDefinition)
        {
            return new(rowDefinition);
        }

        public static Row Create(FieldSet primaryKey, ValueSet values)
        {
            return new(primaryKey, values);
        }

        private void RowField_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        private void RowField_PropertyChanging(object? sender, PropertyChangingEventArgs e)
        {
            PropertyChanging?.Invoke(sender, e);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!DisposedValue)
            {
                if (disposing)
                {
                    foreach (var field in _dictionary.Values)
                    {
                        field.PropertyChanged -= RowField_PropertyChanged;
                        field.PropertyChanging -= RowField_PropertyChanging;
                    }

                    _dictionary.Clear();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

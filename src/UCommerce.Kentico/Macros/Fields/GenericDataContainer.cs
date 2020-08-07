using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CMS.Base;
using UCommerce.EntitiesV2;

namespace UCommerce.Kentico.Macros.Fields
{
    public class GenericDataContainer<T> : IDataContainer where T : class
    {
        protected T Source;
        private HashSet<string> _columnNamesHashSet;

        public GenericDataContainer(T source)
        {
            Source = source;
        }

        public object GetValue(string columnName)
        {
            object value;
            TryGetValue(columnName, out value);
            return value;
        }

        public bool SetValue(string columnName, object value)
        {
            throw new NotImplementedException();
        }

        public object this[string columnName]
        {
            get { return GetValue(columnName); }
            set { SetValue(columnName, value); }
        }

        public bool TryGetValue(string columnName, out object value)
        {
            // Use reflection, to find the property with the given name, on the type of the source.
            var propertyInfo = typeof(T).GetProperty(columnName);
            if (propertyInfo == null)
            {
                value = null;
                return false;
            }

            // Use reflection to get the value of the Property on the Source object.
            value = WrapValueInDataContainer(propertyInfo.GetValue(Source));
            return true;
        }

        public bool ContainsColumn(string columnName)
        {
            return ColumnNamesHashSet.Contains(columnName);
        }

        public List<string> ColumnNames
        {
            get { return ColumnNamesHashSet.AsEnumerable().ToList(); }
        }

        protected HashSet<string> ColumnNamesHashSet
        {
            get {
                return _columnNamesHashSet ?? (_columnNamesHashSet = new HashSet<string>(typeof(T).GetProperties()
                    .Where(x => x.GetIndexParameters().Length == 0) // Skip the "this[]" indexer.
                    .Select(x => x.Name)));
            }
        }

        protected object WrapValueInDataContainer(object val)
        {
            if (val is IEntity)
            {
                // Value was a Ucommerce entity. So wrapping is needed.
                Type typeToConstruct = typeof(GenericDataContainer<>).MakeGenericType(val.GetType());
                ConstructorInfo constructor = typeToConstruct.GetConstructor(new[] { val.GetType() });
                var wrappedValue = constructor.Invoke(new[] { val });

                return wrappedValue;
            }

            if (val is IEnumerable<IEntity> asCollectionOfEntities)
            {
                // Value was an enumerable of Ucommerce entities. So wrapping is needed for the individual entities.
                var wrappedCollection = new List<object>();
                foreach (var entity in asCollectionOfEntities)
                {
                    wrappedCollection.Add(WrapValueInDataContainer(entity));
                }
                return wrappedCollection;
            }

            // No wrapping necessary.
            return val;
        }
    }
}

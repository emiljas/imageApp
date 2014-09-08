using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace ImageApp.UnitTests.Mock
{
    public class PropertySetMock : IPropertySet
    {
        public event MapChangedEventHandler<string, object> MapChanged;

        private Dictionary<string, object> dictionary = new Dictionary<string,object>();

        public void Add(string key, object value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out object value)
        {
            throw new NotImplementedException();
        }

        public ICollection<object> Values
        {
            get { throw new NotImplementedException(); }
        }

        public object this[string key]
        {
            get
            {
                object value;
                dictionary.TryGetValue(key, out value);
                return value;
            }
            set
            {
                dictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}

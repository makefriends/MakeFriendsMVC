using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MakeFriends.Common.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DBFieldAttribute : Attribute
    {
        public bool isKey
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int MaxLength
        {
            get;
            set;
        }

        public bool IsIdentity
        {
            get;
            set;
        }

        public bool IsSearchField
        {
            get;
            set;
        }
        
        public DBFieldAttribute(string name) : this(name, int.MaxValue, false , false ,false ) { }

        public DBFieldAttribute(string name, bool isKey, bool isIdentity) : this(name, int.MaxValue, isKey, isIdentity , false) { }

        public DBFieldAttribute(string name, bool isKey, bool isIdentity, bool isForeign) : this(name, int.MaxValue, isKey, isIdentity, isForeign) { }

        public DBFieldAttribute(string name, int maxLength, bool isKey, bool isIdentity , bool isForeign)            
        {
            this.Name = name;
            this.MaxLength = maxLength;
            this.isKey = isKey;
            this.IsIdentity = isIdentity;
            this.IsSearchField = isForeign;
        }

        //public DBFieldAttribute(string name, int maxLength, string function)
        //{
        //    this.Name = name;
        //    this.MaxLength = maxLength;
        //}

        public object GetDataBaseValue(PropertyInfo info, object instance)
        {
            object res = info.GetValue(instance, null);

            if (res == null)
            {
                return null;
            }

            if (info.PropertyType == typeof(string))
            {
                if (Encoding.UTF8.GetByteCount(res.ToString()) > this.MaxLength)
                {
                    ArraySegment<byte> segment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(res.ToString()), 0, this.MaxLength - 1);
                    return Encoding.UTF8.GetString(segment.Array.Take(segment.Count).ToArray<byte>());
                }

                return res;
            }

            return res;
        }
    }
}

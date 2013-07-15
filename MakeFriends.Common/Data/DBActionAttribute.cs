using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MakeFriends.Common.Data
{
    [Flags()]
    public enum DBActionType
    {
        Create = 1,
        Save = 2,
        Get = 4,
        Delete = 8
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DBActionAttribute : Attribute
    {
        public DBActionType Type { get; set; }
        public string Name { get; set; }

        public DBActionAttribute(string name, DBActionType type)
            : base()
        {
            this.Name = name;
            this.Type = type;
        }

        public DBActionAttribute(string name)
            : this(name, DBActionType.Create | DBActionType.Delete | DBActionType.Get | DBActionType.Save) { }
    }
}

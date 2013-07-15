using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MakeFriends.Common
{
    public class LookUps
    {
        static DateTime lastUpdate = DateTime.Now;
        static Dictionary<string, IEntityCollection> m_libraries = new Dictionary<string, IEntityCollection>();
        static object m_locker = new object();

        //LookUps() { }

        //static LookUps()
        //{
        //    m_libraries 
        //    m_locker
        //}

        public static T Get<T>(string name)
            where T : IEntityCollection, new()
        {
            lock (m_locker)
            {
                if (DateTime.Compare(lastUpdate.AddDays(1), DateTime.Now) < 0)
                {
                    m_libraries.Clear();
                    lastUpdate = DateTime.Now;
                }
                if (!m_libraries.ContainsKey(name))
                {
                    T library = new T();
                    library.GetAll();
                    m_libraries.Add(name, library);
                }

                return (T)m_libraries[name];
            }
        }

        public static T Get<T>()
            where T : IEntityCollection, new()
        {
            return LookUps.Get<T>(typeof(T).FullName);
        }

        public static bool Remove<T>()
            where T : IEntityCollection, new()
        {

            string key = typeof(T).FullName;
            bool result = false;

            lock (m_locker)
            {
                if (m_libraries.ContainsKey(key))
                {
                    result = m_libraries.Remove(key);
                }
            }

            return result;
        }

        public static void Reset<T>()
            where T : IEntityCollection, new()
        {
            Remove<T>();
            Get<T>();
        }
    }
}

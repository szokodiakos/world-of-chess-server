using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chessgine
{
    public static class Extensions
    {
        public static void AddIfNotNull<T>(this List<T> list, T item)
        {
            if (item != null)
            {
                list.Add(item);
            }
        }

        public static void AddIfNotNull<T>(this List<T> list, List<T> otherList)
        {
            foreach (var item in otherList)
            {
                list.AddIfNotNull(item);
            }
        }

        public static List<T> SectionWith<T>(this List<T> list, List<T> otherList)
        {
            List<T> section = new List<T>();

            foreach (var item in list)
            {
                foreach (var otherItem in otherList)
                {
                    if (item.Equals(otherItem))
                    {
                        section.Add(item);
                    }
                }
            }

            return section;
        }
    }
}

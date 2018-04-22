using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    public static class Extensions
    {
        public static T GetValue<T>(this SerializationInfo info, string fieldName)
        {
            return (T)info.GetValue(fieldName, typeof(T));
        }
    }
}

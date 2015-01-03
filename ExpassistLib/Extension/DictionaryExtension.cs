using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpassistLib.Extension
{
    public static class DictionaryExtension
    {
        public static Dictionary<S, T> ToDic<S, T>(this IEnumerable<KeyValuePair<S, T>> input)
        {
            Dictionary<S, T> dic = new Dictionary<S, T>();
            foreach (var kv in input)
                if (!dic.ContainsKey(kv.Key))
                    dic.Add(kv.Key, kv.Value);

            return dic;
        }
    }
}

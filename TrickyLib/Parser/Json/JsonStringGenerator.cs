using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TrickyLib.Parser.Json
{
    /// <summary>
    /// Represents either a JsonObject or JsonArray
    /// </summary>
    class JsonStringLevel {
        public IEnumerator enumerator;
        /// <summary>
        /// Whether this level has a value or is just an empty container
        /// </summary>
        public bool HasValue;
    }
    class JsonStringGenerator {
        public static string GetJsonString(JsonStringLevel tsl,string newline, string indent) {
            StringBuilder sb = new StringBuilder();
            Stack<JsonStringLevel> stack = new Stack<JsonStringLevel>();

            if (tsl.enumerator is List<object>.Enumerator) sb.Append('[');
            else sb.Append('{');
            stack.Push(tsl);

            while (stack.Count > 0) {
                if (!stack.Peek().enumerator.MoveNext()) {
                    sb.Append(newline);

                    for (int i = 1; i < stack.Count; i++) {
                        sb.Append(indent);
                    }

                    if (stack.Pop().enumerator is List<object>.Enumerator) sb.Append(']');
                    else sb.Append('}');

                    continue;
                }
                if (stack.Peek().HasValue) sb.Append(',');

                sb.Append(newline);
                for (int i = 0; i < stack.Count; i++) {
                    sb.Append(indent);
                }

                object data;
                if (stack.Peek().enumerator is Dictionary<string, object>.Enumerator) {
                    Dictionary<string, object>.Enumerator e = (Dictionary<string, object>.Enumerator)stack.Peek().enumerator;
                    JsonHelper.WriteString(sb, e.Current.Key);
                    sb.Append(':');
                    data = e.Current.Value;
                } else {
                    List<object>.Enumerator e = (List<object>.Enumerator)stack.Peek().enumerator;
                    data = e.Current;
                }
                stack.Peek().HasValue = true;
                if (data is JsonObject) {
                    sb.Append('{');
                    JsonStringLevel level = new JsonStringLevel();
                    level.enumerator = (data as JsonObject).Entries.GetEnumerator();
                    level.HasValue = false;
                    stack.Push(level);
                    continue;
                }
                if (data is JsonArray) {
                    sb.Append('[');
                    JsonStringLevel level = new JsonStringLevel();
                    level.enumerator = (data as JsonArray)._Objects.GetEnumerator();
                    level.HasValue = false;
                    stack.Push(level);

                    continue;
                }
                if (data == null) sb.Append("null");
                else if (data is string) JsonHelper.WriteString(sb, data as string);
                else sb.Append(data.ToString());
            }

            return sb.ToString();
        }
    }
}

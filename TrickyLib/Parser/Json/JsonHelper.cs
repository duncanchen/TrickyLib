using System;
using System.Text;

namespace TrickyLib.Parser.Json
{
    internal static class JsonHelper {
        static Type[] ValidTypes = new Type[] {typeof(JsonArray),typeof(JsonObject),
                        typeof(string),typeof(bool),typeof(byte),typeof(sbyte),
                        typeof(short),typeof(ushort),typeof(int),typeof(uint),typeof(long),typeof(ulong),
                        typeof(float),typeof(double),typeof(decimal)};

        internal static void CheckValidType(object Value) {
            if (Value != null) {
                for (int i = 0; i < ValidTypes.Length; i++) if (Value.GetType() == ValidTypes[i]) return;
                throw new FormatException("Invalid value type: " + Value.GetType().ToString());
            }
        }
        internal static void WriteString(StringBuilder sb, string s) {
            sb.Append('"');
            for (int i = 0; i < s.Length; i++) {
                char c = s[i];
                if (c == '"') {
                    sb.Append("\\\"");
                    continue;
                }
                if (c == '\\') {
                    sb.Append("\\\\");
                    continue;
                }
                if (c == '\n') {
                    sb.Append("\\n");
                    continue;
                }
                if (c == '\r') {
                    sb.Append("\\r");
                    continue;
                }
                if (c == '\t') {
                    sb.Append("\\t");
                    continue;
                }
                sb.Append(c);
            }
            sb.Append('"');
        }
    }
}

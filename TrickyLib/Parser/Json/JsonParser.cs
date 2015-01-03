using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using TrickyLib.Parser.Json;

namespace TrickyLib.Parser.Json
{
    /// <summary>
    /// A class for parsing JSON syntax strings
    /// </summary>
    public static class JsonParser {
        /// <summary>
        /// Parse a JSON string into a JsonObject or JsonArray instance
        /// </summary>
        /// <param name="s">JSON string</param>
        /// <returns>a JsonObject or JsonArray instance, depending on the input string</returns>
        /// <exception cref="FormatException">The string contains invalid JSON syntax.</exception>
        public static object Parse(string s) {
            Stack<IJsonContainer> stack = new Stack<IJsonContainer>();
            StringBuilder sb = new StringBuilder();
            string key = null;
            object root = null;
            bool aftercomma = false;

            short state = 0;
            int length = s.Length;
            char c;
            uint hexvalue;
            int i = 0;

            int strStart = -1;
            int strLength = 0;
            do {
                c = s[i];

                switch (state) {

                    #region ReadChar
                    case 4:
                        switch (c) {
                            case '"':
                                if (strLength > 0) {
                                    sb.Append(s, strStart, strLength);
                                    strLength = 0;
                                }
                                strStart = -1;
                                if (!stack.Peek().IsArray && key == null) {
                                    if (sb.Length == 0) throw MakeException(s, i, "Key of value in JSON object cannot be empty string");
                                    state = 7;
                                } else {
                                    stack.Peek().InternalAdd(key, sb.ToString());
                                    key = null;
                                    sb.Length = 0;
                                    state = 8;

                                }
                                continue;
                            case '\\':
                                if (strLength > 0) {
                                    sb.Append(s, strStart, strLength);
                                    strLength = 0;
                                }
                                strStart = -1;
                                state = 5;
                                continue;
                            default:
                                //sb.Append(c);
                                //if (strStart == -1) strStart = i;
                                ++strLength;
                                continue;
                        }
                    #endregion

                    #region WaitingValue
                    case 1:
                        if (c == ' ' || c == '\n' || c == '\r' || c == '\t') continue;
                        if (c == '"') {
                            strStart = i + 1;
                            state = 4; continue;
                        }
                        if (c == '{') {
                            aftercomma = false;
                            JsonObject jo = new JsonObject();
                            stack.Peek().InternalAdd(key, jo);
                            key = null;
                            stack.Push(jo);
                            state = 3; continue;
                        }
                        if (c == '[') {
                            aftercomma = false;
                            JsonArray ja = new JsonArray();
                            stack.Peek().InternalAdd(key, ja);
                            key = null;
                            stack.Push(ja);
                            continue;
                        }
                        if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '-') {
                            sb.Append(c);
                            state = 2; continue;
                        }
                        if (!aftercomma && c == ']') {
                            if (!stack.Pop().IsArray) throw MakeException(s, i, "Invalid ']' character");
                            if (stack.Count > 0) {
                                state = 8; continue;
                            }
                            state = 9; continue;
                        }
                        throw MakeException(s, i, "Unknown value expression.");
                    #endregion

                    #region ReadValue
                    case 2:
                        if (c == ' ' || c == '\n' || c == '\r' || c == '\t') continue;
                        if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '.' || c=='+') {
                            sb.Append(c);
                            continue;
                        }
                        if (c == ',') {
                            aftercomma = true;
                            stack.Peek().InternalAdd(key, FormatJsonProperty(sb.ToString()));
                            key = null;
                            sb.Length = 0;
                            if (stack.Peek().IsArray) {
                                state = 1; continue;
                            } else {
                                state = 3; continue;
                            }
                        }
                        if (c == ']') {
                            if (!stack.Peek().IsArray) throw MakeException(s, i, "Invalid ']' character");
                            stack.Pop().InternalAdd(null, FormatJsonProperty(sb.ToString()));
                            sb.Length = 0;
                            if (stack.Count > 0) {
                                state = 8; continue;
                            }
                            state = 9; continue;
                        }
                        if (c == '}') {
                            if (stack.Peek().IsArray) throw MakeException(s, i, "Invalid '}' character");
                            stack.Pop().InternalAdd(key, FormatJsonProperty(sb.ToString()));
                            key = null;
                            sb.Length = 0;
                            if (stack.Count > 0) {
                                state = 8; continue;
                            }
                            state = 9; continue;
                        }
                        throw MakeException(s, i, "Invalid character in non-string value");
                    #endregion

                    #region WaitBeginString
                    case 3:
                        switch (c) {
                            case ' ':
                            case '\n':
                            case '\r':
                            case '\t':
                                continue;
                            case '"':
                                strStart = i + 1;
                                state = 4;
                                continue;
                            case '}':
                                if (aftercomma) goto default;
                                if (stack.Pop().IsArray) throw MakeException(s, i, "Invalid '}' character");
                                if (stack.Count == 0) state = 9;
                                else state = 8;
                                continue;
                            default:
                                throw MakeException(s, i, "Expected double quotation character to mark beginning of string");
                        }
                    #endregion

                    #region ReadEscapedChar
                    case 5:
                        switch (c) {
                            case ' ':
                            case '\n':
                            case '\r':
                            case '\t': continue;
                            case '\\':
                                sb.Append('\\');
                                strStart = i + 1;
                                state = 4; continue;
                            case '/':
                                sb.Append('/');
                                strStart = i + 1;
                                state = 4; continue;
                            case '"':
                                sb.Append('"');
                                strStart = i + 1;
                                state = 4; continue;
                            case 'n':
                                sb.Append('\n');
                                strStart = i + 1;
                                state = 4; continue;
                            case 'r':
                                sb.Append('\r');
                                strStart = i + 1;
                                state = 4; continue;
                            case 't':
                                sb.Append('\t');
                                strStart = i + 1;
                                state = 4; continue;
                            case 'u':
                                state = 6;
                                continue;
                            default:
                                throw MakeException(s, i, "Unknown escaped character");
                        }
                    #endregion

                    #region ReadUnicodeHex
                    case 6:
                        if (c == ' ' || c == '\n' || c == '\r' || c == '\t') continue;
                        if (i + 3 >= length) throw new FormatException("Incomplete JSON string");
                        hexvalue = (CharToHex(c) << 12) | (CharToHex(s[i + 1]) << 8) | (CharToHex(s[i + 2]) << 4) | CharToHex(s[i + 3]);
                        sb.Append((char)hexvalue);
                        i += 3;
                        strStart = i + 1;
                        state = 4;
                        continue;
                    #endregion

                    #region WaitColon
                    case 7:
                        switch (c) {
                            case ' ':
                            case '\n':
                            case '\r':
                            case '\t': continue;
                            case ':':
                                key = sb.ToString();
                                sb.Length = 0;
                                state = 1;
                                continue;
                            default:
                                throw MakeException(s, i, "Expected colon(:) to seperate key and values in JSON object");
                        }
                    #endregion

                    #region WaitClose
                    case 8:
                        switch (c) {
                            case ' ':
                            case '\n':
                            case '\r':
                            case '\t':
                                continue;
                            case ',':
                                aftercomma = true;
                                state = 1;
                                continue;
                            case ']':
                                if (!stack.Pop().IsArray) throw MakeException(s, i, "Invalid ']' character");
                                if (stack.Count == 0) state = 9;
                                continue;
                            case '}':
                                if (stack.Pop().IsArray) throw MakeException(s, i, "Invalid '}' character");
                                if (stack.Count == 0) state = 9;
                                continue;
                            default:
                                throw MakeException(s, i, "Expect comma or close bracket after value");
                        }
                    #endregion

                    #region Start
                    case 0:
                        switch (c) {
                            case ' ':
                            case '\n':
                            case '\r':
                            case '\t':
                                continue;
                            case '[':
                                JsonArray JA = new JsonArray();
                                root = JA;
                                stack.Push(JA);
                                state = 1; continue;
                            case '{':
                                JsonObject JO = new JsonObject();
                                root = JO;
                                stack.Push(JO);
                                state = 3; continue;
                            default:
                                throw MakeException(s, i, "Expect '{' or '[' to begin JSON string");
                        }
                    #endregion

                    #region End
                    case 9:
                        switch (c) {
                            case ' ':
                            case '\n':
                            case '\r':
                            case '\t':
                                continue;
                            default:
                                throw MakeException(s, i, "Unexpected character(s) after termination of JSON string");
                        }
                    #endregion
                }
            } while (++i < length);
            if (state != 9) throw new FormatException("Incomplete JSON string");
            return root;
        }

        private static uint CharToHex(char c) {
            if (c >= '0' && c <= '9') return (uint)(c - '0');
            if (c >= 'a' && c <= 'f') return (uint)(c - 'a' + 10);
            if (c >= 'A' && c <= 'F') return (uint)(c - 'A' + 10);
            throw new FormatException(c + " is not a valid hex value");
        }
        private static object FormatJsonProperty(string jsonString) {
            int result;
            if (int.TryParse(jsonString, NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, null, out result)) return result;

            long result_long;
            if (long.TryParse(jsonString, NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, null, out result_long)) return result_long;

            double result_double;
            if (double.TryParse(jsonString, NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, null, out result_double)) return result_double;

            string lower = jsonString.ToLower();
            if (lower == "true") return true;
            if (lower == "false") return false;
            if (lower == "null") return null;

            throw new FormatException(string.Format("Unknown JSON value: \"{0}\"", jsonString));
        }

        private static FormatException MakeException(string errorString, int position, string message) {
            int start = position - 5;
            if (start < 0) start = 0;
            int length = errorString.Length - position;
            if (length > 5) length = 5;
            length += 5;
            StringBuilder sb = new StringBuilder(message);
            sb.Append(" at character position " + position + ", near ");
            JsonHelper.WriteString(sb, errorString.Substring(start, length));
            return new FormatException(sb.ToString());
        }
    }
}
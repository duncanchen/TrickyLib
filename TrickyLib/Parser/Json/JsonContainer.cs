using System;
using System.Collections.Generic;
using System.Text;

namespace TrickyLib.Parser.Json
{
    interface IJsonContainer {
        bool IsArray { get; }
        void InternalAdd(string key, object value);
    }
}

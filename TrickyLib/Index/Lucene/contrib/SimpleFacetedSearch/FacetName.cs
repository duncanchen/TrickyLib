﻿/* 
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib.Index.Lucene.Search
{
    public partial class SimpleFacetedSearch
    {
        public class FacetName
        {
            string[] _Names;
            internal FacetName(string[] names)
            {
                this._Names = names;
            }

            public string this[int i]
            {
                get { return _Names[i]; }
            }

            public int Length
            {
                get { return _Names.Length; }
            }

            public override string ToString()
            {
                return String.Join("/", _Names);
            }
        }
    }
}
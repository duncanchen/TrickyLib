/* 
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

/* Generated By:JavaCC: Do not edit this line. QueryParserConstants.java */

using System;

namespace TrickyLib.Index.Lucene.QueryParsers
{
	
	
	/// <summary> Token literal values and constants.
	/// Generated by org.javacc.parser.OtherFilesGen#start()
	/// </summary>
	public class QueryParserConstants
	{
		/// <summary>End of File. </summary>
		protected internal const int EndOfFileToken = 0;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int NumCharToken = 1;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int EscapedCharToken = 2;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int TermStartCharToken = 3;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int TermCharToken = 4;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int WhitespaceToken = 5;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int QuotedCharToken = 6;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int AndToken = 8;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int OrToken = 9;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int NotToken = 10;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int PlusToken = 11;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int MinusToken = 12;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int LParanToken = 13;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int RParenToken = 14;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int ColonToken = 15;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int StarToken = 16;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int CaratToken = 17;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int QuotedToken = 18;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int TermToken = 19;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int FuzzySlopToken = 20;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int PrefixTermToken = 21;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int WildTermToken = 22;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int RangeInStartToken = 23;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int RangeExStartToken = 24;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int NumberToken = 25;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int RangeInToToken = 26;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int RangeInEndToken = 27;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int RangeInQuotedToken = 28;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int RangeInGoopToken = 29;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int RangeExToToken = 30;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int RangeExEndToken = 31;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int RangeExQuotedToken = 32;
		/// <summary>RegularExpression Id. </summary>
		protected internal const int RangeExGoopToken = 33;
		/// <summary>Lexical state. </summary>
		protected internal const int BoostToken = 0;
		/// <summary>Lexical state. </summary>
		protected const int RangeExToken = 1;
		/// <summary>Lexical state. </summary>
		protected internal const int RangeInToken = 2;
		/// <summary>Lexical state. </summary>
		protected internal const int DefaultToken = 3;
		/// <summary>Literal token values. </summary>
		protected internal static System.String[] tokenImage = new System.String[] {
            "<EOF>", 
            "<_NUM_CHAR>", 
            "<_ESCAPED_CHAR>", 
            "<_TERM_START_CHAR>", 
            "<_TERM_CHAR>", 
            "<_WHITESPACE>", 
            "<_QUOTED_CHAR>", 
            "<token of kind 7>", 
            "<AND>", 
            "<OR>", 
            "<NOT>", 
            "\"+\"", 
            "\"-\"", 
            "\"(\"", 
            "\")\"", 
            "\":\"", 
            "\"*\"", 
            "\"^\"", 
            "<QUOTED>", 
            "<TERM>", 
            "<FUZZY_SLOP>", 
            "<PREFIXTERM>", 
            "<WILDTERM>", 
            "\"[\"", 
            "\"{\"", 
            "<NUMBER>", 
            "\"TO\"", 
            "\"]\"", 
            "<RANGEIN_QUOTED>", 
            "<RANGEIN_GOOP>", 
            "\"TO\"", 
            "\"}\"", 
            "<RANGEEX_QUOTED>", 
            "<RANGEEX_GOOP>"
        };
	}
}
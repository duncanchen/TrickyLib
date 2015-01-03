using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.IO;

namespace TrickyLib
{
    static public class Parameters
    {
        /// <summary>
        /// If "true", we will segment query, otherwise we will get dominant sense
        /// </summary>
        static public bool IS_GET_SEGMENTATION_NOT_DOMINANT_SENSE = false;

        static public bool IS_SERVER_VERSION = false;

        static public int MAX_QUERY_NUM_PER_TREE = 10000;

        static public int MAX_ROOT_QUERY_WORD_NUM = 5;

        #region Parameters to get dominant senses

        /// <summary>
        /// Should we add cluster genereated at leaf node to father node?
        /// </summary>
        static public bool ADD_LEAF_CLUSTER_INTO_DICT = false;

        #region Procedure related parameters

        /// <summary>
        /// If "true", we will only consider direct connected children query
        /// </summary>
        static public bool IS_ONLY_CONSIDER_DIRECT_CHILDREN = true;

        #endregion

        #region Feature related parameters

        /// <summary>
        /// If true, we will consider co-clicked urls for the query
        /// </summary>
        static public bool IS_CONSIDER_COCLICKED_URLS = true;

        #endregion

        #region clustering algorithm related features

        /// <summary>
        /// Minimum consine similarity between each sense
        /// </summary>
        static public double MIN_SIMILARITY_COSIN = 0.3;//Minimum similarity for url clustering

        /// <summary>
        /// The value means the ratio of how many clicked urls comparing to all clicked urls
        /// </summary>
        static public double MINIMUN_URL_RATIO_AS_CLUSTER = 0.55;

        #endregion

        #region How to show middle results and final results

        /// <summary>
        /// How many queries will be kept as attributes of docminant sense
        /// </summary>
        static public int MAX_ATTRIBUTE_QUERY_FOR_DOMINANT_SENSE = 100;

        /// <summary>
        /// How many urls will be kept as evidence of dominant sense
        /// </summary>
        static public int MAX_EVIDENCE_URL_FOR_DOMINANT_SENSE = 10;

        /// <summary>
        /// If "true", we will show single dominant sense although it has no disambiguated senses
        /// </summary>
        static public bool IS_SHOW_SINGLE_DOMINANT_SENSE = true;

        /// <summary>
        /// If "true", we will show detailed output during the run time
        /// </summary>
        static public bool IS_SHOW_OUTPUT = false;

        /// <summary>
        /// If "true" we will show dominant sense which has no urls overlap with current query
        /// </summary>
        static public bool IS_SHOW_CLUSTER_NOT_RELATED_TO_QUERY = false;

        /// <summary>
        /// Should we show single url as cluster?
        /// </summary>
        static public bool IS_SHOW_SINGLE_URL_CLUSTER = true;

        /// <summary>
        /// If "true", we will take dominant url as a cluster. In most case the query has only dominant url as a cluster.
        /// </summary>
        static public bool IS_SHOW_DOMINANT_URL_AS_CLUSTER = true;

        /// <summary>
        /// If "true", we will show leaf node sense such as: harry shaw	[]	["http://www.harryshaw.co.uk/"]
        /// </summary>
        static public bool IS_SHOW_LEAF_NODE_SENSE = true;

        #endregion

        #endregion

        #region Parameters to get high confidence attributes

        /// <summary>
        /// Minimum number of queries which are related to father queries 
        /// </summary>
        static public int MIN_POS_QUERY_NUM = 5;
        /// <summary>
        /// Minimum number of clicked urls which are related to father queries 
        /// </summary>
        static public int MIN_POS_CLICKEDURL_NUM = 10;
        /// <summary>
        /// Minimum number of queries which are not related to father queries 
        /// </summary>
        static public int MIN_NEG_QUERY_NUM = 5;
        /// <summary>
        /// Minimum number of clicked urls which are not related to father queries 
        /// </summary>
        static public int MIN_NEG_CLICKEDURL_NUM = 10;
        /// <summary>
        /// Minimum averaged number of clicked url per query for related query-subquery pair
        /// </summary>
        static public double MIN_POS_MEAN_CLICKEDURL_NUM = 40;
        /// <summary>
        /// Maximum averaged number of clicked url per query for notrelated query-subquery pair
        /// </summary>
        static public double MAX_NEG_MEAN_CLICKEDURL_NUM = 20;

        #endregion

        /// <summary>
        /// Maximum time out for submitting one request
        /// </summary>
        static public int MAXIMUM_TIMEOUT = 24 * 3600 * 1000;

        /// <summary>
        /// Maximum record number for opening database connection
        /// </summary>  
        static public int MAX_RECORD_NUM = 500000;

        /// <summary>
        /// Maximum record number for submitting a request
        /// </summary>
        static public int MAX_BATCH_PROCESS_NUM = 10000;

        /// <summary>
        /// Minimum user number to keep the query ( query less than this number will be filtered)
        /// </summary>
        static public int MIN_USER_NUM = 2;

        /// <summary>
        /// Minimum click number to keep the query ( query less than this number will be filtered)
        /// </summary>
        static public int MIN_CLICK_NUM = 2;

        static public char[] SEPARATOR_SPACE = { ' ' };

        static public char[] SEPARATOR_TAP = { '\t' };

        static public char[] SEPARATOR_COMMA = { ',' };

        static public string[] SEPARATOR_QUOTATION = { "[\"", "\",\"", "\"]" };

        static public int MIN_CLICK_NUM_FOR_TOP_QUERY = 2;

        static public int MAX_URL_LENGTH = 512;
        static public int MAX_QUERY_LENGTH = 256;

        static public int MIN_COCLICK_NUM_FOR_URL = 2;

        static public double MIN_COCLICK_RATIO_FOR_URL = 0.2;

        /// <summary>
        /// How many top clicked queries will be output
        /// </summary>
        static public int MIN_TOPN_NUM_FOR_TOP_QUERY = 10;


        /// <summary>
        /// Maximum query word tree depth. If a query has more than 6 words, it will also be stored in a 6 level tree.
        /// </summary>
        static public short MAXIMUM_QUERY_WORD_TREE_LEVEL = 6;
        /// <summary>
        /// Use how many words for attract list from file.
        /// </summary>
        static public short WORD_NUM_FOR_ORDER = 2;
        /// <summary>
        /// Thread num to run QueryLogAnalysis
        /// </summary>
        static public short THREAD_NUM = 4;
        /// <summary>
        /// The stopWords with out article ("the", "a", "an")
        /// </summary>
        static public HashSet<string> StopWords_NoArticle = new HashSet<string>(FileReader.ReadColumn(@"F:\users\v-haowu\QueryProbe\StopWordList.txt", 0, ItemOperator.IgnoreHeader));
        /// <summary>
        /// The stopWords
        /// </summary>
        static public HashSet<string> StopWords = new HashSet<string>(FileReader.ReadColumn(@"F:\users\v-haowu\QueryProbe\StopWordList.txt", 0, ItemOperator.IgnoreHeader).Concat(new string[] { "the", "a", "an" }));
    }   
}

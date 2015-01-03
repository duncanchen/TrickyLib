using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib.Relevance
{
    public class LCS
    {
        #region LCS算法简绍
        /*
         * LCS (Longest Common Subsequence) 算法用于找出两个字符串最长公共子串。

        算法原理：

        (1) 将两个字符串分别以行和列组成矩阵。
        (2) 计算每个节点行列字符是否相同，如相同则为 1。
        (3) 通过找出值为 1 的最长对角线即可得到最长公共子串。

　         人 民 共 和 时 代
        中 0, 0, 0, 0, 0, 0
        华 0, 0, 0, 0, 0, 0
        人 1, 0, 0, 0, 0, 0
        民 0, 1, 0, 0, 0, 0
        共 0, 0, 1, 0, 0, 0
        和 0, 0, 0, 1, 0, 0
        国 0, 0, 0, 0, 0, 0

        为进一步提升该算法，我们可以将字符相同节点(1)的值加上左上角(d[i-1, j-1])的值，这样即可获得最大公用子串的长度。如此一来只需以行号和最大值为条件即可截取最大子串。

　         人 民 共 和 时 代
        中 0, 0, 0, 0, 0, 0
        华 0, 0, 0, 0, 0, 0
        人 1, 0, 0, 0, 0, 0
        民 0, 2, 0, 0, 0, 0
        共 0, 0, 3, 0, 0, 0
        和 0, 0, 0, 4, 0, 0
        国 0, 0, 0, 0, 0, 0

         */
        #endregion

        #region LCS算法实现
        /// <summary>
        /// 最大公共字符串
        /// LCS算法
        /// Longest Common Subsequence
        /// </summary>
        /// <param name="str1">字符串A</param>
        /// <param name="str2">字符串B</param>
        /// <returns></returns>
        public static string GetLCS(string str1, string str2)
        {
            if (str1 == str2)
            {
                return str1;
            }
            else if (String.IsNullOrEmpty(str1) || String.IsNullOrEmpty(str2))
            {
                return null;
            }
            var d = new int[str1.Length, str2.Length];
            var index = 0;
            var length = 0;
            for (int i = 0; i < str1.Length; i++)
            {
                for (int j = 0; j < str2.Length; j++)
                {
                    //左上角
                    var n = i - 1 >= 0 && j - 1 >= 0 ? d[i - 1, j - 1] : 0;
                    //当前节点值 = “1 + 左上角的值”：“0”
                    d[i, j] = str1[i] == str2[j] ? 1 + n : 0;
                    //如果是最大值，则记录该值和行号
                    if (d[i, j] > length)
                    {
                        length = d[i, j];
                        index = i;
                    }

                }
            }
            return str1.Substring(index - length + 1, length);

        }

        public static IEnumerable<T> GetLCS<T>(IEnumerable<T> array1, IEnumerable<T> array2, Comparer<T> comparer)
        {
            if (array1 == null || array1.Count() <= 0 || array2 == null || array2.Count() <= 0)
                yield break;

            var d = new int[array1.Count(), array2.Count()];
            var index = 0;
            var length = 0;
            for (int i = 0; i < array1.Count(); i++)
            {
                for (int j = 0; j < array2.Count(); j++)
                {
                    //左上角
                    var n = i - 1 >= 0 && j - 1 >= 0 ? d[i - 1, j - 1] : 0;
                    //当前节点值 = “1 + 左上角的值”：“0”
                    d[i, j] = comparer.Compare(array1.ElementAt(i), array2.ElementAt(j)) == 0 ? 1 + n : 0;
                    //如果是最大值，则记录该值和行号
                    if (d[i, j] > length)
                    {
                        length = d[i, j];
                        index = i;
                    }

                }
            }
            for (int i = 0; i < length; ++i)
                yield return array1.ElementAt(index - length + 1 + i);
        }

        public static IEnumerable<T> GetLCS<T>(IEnumerable<T> array1, IEnumerable<T> array2)
        {
            return GetLCS(array1, array2, Comparer<T>.Default);
        }
        #endregion
    }
}

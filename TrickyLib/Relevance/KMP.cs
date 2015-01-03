using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib.Relevance
{
    public class KMP<T>
    {
        /// <summary>
        /// 求一个字符串的回溯函数。
        /// 约定序列下标从0开始。
        /// 回溯函数是整数集[0,n-1]到N的映射，n为字符串的长度。
        /// 回溯函数的定义：
        /// 设存在非空序列L，i为其合法下标；
        /// L[i]的前置序列集为：{空集,L中所有以i-1为最后一个元素下标的子序列}
        /// L的前置序列集为：{空集,L中所有以0为第一个元素下标的子序列}
        /// 下标i的回溯函数值的定义为：
        /// 如果i=0,回溯函数值为-1
        /// 否则i的回溯函数值为i的前置序列集和L的前置序列集中相等元素的最大长度,但是相等的两个元素不能是L中的同一个子串，例如[0-i,1]~[i-1,0]reversed
        /// 换句话说是，设集合V={x,x属于i的前置序列集,并且x属于L的前置序列集，并且x的长度小于i}，回溯函数值为max(V).length
        /// 当i=0时并不存在这样的一个x，所以约定此时的回溯函数值为-1
        /// 回溯函数的意义：
        /// 如果子串中标号为j的字符同主串失配，那么将子串回溯到next[j]继续与主串匹配，如果next[j]=-1,则主串的匹配点后移一位，同子串的第一个元素开始匹配。
        /// 同一般的模式匹配算法相比，kmp通过回溯函数在失配的情况下跳过了若干轮匹配(向右滑动距离可能大于1)
        /// kmp算法保证跳过去的这些轮匹配一定是失配的，这一点可以证明
        /// </summary>
        /// <param name="pattern">模式串，上面的注释里将其称为子串</param>
        /// <returns>回溯函数是kmp算法的核心，本函数依照其定义求出回溯函数，KMP函数依照其意义使用回溯函数。</returns>
        public static int[] Next(string pattern)
        {
            int[] next = new int[pattern.Length];
            next[0] = -1;
            if (pattern.Length < 2) //如果只有1个元素不用kmp效率会好一些
            {
                return next;
            }
            next[1] = 0;    //第二个元素的回溯函数值必然是0，可以证明：
            //1的前置序列集为{空集,L[0]}，L[0]的长度不小于1，所以淘汰，空集的长度为0，故回溯函数值为0
            int i = 2;  //正被计算next值的字符的索引
            int j = 0;  //计算next值所需要的中间变量，每一轮迭代初始时j总为next[i-1]
            while (i < pattern.Length)    //很明显当i==pattern.Length时所有字符的next值都已计算完毕，任务已经完成
            { //状态点
                if (pattern[i - 1] == pattern[j])   //首先必须记住在本函数实现中，迭代计算next值是从第三个元素开始的
                {   //如果L[i-1]等于L[j]，那么next[i] = j + 1
                    next[i++] = ++j;
                }
                else
                {   //如果不相等则检查next[i]的下一个可能值----next[j]
                    j = next[j];
                    if (j == -1)    //如果j == -1则表示next[i]的值是1
                    {   //可以把这一部分提取出来与外层判断合并
                        //书上的kmp代码很难理解的一个原因就是已经被优化，从而遮蔽了其实际逻辑
                        next[i++] = ++j;
                    }
                }
            }
            return next;
        }

        /// <summary>
        /// KMP函数同普通的模式匹配函数的差别在于使用了next函数来使模式串一次向右滑动多位称为可能
        /// next函数的本质是提取重复的计算
        /// </summary>
        /// <param name="source">主串</param>
        /// <param name="pattern">用于查找主串中一个位置的模式串</param>
        /// <returns>-1表示没有匹配，否则返回匹配的标号</returns>
        public static int ExecuteKMP(string source, string pattern)
        {
            int[] next = Next(pattern);
            int i = 0;  //主串指针
            int j = 0;  //模式串指针
            //如果子串没有匹配完毕并且主串没有搜索完成
            while (j < pattern.Length && i < source.Length)
            {
                if (source[i] == pattern[j])    //i和j的逻辑意义体现于此，用于指示本轮迭代中要判断是否相等的主串字符和模式串字符
                {
                    i++;
                    j++;
                }
                else
                {
                    j = next[j];    //依照指示迭代回溯
                    if (j == -1)    //回溯有情况，这是第二种
                    {
                        i++;
                        j++;
                    }
                }
            }
            //如果j==pattern.Length则表示循环的退出是由于子串已经匹配完毕而不是主串用尽
            return j < pattern.Length ? -1 : i - j;
        }

        #region KMP generic
        private static int[] Next<T>(IList<T> pattern)
        {
            int[] next = new int[pattern.Count];
            next[0] = -1;
            if (pattern.Count < 2) //如果只有1个元素不用kmp效率会好一些
            {
                return next;
            }
            next[1] = 0;    //第二个元素的回溯函数值必然是0，可以证明：
            //1的前置序列集为{空集,L[0]}，L[0]的长度不小于1，所以淘汰，空集的长度为0，故回溯函数值为0
            int i = 2;  //正被计算next值的字符的索引
            int j = 0;  //计算next值所需要的中间变量，每一轮迭代初始时j总为next[i-1]
            while (i < pattern.Count)    //很明显当i==pattern.Length时所有字符的next值都已计算完毕，任务已经完成
            { //状态点
                //用Equals作为元素匹配条件
                if (pattern[i - 1].Equals(pattern[j]))   //首先必须记住在本函数实现中，迭代计算next值是从第三个元素开始的
                {   //如果L[i-1]等于L[j]，那么next[i] = j + 1
                    next[i++] = ++j;
                }
                else
                {   //如果不相等则检查next[i]的下一个可能值----next[j]
                    j = next[j];
                    if (j == -1)    //如果j == -1则表示next[i]的值是1
                    {   //可以把这一部分提取出来与外层判断合并
                        //书上的kmp代码很难理解的一个原因就是已经被优化，从而遮蔽了其实际逻辑
                        next[i++] = ++j;
                    }
                }
            }
            return next;
        }
        public static int ExecuteKMP<T>(IEnumerable<T> source, IList<T> pattern)
        {
            int[] next = Next(pattern);
            return ExecuteKMPInternal(source, pattern, next);
        }
        private static int ExecuteKMPInternal<T>(IEnumerable<T> source, IList<T> pattern, int[] next)
        {
            IEnumerator<T> iterator = source.GetEnumerator();
            int i = iterator.MoveNext() ? 0 : -1;//这两条语句必须总是一起执行 //主串指针
            int j = 0;  //模式串指针
            //如果子串没有匹配完毕并且主串没有搜索完成
            while (j < pattern.Count && i > -1)
            {
                if (iterator.Current.Equals(pattern[j]))    //i和j的逻辑意义体现于此，用于指示本轮迭代中要判断是否相等的主串字符和模式串字符
                {
                    i = iterator.MoveNext() ? i + 1 : -1;
                    j++;
                }
                else
                {
                    j = next[j];    //依照指示迭代回溯
                    if (j == -1)    //回溯有情况，这是第二种
                    {
                        i = iterator.MoveNext() ? i + 1 : -1;
                        j++;
                    }
                }
            }
            //如果j==pattern.Length则表示循环的退出是由于子串已经匹配完毕而不是主串用尽
            return j < pattern.Count ? -1 : i - j;
        }
        /// <summary>
        /// 泛型版的Next函数
        /// </summary>
        /// <param name="pattern">模式串可以是一个实现了IList的对象，所有数组都实现了IList</param>
        /// <param name="isEqual">此函数必须是反映一个等价关系，即满足自反、传递、交换，否则算法会出现逻辑错误。这是KMP算法的前提。</param>
        /// <returns>返回Next回溯函数</returns>
        private static int[] Next<T>(IList<T> pattern, Func<T, T, bool> isEqual)
        {
            int[] next = new int[pattern.Count];
            next[0] = -1;
            if (pattern.Count < 2) //如果只有1个元素不用kmp效率会好一些
            {
                return next;
            }
            next[1] = 0;    //第二个元素的回溯函数值必然是0，可以证明：
            //1的前置序列集为{空集,L[0]}，L[0]的长度不小于1，所以淘汰，空集的长度为0，故回溯函数值为0
            int i = 2;  //正被计算next值的字符的索引
            int j = 0;  //计算next值所需要的中间变量，每一轮迭代初始时j总为next[i-1]
            while (i < pattern.Count)    //很明显当i==pattern.Length时所有字符的next值都已计算完毕，任务已经完成
            { //状态点
                //用Equals作为元素匹配条件
                if (isEqual(pattern[i - 1], pattern[j]))   //首先必须记住在本函数实现中，迭代计算next值是从第三个元素开始的
                {   //如果L[i-1]等于L[j]，那么next[i] = j + 1
                    next[i++] = ++j;
                }
                else
                {   //如果不相等则检查next[i]的下一个可能值----next[j]
                    j = next[j];
                    if (j == -1)    //如果j == -1则表示next[i]的值是1
                    {   //可以把这一部分提取出来与外层判断合并
                        //书上的kmp代码很难理解的一个原因就是已经被优化，从而遮蔽了其实际逻辑
                        next[i++] = ++j;
                    }
                }
            }
            return next;
        }
        public static int ExecuteKMP<T>(IEnumerable<T> source, IList<T> pattern, Func<T, T, bool> isEqual)
        {
            int[] next = Next(pattern, isEqual);
            return ExecuteKMPInternal(source, pattern, isEqual, next);
        }
        private static int ExecuteKMPInternal<T>(IEnumerable<T> source, IList<T> pattern, Func<T, T, bool> isEqual, int[] next)
        {
            IEnumerator<T> iterator = source.GetEnumerator();
            int i = iterator.MoveNext() ? 0 : -1;//这两条语句必须总是一起执行 //主串指针
            int j = 0;  //模式串指针
            //如果子串没有匹配完毕并且主串没有搜索完成
            while (j < pattern.Count && i > -1)
            {
                if (isEqual(iterator.Current, pattern[j]))    //i和j的逻辑意义体现于此，用于指示本轮迭代中要判断是否相等的主串字符和模式串字符
                {
                    i = iterator.MoveNext() ? i + 1 : -1;
                    j++;
                }
                else
                {
                    j = next[j];    //依照指示迭代回溯
                    if (j == -1)    //回溯有情况，这是第二种
                    {
                        i = iterator.MoveNext() ? i + 1 : -1;
                        j++;
                    }
                }
            }
            //如果j==pattern.Length则表示循环的退出是由于子串已经匹配完毕而不是主串用尽
            return j < pattern.Count ? -1 : i - j;
        }
        private static int[] NextVal<T>(IList<T> pattern)
        {
            int[] next = new int[pattern.Count];
            next[0] = -1;
            if (pattern.Count < 2) //如果只有1个元素不用kmp效率会好一些
            {
                return next;
            }
            next[1] = 0;    //第二个元素的回溯函数值必然是0，可以证明：
            //1的前置序列集为{空集,L[0]}，L[0]的长度不小于1，所以淘汰，空集的长度为0，故回溯函数值为0
            int i = 2;  //正被计算next值的字符的索引
            int j = 0;  //计算next值所需要的中间变量，每一轮迭代初始时j总为next[i-1]
            while (i < pattern.Count)    //很明显当i==pattern.Length时所有字符的next值都已计算完毕，任务已经完成
            { //状态点
                //用Equals作为元素匹配条件
                if (j == -1 || pattern[i - 1].Equals(pattern[j]))   //首先必须记住在本函数实现中，迭代计算next值是从第三个元素开始的
                {   //如果L[i-1]等于L[j]，那么next[i] = j + 1
                    j++;
                    if (pattern[i].Equals(pattern[j]))
                    {
                        next[i] = next[j];
                    }
                    else
                    {
                        next[i] = j;
                    }
                    i++;
                }
                else
                {   //如果不相等则检查next[i]的下一个可能值----next[j]
                    j = next[j];
                }
            }
            return next;
        }
        public static int ExecuteKMPP<T>(IEnumerable<T> source, IList<T> pattern)
        {
            int[] next = NextVal(pattern);
            return ExecuteKMPInternal(source, pattern, next);
        }
        #endregion
    }
}

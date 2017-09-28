using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AStarStudy
{
    public static class WordValidateHelper
    {
        static readonly KeywordSearch Ks = new KeywordSearch(KeywordsFilterClass.WordArray);
        /// <summary>
        /// 关键字验证
        /// </summary>
        /// <param name="word">验证字段或语句</param>
        /// <returns>true:验证通过;false:验证失败</returns>
        public static List<string> FilterForBool(string word)
        {
            return Ks.FindAllKeywords(word).
                Select(p => p.Keyword).
                ToList();
        }


        /// <summary>
        /// 关键字过滤
        /// </summary>
        /// <param name="word">验证字段或语句</param>
        /// <returns>过滤后的语句</returns>
        public static string FilterForStr(string word)
        {
            /*IOrderedEnumerable<string> publiclist = UpdateKeyWord(word);
            if (publiclist != null && publiclist.Count() > 0)
            {
                string enumerable = string.Empty;
                foreach (string str in publiclist)
                {
                    enumerable += str;
                }
                LogRshelper.WriteLog("查询限制词接口，记录日志！errorMsg:" + enumerable + " word:" + word, "黄国春17642getSensitiveWordList");
                KeywordSearch getkword = new KeywordSearch(publiclist);
                word = getkword.FilterKeywords(word);
            }
            word = Ks.FilterKeywords(word);
            return word;*/
            var restule = Ks.FilterKeywords(word);
            return restule;
        }        

        /// <summary>
        /// 去掉所有标点符号
        /// </summary>
        /// <param name="wordstr">待处理的字符串</param>
        /// <returns></returns>
        public static string getWillWord(string wordstr)
        {
            string strwill = string.Empty;
            for (int j = 0; j < wordstr.Length; j++)
            {
                int currentcode = (int)wordstr[j];
                if ((currentcode > 19968 && currentcode < 40869) || (currentcode >= 48 && currentcode <= 57) || (currentcode >= 65 && currentcode <= 90) || (currentcode >= 97 && currentcode <= 122))
                {
                    strwill += wordstr[j];
                }
            }
            return strwill;
        }
    }

    #region 关键字过滤类
    /// <summary>    
    /// 关键字过滤类   
    /// /// </summary>    
    public class KeywordsFilterClass
    {

        //词组
        private static IOrderedEnumerable<string> _wordArray;
        //词组
        public static IOrderedEnumerable<string> WordArray
        {
            get
            {
                if (_wordArray == null)
                {
                    var str = @"zui、国家级、世界级、最高级、唯一、首个、首选、顶级、绝对、极致、绝无仅有、史无前例、首发、世界领先、独家、首家、最、第一、NO.1、Top、Top1、星级、三星、四星、五星、全程五星、全程四星、全程三星、全程入住五星、全程入住四星、全程入住三星、精选五星、精选四星、精选三星、三星级、四星级、五星级、五星级参考酒店、四星级参考酒店、三星级参考酒店、豪华五星、精选星级酒店、当五、当四、当三、4星、3星、5星、钻、七钻、六钻、五钻、四钻、三钻、二钻、一钻、7钻、6钻、5钻、4钻、3钻、2钻、1钻、top、TOP";
                    var words = str.Split(new string[] { "、" }, StringSplitOptions.RemoveEmptyEntries);
                    var listword = new List<string>();
                    foreach (var word in words)
                    {
                        listword.Add(word);
                    }
                    _wordArray = listword
                        .OrderByDescending(p => p.Length);
                }
                return _wordArray;
                 
            }
        }
    }
    #endregion

    /// <summary>
    /// 表示一个查找结果
    /// </summary>
    public struct KeywordSearchResult
    {
        private int index;
        private string keyword;
        public static readonly KeywordSearchResult Empty = new KeywordSearchResult(-1, string.Empty);

        public KeywordSearchResult(int index, string keyword)
        {
            this.index = index;
            this.keyword = keyword;
        }

        /// <summary>
        /// 位置
        /// </summary>
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        /// 关键词
        /// </summary>
        public string Keyword
        {
            get { return keyword; }
        }
    }


    /// <summary>
    /// Aho-Corasick算法实现
    /// </summary>
    public class KeywordSearch
    {
        /// <summary>
        /// 构造节点
        /// </summary>
        private class Node
        {
            private Dictionary<char, Node> transDict;

            public Node(char c, Node parent)
            {
                this.Char = c;
                this.Parent = parent;
                this.Transitions = new List<Node>();
                this.Results = new List<string>();

                this.transDict = new Dictionary<char, Node>();
            }

            public char Char
            {
                get;
                private set;
            }

            public Node Parent
            {
                get;
                private set;
            }

            public Node Failure
            {
                get;
                set;
            }

            public List<Node> Transitions
            {
                get;
                private set;
            }

            public List<string> Results
            {
                get;
                private set;
            }

            public void AddResult(string result)
            {
                if (!Results.Contains(result))
                {
                    Results.Add(result);
                }
            }

            public void AddTransition(Node node)
            {
                this.transDict.Add(node.Char, node);
                this.Transitions = this.transDict.Values.ToList();
            }

            public Node GetTransition(char c)
            {
                Node node;
                if (this.transDict.TryGetValue(c, out node))
                {
                    return node;
                }

                return null;
            }

            public bool ContainsTransition(char c)
            {
                return GetTransition(c) != null;
            }
        }

        private Node root; // 根节点
        private string[] keywords; // 所有关键词

        public KeywordSearch(IEnumerable<string> keywords)
        {
            this.keywords = keywords.ToArray();
            this.Initialize();
        }

        /// <summary>
        /// 根据关键词来初始化所有节点
        /// </summary>
        private void Initialize()
        {
            this.root = new Node(' ', null);

            // 添加模式
            foreach (string k in this.keywords)
            {
                Node n = this.root;
                foreach (char c in k)
                {
                    Node temp = null;
                    foreach (Node tnode in n.Transitions)
                    {
                        if (tnode.Char == c)
                        {
                            temp = tnode; break;
                        }
                    }

                    if (temp == null)
                    {
                        temp = new Node(c, n);
                        n.AddTransition(temp);
                    }
                    n = temp;
                }
                n.AddResult(k);
            }

            // 第一层失败指向根节点
            List<Node> nodes = new List<Node>();
            foreach (Node node in this.root.Transitions)
            {
                // 失败指向root
                node.Failure = this.root;
                foreach (Node trans in node.Transitions)
                {
                    nodes.Add(trans);
                }
            }
            // 其它节点 BFS
            while (nodes.Count != 0)
            {
                List<Node> newNodes = new List<Node>();
                foreach (Node nd in nodes)
                {
                    Node r = nd.Parent.Failure;
                    char c = nd.Char;

                    while (r != null && !r.ContainsTransition(c))
                    {
                        r = r.Failure;
                    }

                    if (r == null)
                    {
                        // 失败指向root
                        nd.Failure = this.root;
                    }
                    else
                    {
                        nd.Failure = r.GetTransition(c);
                        foreach (string result in nd.Failure.Results)
                        {
                            nd.AddResult(result);
                        }
                    }

                    foreach (Node child in nd.Transitions)
                    {
                        newNodes.Add(child);
                    }
                }
                nodes = newNodes;
            }
            // 根节点的失败指向自己
            this.root.Failure = this.root;
        }

        /// <summary>
        /// 找出所有出现过的关键词
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<KeywordSearchResult> FindAllKeywords(string text)
        {
            List<KeywordSearchResult> list = new List<KeywordSearchResult>();

            Node current = this.root;
            for (int index = 0; index < text.Length; ++index)
            {
                Node trans;
                do
                {
                    trans = current.GetTransition(text[index]);

                    if (current == this.root)
                        break;

                    if (trans == null)
                    {
                        current = current.Failure;
                    }
                } while (trans == null);

                if (trans != null)
                {
                    current = trans;
                }

                foreach (string s in current.Results)
                {
                    list.Add(new KeywordSearchResult(index - s.Length + 1, s));
                }
            }

            return list;
        }

        /// <summary>
        /// 简单地过虑关键词
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string FilterKeywords(string text)
        {
            StringBuilder sb = new StringBuilder();

            Node current = this.root;
            for (int index = 0; index < text.Length; index++)
            {
                Node trans;
                do
                {
                    trans = current.GetTransition(text[index]);

                    if (current == this.root)
                        break;

                    if (trans == null)
                    {
                        current = current.Failure;
                    }

                } while (trans == null);

                if (trans != null)
                {
                    current = trans;
                }

                // 处理字符
                if (current.Results.Count > 0)
                {
                    string first = current.Results[0];
                    sb.Remove(sb.Length - first.Length + 1, first.Length - 1);// 把匹配到的替换为**
                    sb.Append(new string('^', current.Results[0].Length));
                    sb.Append("");
                }
                else
                {
                    sb.Append(text[index]);
                }
            }

            return sb.ToString().Replace("^", "");
        }
    }
}

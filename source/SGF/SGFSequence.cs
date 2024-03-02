using GoSharp.Extensions;

namespace GoSharp.SGF
{
    /// <summary>
    /// Represents an SGF sequence, see the SGF specification at
    /// <a href="http://www.red-bean.com/sgf">http://www.red-bean.com/sgf</a>
    /// </summary>
    public class SGFSequence
    {
        /// <summary>
        /// Contains a list of SGF nodes.
        /// </summary>
        public List<SGFNode> Nodes = [];

        internal void Read(TextReader sr)
        {
            sr.EatWS();
            while ((char)sr.Peek() == ';')
            {
                var node = new SGFNode();
                node.Read(sr);
                Nodes.Add(node);
                sr.EatWS();
            }
        }

        internal IEnumerable<SGFProperty> GetAllProperties()
        {
            return Nodes.SelectMany(x => x.Properties);
        }

        internal IEnumerable<SGFProperty> GetProperties()
        {
            return Nodes.SelectMany(x => x.Properties.Where(y => !y.IsRoot).OrderBy(y => y.Priority));
        }

        internal IEnumerable<SGFProperty> GetRootProperties()
        {
            return Nodes.SelectMany(x => x.Properties.Where(y => y.IsRoot));
        }
    }
}

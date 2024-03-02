using GoSharp.Extensions;
using System.Text;

namespace GoSharp.SGF
{
    /// <summary>
    /// Represents an SGF collection, see the SGF specification at
    /// <a href="http://www.red-bean.com/sgf">http://www.red-bean.com/sgf</a>
    /// </summary>
    public class SGFCollection
    {
        /// <summary>
        /// Contains a list of SGF game-tree objects.
        /// </summary>
        public List<SGFGameTree> GameTrees = [];

        /// <summary>
        /// Parse an SGFCollection object from a TextReader.
        /// </summary>
        /// <param name="reader">The source TextReader.</param>
        public void Read(TextReader reader)
        {
            reader.EatWS();
            while ((char)reader.Peek() == '(')
            {
                var gameTree = new SGFGameTree();
                gameTree.Read(reader);
                GameTrees.Add(gameTree);
                reader.EatWS();
            }
        }

        /// <summary>
        /// Create an SGFCollection object from a byte array.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        public static SGFCollection Create(byte[] bytes)
        {
            using var ms = new MemoryStream(bytes);
            return Create(ms);
        }

        /// <summary>
        /// Create an SGFCollection object from a stream.
        /// </summary>
        public static SGFCollection Create(Stream stream)
        {
            var sgf = new SGFCollection();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            sgf.Read(reader);
            return sgf;
        }
    }
}

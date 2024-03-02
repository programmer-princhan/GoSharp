using GoSharp.Extensions;

namespace GoSharp.SGF
{
    /// <summary>
    /// Represents an SGF game-tree, see the SGF specification at
    /// <a href="http://www.red-bean.com/sgf">http://www.red-bean.com/sgf</a>
    /// </summary>
    public class SGFGameTree
    {
        /// <summary>
        /// Contains the SGF sequence.
        /// </summary>
        public SGFSequence Sequence = new();

        /// <summary>
        /// Contains a list of SGF game-tree objects.
        /// </summary>
        public List<SGFGameTree> GameTrees = [];

        internal void Read(TextReader reader)
        {
            var c = (char)reader.Read();
            if (c != '(')
                throw new InvalidDataException("Game-tree doesn't begin with a '('.");
            Sequence.Read(reader);
            reader.EatWS();
            while ((char)reader.Peek() == '(')
            {
                var gameTree = new SGFGameTree();
                gameTree.Read(reader);
                GameTrees.Add(gameTree);
                reader.EatWS();
            }
            c = (char)reader.Read();
            if (c != ')')
                throw new InvalidDataException("Game-tree doesn't end with a ')'.");
        }
    }
}

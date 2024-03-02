using GoSharp.Extensions;

namespace GoSharp.SGF
{
    /// <summary>
    /// Represents an SGF node, see the SGF specification at
    /// <a href="http://www.red-bean.com/sgf">http://www.red-bean.com/sgf</a>
    /// </summary>
    public class SGFNode
    {
        /// <summary>
        /// Contains a list of SGF properties.
        /// </summary>
        public List<SGFProperty> Properties = [];

        internal void Read(TextReader reader)
        {
            char c = (char)reader.Read();
            if (c != ';')
                throw new InvalidDataException("Node doesn't begin with a ';'.");
            reader.EatWS();
            while (char.IsUpper((char)reader.Peek()))
            {
                var prop = new SGFProperty();
                prop.Read(reader);
                Properties.Add(prop);
                reader.EatWS();
            }
        }
    }
}

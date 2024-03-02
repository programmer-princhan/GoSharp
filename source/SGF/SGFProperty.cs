using GoSharp.Extensions;
using System.Text;

namespace GoSharp.SGF
{
    /// <summary>
    /// Represents an SGF property, see the SGF specification at
    /// <a href="http://www.red-bean.com/sgf">http://www.red-bean.com/sgf</a>
    /// </summary>
    public class SGFProperty
    {
        /// <summary>
        /// Contains the property name.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// Contains a list of SGF property-value objects.
        /// </summary>
        public List<SGFPropValue> Values = [];

        /// <summary>
        /// Returns true if this property is a move property.
        /// </summary>
        public bool IsMove => Name == "W" || Name == "B";

        /// <summary>
        /// Returns true if this property is a setup property.
        /// </summary>
        public bool IsSetup => Name == "AE" || Name == "AB" || Name == "AW" || Name == "PL";

        private readonly HashSet<string> _moveProperties =
        [
            "W","B","AB","AW","AE"
        ];

        /// <summary>
        /// Returns true if this property is a file format property.
        /// </summary>
        public bool IsRoot => !_moveProperties.Contains(Name);

        /// <summary>
        /// Returns the property priority when writing an SGF file.
        /// </summary>
        public int Priority
        {
            get
            {
                if (IsRoot) return 0;
                if (IsSetup) return 1;
                if (IsMove) return 2;
                return 3;
            }
        }

        internal void Read(TextReader reader)
        {
            char c;
            Name = "";
            reader.EatWS();
            while (char.IsUpper((char)reader.Peek()))
            {
                c = (char)reader.Read();
                Name += c;
            }
            reader.EatWS();
            while (reader.Peek() == '[')
            {
                ReadValue(reader);
                reader.EatWS();
            }
        }

        private void ReadValue(TextReader reader)
        {
            char c = (char)reader.Read();
            if (c != '[')
                throw new InvalidDataException("Property value doesn't begin with a '['.");

            bool verbatim = false;
            var sb = new StringBuilder();

            for (; ; )
            {
                c = (char)reader.Read();

                /* Spec 3.2. Text/Formatting
                 * Formatting:
                 * Soft line break: linebreaks preceded by a "\" (soft linebreaks are converted to "", i.e. they are removed)
                 * Hard line breaks: any other linebreaks encountered
                 *
                 * Attention: a single linebreak is represented differently on different systems, e.g. "LFCR" for DOS, "LF" on Unix. An application should be able to deal with following linebreaks: LF, CR, LFCR, CRLF.
                 * [...]
                 * Escaping: "\" is the escape character. Any char following "\" is inserted verbatim (exception: whitespaces still have to be converted to space!). Following chars have to be escaped, when used in Text: "]", "\" and ":" (only if used in compose data type).
                 */
                if (verbatim)
                {
                    if (c == '\r' || c == '\n')
                    {
                        var next = reader.Peek();
                        if (next != c && (next == '\r' || next == '\n'))
                        {
                            reader.Read();
                        }
                    }
                    else
                    {
                        if (char.IsWhiteSpace(c))
                        {
                            c = ' ';
                        }
                        sb.Append(c);
                    }
                    verbatim = false;
                    continue;
                }

                if (c == '\\')
                {
                    verbatim = true;
                    continue;
                }

                if (c == ']')
                {
                    break;
                }

                sb.Append(c);
            }

            Values.Add(new SGFPropValue(sb.ToString()));
        }

        public override string ToString()
        {
            var vs = Values.Select(v => v.ToString()).ToArray();
            return Name + ":" + string.Join(", ", vs);
        }
    }
}

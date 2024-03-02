using System.Text;

namespace GoSharp
{
    /// <summary>
    /// Represents a pair of board coordinates (x and y).
    /// </summary>
    /// <remarks>
    /// Constructs a Point object from the specified coordinates.
    /// </remarks>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    public struct Point(int x, int y)
    {
        /// <summary>
        /// The X value of the coordinate.
        /// </summary>
        public int X = x;

        /// <summary>
        /// The Y value of the coordinate.
        /// </summary>
        public int Y = y;

        /// <summary>
        /// Returns a string representation of the Point in the format of (x,y).
        /// </summary>
        /// <returns>Returns a string representation of the Point in the format of (x,y).</returns>
        public override readonly string ToString()
            => "(" + X + "," + Y + ")";

        /// <summary>
        /// Returns a hash code based on the x and y values of the object.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode()
            => (X << 5) + Y;

        /// <summary>
        /// Converts a point to SGF move format (e.g. 2,3 to "cd").
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The point in SGF format.</returns>
        public static string ConvertToSGF(int x, int y)
            => Encoding.ASCII.GetString([(byte)(x + 97), (byte)(y + 97)]);

        /// <summary>
        /// Converts a point to SGF move format (e.g. 2,3 to "cd").
        /// </summary>
        /// <param name="point">The coordinates.</param>
        /// <returns>The point in SGF format.</returns>
        public static string ConvertToSGF(Point point)
            => Game.PassMove.Equals(point)
            ? ""
            : ConvertToSGF(point.X, point.Y);

        /// <summary>
        /// Converts an SGF format point to a Point object.
        /// </summary>
        /// <param name="sgf">The point in SGF format.</param>
        /// <returns>The Point object representing the position.</returns>
        public static Point ConvertFromSGF(string sgf)
        {
            if (sgf == "") return Game.PassMove;
            var bb = Encoding.ASCII.GetBytes(sgf);
            int x = bb[0] >= 'a' ? bb[0] - 'a' : bb[0] - 'A' + 26;
            int y = bb[1] >= 'a' ? bb[1] - 'a' : bb[1] - 'A' + 26;
            return new(x, y);
        }
    }
}

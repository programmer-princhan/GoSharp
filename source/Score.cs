namespace GoSharp
{
    public class Score
    {
        public HashSet<Point> Black { get; private set; }
        public HashSet<Point> White { get; private set; }

        public Score()
        {
            Black = [];
            White = [];
        }
    }
}
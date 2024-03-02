namespace GoSharp
{
    public class Variation(Point move, Game game)
    {
        public Point Move { get; set; } = move;
        public Game Game { get; set; } = game;
    }
}

using GeoLibrary.Model;

namespace GeoLibrary.Operation
{
    internal enum EdgeSide
    {
        Left,
        Right
    }

    internal class Edge
    {
        public Point Bottom { get; set; }
        public Point Current { get; set; }
        public Point Top { get; set; }
        public Point Delta { get; set; }
        public double Dx { get; set; }
        public EdgeSide Side { get; set; }
        public int WindDelta { get; set; } //1 or -1 depending on winding direction
        public int WindCount { get; set; }
        public int WindCountOpposite { get; set; } //winding count of the opposite polytype
        public int OutIndex { get; set; }
        public Edge Next { get; set; }
        public Edge Prev { get; set; }
        public Edge NextInLML { get; set; }
        public Edge NextInAEL { get; set; }
        public Edge PrevInAEL { get; set; }
        public Edge NextInSEL { get; set; }
        public Edge PrevInSEL { get; set; }
    }
}

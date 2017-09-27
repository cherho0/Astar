using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AStarStudy
{
    public class MapPoint
    {

        public MapPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }

        public int Y { get; set; }

        public bool CanCross { get; set; } = true;

        public override string ToString()
        {
            return X+","+Y;
        }
    }

    public class Map
    {
        public Map(int w, int h, MapPoint end, List<MapPoint> ob)
        {
            Width = w;
            Height = h;
            EndPoint = end;
            Points = new List<MapPoint>();
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    var node = new MapPoint(i, j);
                    if (ob.Any(p=>p.X == i && p.Y == j))
                    {
                        node.CanCross = false;
                    }
                    Points.Add(node);
                }
            }
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public List<MapPoint> Points { get; set; }

        public MapPoint EndPoint { get; set; }

        public List<AStarNode> OpenList = new List<AStarNode>();

        public List<AStarNode> CloseList = new List<AStarNode>();

    }
}

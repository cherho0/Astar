using System;
using System.Collections.Generic;
using System.Text;

namespace AStarStudy
{
    public class AStarNode
    {
        public AStarNode(int x , int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int G { get; set; }

        public int H { get; set; }

        public AStarNode ParentNode { get; set; }

        public override string ToString()
        {
            return X + "," + Y;
        }
    }
}

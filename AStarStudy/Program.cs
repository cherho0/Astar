using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStarStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello AStar!");
            var obses = new List<MapPoint>();
            obses.Add(new MapPoint(3, 0));
            obses.Add(new MapPoint(3, 1));
            obses.Add(new MapPoint(2, 1));
            obses.Add(new MapPoint(2, 3));
            obses.Add(new MapPoint(1, 3));
            obses.Add(new MapPoint(3, 2));
            obses.Add(new MapPoint(3, 3));
            obses.Add(new MapPoint(3, 4));
            obses.Add(new MapPoint(3, 5));
            obses.Add(new MapPoint(3, 6));
            obses.Add(new MapPoint(4, 0));
            obses.Add(new MapPoint(5, 0));
            obses.Add(new MapPoint(5, 3));

            obses.Add(new MapPoint(6, 2));
            FindPath f = new FindPath();

            var start = new MapPoint(2, 2);
            var end = new MapPoint(17, 23);
            Map map = new Map(30, 30, end, obses);

            var node = f.FindPathNode(start, end, map);
            var paths = new List<AStarNode>();
            while (node.ParentNode != null)
            {
                paths.Insert(0, node.ParentNode);
                node = node.ParentNode;
            }
            for (int j = 0; j < map.Height; j++)
            {
                for (int i = 0; i < map.Width; i++)
                {
                    var road = "□";
                    var got = "☆";
                    var obs = "■";
                    var route = "★";
                    var str = "";

                    //是不是路
                    str = road;

                    //是不是障碍
                    if (obses.Any(p => p.X == i && p.Y == j))
                    {
                        str = obs;
                    }
                    //是不是走过的

                    if (map.CloseList.Any(p=>p.X == i && p.Y == j))
                    {
                        str = got;
                    }

                    //是不是路径
                    if (paths.Any(p=>p.X == i && p.Y == j))
                    {
                        str = route;
                    }

                    if (i == start.X && j == start.Y)
                    {
                        str = " S";
                    }

                    if (i == end.X && j == end.Y)
                    {
                        str = " E";
                    }

                    Console.Write(str);
                    
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}

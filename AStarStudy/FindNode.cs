using System;
using System.Collections.Generic;
using System.Text;

namespace AStarStudy
{
    public class FindNode
    {
        public static TreeNode Find(TreeNode start, TreeNode end)
        {
            foreach (var node in start.Childs)
            {
                if (!end.Visited)
                {
                    node.Parent = start;
                    node.Visited = true;
                    Console.WriteLine(node);
                    if (node == end)
                    {
                        return node;
                    }

                    Find(node, end);
                }

            }
            if (end.Visited)
            {
                return end;
            }
            else
            {
                return null;
            }
        }
    }
}

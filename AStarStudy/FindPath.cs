using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AStarStudy
{
    public class FindPath
    {


        public AStarNode FindPathNode(MapPoint from, MapPoint to,Map map )
        {
            var currentNode = new AStarNode(from.X, from.Y);
            map.OpenList.Add(currentNode);
            return Plan(currentNode,currentNode                , map);
        }

        private AStarNode Plan(AStarNode currentNode,AStarNode start, Map map)
        {
            for (int i = currentNode.X - 1; i <= currentNode.X + 1; i++)
            {
                for (int j = currentNode.Y - 1; j <= currentNode.Y + 1; j++)
                {
                    var node = map.Points.FirstOrDefault(p => p.X == i && p.Y == j);
                    
                    if (node == null)
                    {
                        continue;
                    }
                    
                    if (!node.CanCross)
                    {
                        continue;
                    }
                   
                    if (node.X == currentNode.X && node.Y == currentNode.Y)
                    {
                        continue;
                    }

                    if (IsInList(node, map.CloseList))
                    {
                        continue;
                    }

                    //获取G值
                    var G = (node.X == currentNode.X || node.Y == currentNode.Y) ? 10 : 14;
                    //获取H值
                    var H = (Math.Abs(node.X - map.EndPoint.X) + Math.Abs(node.Y - map.EndPoint.Y)) *10;
                    if (H == 0)
                    {
                        //找到终点了，返回吧
                        var end = new AStarNode(map.EndPoint.X, map.EndPoint.Y);
                        end.ParentNode = currentNode;
                        return end;
                    }
                    var exitsnode = GetNodeFromMap(node, map);
                    
                    if (exitsnode != null)
                    {                        
                        //已经走过的路回头看一下把值更新掉
                        if (exitsnode.G  > G )
                        {
                            exitsnode.G = G;
                            exitsnode.ParentNode = currentNode;
                        }
                    }
                    else
                    {
                        // 将整理好的节点放入待处理列表
                        var newNode = new AStarNode(node.X, node.Y);
                        newNode.G = G;
                        newNode.H = H;
                        newNode.ParentNode = currentNode;
                        map.OpenList.Add(newNode);
                    }
                }
            }
            //将处理完的节点关闭，下次不再循环它了
            map.OpenList.Remove(currentNode);
            map.CloseList.Add(currentNode);

            var minnode = GetMinNode(map.OpenList);
            //无路可走
            if (minnode == null)
            {
                return null;
            }
            //继续走
            return Plan(minnode,start, map);

        }

        private AStarNode GetNodeFromMap(MapPoint node, Map map)
        {
            foreach (var n in map.OpenList)
            {
                if (node.X == n.X && node.Y == n.Y)
                {
                    return n;
                }
            }

            //foreach (var n in map.CloseList)
            //{
            //    if (node.X == n.X && node.Y == n.Y)
            //    {
            //        return n;
            //    }
            //}

            return null;
        }

        private AStarNode GetMinNode(List<AStarNode> openList)
        {
            var tmpMinF = openList[0].G + openList[0].H;
            var node = openList[0];
            foreach (var item in openList)
            {
                var currentF = item.G + item.H;
                if (currentF < tmpMinF)
                {
                    tmpMinF = currentF;
                    node = item;
                }
            }

            return node;
        }

        private bool IsInList(MapPoint to, List<AStarNode> openList)
        {
            foreach (var node in openList)
            {
                if (node.X == to.X && node.Y == to.Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AStarStudy
{
    public class TreeNode
    {

        public TreeNode(string name, TreeNode tn)
        {
            Childs = new List<TreeNode>();
            Name = name;
            if (tn != null)
            {
                Parent = tn;
                tn.Childs.Add(this);
            }
        }

        public bool Visited { get; set; }
        public string Name { get; set; }

        public TreeNode Parent { get; set; }

        public List<TreeNode> Childs { get; set; }

        public override string ToString()
        {
            return $"name:{Name},parent:{Parent.Name}";
        }
    }
}

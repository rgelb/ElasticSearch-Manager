using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElasticSearchManager.Common {
    static class Extensions {
        public static IEnumerable<TreeNode> FlattenTree(this TreeView tv) {
            return FlattenTree(tv.Nodes);
        }

        public static IEnumerable<TreeNode> FlattenTree(this TreeNodeCollection coll) {
            return coll.Cast<TreeNode>()
                .Concat(coll.Cast<TreeNode>()
                    .SelectMany(x => FlattenTree(x.Nodes)));
        }

        public static TreeNode FindByFullPath(this TreeNodeCollection tncoll, string fullPath) {
            TreeNode tnFound;
            foreach (TreeNode tnCurr in tncoll) {
                if (tnCurr.FullPath == fullPath) {
                    return tnCurr;
                }
                tnFound = tnCurr.Nodes.FindByFullPath(fullPath);
                if (tnFound != null) {
                    return tnFound;
                }
            }
            return null;
        }
    }
}

using System.Windows;

namespace Atlasser.Model.Nodes
{
    public class Sprite : AnimatedNode
    {
        public Sprite(string id, string texture, string atlas, Point size)
            : base(id, texture, atlas, size, NodeType.Sprite)
        {
        }

        public Sprite(string id, string texture, string atlas, Point size, int x, int y)
            : base(id, texture, atlas, size, NodeType.Sprite, x, y)
        {
        }
    }
}

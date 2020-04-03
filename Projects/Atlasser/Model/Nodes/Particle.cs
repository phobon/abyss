using System.Windows;

namespace Atlasser.Model.Nodes
{
    public class Particle : AnimatedNode
    {
        public Particle(string id, string texture, string atlas, Point size)
            : base(id, texture, atlas, size, NodeType.Particle)
        {
        }

        public Particle(string id, string texture, string atlas, Point size, int x, int y)
            : base(id, texture, atlas, size, NodeType.Sprite, x, y)
        {
        }
    }
}

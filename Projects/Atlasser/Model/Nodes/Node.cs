using System;
using System.Windows;
using System.Xml;

namespace Atlasser.Model.Nodes
{
    public abstract class Node : NotifyPropertyChangedBase, ISerializable
    {
        private string id;
        private Point size;

        protected Node(string id, string texture, Point size, NodeType nodeType)
        {
            this.id = id;
            this.Texture = texture;
            this.size = size;

            this.NodeType = nodeType;
        }

        public string Id
        {
            get
            {
                return this.id;
            }

            set
            {
                if (this.id.Equals(value, StringComparison.InvariantCulture))
                {
                    return;
                }

                this.id = value;
                this.OnPropertyChanged("Id");
            }
        }

        public string Texture
        {
            get; private set;
        }

        public Point Size
        {
            get
            {
                return this.size;
            }

            set
            {
                if (this.size == value)
                {
                    return;
                }

                this.size = value;
                this.OnPropertyChanged("Size");
            }
        }

        public NodeType NodeType
        {
            get; private set;
        }

        public abstract void Serialize(XmlWriter writer);
    }
}

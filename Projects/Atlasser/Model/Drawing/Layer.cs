using System;
using System.Windows;
using System.Xml;

namespace Atlasser.Model.Drawing
{
    public class Layer : NotifyPropertyChangedBase, ISerializable
    {
        private LayerDepth depth;
        private string id;
        private int x;
        private int y;

        public Layer(string texture, Point size)
        {
            this.Texture = texture;
            this.Size = size;
        }

        public string Id
        {
            get
            {
                return this.id;
            }

            set
            {
                if (this.id == value)
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

        public LayerDepth Depth
        {
            get
            {
                return this.depth;
            }

            set
            {
                if (this.depth == value)
                {
                    return;
                }

                this.depth = value;
                this.OnPropertyChanged("Depth");
            }
        }

        public Point Size
        {
            get; private set;
        }
        
        public int X
        {
            get
            {
                return this.x;
            }

            set
            {
                if (this.x == value)
                {
                    return;
                }

                this.x = value;
                this.OnPropertyChanged("X");
            }
        }
        
        public int Y
        {
            get
            {
                return this.y;
            }

            set
            {
                if (this.y == value)
                {
                    return;
                }

                this.y = value;
                this.OnPropertyChanged("Y");
            }
        }

        public void Serialize(XmlWriter writer)
        {
            writer.WriteStartElement("Layer");

            if (!string.IsNullOrEmpty(this.Id))
            {
                writer.WriteAttributeString("id", this.Id);
            }

            writer.WriteAttributeString("texture", this.Texture);
            writer.WriteAttributeString("depth", this.Depth.ToString());

            writer.WriteStartElement("Size");
            writer.WriteAttributeString("x", this.Size.X.ToString());
            writer.WriteAttributeString("y", this.Size.Y.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("SpriteLocation");
            writer.WriteAttributeString("x", this.X.ToString());
            writer.WriteAttributeString("y", this.Y.ToString());
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}

using System.Windows;
using System.Xml;

namespace Atlasser.Model.Nodes
{
    public class Doodad : Node
    {
        private bool isAnimated;
        private int speed;
        private int loopLag;
        private Point spriteLocation;
        private string frames;

        public Doodad(string id, string texture, Point size) 
            : base(id, texture, size, NodeType.Doodad)
        {
            this.Speed = 30;
            this.LoopLag = 60;
            this.Frames = "0-2";
        }

        public bool IsAnimated
        {
            get
            {
                return this.isAnimated;
            }

            set
            {
                if (this.isAnimated == value)
                {
                    return;
                }

                this.isAnimated = value;
                this.OnPropertyChanged("IsAnimated");
            }
        }

        public int Speed
        {
            get
            {
                return this.speed;
            }

            set
            {
                if (this.speed == value)
                {
                    return;
                }

                this.speed = value;
                this.OnPropertyChanged("Speed");
            }
        }

        public int LoopLag
        {
            get
            {
                return this.loopLag;
            }

            set
            {
                if (this.loopLag == value)
                {
                    return;
                }

                this.loopLag = value;
                this.OnPropertyChanged("LoopLag");
            }
        }

        public string Frames
        {
            get
            {
                return this.frames;
            }

            set
            {
                if (this.frames == value)
                {
                    return;
                }

                this.frames = value;
                this.OnPropertyChanged("Frames");
            }
        }

        public Point SpriteLocation
        {
            get
            {
                return this.spriteLocation;
            }

            set
            {
                if (this.spriteLocation == value)
                {
                    return;
                }

                this.spriteLocation = value;
                this.OnPropertyChanged("SpriteLocation");
            }
        }

        public override void Serialize(XmlWriter writer)
        {
            writer.WriteStartElement("Doodad");
            writer.WriteAttributeString("id", this.Id);
            writer.WriteAttributeString("isAnimated", this.IsAnimated.ToString());

            if (this.IsAnimated)
            {
                writer.WriteAttributeString("speed", this.Speed.ToString());
                writer.WriteAttributeString("looplag", this.LoopLag.ToString());
                writer.WriteAttributeString("frames", this.Frames);
            }

            writer.WriteStartElement("SpriteLocation");
            writer.WriteAttributeString("x", this.SpriteLocation.X.ToString());
            writer.WriteAttributeString("y", this.SpriteLocation.Y.ToString());
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}

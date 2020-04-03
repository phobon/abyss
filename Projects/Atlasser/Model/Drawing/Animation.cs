using System.Windows.Media;
using System.Xml;

namespace Atlasser.Model.Drawing
{
    public class Animation : NotifyPropertyChangedBase, ISerializable
    {
        private string name;
        private int speed;
        private int loopLagFrames;
        private bool isLooping;
        private string frames;
        private int x;
        private int y;
        private SolidColorBrush uniqueColorBrush;

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name == value)
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged("Name");
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

        public bool IsLooping
        {
            get
            {
                return this.isLooping;
            }

            set
            {
                if (this.isLooping == value)
                {
                    return;
                }

                this.isLooping = value;
                this.OnPropertyChanged("IsLooping");
            }
        }

        public int LoopLagFrames
        {
            get
            {
                return this.loopLagFrames;
            }

            set
            {
                if (this.loopLagFrames == value)
                {
                    return;
                }

                this.loopLagFrames = value;
                this.OnPropertyChanged("LoopLagFrames");
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

        public SolidColorBrush UniqueColorBrush
        {
            get
            {
                if (this.uniqueColorBrush == null)
                {
                    this.uniqueColorBrush = Colours.GetRandomBrush();
                }

                return this.uniqueColorBrush;
            }
        }

        public void Serialize(XmlWriter writer)
        {
            writer.WriteStartElement("Animation");
            writer.WriteAttributeString("name", this.Name);
            writer.WriteAttributeString("speed", this.Speed.ToString());
            writer.WriteAttributeString("loop", this.IsLooping.ToString());
            writer.WriteAttributeString("looplag", this.LoopLagFrames.ToString());
            writer.WriteAttributeString("frames", this.Frames);
            
            writer.WriteStartElement("SpriteLocation");
            writer.WriteAttributeString("x", this.X.ToString());
            writer.WriteAttributeString("y", this.Y.ToString());
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}

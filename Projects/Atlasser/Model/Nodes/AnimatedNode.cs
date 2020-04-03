using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;
using Atlasser.Model.Drawing;
using Atlasser.Model.ViewModes;

namespace Atlasser.Model.Nodes
{
    public class AnimatedNode : Node
    {
        private string atlas;
        private bool centre;
        private bool tile;

        public AnimatedNode(string id, string texture, string atlas, Point size, NodeType nodeType) 
            : base(id, texture, size, nodeType)
        {
            this.atlas = atlas;
            this.Layers = new ObservableCollection<Layer>();
            this.Animations = new ObservableCollection<Animation>();
        }

        public AnimatedNode(string id, string texture, string atlas, Point size, NodeType nodeType, int x, int y)
            : this(id, texture, atlas, size, nodeType)
        {
            this.Layers.Add(new Layer(texture, size)
            {
                X = x, 
                Y = y
            });
        }

        public IEnumerable<string> AtlasKeys
        {
            get
            {
                return SpriteSheetPresenter.AtlasKeys;
            }
        }

        public string Atlas
        {
            get
            {
                return this.atlas;
            }

            set
            {
                if (this.atlas.Equals(value, StringComparison.InvariantCulture))
                {
                    return;
                }

                this.atlas = value;
                this.OnPropertyChanged("Atlas");
            }
        }

        public bool Tile
        {
            get
            {
                return this.tile;
            }

            set
            {
                if (this.tile == value)
                {
                    return;
                }

                this.tile = value;
                this.OnPropertyChanged("Tile");
            }
        }

        public bool Centre
        {
            get
            {
                return this.centre;
            }

            set
            {
                if (this.centre == value)
                {
                    return;
                }

                this.centre = value;
                this.OnPropertyChanged("Centre");
            }
        }

        public ObservableCollection<Layer> Layers
        {
            get; private set;
        }

        public ObservableCollection<Animation> Animations
        {
            get; private set;
        }

        public override void Serialize(XmlWriter writer)
        {
            writer.WriteStartElement(this.NodeType.ToString());
            writer.WriteAttributeString("id", this.Id);
            writer.WriteAttributeString("atlas", this.Atlas);

            if (this.Centre)
            {
                writer.WriteAttributeString("centre", this.Centre.ToString());
            }

            if (this.Tile)
            {
                writer.WriteAttributeString("tile", this.Tile.ToString());
            }

            writer.WriteStartElement("Size");
            writer.WriteAttributeString("x", this.Size.X.ToString());
            writer.WriteAttributeString("y", this.Size.Y.ToString());
            writer.WriteEndElement();

            // Layers.
            if (this.Layers.Any())
            {
                writer.WriteStartElement("Layers");
                foreach (var l in this.Layers)
                {
                    l.Serialize(writer);
                }
                writer.WriteEndElement();
            }

            // Animations.
            if (this.Animations.Any())
            {
                writer.WriteStartElement("Animations");
                foreach (var l in this.Animations)
                {
                    l.Serialize(writer);
                }
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}

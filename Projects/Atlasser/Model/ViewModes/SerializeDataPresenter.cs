using System.Xml;
using Atlasser.Model.Nodes;

namespace Atlasser.Model.ViewModes
{
    public class SerializeDataPresenter : ViewMode
    {
        public const string DefaultAtlasTarget = "spriteatlas.xml";

        private string atlasTarget;

        public SerializeDataPresenter(Atlasser parent) 
            : base(parent, Atlasser.SerializeModeName)
        {
            this.AtlasTarget = DefaultAtlasTarget;
        }

        public string AtlasTarget
        {
            get { return this.atlasTarget; }

            set
            {
                this.atlasTarget = value;
                this.OnPropertyChanged("AtlasTarget");
                this.OnPropertyChanged("CanSerialize");
            }
        }

        public bool CanSerialize
        {
            get
            {
                return !string.IsNullOrEmpty(this.atlasTarget);
            }
        }

        public void Serialize()
        {
            var spriteSheets = (SpriteSheetPresenter) this.Parent.ViewModes[Atlasser.SpritesheetModeName];

            var atlas = XmlWriter.Create(this.AtlasTarget);
            atlas.WriteStartDocument();

            atlas.WriteStartElement("SpriteAtlas");

            // Write sprites.
            atlas.WriteStartElement("Sprites");
            foreach (var s in spriteSheets.GetNodesByType(NodeType.Sprite))
            {
                s.Serialize(atlas);
            }
            atlas.WriteEndElement();

            // Write particles.
            atlas.WriteStartElement("Particles");
            foreach (var s in spriteSheets.GetNodesByType(NodeType.Particle))
            {
                s.Serialize(atlas);
            }
            atlas.WriteEndElement();

            // Write doodads.
            atlas.WriteStartElement("Doodads");
            foreach (var s in spriteSheets.GetNodesByType(NodeType.Doodad))
            {
                s.Serialize(atlas);
            }
            atlas.WriteEndElement();

            atlas.WriteEndElement();

            atlas.WriteEndDocument();

            atlas.Close();
        }
    }
}

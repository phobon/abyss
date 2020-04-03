using System.Collections.Generic;
using System.Collections.ObjectModel;
using Atlasser.Model.Nodes;
using Atlasser.Model.SpriteSheets;

namespace Atlasser.Model.ViewModes
{
    public class SpriteSheetPresenter : ViewMode
    {
        private SpriteSheet currentSpriteSheet;

        public SpriteSheetPresenter(Atlasser parent) 
            : base(parent, Atlasser.SpritesheetModeName)
        {
            this.SpriteSheets = new ObservableCollection<SpriteSheet>();
            AtlasKeys = new ObservableCollection<string>();
        }

        public SpriteSheet CurrentSpriteSheet
        {
            get
            {
                return this.currentSpriteSheet;
            }

            set
            {
                if (this.currentSpriteSheet == value)
                {
                    return;
                }

                this.currentSpriteSheet = value;
                this.OnPropertyChanged("CurrentSpriteSheet");
            }
        }

        public ObservableCollection<SpriteSheet> SpriteSheets
        {
            get; private set;
        }

        public static ObservableCollection<string> AtlasKeys
        {
            get; private set;
        }

        public IEnumerable<Node> GetNodesByType(NodeType nodeType)
        {
            var nodes = new List<Node>();
            foreach (var s in this.SpriteSheets)
            {
                foreach (var n in s.GetNodesByType(nodeType))
                {
                    nodes.Add(n);
                }
            }

            return nodes;
        }
    }
}

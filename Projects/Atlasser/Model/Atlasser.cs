using System.Collections.ObjectModel;
using Atlasser.Model.ViewModes;
using System.Collections.Generic;

namespace Atlasser.Model
{
    public class Atlasser : NotifyPropertyChangedBase
    {
        public const string DataModeName = "Data";
        public const string SpritesheetModeName = "Spritesheets";
        public const string SerializeModeName = "Serialize";

        private IViewMode currentViewMode;

        public Atlasser()
        {
            this.ViewModes = new Dictionary<string, IViewMode>
            {
                { DataModeName, new LoadDataPresenter(this) },
                { SpritesheetModeName, new SpriteSheetPresenter(this) },
                { SerializeModeName, new SerializeDataPresenter(this) },
            };

            this.CurrentViewMode = this.ViewModes[DataModeName];
        }
        
        public IViewMode CurrentViewMode
        {
            get
            {
                return this.currentViewMode;
            }

            set
            {
                if (this.currentViewMode == value)
                {
                    return;
                }

                // Deactivate the old view mode.
                if (this.currentViewMode != null)
                {
                    this.currentViewMode.IsActive = false;
                }

                this.currentViewMode = value;
                this.currentViewMode.IsActive = true;
                this.OnPropertyChanged("CurrentViewMode");
            }
        }

        public Dictionary<string, IViewMode> ViewModes
        {
            get; private set;
        }

        public SpriteSheetPresenter SpriteSheetPresenter
        {
            get
            {
                return (SpriteSheetPresenter)this.ViewModes[SpritesheetModeName];
            }
        }
    }
}

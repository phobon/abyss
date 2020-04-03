using System;

namespace Atlasser.Model.ViewModes
{
    public abstract class ViewMode : NotifyPropertyChangedBase, IViewMode
    {
        private bool isAccessible;
        private bool isActive;

        protected ViewMode(Atlasser parent, string name)
        {
            this.Parent = parent;
            this.Name = name;
        }

        public event EventHandler Activated;

        public Atlasser Parent { get; private set; }

        public string Name { get; private set; }

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }

            set
            {
                if (this.isActive == value)
                {
                    return;
                }

                this.isActive = value;
                this.OnPropertyChanged("IsActive");

                if (this.isActive)
                {
                    this.Activate();
                }
            }
        }
        
        public bool IsAccessible
        {
            get { return this.isAccessible; }

            set
            {
                if (this.isAccessible == value)
                {
                    return;
                }

                this.isAccessible = value;
                this.OnPropertyChanged("IsAccessible");
            }
        }

        protected virtual void Activate()
        {
            this.OnActivated();
        }

        private void OnActivated()
        {
            var handler = this.Activated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}

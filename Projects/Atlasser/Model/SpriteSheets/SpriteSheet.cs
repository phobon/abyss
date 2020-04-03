using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Atlasser.Model.Nodes;
using Atlasser.Model.ViewModes;

namespace Atlasser.Model.SpriteSheets
{
    public class SpriteSheet : NotifyPropertyChangedBase
    {
        private Point spriteSize;
        private BitmapImage image;
        private Node selectedNode;
        private Point selectedPoint;
        private int scale;
        private bool isPointSelected;

        public SpriteSheet(string id, string location, Point size, Point spriteSize)
        {
            this.Id = id;
            this.Location = location;
            this.Size = size;
            this.Width = (int)size.X;
            this.Height = (int)size.Y;
            this.SpriteSize = spriteSize;

            this.Nodes = new ObservableCollection<Node>();
            this.Nodes.CollectionChanged += (sender, args) => this.OnPropertyChanged("NodeMap");
            this.NodeMap = new Dictionary<Point, Node>();

            this.scale = 3;
        }

        public string Id
        {
            get; private set;
        }

        public string Location
        {
            get; private set;
        }

        public BitmapImage Image
        {
            get
            {
                if (image == null)
                {
                    // Create source
                    this.image = new BitmapImage();

                    // BitmapImage.UriSource must be in a BeginInit/EndInit block
                    this.image.BeginInit();
                    this.image.UriSource = new Uri(this.Location);

                    // To save significant application memory, set the DecodePixelWidth or   
                    // DecodePixelHeight of the BitmapImage value of the image source to the desired  
                    // height or width of the rendered image. If you don't do this, the application will  
                    // cache the image as though it were rendered as its normal size rather then just  
                    // the size that is displayed. 
                    // Note: In order to preserve aspect ratio, set DecodePixelWidth 
                    // or DecodePixelHeight but not both.
                    this.image.DecodePixelWidth = this.Width;
                    this.image.EndInit();
                }

                return this.image;
            }
        }

        public Point Size
        {
            get; private set;
        }

        public int Width
        {
            get; private set;
        }

        public int Height
        {
            get; private set;
        }

        public int Scale
        {
            get
            {
                return this.scale;
            }

            set
            {
                if (this.scale == value)
                {
                    return;
                }

                // Do not change if the value is less than 1 (ie: 0) or greater than 5;
                if (value < 1 || value > 5)
                {
                    return;
                }

                this.scale = value;
                this.OnPropertyChanged("Scale");
            }
        }

        public Point SpriteSize
        {
            get
            {
                return this.spriteSize;
            }

            set
            {
                if (this.spriteSize == value)
                {
                    return;
                }

                this.spriteSize = value;
                this.OnPropertyChanged("SpriteSize");
            }
        }
        
        public Node SelectedNode
        {
            get
            {
                return this.selectedNode;
            }

            set
            {
                if (this.selectedNode == value)
                {
                    return;
                }

                this.selectedNode = value;
                this.OnPropertyChanged("SelectedNode");
            }
        }

        public Point SelectedPoint
        {
            get
            {
                return this.selectedPoint;
            }

            set
            {
                this.selectedPoint = value;
                this.OnPropertyChanged("SelectedPoint");

                this.IsPointSelected = true;

                // Select the node if it exists.
                if (this.NodeMap.ContainsKey(this.selectedPoint))
                {
                    this.SelectedNode = this.NodeMap[this.selectedPoint];
                }
                else
                {
                    this.SelectedNode = null;
                }
            }
        }

        public bool IsPointSelected
        {
            get
            {
                return this.isPointSelected;
            }

            private set
            {
                if (this.isPointSelected == value)
                {
                    return;
                }

                this.isPointSelected = value;
                this.OnPropertyChanged("IsPointSelected");
            }
        }

        public Dictionary<Point, Node> NodeMap
        {
            get; private set;
        }

        public ObservableCollection<Node> Nodes
        {
            get; private set;
        }

        public void AddNode(Node node)
        {
            this.NodeMap.Add(this.SelectedPoint, node);
            this.Nodes.Add(node);
            this.SelectedNode = node;
        }

        public void AddNode(Point spriteLocation, Node node)
        {
            this.NodeMap.Add(spriteLocation, node);
            this.Nodes.Add(node);
            this.SelectedNode = node;
        }

        public void RemoveNode(Point location, Node node)
        {
            this.NodeMap.Remove(location);
            this.Nodes.Remove(node);
            this.SelectedNode = null;
        }

        public IEnumerable<Node> GetNodesByType(NodeType nodeType)
        {
            switch (nodeType)
            {
                case NodeType.Doodad:
                    return this.Nodes.OfType<Doodad>();
                case NodeType.Particle:
                    return this.Nodes.OfType<Particle>();
                case NodeType.Sprite:
                    return this.Nodes.OfType<Sprite>();
            }

            return null;
        }
    }
}

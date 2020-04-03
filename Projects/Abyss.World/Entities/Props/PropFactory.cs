using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abyss.World.Entities.Items;
using Abyss.World.Entities.Props.Concrete;

using Occasus.Core.Components;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Entities;
using Occasus.Core.Maths;

namespace Abyss.World.Entities.Props
{
    using Microsoft.Xna.Framework;

    public static class PropFactory
    {
        private const string Qualifier = "Abyss.World.Entities.Props.Concrete.";
        private const string LavaColumnQualifier = "Abyss.World.Entities.Props.Concrete.LavaColumns.";

        private static readonly List<string> HelpfulProps = new List<string>
                                                   {
                                                       Qualifier + "ShopEntrance",
                                                       Qualifier + "TreasureChest"
                                                   };

        private static readonly List<string> HarmfulProps = new List<string>
                                                   {
                                                       Qualifier + "SpikeTrap"
                                                   };

        public static IProp GetProp(string id)
        {
            var qualifiedName = Qualifier + id;
            return (IProp)Activator.CreateInstance(Type.GetType(qualifiedName));
        }

        public static IProp GetProp(string id, Vector2 position)
        {
            if (id.Equals("PurchasableItem"))
            {
                return GetRandomPurchasableItem(position);
            }

            var qualifiedName = Qualifier + id;
            return (IProp)Activator.CreateInstance(Type.GetType(qualifiedName), position);
        }

        public static IProp GetProp(string id, Vector2 position, Rectangle boundingBox)
        {
            var qualifiedName = Qualifier + id;
            return (IProp)Activator.CreateInstance(Type.GetType(qualifiedName), position, boundingBox);
        }

        public static IProp GetRandomHelpfulProp(Vector2 position)
        {
            var randomHelpfulPropId = HelpfulProps.ElementAt(MathsHelper.Random(HelpfulProps.Count));
            var randomHelpfulProp = (IProp)Activator.CreateInstance(Type.GetType(randomHelpfulPropId), position);
            return randomHelpfulProp;
        }

        public static IProp GetRandomHarmfulProp(Vector2 position)
        {
            var randomHarmfulPropId = HarmfulProps.ElementAt(MathsHelper.Random(HarmfulProps.Count));
            var randomHarmfulProp = (IProp)Activator.CreateInstance(Type.GetType(randomHarmfulPropId), position);
            return randomHarmfulProp;
        }

        public static IProp GetRandomProp(Vector2 position)
        {
            var random = MathsHelper.Random();
            if (random < 75)
            {
                return GetRandomHarmfulProp(position);
            }

            return GetRandomHelpfulProp(position);
        }

        public static IProp GetHorizontalLavaColumn(Direction eruptionDirection)
        {
            var column = (IProp)Activator.CreateInstance(Type.GetType(LavaColumnQualifier + "HorizontalLavaColumn"), eruptionDirection);
            column.Initialize();
            return column;
        }

        public static IProp GetVerticalLavaColumn()
        {
            var column = (IProp)Activator.CreateInstance(Type.GetType(LavaColumnQualifier + "VerticalLavaColumn"));
            column.Initialize();
            return column;
        }
        
        public static IProp GetVoidPatch()
        {
            // Get the size of this void patch.
            var sizeMultiplier = MathsHelper.Random() > 50 ? 1 : 0;
            var size = DrawingManager.TileWidth + (sizeMultiplier * DrawingManager.TileWidth);
            var boundingBox = new Rectangle(0, 0, size, size);
            
            // Get the position of this void patch.
            var verticalPosition = MathsHelper.Random(Monde.GameManager.ViewPort.Y, Monde.GameManager.ViewPort.Y + Monde.GameManager.ViewPort.Height) * DrawingManager.TileHeight;
            verticalPosition += MathsHelper.Random(DrawingManager.TileHeight);
            var horizontalPosition = MathsHelper.Random(Monde.GameManager.ViewPort.Width)*DrawingManager.TileWidth;
            horizontalPosition += MathsHelper.Random(DrawingManager.TileWidth);

            // Get a random spritelocation based on the sizeMultiplier.
            var spriteLocation = sizeMultiplier > 0 ? new Point(MathsHelper.Random() > 50 ? 1 : 2, MathsHelper.Random() > 50 ? 3 : 4) : new Point(1, 3);
            var position =
                new Vector2(
                    horizontalPosition,
                    verticalPosition);
            var patch = (IProp)Activator.CreateInstance(Type.GetType(Qualifier + "VoidPatch"), position, boundingBox);
            patch.Initialize();
            return patch;
        }

        public static IProp GetRiftSheet(Direction originDirection)
        {
            // Rift sheets will always be about the size of the screen.
            var boundingBox = new Rectangle(
                0, 0, 
                (int)DrawingManager.BaseResolutionWidth,
                (int)DrawingManager.BaseResolutionHeight + 10);

            var horizontalPosition = originDirection == Direction.Left
                ? -DrawingManager.BaseResolutionWidth * 1.5
                : DrawingManager.BaseResolutionWidth * 1.5;
            var verticalPosition = Monde.GameManager.ViewPort.Center.Y;
            var sheet = (IProp)Activator.CreateInstance(
                Type.GetType(Qualifier + "RiftSheet"), 
                new Vector2((int)horizontalPosition, verticalPosition), boundingBox);
            sheet.Initialize();
            return sheet;
        }

        public static IProp GetRandomPurchasableItem(Vector2 position)
        {
            var item = ItemFactory.GetRandomTreasureItem(position);
            var purchasableItem = (IProp)Activator.CreateInstance(Type.GetType(Qualifier + "PurchasableItem"), item, position, (ISprite)item.Components[Sprite.Tag], item.Collider.BoundingBox, 100);
            purchasableItem.Initialize();
            return purchasableItem;
        }

        public static IProp GetExplosion(Vector2 position)
        {
            var explosion = (IProp)Activator.CreateInstance(
                 Type.GetType(Qualifier + "Explosion"),
                 position);
            explosion.Initialize();
            return explosion;
        }

        public static IProp GetLight(Vector2 position, Color color, float intensity, float scale)
        {
            var light = (IProp)Activator.CreateInstance(
                 Type.GetType(Qualifier + "Light"),
                 position, color, intensity, scale);
            light.Initialize();
            return light;
        }
    }
}

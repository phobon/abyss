using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Entities;
using Occasus.Core.Input;

namespace Occasus.Core.Debugging
{
    public class Debugger : EngineComponent, IDebugger
    {
        private const float DebugStringColumnBase = 50f;
        private const float DebugStringRowBase = 50f;
        private const float DebugStringStep = 15f;

        private int frameCounter;
        private TimeSpan elapsedTime = TimeSpan.Zero;

        public Debugger()
            : base("Debug", "Provides debug information for the engine.")
        {
            this.DebugInstances = new Dictionary<string, string>();
        }

        public int Framerate { get; private set; }

        public int EntityCount { get; set; }

        public IDictionary<string, string> DebugInstances { get; private set; }

        public void Add(string key, string value)
        {
            if (this.DebugInstances.ContainsKey(key))
            {
                this.DebugInstances[key] = value;
            }
            else
            {
                this.DebugInstances.Add(key, value);
            }
        }

        public void Remove(string key)
        {
            if (this.DebugInstances.ContainsKey(key))
            {
                this.DebugInstances.Remove(key);
            }
        }

        public override void Update(GameTime gameTime, IInputState inputState)
        {
            this.EntityCount = 0;

            this.elapsedTime += gameTime.ElapsedGameTime;

            // Get the framerate.
            if (this.elapsedTime > TimeSpan.FromSeconds(1))
            {
                this.elapsedTime -= TimeSpan.FromSeconds(1);
                this.Framerate = frameCounter;
                this.frameCounter = 0;
            }

            frameCounter++;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Display any unique debug information as required.
            var row = 1;
            foreach (var debugInstance in Engine<IGameManager<IEntity>>.Debugger.DebugInstances)
            {
                var y = DebugStringRowBase + (row * DebugStringStep);

                spriteBatch.DrawString(DrawingManager.Font, debugInstance.Key, new Vector2(DebugStringColumnBase, y), Color.Yellow);

                y = DebugStringRowBase + (row * DebugStringStep);
                var position = new Vector2(DebugStringColumnBase + DrawingManager.Font.MeasureString(debugInstance.Key).X + DebugStringStep, y);
                spriteBatch.DrawString(DrawingManager.Font, debugInstance.Value, position, Color.White);
                row += 2;
            }
        }
    }
}

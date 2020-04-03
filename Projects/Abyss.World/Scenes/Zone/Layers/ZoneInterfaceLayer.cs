using System;
using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Entities.Player;
using Abyss.World.Entities.Relics;
using Abyss.World.Phases;
using Abyss.World.Scenes.Zone.Layers.Interface;
using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core;
using Occasus.Core.Assets;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Interface;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Drawing.Text;
using Occasus.Core.Layers;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using Occasus.Core.Scenes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Abyss.World.Scenes.Zone.Layers
{
    /// <summary>
    /// Interface layer for the Zone scene.
    /// </summary>
    public class ZoneInterfaceLayer : Layer
    {
        private static readonly Vector2 riftTotalPosition = new Vector2(576f, 60f);
        private static readonly Vector2 scoreTotalPosition = new Vector2(322f, 35f);

        // Coroutine keys.
        private readonly string beginKey;
        private readonly string endKey;
        private readonly string progressPhaseBarKey;
        private readonly string notifyNewPhaseKey;

        // Interface elements.
        private readonly IInterfaceElement gameInterface;
        private readonly IInterfaceElement riftGlyph;
        private readonly IList<HeartGlyph> hearts;
        private readonly PhaseNotification phaseNotification;

        private readonly ITextElement riftTotal;
        private readonly ITextElement scoreTotal;
        
        private static readonly Vector2 phaseBarPosition = new Vector2(0, 16f);

        // Rectangles.
        private static readonly Rectangle phaseBarRect = new Rectangle(0, 0, (int)DrawingManager.ScaledWindowWidth, 16);

        private static readonly Rectangle relicBarRect = new Rectangle(0, 88, 272, 24);
        private static readonly Rectangle relicBarColourRect = new Rectangle(4, 112, 264, 16);
        
        private float phaseBarWidthPercentage;
        
        // Relic notifications.
        private bool drawRelicTimer;
        private Color relicTimerColor;
        private float relicTimerBarWidthPercentage;

        // Dimension notifications.
        private Color currentDimensionTint;
        private readonly DimensionIndicator dimensionIndicator;

        private readonly IInterfaceParticleEffect relicAuraEffect;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneInterfaceLayer"/> class.
        /// </summary>
        /// <param name="parentScene">The parent scene.</param>
        public ZoneInterfaceLayer(IScene parentScene)
            : base(
            parentScene, 
            "Zone Interface Layer", 
            "Interface layer for a zone.", 
            LayerType.Interface,
            2)
        {
            // Defer the Begin call of this Layer. This is done manually.
            this.Flags[EngineFlag.DeferredBegin] = true;

            // Hook up the PhaseChanged event from the parent ZoneScene; this helps us create some special UI effects when needed.
            PhaseManager.PlaneCoalesced += this.ZoneSceneOnPhaseChanged;
            PhaseManager.PlaneFolded += this.ZoneSceneOnPhaseChanged;
            Monde.GameManager.DimensionShifted += this.ZoneSceneOnDimensionShifted;

            this.relicAuraEffect = ParticleEffectFactory.GetInterfaceParticleEffect("Aura", ParticleDensity.High, Color.White);
            
            // Initialise interface elements.
            this.gameInterface = new GameInterface();
            this.riftGlyph = new RiftGlyph();
            this.phaseNotification = new PhaseNotification();
            this.dimensionIndicator = new DimensionIndicator();
            this.hearts = new List<HeartGlyph>
            {
                new HeartGlyph(new Vector2(20f, 56f)),
                new HeartGlyph(new Vector2(56f, 56f)),
                new HeartGlyph(new Vector2(92f, 56f))
            };
            this.riftTotal = new TextElement("Rift Total", "Notifies the player of the amount of rift they have collected.", "000", TextAlignment.Center, riftTotalPosition);
            this.scoreTotal = new TextElement("Score Total", "Notifies the player of the total score they have.", "000000", TextAlignment.Center, scoreTotalPosition);

            this.beginKey = this.Id + "_Begin";
            this.endKey = this.Id + "_End";
            this.progressPhaseBarKey = this.Id + "_ProgressPhaseBar";
            this.notifyNewPhaseKey = this.Id + "_NotifyNewPhase";
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //if (this.drawRelicTimer)
            //{
            //    barRect = relicBarColourRect;
            //    barRect.Width = (int)(barRect.Width * this.relicTimerBarWidthPercentage);
            //    spriteBatch.Draw(this.uiPartsTexture, this.relicTimerBarTransform.Position, relicBarRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            //    spriteBatch.Draw(this.uiPartsTexture, new Vector2(this.relicTimerBarTransform.Position.X + 4, this.relicTimerBarTransform.Position.Y + 4), barRect, this.relicTimerColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            //}

            base.Draw(gameTime, spriteBatch);

            // Determine the size of the phase bar.
            var barRect = phaseBarRect;
            barRect.Width = (int)(barRect.Width * this.phaseBarWidthPercentage);
            spriteBatch.Draw(TextureManager.Textures["UIParts"], phaseBarPosition, barRect, this.currentDimensionTint, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, Occasus.Core.Input.IInputState inputState)
        {
            base.Update(gameTime, inputState);

            this.scoreTotal.Text = Monde.GameManager.StatisticManager.TotalScore.ToString("d6");
            this.scoreTotal.Measure(scoreTotalPosition);
        }

        /// <summary>
        /// Loads content for the Engine Component.
        /// </summary>
        public override void LoadContent()
        {
            this.scoreTotal.Font = DrawingManager.ContentManager.Load<SpriteFont>("Graphics/Fonts/gamecounterfont");
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            this.currentDimensionTint = UniverseConstants.NormalDimensionColor;

            this.riftTotal.Opacity = 0f;
            this.scoreTotal.Opacity = 0f;

            base.Begin();

            // Hook up stuff.
            Monde.GameManager.RelicCollection.RelicsActivated += this.RelicCollectionOnRelicCollected;
            Monde.GameManager.Player.LifeGained += this.PlayerOnLifeGained;
            Monde.GameManager.Player.LifeLost += this.PlayerOnLifeLost;
            Monde.GameManager.Player.RiftCollected += PlayerOnRiftCollected;
            Monde.GameManager.Player.PhaseGem.ChargeGenerated += this.PhaseGemOnChargeGenerated;

            CoroutineManager.Add(this.beginKey, this.BeginEffect());
        }

        

        /// <summary>
        /// Ends the Engine Component.
        /// </summary>
        public override void End()
        {
            CoroutineManager.Remove(this.beginKey);
            CoroutineManager.Add(this.endKey, this.EndEffect());
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            PhaseManager.PlaneCoalesced -= this.ZoneSceneOnPhaseChanged;
            PhaseManager.PlaneFolded -= this.ZoneSceneOnPhaseChanged;
            Monde.GameManager.DimensionShifted -= this.ZoneSceneOnDimensionShifted;
            Monde.GameManager.RelicCollection.RelicsActivated -= this.RelicCollectionOnRelicCollected;
            Monde.GameManager.Player.LifeGained -= this.PlayerOnLifeGained;
            Monde.GameManager.Player.LifeLost -= this.PlayerOnLifeLost;
            Monde.GameManager.Player.RiftCollected -= PlayerOnRiftCollected;
        }

        protected override void InitializeEntityCache()
        {
            // Add the aura effect and update entity cache.
            this.AddEntity(this.relicAuraEffect);
            
            // Add main ui elements.
            this.AddEntity(this.gameInterface);
            this.AddEntity(this.riftGlyph);

            // Add heart element interface.
            foreach (var h in this.hearts)
            {
                this.AddEntity(h);
            }
            this.AddEntity(this.phaseNotification);
            this.AddEntity(this.dimensionIndicator);

            // Add text elements.
            this.AddEntity(this.riftTotal);
            this.AddEntity(this.scoreTotal);

            base.InitializeEntityCache();
        }

        private void ZoneSceneOnPhaseChanged(PhaseChangedEventArgs eventArgs)
        {
            // Handle special case phase changes.
            if (eventArgs.NewPhase == null)
            {
                return;
            }

            // Fire off a timer to determine the width of the phase bar.
            CoroutineManager.Add(this.progressPhaseBarKey, this.ProgressPhaseBar(eventArgs.NewPhase));

            // Start a new coroutine to animate a new phase starting.
            CoroutineManager.Add(this.notifyNewPhaseKey, this.NotifyNewPhase(eventArgs.NewPhase));
            
            if (eventArgs.NewPhase.Name.Equals(Phase.CursedShifts))
            {
                this.dimensionIndicator.ApplyCurse();
            }

            if (eventArgs.PreviousPhases == null)
            {
                return;
            }

            if (eventArgs.PreviousPhases.Any(o => o.Name.Equals(Phase.CursedShifts)))
            {
                this.dimensionIndicator.RemoveCurse();
            }
        }

        private void ZoneSceneOnDimensionShifted(DimensionShiftedEventArgs dimensionShiftedEventArgs)
        {
            this.currentDimensionTint = dimensionShiftedEventArgs.NewDimension == Dimension.Normal ? UniverseConstants.LimboDimensionColor: UniverseConstants.NormalDimensionColor;
        }

        private void PhaseGemOnChargeGenerated(object sender, EventArgs eventArgs)
        {
        }

        private void PlayerOnLifeLost(StatChangedEventArgs lifeChangedEventArgs)
        {
            this.hearts[lifeChangedEventArgs.NewLife].Break();
            DrawingManager.Camera.Flash(5, Color.Red);
        }

        private void PlayerOnLifeGained(StatChangedEventArgs lifeChangedEventArgs)
        {
            this.hearts[lifeChangedEventArgs.NewLife - 1].Mend();
        }

        private void RelicCollectionOnRelicCollected(RelicsActivatedEventArgs relicCollectedEventArgs)
        {
            this.drawRelicTimer = false;
            CoroutineManager.Remove(this.Id + "_RelicTimer");

            // Determine the correct relic timer color.
            //switch (relicCollectedEventArgs.Relic.RelicEffect)
            //{
            //    case RelicEffect.Cataclysm:
            //        this.relicTimerColor = Color.Red;
            //        break;
            //    case RelicEffect.Conduit:
            //        this.relicTimerColor = Color.Aqua;
            //        break;
            //    case RelicEffect.Invulnerability:
            //        this.relicTimerColor = Color.Green;
            //        break;
            //    case RelicEffect.Phaser:
            //        this.relicTimerColor = Color.Orange;
            //        break;
            //    case RelicEffect.Shield:
            //        this.relicTimerColor = Color.Purple;
            //        break;
            //    case RelicEffect.SlowTime:
            //        this.relicTimerColor = Color.Yellow;
            //        break;
            //}

            // Set the relic aura effect colour, then add a coroutine to handle the relic effects.
            //this.relicAuraEffect.Color = this.relicTimerColor;
            //CoroutineManager.Add(this.Id + "_RelicTimer", this.RelicTimerEffect(relicCollectedEventArgs.Relic.LifeSpan));
        }

        private void PlayerOnRiftCollected(StatChangedEventArgs statChangedEventArgs)
        {
            // Update the rift total element.
            riftTotal.Text = Monde.GameManager.Player.Rift.ToString("d3");
            riftTotal.Measure(riftTotalPosition);

            CoroutineManager.Add(this.RiftCollectedEffect());
        }

        private IEnumerator RiftCollectedEffect()
        {
            this.riftTotal.Color = UniverseConstants.NeutralColor;
            yield return this.riftTotal.Transform.Shake(2f, TimingHelper.GetFrameCount(0.2f));
            this.riftTotal.Color = Color.White;
        }

        private IEnumerator BeginEffect()
        {
            // Transition in the main UI.
            this.gameInterface.Begin();
            yield return Coroutines.Pause(TimingHelper.GetFrameCount(0.5f));
            this.riftGlyph.Begin();

            yield return Coroutines.Pause(TimingHelper.GetFrameCount(1f));
            
            // Transition each of the ui parts in.
            for (var i = 0; i < Monde.GameManager.Player.Lives; i++)
            {
                this.hearts[i].Begin();
                yield return Coroutines.Pause(5);
            }

            this.riftTotal.Measure(riftTotalPosition);
            yield return this.riftTotal.SetOpacity(1f, 20, Ease.QuadOut);

            this.scoreTotal.Measure(scoreTotalPosition);
            yield return this.scoreTotal.SetOpacity(1f, 20, Ease.QuadOut);

            this.dimensionIndicator.Begin();
        }

        private IEnumerator EndEffect()
        {
            // Transition the UI out.
            this.gameInterface.End();
            yield return Coroutines.Pause(TimingHelper.GetFrameCount(2f));

            // Call the base End method.
            base.End();
        }

        private IEnumerator ProgressPhaseBar(IPhase newPhase)
        {
            // Determine the number of frames that the phase requires to complete.
            while (true)
            {
                this.phaseBarWidthPercentage = newPhase.PercentComplete;
                yield return null;
            }
        }

        private IEnumerator NotifyNewPhase(IPhase newPhase)
        {
            this.phaseNotification.PhaseName = newPhase.Name;
            this.phaseNotification.FadeIn();

            // Leave the notification up for a short amount of time.
            yield return Coroutines.Pause(TimingHelper.GetFrameCount(5));

            this.phaseNotification.FadeOut();
        }

        private IEnumerator RelicTimerEffect(float duration)
        {
            //this.drawRelicTimer = true;
            //this.relicTimerBarWidthPercentage = 1f;

            // Transition the relic bar in.
            //yield return this.relicTimerBarTransform.MoveTo(new Vector2(184f, DrawingManager.WindowHeight - 50f), 1f);

            // Make the aura effect emit.
            this.relicAuraEffect.Emit(duration);
            yield return null;

            // Determine the number of frames that the phase requires to complete.
            //var framesLeft = (float)Math.Round(duration / Engine.DeltaTime);
            //var elapsedFrames = 0;

            //while (elapsedFrames <= framesLeft)
            //{
            //    this.relicTimerBarWidthPercentage = 1 - (elapsedFrames / framesLeft);
            //    yield return null;
            //    elapsedFrames++;
            //}

            //yield return this.relicTimerBarTransform.MoveTo(new Vector2(184f, DrawingManager.WindowHeight), 0.5f);
            //this.drawRelicTimer = false;
        }
    }
}

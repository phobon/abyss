using System;
using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Drawing.ParticleEffects.Concrete;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Interface;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using Occasus.Core.States;
using System.Collections;

namespace Abyss.World.Scenes.Zone.Layers.Interface
{
    public class DimensionIndicator : InterfaceElement
    {
        private const string DeactivatedState = "Deactivated";
        private const string ActivatingState = "Activating";
        private const string ActivatedState = "Activated";
        private const string CursedState = "Cursed";

        private const string RechargingState = "Recharging";
        private const string InertState = "Inert";

        private const string PatternFadeEffectKey = "DimensionIndicator_PatternFadeEffect";

        private const string GemLayerKey = "gem";
        private const string Pattern1LayerKey = "pattern1";
        private const string Pattern2LayerKey = "pattern2";
        private const string Pattern3LayerKey = "pattern3";
        private const string Pattern4LayerKey = "pattern4";

        private readonly float[] patternOpacities = { 0f, 0f, 0f, 0f };

        private static readonly Vector2 initialPosition = new Vector2(280f, DrawingManager.ScaledWindowHeight - 133f);

        private readonly string shakeKey;

        private bool fadeInEffectsActivated;

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionIndicator"/> class.
        /// </summary>
        public DimensionIndicator()
            : base(
            "DimensionIndicatorElement",
            "Dimension indicator element.",
            initialPosition)
        {
            this.shakeKey = this.Id + "_Shake";
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            // Update sprite layer opacities.
            var sprite = this.GetSprite();
            sprite.Layers[Pattern1LayerKey].Opacity = this.patternOpacities[0];
            sprite.Layers[Pattern2LayerKey].Opacity = this.patternOpacities[1];
            sprite.Layers[Pattern3LayerKey].Opacity = this.patternOpacities[2];
            sprite.Layers[Pattern4LayerKey].Opacity = this.patternOpacities[3];
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            Monde.GameManager.Player.PhaseGem.ChargeGenerated += PhaseGemOnChargeGenerated;
            Monde.GameManager.Player.PhaseGem.ChargeUsed += PhaseGemOnChargeUsed;

            this.StateChanged += delegate(StateChangedEventEventArgs args)
            {
                if (args.OldState.Equals(CursedState))
                {
                    if (this.Components.ContainsKey(Curse.ComponentName))
                    {
                        this.Components[Curse.ComponentName].Suspend();
                    }

                    CoroutineManager.Remove(this.Id + "_Shake");

                    this.Transform.Position = initialPosition;
                }
            };
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            Monde.GameManager.Player.PhaseGem.ChargeGenerated -= PhaseGemOnChargeGenerated;
            Monde.GameManager.Player.PhaseGem.ChargeUsed -= PhaseGemOnChargeUsed;
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();
            this.CurrentState = ActivatingState;
        }

        /// <summary>
        /// Fades the phase notification bar in.
        /// </summary>
        public void FadeIn()
        {
            this.CurrentState = ActivatingState;
        }

        /// <summary>
        /// Fades the out.
        /// </summary>
        public void FadeOut()
        {
            this.CurrentState = DeactivatedState;
        }

        /// <summary>
        /// Applies a curse to this treasure chest.
        /// </summary>
        public void ApplyCurse()
        {
            this.CurrentState = CursedState;
        }

        /// <summary>
        /// Removes the curse from this treasure chest.
        /// </summary>
        public void RemoveCurse()
        {
            this.CurrentState = ActivatedState;
        }

        protected override void SetupStates()
        {
            base.SetupStates();

            var deactivatedState = new State(DeactivatedState, this.DeactivatedEffect(), false);
            this.States.Add(DeactivatedState, deactivatedState);

            var activatingState = new State(ActivatingState, this.ActivatingEffect(), false);
            this.States.Add(ActivatingState, activatingState);

            this.States.Add(ActivatedState, new State(ActivatingState, this.ActivatedEffect(), false));
            this.States.Add(CursedState, new State(CursedState, this.CursedEffect(), false));

            this.States.Add(InertState, new State(InertState, this.GemDimensionEffect("inert"), false));
            this.States.Add(RechargingState, new State(RechargingState, this.RechargingEffect(), false));

            this.CurrentState = DeactivatedState;
        }

        protected override void InitializeFlags()
        {
            this.Flags[EngineFlag.DeferredBegin] = true;
        }

        protected override void InitializeSprite()
        {
            base.InitializeSprite();

            var sprite = this.GetSprite();
            this.Transform.Scale = Vector2.One * DrawingManager.WindowScale;
            sprite.Opacity = 0f;
            sprite.Layers[GemLayerKey].Opacity = 0f;

            // Set up various layer opacity.
            sprite.Layers[Pattern1LayerKey].Opacity = 0f;
            sprite.Layers[Pattern2LayerKey].Opacity = 0f;
            sprite.Layers[Pattern3LayerKey].Opacity = 0f;
            sprite.Layers[Pattern4LayerKey].Opacity = 0f;
        }

        protected override void InitializeComponents()
        {
            var curseEffect = ParticleEffectFactory.GetParticleEffect(
                this,
                Curse.ComponentName,
                new Vector2(50f, -10f));
            curseEffect.Scale = Vector2.One * 4;
            curseEffect.MaximumParticles = 10;

            this.AddComponent(Curse.ComponentName, curseEffect);
        }

        private IEnumerable ActivatedEffect()
        {
            CoroutineManager.Add(this.FadeInGem());
            this.CurrentState = InertState;
            yield return null;
        }

        private IEnumerator FadeInGem()
        {
            var elapsedFrames = 0f;
            var totalFrames = 20f;
            var sprite = this.GetSprite();
            while (elapsedFrames <= totalFrames)
            {
                sprite.Layers[GemLayerKey].Opacity = elapsedFrames / totalFrames;
                elapsedFrames++;
                yield return null;
            }

            if (!this.fadeInEffectsActivated)
            {
                CoroutineManager.Add(PatternFadeEffectKey + "0", PatternFadeEffect(0));
                yield return Coroutines.Pause(MathsHelper.Random(15, 30));
                CoroutineManager.Add(PatternFadeEffectKey + "1", PatternFadeEffect(2));
                yield return Coroutines.Pause(MathsHelper.Random(15, 30));
                CoroutineManager.Add(PatternFadeEffectKey + "2", PatternFadeEffect(1));
                yield return Coroutines.Pause(MathsHelper.Random(15, 30));
                CoroutineManager.Add(PatternFadeEffectKey + "3", PatternFadeEffect(3));

                this.fadeInEffectsActivated = true;
            }
        }

        private IEnumerable ActivatingEffect()
        {
            // Fade this UI element in.
            this.Resume();
            yield return this.SetOpacity(1f, 60, Ease.QuadIn);
            this.CurrentState = ActivatedState;
        }

        private IEnumerable DeactivatedEffect()
        {
            // Fade this UI element out.
            yield return this.SetOpacity(0f, 30, Ease.QuadIn);
            this.Suspend();
        }

        private IEnumerable GemDimensionEffect(string dimension)
        {
            var sprite = this.GetSprite();
            sprite.Layers[GemLayerKey].IsVisible = true;
            sprite.Layers[Pattern1LayerKey].IsVisible = true;
            sprite.Layers[Pattern2LayerKey].IsVisible = true;
            sprite.Layers[Pattern3LayerKey].IsVisible = true;
            sprite.Layers[Pattern4LayerKey].IsVisible = true;
            sprite.GoToAnimation(dimension);
            yield return null;
        }

        private IEnumerator PatternFadeEffect(int index)
        {
            var elapsedFrames = 0;
            var totalFrames = MathsHelper.Random(30, 60);
            while (true)
            {
                while (elapsedFrames <= totalFrames)
                {
                    this.patternOpacities[index] = (float)elapsedFrames / (float)totalFrames;
                    elapsedFrames++;
                    yield return null;
                }

                elapsedFrames = 0;

                while (elapsedFrames <= totalFrames)
                {
                    this.patternOpacities[index] = 1 - (float)elapsedFrames / (float)totalFrames;
                    elapsedFrames++;
                    yield return null;
                }

                elapsedFrames = 0;
            }
        }

        private IEnumerable CursedEffect()
        {
            var sprite = this.GetSprite();
            sprite.Color = new Color(54, 0, 66);

            // Emit the curse effect and shake this thing about.
            ((IParticleEffect)this.Components[Curse.ComponentName]).Emit();

            CoroutineManager.Add(this.shakeKey, this.Transform.Shake(1f, TimingHelper.GetFrameCount(999f)));
            yield return null;
        }

        private IEnumerable RechargingEffect()
        {
            // TODO: Animation for dimension shifting
            var sprite = this.GetSprite();
            sprite.Layers[GemLayerKey].IsVisible = false;
            sprite.Layers[Pattern1LayerKey].IsVisible = false;
            sprite.Layers[Pattern2LayerKey].IsVisible = false;
            sprite.Layers[Pattern3LayerKey].IsVisible = false;
            sprite.Layers[Pattern4LayerKey].IsVisible = false;
            yield return null;
        }

        private void PhaseGemOnChargeUsed(object sender, EventArgs eventArgs)
        {
            if (Monde.GameManager.Player.PhaseGem.Charges == 0)
            {
                this.CurrentState = RechargingState;
            }
        }

        private void PhaseGemOnChargeGenerated(object sender, EventArgs eventArgs)
        {
            this.CurrentState = ActivatedState;
        }
    }
}

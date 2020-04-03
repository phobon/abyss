using System;
using Abyss.World.Entities.Player.Components;
using Abyss.World.Entities.Props;
using Abyss.World.Entities.Relics;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using Occasus.Core.Physics;

namespace Abyss.World.Entities.Player
{
    /// <summary>
    /// Interface for the Player in Abyss. The Player is the protagonist of the game and is the main controllable Actor
    /// </summary>
    public interface IPlayer : IEntity
    {
        /// <summary>
        /// Occurs when the player gains a life.
        /// </summary>
        event StatChangedEventHandler LifeGained;

        /// <summary>
        /// Occurs when the player loses a life.
        /// </summary>
        event StatChangedEventHandler LifeLost;

        /// <summary>
        /// Occurs when the player collects rift.
        /// </summary>
        event StatChangedEventHandler RiftCollected;

        /// <summary>
        /// Occurs when the player stomps a monster.
        /// </summary>
        event EventHandler MonsterStomped;

        /// <summary>
        /// Occurs when the player stomps a platform.
        /// </summary>
        event PlatformStompedEventHandler PlatformStomped;

        /// <summary>
        /// Gets the Phase Gem that the player uses to switch dimensions.
        /// </summary>
        IPhaseGem PhaseGem { get; }

        /// <summary>
        /// Gets the barrier that protects the player.
        /// </summary>
        IBarrier Barrier { get; }

        /// <summary>
        /// Gets or sets the Player's current Rift.
        /// </summary>
        /// <remarks>
        /// Rift is the Player's main resource. It is used to buy items and other things.
        /// </remarks>
        int Rift { get; set; }

        /// <summary>
        /// Gets or sets the maximum rift a player can carry.
        /// </summary>
        /// <value>
        /// The maximum rift.
        /// </value>
        int MaximumRift { get; set; }

        /// <summary>
        /// Gets or sets the maximum lives.
        /// </summary>
        /// <value>
        /// The maximum lives.
        /// </value>
        int MaximumLives { get; set; }

        /// <summary>
        /// Gets or sets the Player's remaining lives.
        /// </summary>
        /// <value>
        /// Lives are another of the Player's main resources. When the Player runs out of lives, the game is over.
        /// </value>
        int Lives { get; set; }

        /// <summary>
        /// Gets or sets the Player's movement speed.
        /// </summary>
        /// <value>
        /// The movement speed.
        /// </value>
        int MovementSpeed { get; set; }

        /// <summary>
        /// Gets or sets the Player's fall speed.
        /// </summary>
        /// <value>
        /// The fall speed.
        /// </value>
        int FallSpeed { get; set; }

        /// <summary>
        /// Gets or sets the time the player can spend in Limbo.
        /// </summary>
        /// <value>
        /// The time that the player can spend in Limbo.
        /// </value>
        int LimboTime { get; set; }

        /// <summary>
        /// Gets or sets the current prop.
        /// </summary>
        /// <value>
        /// The current prop.
        /// </value>
        IActiveProp CurrentProp { get; set; }

        /// <summary>
        /// Player takes damage.
        /// </summary>
        /// <param name="damageSource">The damage source.</param>
        /// <returns>
        /// True if the player takes damage; otherwise, false.
        /// </returns>
        /// <remarks>
        /// Depending on the current PlayerFlags, this method can spawn off a variety of different outcomes.
        /// </remarks>
        bool TakeDamage(string damageSource);

        /// <summary>
        /// Collects the relic.
        /// </summary>
        /// <param name="relic">The relic.</param>
        void CollectRelic(IRelic relic);

        /// <summary>
        /// Player stomps the ground or a monster.
        /// </summary>
        void Stomp(CollisionEventArgs args);

        /// <summary>
        /// Gains invulnerability for an amount of time.
        /// </summary>
        /// <param name="totalFrames">The total frames.</param>
        void GainInvulnerability(int totalFrames);

        /// <summary>
        /// Handles the input.
        /// </summary>
        /// <param name="inputState">State of the input.</param>
        void HandleInput(IInputState inputState);

        void ShowOutline(Color color, bool pulse);

        void HideOutline();

        //bool MoveLeft();

        //bool MoveRight();
    }
}

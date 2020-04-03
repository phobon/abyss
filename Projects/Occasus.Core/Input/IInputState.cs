using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Occasus.Core.Input
{
    public interface IInputState
    {
        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        void Update();

        /// <summary>
        /// Gets a GamePadState for the specified player.
        /// </summary>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <returns>GamePadState</returns>
        GamePadState GetGamePadState(PlayerIndex controllingPlayer);

        /// <summary>
        /// Helper for checking if a key is being continuously pressed during this update. A "new" keypress, cannot be a continuous keypress.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns><c>True</c> if [is key is is pressed]; otherwise, <c>false</c>.</returns>
        bool IsKeyPressed(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex);

        /// <summary>
        /// Helper for checking if a button is being continuously pressed during this update. A "new" button press, cannot be a continuous button press.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns><c>True</c> if [is button is pressed]; otherwise, <c>false</c>.</returns>
        bool IsButtonPressed(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex);

        /// <summary>
        /// Helper for checking if a key was newly pressed during this update. The controlling Player parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a key press is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns><c>True</c> if [is new key press] [the specified key]; otherwise, <c>false</c>.</returns>
        bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex);

        /// <summary>
        /// Helper for checking if a button was newly pressed during this update. The controllingPlayer parameter specifies which player to read input for. If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns><c>True</c> if [is new button press] [the specified button]; otherwise, <c>false</c>.</returns>
        bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex);
    }
}

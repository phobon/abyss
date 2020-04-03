using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Occasus.Core.Input
{
    /// <summary>
    /// Helper for reading input from keyboard, gamepad, and touch input. This class tracks both the current and previous state of the input devices, and implements 
    /// query methods for high level input actions such as "move up through the menu" or "pause the game".
    /// </summary>
    public class InputState : IInputState
    {
        public const int MaxInputs = 4;

        private readonly KeyboardState[] currentKeyboardStates;
        private readonly GamePadState[] currentGamePadStates;
        private readonly KeyboardState[] lastKeyboardStates;
        private readonly GamePadState[] lastGamePadStates;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputState"/> class.
        /// </summary>
        public InputState()
        {
            this.currentKeyboardStates = new KeyboardState[MaxInputs];
            this.currentGamePadStates = new GamePadState[MaxInputs];

            this.lastKeyboardStates = new KeyboardState[MaxInputs];
            this.lastGamePadStates = new GamePadState[MaxInputs];

            this.GamePadWasConnected = new bool[MaxInputs];
        }

        /// <summary>
        /// Gets the game pad was connected.
        /// </summary>
        public bool[] GamePadWasConnected
        {
            get; private set;
        }

        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                this.lastKeyboardStates[i] = this.currentKeyboardStates[i];
                this.lastGamePadStates[i] = this.currentGamePadStates[i];

                this.currentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                this.currentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                // Keep track of whether a gamepad has ever been connected, so we can detect if it is unplugged.
                if (this.currentGamePadStates[i].IsConnected)
                {
                    this.GamePadWasConnected[i] = true;
                }
            }
        }

        public GamePadState GetGamePadState(PlayerIndex controllingPlayer)
        {
            var playerIndex = controllingPlayer;
            var i = (int)playerIndex;

            return this.currentGamePadStates[i];
        }

        /// <summary>
        /// Helper for checking if a key is being continuously pressed during this update. A "new" keypress, cannot be a continuous keypress.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns><c>True</c> if [is key is is pressed]; otherwise, <c>false</c>.</returns>
        public bool IsKeyPressed(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (!controllingPlayer.HasValue)
            {
                // Accept input from any player.
                return this.IsKeyPressed(key, PlayerIndex.One, out playerIndex) ||
                       this.IsKeyPressed(key, PlayerIndex.Two, out playerIndex) ||
                       this.IsKeyPressed(key, PlayerIndex.Three, out playerIndex) ||
                       this.IsKeyPressed(key, PlayerIndex.Four, out playerIndex);
            }

            // Read input from the specified player.
            playerIndex = controllingPlayer.Value;
            var i = (int)playerIndex;

            return this.currentKeyboardStates[i].IsKeyDown(key);
        }

        /// <summary>
        /// Helper for checking if a key was newly pressed during this update. The controlling Player parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a key press is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns><c>True</c> if [is new key press] [the specified key]; otherwise, <c>false</c>.</returns>
        public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (!controllingPlayer.HasValue)
            {
                // Accept input from any player.
                return this.IsNewKeyPress(key, PlayerIndex.One, out playerIndex) ||
                       this.IsNewKeyPress(key, PlayerIndex.Two, out playerIndex) ||
                       this.IsNewKeyPress(key, PlayerIndex.Three, out playerIndex) ||
                       this.IsNewKeyPress(key, PlayerIndex.Four, out playerIndex);
            }

            // Read input from the specified player.
            playerIndex = controllingPlayer.Value;
            var i = (int)playerIndex;

            return this.currentKeyboardStates[i].IsKeyDown(key) && this.lastKeyboardStates[i].IsKeyUp(key);
        }

        /// <summary>
        /// Helper for checking if a button is being continuously pressed during this update. A "new" button press, cannot be a continuous button press.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns><c>True</c> if [is button is pressed]; otherwise, <c>false</c>.</returns>
        public bool IsButtonPressed(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (!controllingPlayer.HasValue)
            {
                // Accept input from any player.
                return this.IsButtonPressed(button, PlayerIndex.One, out playerIndex) ||
                       this.IsButtonPressed(button, PlayerIndex.Two, out playerIndex) ||
                       this.IsButtonPressed(button, PlayerIndex.Three, out playerIndex) ||
                       this.IsButtonPressed(button, PlayerIndex.Four, out playerIndex);
            }

            // Read input from the specified player.
            playerIndex = controllingPlayer.Value;
            var i = (int)playerIndex;

            return this.currentGamePadStates[i].IsButtonDown(button);
        }

        /// <summary>
        /// Helper for checking if a button was newly pressed during this update. The controllingPlayer parameter specifies which player to read input for. If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns><c>True</c> if [is new button press] [the specified button]; otherwise, <c>false</c>.</returns>
        public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (!controllingPlayer.HasValue)
            {
                // Accept input from any player.
                return this.IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                       this.IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                       this.IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                       this.IsNewButtonPress(button, PlayerIndex.Four, out playerIndex);
            }

            // Read input from the specified player.
            playerIndex = controllingPlayer.Value;
            var i = (int)playerIndex;

            return this.currentGamePadStates[i].IsButtonDown(button) && this.lastGamePadStates[i].IsButtonUp(button);
        }

        /// <summary>
        /// Checks for a "menu select" input action. The controllingPlayer parameter specifies which player to read input for. If this is null, it will accept input from any player.
        /// When the action is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns><c>True</c> if [is menu select] [the specified controlling player]; otherwise, <c>false</c>.</returns>
        public bool IsMenuSelect(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            return this.IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
                   this.IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex) ||
                   this.IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex) ||
                   this.IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Checks for a "menu cancel" input action. The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When the action is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns><c>True</c> if [is menu cancel] [the specified controlling player]; otherwise, <c>false</c>.</returns>
        public bool IsMenuCancel(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            return this.IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                   this.IsNewButtonPress(Buttons.B, controllingPlayer, out playerIndex) ||
                   this.IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Checks for a "menu up" input action. The controllingPlayer parameter specifies which player to read input for. If this is null, it will accept input from any player.
        /// </summary>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <returns><c>True</c> if [is menu up] [the specified controlling player]; otherwise, <c>false</c>.</returns>
        public bool IsMenuUp(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return this.IsNewKeyPress(Keys.Up, controllingPlayer, out playerIndex) ||
                   this.IsNewButtonPress(Buttons.DPadUp, controllingPlayer, out playerIndex) ||
                   this.IsNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Checks for a "menu down" input action. The controllingPlayer parameter specifies which player to read input for. If this is null, it will accept input from any player.
        /// </summary>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <returns>
        ///   <c>true</c> if [is menu down] [the specified controlling player]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMenuDown(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return this.IsNewKeyPress(Keys.Down, controllingPlayer, out playerIndex) ||
                   this.IsNewButtonPress(Buttons.DPadDown, controllingPlayer, out playerIndex) ||
                   this.IsNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Checks for a "pause the game" input action. The controllingPlayer parameter specifies which player to read input for. If this is null, it will accept input from any player.
        /// </summary>
        /// <param name="controllingPlayer">The controlling player.</param>
        /// <returns>
        ///   <c>true</c> if [is pause game] [the specified controlling player]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPauseGame(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return this.IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                   this.IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex) ||
                   this.IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }
    }
}

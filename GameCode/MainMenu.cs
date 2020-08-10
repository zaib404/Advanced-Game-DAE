using Engine.UI;
using Microsoft.Xna.Framework;
using System;

namespace GameCode
{
    /// <summary>
    /// Main Menu players select how many are competing 
    /// </summary>
    class MainMenu
    {
        #region Data Members

        IInteractiveUI Button2Players { get; set; }
        IInteractiveUI Button3Players { get; set; }
        IInteractiveUI Button4Players { get; set; }

        // events
        LoadUIButtons loadButtons;
        UnloadUIButtons unloadButtons;
        StartLevel startLevel;

        #endregion

        /// <summary>
        /// Main Menu constructor 
        /// </summary>
        /// <param name="pLoadButton">Event for talking to game main to load ui buttons</param>
        /// <param name="pUnloadButton">To remove buttons and unsub</param>
        /// <param name="pStartLevel"> when to start level</param>
        public MainMenu(LoadUIButtons pLoadButton, UnloadUIButtons pUnloadButton, StartLevel pStartLevel)
        {
            loadButtons = pLoadButton;
            unloadButtons = pUnloadButton;
            startLevel = pStartLevel;

            LoadButtons();
        }

        /// <summary>
        /// Load the UI Buttons
        /// </summary>
        void LoadButtons()
        {
            Button2Players = loadButtons?.Invoke(Button2Players, "Buttons/2player-button", new Vector2(200, 400));
            Button3Players = loadButtons?.Invoke(Button3Players, "Buttons/3player-button", new Vector2(700, 400));
            Button4Players = loadButtons?.Invoke(Button4Players, "Buttons/4player-button", new Vector2(1200, 400));

            // Sub on click event
            Button2Players.OnClick += Button2Players_OnClick;
            Button3Players.OnClick += Button3Players_OnClick;
            Button4Players.OnClick += Button4Players_OnClick;
        }

        private void Button2Players_OnClick(object sender, EventArgs e)
        {
            // start level with 2 players
            startLevel?.Invoke(2);
        }

        private void Button3Players_OnClick(object sender, EventArgs e)
        {
            startLevel?.Invoke(3);
        }

        private void Button4Players_OnClick(object sender, EventArgs e)
        {
            startLevel?.Invoke(4);
        }

        /// <summary>
        /// Remove UI buttons from screen
        /// </summary>
        public void RemoveUI()
        {
            Button4Players.OnClick -= Button4Players_OnClick;
            Button3Players.OnClick -= Button3Players_OnClick;
            Button2Players.OnClick -= Button2Players_OnClick;

            unloadButtons?.Invoke(Button2Players);
            unloadButtons?.Invoke(Button3Players);
            unloadButtons?.Invoke(Button4Players);
        }
    }
}
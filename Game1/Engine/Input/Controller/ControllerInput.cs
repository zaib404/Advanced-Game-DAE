using System.Collections.Generic;
using Engine.Managers;
using Microsoft.Xna.Framework.Input;

namespace Engine.Input
{
    public class ControllerInput : iControllerObserverable, iManager
    {
        static List<EntityButton> m_entityButtonList = new List<EntityButton>();
        static List<iControllerObserver> m_subList = new List<iControllerObserver>();

        private static Dictionary<int, iControllerObserver> playerDict = new Dictionary<int, iControllerObserver>();
        private const int maxControllers = 4;

        /// <summary>
        /// Struct containing list of buttons and the entities uid
        /// </summary>
        private struct EntityButton
        {
            public int uid;
            public List<Buttons> buttons;

            public EntityButton(int id, List<Buttons> button)
            {
                uid = id;
                buttons = button;
            }
        }
        

        public ControllerInput()
        {

        }

        /// <summary>
        /// Subscribe to controller input events
        /// </summary>
        /// <param name="sub">The instance subscribing</param>
        /// <param name="buttons">A list of buttons it cares about</param>
        /// <param name="playerCount">The player number</param>
        public static void Subscribe(iControllerObserver sub, List<Buttons> buttons, int playerCount)
        {
            m_subList.Add(sub);
            m_entityButtonList.Add(new EntityButton(0, buttons));
            playerDict.Add(playerCount, sub);
        }

        /// <summary>
        /// Polls connected controllers for input and notifies each subscriber of any changes
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < maxControllers; i++)
            {
                if (GamePad.GetCapabilities(i).IsConnected)
                {
                    GamePadState gamePadState = GamePad.GetState(i);

                    foreach (EntityButton sub in m_entityButtonList)
                    {
                        foreach (Buttons button in sub.buttons)
                        {
                            if (gamePadState.IsButtonDown(button))
                            {
                                notifyGamePadInput(i, button, gamePadState.ThumbSticks);
                            }
                            else if(gamePadState.ThumbSticks.Left.X != 0 || gamePadState.ThumbSticks.Left.Y != 0)
                            {
                                notifyGamePadInput(i, 0, gamePadState.ThumbSticks);
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Notifies the subscriber to input
        /// </summary>
        /// <param name="playerIndex">The index of the player to alert</param>
        /// <param name="gamePadButtons">The button state</param>
        /// <param name="thumbSticks">The thumbstick properties for the controller</param>
        public void notifyGamePadInput(int playerIndex, Buttons gamePadButtons, GamePadThumbSticks thumbSticks)
        {
            if(playerIndex < playerDict.Count)
            {
                playerDict[playerIndex].gamePadInput(gamePadButtons, thumbSticks);
            }
            
        }
    }
}

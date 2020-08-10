using Microsoft.Xna.Framework.Input;

namespace Engine.Input
{
    public interface iControllerObserverable
    {
        void notifyGamePadInput(int playerIndex, Buttons gamePadButtons, GamePadThumbSticks thumbSticks);
    }
}

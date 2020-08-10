using Microsoft.Xna.Framework.Input;

namespace Engine.Input
{
    public interface iControllerObserver
    {
        void gamePadInput(Buttons gamePadButtons, GamePadThumbSticks thumbSticks);
    }
}

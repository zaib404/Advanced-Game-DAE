using Microsoft.Xna.Framework.Input;
using System;

namespace Engine.Input
{
    public interface IKeyboardInputObserver
    {
        void input(Keys keys);
    }
}

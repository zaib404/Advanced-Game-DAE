using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Input
{
    public interface IMouseInputObserver
    {
        void mouseInput(Vector2 pos, bool pressed);
    }
}

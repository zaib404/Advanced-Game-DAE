using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Input
{
    interface IMouseInputObservable
    {
        void notifyInput(Vector2 pos, bool pressed);
    }
}

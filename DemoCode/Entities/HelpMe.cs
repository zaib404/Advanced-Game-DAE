using System;
using Engine.Entity;
using Engine.Input;
using Engine.UI;
using Microsoft.Xna.Framework;

namespace GameCode.Entities
{
    public class HelpMe : GameEntity, IMouseInputObserver, IInteractiveUI
    {

        public event EventHandler<EventArgs> OnClick;
        public event EventHandler<EventArgs> OnHover;

        public HelpMe()
        {
            Transparency = 0.5f;
        }


        public void mouseInput(Vector2 pos, bool pressed)
        {
        //    if (HitBox.Intersects(new Rectangle((int)pos.X, (int)pos.Y, 1, 1)))
        //    {
        //        Hovering();

        //        if (pressed)
        //        {
        //            Clicked();
        //        }
        //    }
        //    else
        //    {
        //        Transparency = 1f;
        //    }
        }

        public virtual void Clicked()
        {
            OnClick?.Invoke(this, new EventArgs());
        }

        //Not sure if this is really needed?
        public virtual void Hovering()
        {
            Transparency = 0.5f;
            OnHover?.Invoke(this, new EventArgs());
        }
    }
}


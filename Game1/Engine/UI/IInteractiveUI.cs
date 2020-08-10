using System;

namespace Engine.UI
{
    public interface IInteractiveUI
    {
        event EventHandler<EventArgs> OnClick;
        event EventHandler<EventArgs> OnHover;
    }
}

using Engine.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode
{
    public delegate IInteractiveUI LoadUIButtons(IInteractiveUI pButton, string pTex, Vector2 pPos);
    public delegate void UnloadUIButtons(IInteractiveUI pButton);
    public delegate void StartLevel(int num = 0);
}

using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using sfColor = SFML.Graphics.Color;

namespace Editor
{
    static class PublicVariables
    {
        static public RenderWindow renderwindow = null;

        static public void DispatchEvents()
        {
            renderwindow.DispatchEvents();
        }
        static public void ClearRender()
        {
            renderwindow.Clear();
            renderwindow.Display();
            renderwindow.Clear();
            renderwindow.Display();
        }
        static public void ClearRender(sfColor color)
        {
            renderwindow.Clear(color);
            renderwindow.Display();
            renderwindow.Clear(color);
            renderwindow.Display();
        }
        static public void Draw(Sprite sp)
        {
            renderwindow.Draw(sp);
            renderwindow.Display();
            renderwindow.Draw(sp);
            renderwindow.Display();
        }
        static public void Draw(Text txt)
        {
            renderwindow.Draw(txt);
            renderwindow.Display();
            renderwindow.Draw(txt);
            renderwindow.Display();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Util
{
    public static class ViewExtensions
    {
        public static SFML.Graphics.FloatRect GetBroadBounds(this SFML.Graphics.View view)
        {
            var rt = new SFML.Graphics.FloatRect();

            var a = view.Size.X / 2;
            var b = view.Size.Y / 2;

            float c = (float)Math.Sqrt(a * a + b * b);

            rt.Left = view.Center.X - c;
            rt.Top = view.Center.Y - c;
            rt.Width = c * 2;
            rt.Height = c * 2;
            return rt;
        }
    }
}

using SFXT;
using SFXT.Components.Graphics;
using SFXT.Util;
using System;

namespace SFXT_Test
{
    public class ConsoleInfoEntity : Entity
    {
        private SFML.Graphics.Text text;
        private uint fontSize;
        public ConsoleInfoEntity(Activity activity) : base(activity)
        {
            this.fontSize = 18;
            this.text = new SFML.Graphics.Text("", new SFML.Graphics.Font("ARCADECLASSIC.TTF"), this.fontSize);
            this.text.FillColor = SFML.Graphics.Color.Green;
            var drawable = new RawSFMLDrawable(this, this.text);
            drawable.Layer = int.MaxValue;
            this.AddComponent(drawable);
        }

        public override void Update()
        {
            this.activity.CameraManager.Update();
            //this.text.Position = (Vector2)this.activity.CameraManager.GetCamera().View.Center - (Vector2)(this.activity.Game.Dimensions / 2);
            this.text.Position = (Vector2)this.activity.CameraManager.GetCamera().View.Center - ((Vector2)(this.activity.CameraManager.GetCamera().Size / 2) * this.activity.CameraManager.GetCamera().CameraHeight);
            this.text.CharacterSize = (uint)(this.fontSize * this.activity.CameraManager.GetCamera().CameraHeight);

            string toDisplay = "";
            toDisplay += "Draw Calls Last Frame: " + Debug.DrawCalls.ToString() + "\n";
            toDisplay += "Total Frame Count: " + Debug.FrameCount + "\n";
            toDisplay += "Total Run Time: " + activity.Game.GameTime.ElapsedTime.AsSeconds().ToString() + "s\n";
            toDisplay += "Average Update Time: " + activity.Game.AverageUpdateTime.AsMilliseconds().ToString() + "ms\n";
            toDisplay += "Average Frame Time: " + activity.Game.AverageRenderTime.AsMilliseconds().ToString() + "ms";

            this.text.DisplayedString = toDisplay;
        }
    }
}

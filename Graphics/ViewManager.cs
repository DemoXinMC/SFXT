using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Graphics
{
    public class ViewManager
    {
        private Activity activity;
        private Dictionary<string, Camera> cameras;

        private Dictionary<Camera, Entity> chaseCams;
        private Dictionary<Camera, CameraShake> shakyCams;

        public Camera[] Views
        {
            get
            {
                Camera[] ret = new Camera[this.cameras.Count];
                this.cameras.Values.CopyTo(ret, 0);
                return ret;
            }
        }

        public ViewManager(Activity activity)
        {
            this.activity = activity;

            cameras = new Dictionary<string, Camera>();
            cameras.Add("Default", new Camera(this.activity.Game.Window.Size.X, this.activity.Game.Window.Size.Y));

            chaseCams = new Dictionary<Camera, Entity>();
        }

        public void Update()
        {
            foreach(var chaseCam in chaseCams)
            {
                chaseCam.Key.View.Center = chaseCam.Value.Position;
            }

            foreach(var shakyCam in shakyCams)
            {
                // do camera shake here
            }
        }

        public Camera GetCamera(string name)
        {
            var exists = cameras.TryGetValue(name, out Camera ret);

            if (exists) return ret;
            return null;
        }

        public void StartChaseCam(Camera camera, Entity target)
        {
            if (camera == null || target == null)
                return;
            this.chaseCams.Add(camera, target);
        }

        public void StopChaseCam(Camera camera)
        {
            if (this.chaseCams.ContainsKey(camera))
                this.chaseCams.Remove(camera);
        }

        public class Camera
        {
            public SFML.Graphics.View View { get; private set; }
            public SFML.Graphics.RenderStates RenderStates { get; private set; }
            public SFML.Graphics.RenderTexture RenderTexture { get; private set; }

            public Camera(uint width, uint height)
            {
                this.View = new SFML.Graphics.View(new SFML.System.Vector2f(0, 0), new SFML.System.Vector2f(width, height));
                this.RenderStates = new SFML.Graphics.RenderStates();
                this.RenderTexture = new SFML.Graphics.RenderTexture(width, height);
            }
        }

        public class CameraShake
        {
            public uint Intensity;
            public SFML.System.Time Duration;
        }
    }
}

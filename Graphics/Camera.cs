using SFML.System;
using SFXT.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFXT.Graphics
{
    public class CameraManager
    {
        private Activity activity;
        private Dictionary<string, Camera> cameras;

        private Dictionary<Camera, Entity> chaseCams;
        private Dictionary<Camera, CameraShake> shakyCams;
        private Dictionary<Camera, CameraMotion> movingCams;

        public Camera[] Views
        {
            get
            {
                Camera[] ret = new Camera[this.cameras.Count];
                this.cameras.Values.CopyTo(ret, 0);
                return ret;
            }
        }

        public CameraManager(Activity activity)
        {
            this.activity = activity;

            cameras = new Dictionary<string, Camera>();
            cameras.Add("Default", new Camera((uint)this.activity.Game.Dimensions.X, (uint)this.activity.Game.Dimensions.Y));

            chaseCams = new Dictionary<Camera, Entity>();
            shakyCams = new Dictionary<Camera, CameraShake>();
            movingCams = new Dictionary<Camera, CameraMotion>();
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

            foreach(var movingCam in movingCams)
            {

            }
        }

        public Camera GetCamera(string name = "")
        {
            if(name == "")
            {
                if (this.cameras.Count > 0)
                    return this.cameras.First().Value;
                return null;
            }

            var exists = cameras.TryGetValue(name, out Camera ret);

            if (exists) return ret;
            return null;
        }

        public void StartChaseCam(Camera camera, Entity target)
        {
            if (camera == null || target == null)
                return;
            if (this.chaseCams.ContainsKey(camera))
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
                this.RenderStates = new SFML.Graphics.RenderStates(SFML.Graphics.RenderStates.Default);
                this.RenderTexture = new SFML.Graphics.RenderTexture(width, height);
                this.RenderTexture.SetView(this.View);
            }

            public void MoveTo(float x, float y)
            {
                this.View.Center = new SFML.System.Vector2f(x, y);
            }

            public void MoveTo(Entity entity)
            {
                this.View.Center = entity.Position;
            }

            public void Move(double x, double y)
            {
                this.Move(new Vector2(x, y));
            }

            public void Move(Vector2 vector)
            {
                this.View.Center = this.View.Center + (Vector2f)vector;
            }
        }

        public class CameraMotion
        {
            public Vector2 Destination;
            public double Angle;
            public SFML.System.Time Duration;
            public SFML.System.Clock Timer;
        }

        public class CameraShake
        {
            public uint Intensity;
            public SFML.System.Time Duration;
            public SFML.System.Clock Timer;
        }
    }
}

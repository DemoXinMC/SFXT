using SFML.System;
using SFXT.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SFXT
{
    public class Game
    {
        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                this.Window?.SetTitle(value);
            }
        }
        private string title;

        /// <summary>The raw SFML Window for the Game.</summary>
        public SFML.Graphics.RenderWindow Window { get; private set; }

        public bool Fullscreen
        {
            get
            {
                return this.fullscreen;
            }
            set
            {
                var oldValue = this.fullscreen;
                this.fullscreen = value;

                if(this.fullscreen != oldValue)
                    this.CreateWindow();
            }
        }
        private bool fullscreen;

        /// <summary>The Height/Width of the Game Window</summary>
        public Vector2 Dimensions { get; private set; }
        /// <summary>The X/Y position of the Game Window</summary>
        public SFML.System.Vector2i Location { get; private set; }

        /// <summary>The amount of seconds between this update and the last</summary>
        public double Delta { get; private set; }
        /// <summary>The amount of seconds between this frame and the last logic update</summary>
        public double Interpolation { get; private set; }

        /// <summary>The root directory to the game.</summary>
        public string GameDirectory;
        /// <summary></summary>
        public string SaveDirectory;
        /// <summary></summary>
        public string AssetDirectory;
        /// <summary>Change the Root directory of the game.  This is also change the Save and Asset Directories when used.</summary>
        public void SetRootDir(string dir = null)
        {
            if(dir == null)
                this.GameDirectory = Directory.GetCurrentDirectory();
            else
            {
                var fileAttr = File.GetAttributes(dir);
                if((fileAttr & FileAttributes.Directory) != FileAttributes.Directory)
                    throw new ArgumentException("Not a directory.", "dir");
            }
            this.SaveDirectory = Path.Combine(this.GameDirectory, "save");
            this.AssetDirectory = Path.Combine(this.GameDirectory, "assets");
        }

        /// <summary>The target number of Updates per second. <para>Default: 25</para></summary>
        public uint TPS;
        /// <summary>The target number of frames to Render per second. <para>Default: 60</para></summary>
        public uint FPS
        {
            get { return this.fps; }
            set
            {
                this.fps = value;
                if(value > 0)
                    this.Window?.SetFramerateLimit(value);
            }
        }
        private uint fps;

        /// <summary>The Time object representing the amount of time the most recent Update took.</summary>
        public SFML.System.Time UpdateTime { get { return updateTimes.Peek(); } }
        private Queue<SFML.System.Time> updateTimes;
        /// <summary>The Time object representing the amount of time the most recent Render took.</summary>
        public SFML.System.Time RenderTime { get { return renderTimes.Peek(); } }
        private Queue<SFML.System.Time> renderTimes;

        /// <summary>The Time object representing how long the game has been running.</summary>
        public SFML.System.Clock GameTime;

        public SFML.System.Time AverageUpdateTime
        {
            get
            {
                var totalTime = new SFML.System.Time();
                foreach(var time in updateTimes)
                    totalTime += time;

                return totalTime / updateTimes.Count;
            }
        }
        public SFML.System.Time AverageRenderTime
        {
            get
            {
                var totalTime = new SFML.System.Time();
                foreach(var time in renderTimes)
                    totalTime += time;

                return totalTime / updateTimes.Count;
            }
        }

        public readonly bool Headless;

        /// <summary>
        /// The main game loop.  Call to begin the game.
        /// </summary>
        public void Run()
        {
            this.CreateWindow();

            var timeSecond = SFML.System.Time.FromSeconds(1);
            var updateClock = new SFML.System.Clock();
            var renderClock = new SFML.System.Clock();

            this.GameTime = new Clock();
            this.GameTime.Restart();
            updateClock.Restart();
            var updateTime = this.GameTime.ElapsedTime;

            var tickTimer = (timeSecond / this.TPS);

            Console.WriteLine("Desired Tick Rate: " + tickTimer.AsMilliseconds());

            while (this.Window != null)
            {
                while(this.GameTime.ElapsedTime > updateTime)
                {
                    this.updateTimes.Enqueue(updateClock.ElapsedTime);
                    this.Delta = updateClock.ElapsedTime.AsSeconds();
                    updateClock.Restart();
                    this.Update();
                    updateTime += tickTimer;
                }

                renderClock.Restart();
                this.Interpolation = updateClock.ElapsedTime.AsSeconds();
                this.Render();
                this.renderTimes.Enqueue(renderClock.Restart());

                while(this.updateTimes.Count > this.TPS * 10)
                    this.updateTimes.Dequeue();
                while(this.renderTimes.Count > this.FPS * 10)
                    this.renderTimes.Dequeue();
            }
        }

        public Game()
        {
            this.activities = new Stack<Activity>();
            this.renderTimes = new Queue<Time>();
            this.updateTimes = new Queue<Time>();

            this.Window = null;
            this.Dimensions = new Vector2i(360, 240);
            this.Title = "SFXT Game";
            this.Headless = false;

            this.TPS = 20;
            this.FPS = 60;

            this.SetRootDir(null);
        }

        public Game(string title, uint width, uint height) : this(title, width, height, 25, 60) { }

        public Game(string title, uint width, uint height, uint targetTPS, uint targetFPS) : this()
        {
            this.Title = title;
            this.Dimensions = new Vector2i((int)width, (int)height);

            this.TPS = targetTPS;
            this.FPS = targetFPS;
        }

        private Stack<Activity> activities;

        public delegate void ActivityHandler(Activity activity);
        public event ActivityHandler OnActivityPush;
        public event ActivityHandler OnActivityPop;

        public void PushActivity(Activity activity)
        {
            this.OnActivityPush?.Invoke(activity);
            this.activities.Push(activity);
        }

        public Activity PopActivity()
        {
            var activity = this.activities.Pop();
            this.OnActivityPop?.Invoke(activity);

            return activity;
        }

        public Activity PopTo(Activity target)
        {
            if (!this.activities.Contains(target))
                return null;

            while(this.activities.Peek() != target)
            {
                var popped = this.activities.Pop();
                this.OnActivityPop?.Invoke(popped);
            }

            return this.activities.Peek();
        }

        private void CreateWindow()
        {
            this.Window?.Close();

            var videoMode = new SFML.Window.VideoMode((uint)this.Dimensions.X, (uint)this.Dimensions.Y);
            this.Window = new SFML.Graphics.RenderWindow(videoMode, this.Title, this.Fullscreen ? SFML.Window.Styles.Fullscreen : SFML.Window.Styles.None);
            this.Window.Position = this.Location == null ? this.Location : new Vector2i((int)Math.Floor(((double)SFML.Window.VideoMode.DesktopMode.Width / 2) - (videoMode.Width / 2)), (int)Math.Floor(((double)SFML.Window.VideoMode.DesktopMode.Height / 2) - (videoMode.Height / 2)));

            if(this.FPS > 0)
                this.Window.SetFramerateLimit(this.FPS);

            this.WindowCreated?.Invoke(videoMode.Width, videoMode.Height);
            this.Window.Display();
        }

        private void Update()
        {
            /*
            if(this.activities.Count == 0)
                throw new ApplicationException("SFXT Update requires an active Activity.");*/

            if(this.Window == null || !this.Window.IsOpen)
                throw new ApplicationException("SFXT Update cannot begin without a valid Window.");

            this.Window.DispatchEvents();

            /*
             * this.OnInputBegin();
             * this.Input.Update();
             * this.OnInputEnd();
             */

            this.OnUpdateBegin?.Invoke();

            var updateList = new Stack<Activity>();

            foreach(var activity in this.activities)
            {
                updateList.Push(activity);
                if(!activity.ShouldBelowActivitiesUpdate())
                    break;
            }

            foreach(var activity in updateList.Reverse())
                activity.Update();

            this.OnUpdateEnd?.Invoke();
        }

        private void Render()
        {
            if(this.Window == null || !this.Window.IsOpen)
                throw new ApplicationException("SFXT Render cannot begin without a valid Window.");

            this.OnRenderBegin?.Invoke();

            var renderList = new Stack<Activity>();

            foreach(var activity in this.activities)
            {
                renderList.Push(activity);
                if(!activity.ShouldBelowActivitiesRender())
                    break;
            }

            this.Window.Clear(new SFML.Graphics.Color(100, 149, 237));

            foreach (var activity in renderList.Reverse())
                activity.Render(this.Window, SFML.Graphics.RenderStates.Default);

            /*
            var drawable = new SFML.Graphics.CircleShape(30);
            drawable.FillColor = SFML.Graphics.Color.Red;
            drawable.Position = this.Window.GetView().Center;
            this.Window.Draw(drawable);
            */

            this.OnRenderEnd?.Invoke();
            this.Window.Display();
        }

        public delegate void WindowCreatedHandler(uint width, uint height);
        public event WindowCreatedHandler WindowCreated;

        public delegate void UpdateHandler();
        public event UpdateHandler OnUpdateBegin;
        public event UpdateHandler OnUpdateEnd;

        public delegate void RenderHandler();
        public event RenderHandler OnRenderBegin;
        public event RenderHandler OnRenderEnd;
    }
}

using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFXT.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SFXT
{
    public class Game
    {
        /// <summary>
        /// The Title of the Game's Window
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                Window?.SetTitle(value);
            }
        }
        private string _title;

        /// <summary>The raw SFML Window for the Game.</summary>
        public SFML.Graphics.RenderWindow Window { get; private set; }

        /// <summary>
        /// Whether the Game is running in Fullscreen mode.
        /// </summary>
        public bool Fullscreen
        {
            get
            {
                return _fullscreen;
            }
            set
            {
                var oldValue = _fullscreen;
                _fullscreen = value;

                if(_fullscreen != oldValue)
                    CreateWindow();
            }
        }
        private bool _fullscreen;

        /// <summary>The Height/Width of the Game Window</summary>
        public Vector2 Dimensions { get; private set; }
        /// <summary>The X/Y position of the Game Window</summary>
        public Vector2 Location { get; private set; }

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
                GameDirectory = Directory.GetCurrentDirectory();
            else
            {
                var fileAttr = File.GetAttributes(dir);
                if((fileAttr & FileAttributes.Directory) != FileAttributes.Directory)
                    throw new ArgumentException("Not a directory.", "dir");
            }
            SaveDirectory = Path.Combine(GameDirectory, "save");
            AssetDirectory = Path.Combine(GameDirectory, "assets");
        }

        /// <summary>The target number of Updates per second. <para>Default: 25</para></summary>
        public uint TPS
        {
            get { return _tps; }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Tick Rate cannot be less than 1.", "FPS");
                _tps = value;
            }
        }
        private uint _tps;

        /// <summary>The target number of frames to Render per second. <para>Default: 60</para></summary>
        public uint FPS
        {
            get { return _fps; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Target FPS cannot be less than 0.", "FPS");
                _fps = value;
                Window?.SetFramerateLimit(value);
            }
        }
        private uint _fps;

        /// <summary>The Time object representing the amount of time the most recent Update took.</summary>
        public Time UpdateTime { get { return updateTimes.Reverse().First(); } }
        private Queue<Time> updateTimes;
        /// <summary>The Time object representing the amount of time the most recent Render took.</summary>
        public Time RenderTime { get { return renderTimes.Reverse().First(); } }
        private Queue<Time> renderTimes;

        /// <summary>The Time object representing how long the game has been running.</summary>
        public Clock GameTime;

        public Time AverageUpdateTime
        {
            get
            {
                var totalTime = new Time();
                foreach(var time in updateTimes)
                    totalTime += time;

                if (updateTimes.Count == 0)
                    return Time.Zero;

                return totalTime / updateTimes.Count;
            }
        }
        public Time AverageRenderTime
        {
            get
            {
                var totalTime = new Time();
                foreach(var time in renderTimes)
                    totalTime += time;

                if (renderTimes.Count == 0)
                    return Time.Zero;

                return totalTime / renderTimes.Count;
            }
        }

        public readonly bool Headless;

        /// <summary>
        /// The main Game loop.  Call to begin the game.
        /// </summary>
        public void Run()
        {
            CreateWindow();

            var timeSecond = Time.FromSeconds(1);
            var updateClock = new Clock();
            var renderClock = new Clock();

            GameTime = new Clock();
            GameTime.Restart();
            updateClock.Restart();
            var updateTime = GameTime.ElapsedTime;

            var tickTimer = (timeSecond / TPS);

            while (Window != null)
            {
                while(GameTime.ElapsedTime > updateTime)
                {  
                    Delta = updateClock.ElapsedTime.AsSeconds();
                    updateClock.Restart();
                    Update();
                    updateTimes.Enqueue(updateClock.ElapsedTime);
                    updateTime += tickTimer;
                }

                
                Interpolation = updateClock.ElapsedTime.AsSeconds();
                renderClock.Restart();
                Debug.FrameCount++;
                Render();
                renderTimes.Enqueue(renderClock.Restart());

                while(updateTimes.Peek() < GameTime.ElapsedTime - Time.FromSeconds(5))
                    updateTimes.Dequeue();
                while(this.renderTimes.Peek() < GameTime.ElapsedTime - Time.FromSeconds(5))
                    renderTimes.Dequeue();
            }
        }

        public Game()
        {
            activities = new Stack<Activity>();
            renderTimes = new Queue<Time>();
            updateTimes = new Queue<Time>();

            Window = null;
            Dimensions = new Vector2i(360, 240);
            Title = "SFXT Game";
            Headless = false;

            TPS = 20;
            FPS = 60;

            SetRootDir(null);
        }

        public Game(string title, uint width, uint height) : this(title, width, height, 25, 60) { }

        public Game(string title, uint width, uint height, uint targetTPS, uint targetFPS) : this()
        {
            Title = title;
            Dimensions = new Vector2i((int)width, (int)height);

            TPS = targetTPS;
            FPS = targetFPS;
        }

        private Stack<Activity> activities;

        public delegate void ActivityHandler(Activity activity);
        public event ActivityHandler OnActivityPush;
        public event ActivityHandler OnActivityPop;

        public void PushActivity(Activity activity)
        {
            OnActivityPush?.Invoke(activity);
            activities.Push(activity);
        }

        public Activity PopActivity()
        {
            var activity = activities.Pop();
            OnActivityPop?.Invoke(activity);

            return activity;
        }

        public Activity PopTo(Activity target)
        {
            if (!activities.Contains(target))
                return null;

            while(activities.Peek() != target)
            {
                var popped = activities.Pop();
                OnActivityPop?.Invoke(popped);
            }

            return activities.Peek();
        }

        private void CreateWindow()
        {
            Window?.Close();

            var videoMode = new SFML.Window.VideoMode((uint)this.Dimensions.X, (uint)this.Dimensions.Y);
            Window = new SFML.Graphics.RenderWindow(videoMode, this.Title, this.Fullscreen ? SFML.Window.Styles.Fullscreen : SFML.Window.Styles.None);
            Window.Position = this.Location == null ? this.Location : new Vector2i((int)Math.Floor(((double)SFML.Window.VideoMode.DesktopMode.Width / 2) - (videoMode.Width / 2)), (int)Math.Floor(((double)SFML.Window.VideoMode.DesktopMode.Height / 2) - (videoMode.Height / 2)));

            if(FPS > 0)
                Window.SetFramerateLimit(FPS);

            WindowCreated?.Invoke(videoMode.Width, videoMode.Height);
            Window.Display();
        }

        private void Update()
        {
            
            if(this.activities.Count == 0)
                throw new ApplicationException("SFXT Update requires an active Activity.");

            if (!Headless)
            {
                if (Window == null || !Window.IsOpen)
                    throw new ApplicationException("SFXT Update cannot begin without a valid Window.");

                Window.DispatchEvents();

                /*
                 * this.OnInputBegin();
                 * this.Input.Update();
                 * this.OnInputEnd();
                 */
            }

            OnUpdateBegin?.Invoke();

            var updateList = new Stack<Activity>();

            foreach(var activity in activities)
            {
                updateList.Push(activity);
                if(!activity.ShouldBelowActivitiesUpdate())
                    break;
            }

            foreach(var activity in updateList.Reverse())
                activity.Update();

            OnUpdateEnd?.Invoke();
        }

        private void Render()
        {
            if(Window == null || !Window.IsOpen)
                throw new ApplicationException("SFXT Render cannot begin without a valid Window.");

            OnRenderBegin?.Invoke();

            var renderList = new Stack<Activity>();

            foreach(var activity in activities)
            {
                renderList.Push(activity);
                if(!activity.ShouldBelowActivitiesRender())
                    break;
            }

            Window.Clear(new SFML.Graphics.Color(100, 149, 237));

            foreach (var activity in renderList.Reverse())
                activity.Render(Window, SFML.Graphics.RenderStates.Default);

            OnRenderEnd?.Invoke();
            Window.Display();
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

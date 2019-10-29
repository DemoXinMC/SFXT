using System;
using System.Collections.Generic;
using System.IO;
using SFML.Graphics;
using SFML.Window;
using SFXT;
using SFXT.Components.Graphics;
using SFXT.Graphics;
using SFXT.Graphics.Texels;
using SFXT.Loaders;
using SFXT.Util;

namespace SFXT_Test
{
    class Program
    {
        private static Entity chaseEnt;
        private static Game game;
        private static Activity activity;

        static void Main(string[] args)
        {
            game = new Game("My SFXT Playground", 640, 480, 60, 60);
            activity = new Activity(game);
            game.PushActivity(activity);

            var texturePacker = new TexturePacker(4096);

            /*
            var basicSpriteTexture = new SFML.Graphics.Texture("BasicSprite.png");
            ITexels basicSpriteTexels = new TexelsTexture(basicSpriteTexture);

            var animatedSpriteTexture = new SFML.Graphics.Texture("AnimatedSprite.png");
            ITexels animatedSpriteTexels = new TexelsTexture(animatedSpriteTexture);
            */

            var basicSpriteTexels = texturePacker.PackTexture(new SFML.Graphics.Texture("BasicSprite.png"));
            var animatedSpriteTexels = texturePacker.PackTexture(new SFML.Graphics.Texture("AnimatedSprite.png"));

            var playerEntity = new TestEntity(activity);

            //var lpcTexture = new TexelsTexture(new SFML.Graphics.Texture("LPCSprite.png"));
            var lpcTexture = texturePacker.PackTexture(new SFML.Graphics.Texture("LPCSprite.png"));

            var playerSprite = LPC.Create(playerEntity, lpcTexture);
            var runAnimation = new AnimatedSprite.LoopingAnimation();

            playerEntity.Scale = 1f;
            playerEntity.AddComponent(playerSprite);
            playerEntity.IsPlayerControlled = true;

            playerSprite.Play(LPC.IdleLoop);
            
            activity.AddEntity(playerEntity);

            activity.AddEntity(new ConsoleInfoEntity(activity));

            var mapEntity = new MapEntity(activity);
            mapEntity.Scale = 2;
            mapEntity.Rotation = 20.0f;
            var tilesheet = new TexelsTexture(new SFML.Graphics.Texture("tilesheet.png"));
            var map = new Tilemap(mapEntity, 64, 64, tilesheet);

            uint[][] tiles = new uint[25][];

            for(uint x = 0; x < 25; x++)
            {
                tiles[x] = new uint[25];

                for (int y = 0; y < 25; y++)
                    tiles[x][y] = 18;
            }

            for(int x = 0; x < 25; x++)
            {
                tiles[x][0] = 1;
                tiles[x][24] = 35;
            }

            for (int y = 0; y < 25; y++)
            {
                tiles[0][y] = 17;
                tiles[24][y] = 19;
            }

            tiles[0][0] = 0;
            tiles[24][0] = 2;
            tiles[0][24] = 34;
            tiles[24][24] = 36;

            map.SetTileData(tiles);

            mapEntity.AddComponent(map);
            mapEntity.Layer--;

            activity.AddEntity(mapEntity);

            var rng = new Random();

            string[] filePaths = Directory.GetFiles("LPC-Prefabs");

            var textureList = new List<ITexels>();

            foreach (var file in filePaths)
                textureList.Add(texturePacker.PackTexture(new Texture(file)));

            for (int i = 0; i < 5000; i++)
            {
                var newEnt = new TestEntity(activity);
                newEnt.AddComponent(LPC.Create(newEnt, textureList[rng.Next(textureList.Count)]));
                newEnt.Position = map.GetRandomPoint(rng);
                activity.AddEntity(newEnt);
            }

            chaseEnt = playerEntity;
            activity.OnEntityAdded += CameraSetup;

            game.OnUpdateBegin += HandleFirstUpdate;

            game.Run();
        }

        private static void HandleFirstUpdate()
        {
            game.Window.MouseWheelScrolled += HandleMouseWheel;
            game.OnUpdateBegin -= HandleFirstUpdate;
        }

        private static void HandleMouseWheel(object sender, MouseWheelScrollEventArgs e)
        {
            activity.CameraManager.GetCamera().CameraHeight -= e.Delta;
        }

        private static void CameraSetup(Entity entity)
        {
            if (chaseEnt == null) return;
            if (chaseEnt != entity) return;

            activity.CameraManager.StartChaseCam(activity.CameraManager.GetCamera(), entity);
            activity.CameraManager.GetCamera().CameraHeight = 3;
        }
    }
}

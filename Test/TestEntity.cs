using SFXT;
using SFXT.Components.Graphics;
using SFXT.Graphics;
using SFXT.Graphics.Texels;
using SFXT.Loaders;
using SFXT.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT_Test
{
    public class TestEntity : SFXT.Entity
    {
        public Vector2 Velocity;

        private Vector2 destination;
        private int movementCooldown;

        public bool IsPlayerControlled;
        public TestEntity(Activity activity) : base(activity)
        {
            this.Velocity = new Vector2(0, 0);
            this.destination = null;
            this.movementCooldown = 0;
            this.IsPlayerControlled = false;
        }

        public override void Update()
        {
            base.Update();

            if(this.IsPlayerControlled)
            {
                this.Velocity = new Vector2(0, 0);

                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.W))
                    this.Velocity.Y = -2;
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.S))
                    this.Velocity.Y = 2;
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.D))
                    this.Velocity.X = 2;
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.A))
                    this.Velocity.X = -2;

                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.LShift))
                    this.Velocity *= 2;
            }
            else
            {
                if(this.destination == null)
                {
                    this.movementCooldown--;
                    if(this.movementCooldown <= 0)
                    {
                        var rng = new System.Random();
                        var mapEntity = this.activity.GetEntity<MapEntity>();
                        var newDest = mapEntity.GetRandomPoint();
                        var x = rng.Next((int)this.Position.X - 500, (int)this.Position.X + 500);
                        var y = rng.Next((int)this.Position.Y - 500, (int)this.Position.Y + 500);
                        this.destination = new Vector2(newDest.X, newDest.Y);
                    }
                }

                if (this.destination != null)
                {
                    this.Velocity = this.destination - this.Position;
                    if (this.Velocity.Magnitude < 2)
                    {
                        this.Velocity = new Vector2(0, 0);
                        this.destination = null;
                        this.movementCooldown = new System.Random().Next(300, 3000);
                    }
                    else
                    {
                        this.Velocity.Normalize();
                        this.Velocity *= 1.2;
                    }
                }
            }

            this.Position += this.Velocity;

            var sprite = this.GetComponent<AnimatedSprite>();

            if (sprite == null)
                return;

            if (this.Velocity.Magnitude > 0)
            {
                var direction = Math.Round(4 * (Math.Atan2(this.Velocity.Y, this.Velocity.X) / (2 * Math.PI) + 4)) % 4;
                switch (direction)
                {
                    case 3:
                        if (sprite.CurrentAnimation != LPC.WalkUp)
                            sprite.Play(LPC.WalkUp);
                        break;
                    case 0:
                        if (sprite.CurrentAnimation != LPC.WalkRight)
                            sprite.Play(LPC.WalkRight);
                        break;
                    case 1:
                        if (sprite.CurrentAnimation != LPC.WalkDown)
                            sprite.Play(LPC.WalkDown);
                        break;
                    case 2:
                        if (sprite.CurrentAnimation != LPC.WalkLeft)
                            sprite.Play(LPC.WalkLeft);
                        break;
                }
            }

            if (this.Velocity.Magnitude == 0 && sprite.CurrentAnimation != LPC.IdleLoop)
                sprite.Play(LPC.IdleLoop);
        }
    }
}

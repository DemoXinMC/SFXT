using SFXT.Components.Graphics;
using SFXT.Graphics;
using System;
using static SFXT.Components.Graphics.AnimatedSprite;

namespace SFXT.Loaders
{
    public static class LPC
    {
        public readonly static SFML.System.Time FrameTime;

        public readonly static LoopingAnimation IdleLoop;

        public readonly static LoopingAnimation WalkUp;
        public readonly static LoopingAnimation WalkDown;
        public readonly static LoopingAnimation WalkLeft;
        public readonly static LoopingAnimation WalkRight;

        public readonly static Animation SlashUp;
        public readonly static LoopingAnimation SlashUpLoop;
        public readonly static Animation SlashDown;
        public readonly static LoopingAnimation SlashDownLoop;
        public readonly static Animation SlashLeft;
        public readonly static LoopingAnimation SlashLeftLoop;
        public readonly static Animation SlashRight;
        public readonly static LoopingAnimation SlashRightLoop;

        public readonly static Animation ThrustUp;
        public readonly static LoopingAnimation ThrustUpLoop;
        public readonly static Animation ThrustDown;
        public readonly static LoopingAnimation ThrustDownLoop;
        public readonly static Animation ThrustLeft;
        public readonly static LoopingAnimation ThrustLeftLoop;
        public readonly static Animation ThrustRight;
        public readonly static LoopingAnimation ThrustRightLoop;

        public readonly static Animation ShootUp;
        public readonly static LoopingAnimation ShootUpLoop;
        public readonly static Animation ShootDown;
        public readonly static LoopingAnimation ShootDownLoop;
        public readonly static Animation ShootLeft;
        public readonly static LoopingAnimation ShootLeftLoop;
        public readonly static Animation ShootRight;
        public readonly static LoopingAnimation ShootRightLoop;

        public readonly static Animation SpellcastUp;
        public readonly static LoopingAnimation SpellcastUpLoop;
        public readonly static Animation SpellcastDown;
        public readonly static LoopingAnimation SpellcastDownLoop;
        public readonly static Animation SpellcastLeft;
        public readonly static LoopingAnimation SpellcastLeftLoop;
        public readonly static Animation SpellcastRight;
        public readonly static LoopingAnimation SpellcastRightLoop;

        public static readonly Graphics.Texels.FrameHelper StandardFrameHelper;
        public static readonly Graphics.Texels.FrameHelper OversizedFrameHelper;


        public readonly static Animation Hurt;

        static LPC()
        {
            FrameTime = SFML.System.Time.FromSeconds(1 / 15);

            IdleLoop = new LoopingAnimation(FrameTime, (13 * 10), 6);

            WalkUp = new LoopingAnimation(FrameTime, (13 * 8), 9);
            WalkDown = new LoopingAnimation(FrameTime, (13 * 10) , 9);
            WalkLeft = new LoopingAnimation(FrameTime, (13 * 9) , 9);
            WalkRight = new LoopingAnimation(FrameTime, (13 * 11) , 9);

            SlashUp = new Animation(FrameTime, (13 * 12), 6);
            SlashUpLoop = new LoopingAnimation(FrameTime, (13 * 12));
            SlashDown = new Animation(FrameTime, (13 * 14) , 6);
            SlashDownLoop = new LoopingAnimation(FrameTime, (13 * 14), 6);
            SlashLeft = new Animation(FrameTime, (13 * 13), 6);
            SlashLeftLoop = new LoopingAnimation(FrameTime, (13 * 13), 6);
            SlashRight = new Animation(FrameTime, (13 * 15), 6);
            SlashRightLoop = new LoopingAnimation(FrameTime, (13 * 15), 6);

            ThrustUp = new Animation(FrameTime, 13 * 4, 8);
            ThrustUpLoop = new LoopingAnimation(ThrustUp);
            ThrustDown = new Animation(FrameTime, 13 * 6, 8);
            ThrustDownLoop = new LoopingAnimation(ThrustDown);
            ThrustLeft = new Animation(FrameTime, 13 * 5, 8);
            ThrustLeftLoop = new LoopingAnimation(ThrustLeft);
            ThrustRight = new Animation(FrameTime, 13 * 7, 8);
            ThrustRightLoop = new LoopingAnimation(ThrustRight);

            ShootUp = new Animation(FrameTime, (13 * 16), 13);
            ShootUpLoop = new LoopingAnimation(ShootUp);
            ShootDown = new Animation(FrameTime, (13 * 18), 13);
            ShootDownLoop = new LoopingAnimation(ShootDown);
            ShootLeft = new Animation(FrameTime, (13 * 17), 13);
            ShootLeftLoop = new LoopingAnimation(ShootLeft);
            ShootRight = new Animation(FrameTime, (13 * 19), 13);
            ShootRightLoop = new LoopingAnimation(ShootRight);

            SpellcastUp = new Animation(FrameTime, 0, 7);
            SpellcastUpLoop = new LoopingAnimation(SpellcastUp);
            SpellcastDown = new Animation(FrameTime, 13 * 1, 7);
            SpellcastDownLoop = new LoopingAnimation(SpellcastDown);
            SpellcastLeft = new Animation(FrameTime, 13 * 2, 7);
            SpellcastLeftLoop = new LoopingAnimation(SpellcastLeft);
            SpellcastRight = new Animation(FrameTime, 13 * 3, 7);
            SpellcastRightLoop = new LoopingAnimation(SpellcastRight);

            StandardFrameHelper = new Graphics.Texels.FrameHelper(64, 64);
            OversizedFrameHelper = new Graphics.Texels.FrameHelper(64, 64);
        }

        public static AnimatedSprite Create(Entity entity, ITexels texture)
        {
            SpritesheetStyle type = SpritesheetStyle.Standard;

            var ret = new AnimatedSprite(entity);

            switch(type)
            {    
                case SpritesheetStyle.Oversized:
                    ret.SetTextureData(texture, OversizedFrameHelper);
                    break;
                default:
                    ret.SetTextureData(texture, StandardFrameHelper);
                    break;
            }

            return ret;
        }

        public enum SpritesheetStyle
        {
            Standard,
            Oversized
        }
    }
}

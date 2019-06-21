using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT
{
    public static class Debug
    {
        public static uint DrawCalls
        {
            get => internalDrawCalls;
            set => internalDrawCalls++;
        }
        private static uint internalDrawCalls;

        public static uint FrameCount
        {
            get => internalFrameCount;
            set
            {
                internalFrameCount++;
                internalDrawCalls = 0;
            }
        }
        private static uint internalFrameCount;
    }
}

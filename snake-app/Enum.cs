using System;
using System.Collections.Generic;
using System.Text;

namespace snake_app
{
    public static class Enum
    {
        public enum Direction
        {
            Up = 0,
            Down,
            Left,
            Right
        }

        public enum BodyPart
        {
            None = 0,
            Head,
            Body,
            Tail,
            Point

        }
    }
}

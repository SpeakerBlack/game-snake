using System;
using System.Collections.Generic;
using System.Text;
using static snake_app.Enum;

namespace snake_console.Models
{
    public class Snake
    {
        public int Length { get; set; }
        public DrawPoint[] Trail { get; set; }
        public Direction CurrentDirection { get; set; }
        public DrawPoint CurrentPosition { get; set; }

        public Snake(int length)
        {
            Length = length;
            CurrentDirection = Direction.Left;
            CurrentPosition = new DrawPoint(40, 40);
            Trail = new DrawPoint[Length];
        }
    }
}

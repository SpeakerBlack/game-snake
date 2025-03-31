using System;
using System.Collections.Generic;
using System.Text;

namespace snake_console.Models
{
    public class DrawPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public DrawPoint() { }
        public DrawPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}

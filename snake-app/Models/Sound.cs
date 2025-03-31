using System;
using System.Collections.Generic;
using System.Text;

namespace snake_console.Models
{
    public class Sound
    {
        public int Duration { get; set; }
        public int Frequency { get; set; }
        public Sound(int frequency, int duration)
        {
            this.Frequency = frequency;
            this.Duration = duration;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static snake_app.Enum;

namespace snake_app
{
    public class Render
    {
        private BodyPart[,] Canvas { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }
        private int GameWidth { get; set; }
        private int GameHeight { get; set; }
        private int Scale { get; set; }
        private Bitmap RenderBitmap { get; set; }

        public Render(int width, int height, int scale)
        {
            RenderBitmap = new Bitmap(width, height);
            GameWidth = (width / scale) - 1;
            GameHeight = (height / scale) - 1;
            Width = width;
            Height = height;
            Scale = scale;
        }

        public void SetCanvas(BodyPart[,] canvas)
        {
            Canvas = canvas;
        }

        public Bitmap RenderCanvas()
        {
            for (int h = 0; h < GameHeight; h++)
            {
                for (int w = 0; w < GameWidth; w++)
                {
                    BodyPart canvasElement = Canvas[h, w];
                    if (!object.ReferenceEquals(null, canvasElement))
                    {
                        switch (canvasElement)
                        {
                            case BodyPart.Head:
                                RenderPoint(h, w, Scale, Color.DarkRed);
                                break;
                            case BodyPart.Body:
                                RenderPoint(h, w, Scale, Color.Black);
                                break;
                            case BodyPart.Tail:
                                RenderPoint(h, w, Scale, Color.DarkBlue);
                                break;
                            case BodyPart.Point:
                                RenderPoint(h, w, Scale, Color.Gold);
                                break;
                            case BodyPart.None:
                            default:
                                break;
                        }
                    }
                }
            }

            return RenderBitmap;
        }

        private void RenderPoint(int height, int width, int scale, Color color)
        {
            for (int i = 0; i < scale; i++)
            {
                for (int j = 0; j < scale; j++)
                {
                    RenderBitmap.SetPixel(
                        (height * scale) + i,
                        (width * scale) + j,
                        color);
                }
            }
        }

        public void ClearCanvas()
        {
            RenderBitmap = new Bitmap(Width, Height);
        }
    }
}

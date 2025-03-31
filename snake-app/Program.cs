using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake_app
{
    class Program
    {
        static Render Render;
        static Game Game;
        static PictureBox Canvas; // Objecto donde se va mostrar la imagen generada
        static Form Window; // EL objecto ventana de windows
        static int DeltaTime = 100; // Cada cuanto se va ejecutar un frame 
        static int SizeGame = 500; // Tamaño del juego o del mapa
        static int Scale = 10; // La escala con la que vamos a renderizar nuestra matriz

        static void Main(string[] args)
        {
            Render = new Render(SizeGame, SizeGame, Scale);
            Game = new Game(SizeGame, SizeGame, Scale, DeltaTime);
            CreateWindow();
            Game.Initialize();
            System.Threading.Tasks.Task.Run(() =>
            {
                while (true)
                {
                    Render.SetCanvas(Game.GetCanvas());
                    Canvas.Image = Render.RenderCanvas();
                    Render.ClearCanvas();
                    System.Threading.Thread.Sleep(DeltaTime);
                }
            });
            Application.Run(Window);
        }

        public static void CreateWindow() // Metodo para inicializar la ventana de windows
        {
            Canvas = new PictureBox()
            {
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Location = new System.Drawing.Point(0, 0),
                Size = new System.Drawing.Size(SizeGame, SizeGame)
            };
            Window = new Form()
            {
                Text = "Snake Game!",
                StartPosition = FormStartPosition.CenterScreen,
                ClientSize = new System.Drawing.Size(SizeGame, SizeGame)
            };
            Window.KeyDown += new System.Windows.Forms.KeyEventHandler(Game.ChangeDirection);
            Window.Controls.Add(Canvas);
        }
    }
}

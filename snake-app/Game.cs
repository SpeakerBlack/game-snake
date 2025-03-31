using snake_console.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static snake_app.Enum;

namespace snake_app
{
    public class Game
    {
        public Direction Direction { get; set; } // Direcion en la que se mueve la serpiente
        private int DeltaTime { get; set; } // Los milisegundos a los cuales se ejecuta un frame
        private int RationScreen { get; set; } // Este es la escala a la que vamos a dibujar la matriz
        private int Rows { get; set; } // La cantidad de filas que tendra nuestro juego
        private int Cols { get; set; } // La cantidad de columnas que tendra nuestro juego
        private BodyPart[,] Canvas { get; set; } // El canvas es nuestro mapa, donde la serpiente se movera
        private Snake SnakeGame { get; set; } // Objecto snake
        private bool HasDrawedPoint { get; set; } // Esta bandera indica si hay un punto en el mapa
        public Game(int height, int widht, int ratio, int deltaTime)
        {

            RationScreen = ratio;
            DeltaTime = deltaTime;
            Rows = (height / RationScreen) - 1;
            Cols = (widht / RationScreen) - 1;
            Canvas = new BodyPart[Rows, Cols];
            SnakeGame = new Snake(2);
            HasDrawedPoint = false;

            Sound(new Sound[3] { new Sound(349, 300), new Sound(329, 200), new Sound(415, 300) });
        }

        public void Initialize()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                while (true)
                {
                    Frame();
                    System.Threading.Thread.Sleep(DeltaTime);
                }
            });
        }

        public BodyPart[,] GetCanvas()
        {
            return Canvas;
        }

        private void Frame()
        {
            DrawPoint tempNextPosition = new DrawPoint();
            CheckNextPosition(SnakeGame.CurrentPosition);
            switch (SnakeGame.CurrentDirection)
            {
                case Direction.Up:
                    tempNextPosition = new DrawPoint(SnakeGame.CurrentPosition.X, SnakeGame.CurrentPosition.Y - 1);
                    break;
                case Direction.Down:
                    tempNextPosition = new DrawPoint(SnakeGame.CurrentPosition.X, SnakeGame.CurrentPosition.Y + 1);
                    break;
                case Direction.Left:
                    tempNextPosition = new DrawPoint(SnakeGame.CurrentPosition.X - 1, SnakeGame.CurrentPosition.Y);
                    break;
                case Direction.Right:
                    tempNextPosition = new DrawPoint(SnakeGame.CurrentPosition.X + 1, SnakeGame.CurrentPosition.Y);
                    break;
            }
            SnakeGame.CurrentPosition = tempNextPosition;

            if (!HasDrawedPoint)
            {
                DrawRandomPoint();
            }
            Trace();
        }

        private void Trace()
        {
            DrawPoint[] newTrail = new DrawPoint[SnakeGame.Length];
            if (SnakeGame.Trail[0] != null)
            {
                Canvas[SnakeGame.Trail[0].X, SnakeGame.Trail[0].Y] = BodyPart.None;
            }
            for (int i = SnakeGame.Length - 1; i > 0; i--)
            {
                newTrail[i - 1] = SnakeGame.Trail[i];
                if (newTrail[i - 1] != null)
                {
                    Canvas[newTrail[i - 1].X, newTrail[i - 1].Y] = BodyPart.Body;
                    if (i == SnakeGame.Length - 1)
                    {
                        Canvas[newTrail[i - 1].X, newTrail[i - 1].Y] = BodyPart.Head;
                    }
                    else if (i == 1)
                    {
                        Canvas[newTrail[i - 1].X, newTrail[i - 1].Y] = BodyPart.Tail;
                    }
                }
            }
            SnakeGame.Trail = newTrail;

            SnakeGame.Trail[SnakeGame.Length - 1] = new DrawPoint(SnakeGame.CurrentPosition.X, SnakeGame.CurrentPosition.Y);
        }

        private void CheckNextPosition(DrawPoint nextPoint)
        {
            BodyPart bodyPart = Canvas[nextPoint.X, nextPoint.Y]; // Obtenermos el valor que hay almacenado en la siguiente posicion a la que se mueve la serpiente
            if (nextPoint.X <= 0
                || nextPoint.Y <= 0
                || nextPoint.X >= Rows - 1
                || nextPoint.Y >= Cols - 1) // Verificamos si se salio del mapa
            {
                Sound(new Sound[2] { new Sound(164, 200), new Sound(155, 300) }); // Sonido de muerte
                throw new Exception("Dead. Out of map"); // Excepcion para terminar el juego
            }
            else if (bodyPart == BodyPart.Point) // Verificamos si la serpiente obtuvo un punto
            {
                SnakeGame.Length += 2; // Se aumenta la longitud de la serpiente
                DrawPoint[] tempArray = SnakeGame.Trail; // Se prepara el rastro
                Array.Resize(ref tempArray, SnakeGame.Length); // Como aumentamos la longitud de la serpiente se debe aumentar la logitud del array que guarda el rastro
                SnakeGame.Trail = tempArray;
                HasDrawedPoint = false; // Cambiamos la bandera para que se dibuje otro punto en una posicion aleatoria del mapa
                Sound(new Sound[2] { new Sound(349, 200), new Sound(415, 200) });
            }
            else if (bodyPart == BodyPart.Head
                || bodyPart == BodyPart.Body
                || bodyPart == BodyPart.Tail) // Verificamos si la serpiente se cocha con su propio cuerpo
            {
                Sound(new Sound[2] { new Sound(164, 200), new Sound(155, 300) }); // Sonido de muerte
                throw new Exception("Dead. Self-crash"); // Excepcion para terminar el juego
            }
        }

        private void DrawRandomPoint()
        {
            bool ready = false;
            do
            {
                DrawPoint randomPoint = RandomDrawPoint();
                if (Canvas[randomPoint.X, randomPoint.Y] == BodyPart.None)
                {
                    Canvas[randomPoint.X, randomPoint.Y] = BodyPart.Point;
                    HasDrawedPoint = true;
                    ready = true;
                }
            } while (!ready);
        }

        private DrawPoint RandomDrawPoint()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            int ranX = random.Next(1, Rows - 2);
            int ranY = random.Next(1, Cols - 2);
            return new DrawPoint(ranX, ranY);
        }

        private void Sound(Sound[] sounds)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                foreach (Sound item in sounds)
                {
                    Console.Beep(item.Frequency, item.Duration);
                }
            });
        }

        public void ChangeDirection(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                case Keys.Left:
                    SnakeGame.CurrentDirection = Direction.Left;
                    break;
                case Keys.W:
                case Keys.Up:
                    SnakeGame.CurrentDirection = Direction.Up;
                    break;
                case Keys.S:
                case Keys.Down:
                    SnakeGame.CurrentDirection = Direction.Down;
                    break;
                case Keys.D:
                case Keys.Right:
                    SnakeGame.CurrentDirection = Direction.Right;
                    break;
                case Keys.Space:
                    //game.TogglePause();
                    break;
                default:
                    break;
            }
        }
    }
}

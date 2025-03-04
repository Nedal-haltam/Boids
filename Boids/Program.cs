
using Raylib_cs;
using System.Numerics;
namespace Boids
{
    internal class Program
    {
        public struct Bird
        {

        }
        static void Main(string[] args)
        {
            Raylib.SetConfigFlags(ConfigFlags.AlwaysRunWindow | ConfigFlags.ResizableWindow);
            int w = 800;
            int h = 600;
            Raylib.InitWindow(w, h, "hello");
            float Angle = 0.0f;
            Vector2 center = new(w / 2, h / 2);
            while (!Raylib.WindowShouldClose())
            {
                w = Raylib.GetScreenWidth();
                h = Raylib.GetScreenHeight();
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib_cs.Color.Black);

                float WingHeight = 0.35f * h;
                float WingWidth = 0.15f * w;
                float CenterLength = 0.5f * WingHeight;
                float tail = WingHeight - CenterLength;
                float Thickness = 2.0f;
                Color c = Color.White;
                float RotationSpeed = 0.001f;
                float Speed = 0.1f;
                
                
                Vector2 top = new(center.X + CenterLength * MathF.Sin(Angle), center.Y - CenterLength * MathF.Cos(Angle));

                Vector2 leftend = new(center.X - (WingWidth * MathF.Cos(Angle) + tail * MathF.Sin(Angle)), 
                                      center.Y + (tail * MathF.Cos(Angle) - WingWidth * MathF.Sin(Angle)));
                Vector2 rightend = new(center.X + (WingWidth * MathF.Cos(Angle) - tail * MathF.Sin(Angle)), 
                                      center.Y + (tail * MathF.Cos(Angle) + WingWidth * MathF.Sin(Angle)));

                Raylib.DrawLineEx(center, top, Thickness, c);
                Raylib.DrawLineEx(center, leftend, Thickness, c);
                Raylib.DrawLineEx(center, rightend, Thickness, c);

                Raylib.DrawLineEx(top, leftend,Thickness, c);
                Raylib.DrawLineEx(top, rightend,Thickness, c);

                if (Raylib.IsKeyDown(KeyboardKey.Right))
                {
                    Angle += RotationSpeed;
                }
                if (Raylib.IsKeyDown(KeyboardKey.Left))
                {
                    Angle -= RotationSpeed;
                }
                center.X += Speed * MathF.Sin(Angle);
                center.Y -= Speed * MathF.Cos(Angle);
                


                Raylib.EndDrawing();
            }
        }
    }
}

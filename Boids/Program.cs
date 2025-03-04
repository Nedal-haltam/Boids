
using Raylib_cs;
namespace Boids
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Raylib.SetConfigFlags(ConfigFlags.AlwaysRunWindow | ConfigFlags.ResizableWindow);
            Raylib.InitWindow(800, 600, "hello");
            while(!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib_cs.Color.Red);
                Raylib.EndDrawing();
            }
        }
    }
}

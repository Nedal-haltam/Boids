﻿
using Raylib_cs;
using System.ComponentModel;
using System.Numerics;
using static Boids.Program;
namespace Boids
{
    internal class Program
    {
        static Random random = new();
        public class Boid
        {
            public int x, y;
            public float vx, vy;
            public Boid(int x = 0, int y = 0, float vx = 0, float vy = 0)
            {
                this.x = x;
                this.y = y;
                this.vx = vx;
                this.vy = vy;
            }
        }
        static List<Boid> birds = [];
        static float SizeFactor = 0.4f;
        static float size = 7.0f;
        static float WingHeight = size;
        static float WingWidth = size * SizeFactor;
        static float CenterLength = 0.7f * WingHeight;
        static float tail = WingHeight - CenterLength;
        static float Thickness = 1.0f;
        static Color c = Color.White;

        static float radius = 2.0f;
        static float turnfactor = 0.20f;
        static float visualRange = 40f;
        static float protectedRange = 8f;
        static float centeringfactor = 0.00005f;
        static float avoidfactor = 0.05f;
        static float matchingfactor = 0.05f;
        static float maxspeed = 5.5f;
        static float minspeed = 3.0f;
        static int marginx = 7;
        static int marginy = 7;
        static int N = 2048;

        static int f = 100;
        static int w = 9 * f;
        static int h = 9 * f;
        public static void Update(int StartIndex, int Count)
        {
            //while (true)
            {
                Boid[] CurrentState = new Boid[N];
                birds.CopyTo(CurrentState, 0);
                for (int i = StartIndex; i < StartIndex + Count; i++)
                {
                    float close_dx = 0.0f;
                    float close_dy = 0.0f;
                    float xpos_avg = 0.0f;
                    float ypos_avg = 0.0f;
                    float xvel_avg = 0.0f;
                    float yvel_avg = 0.0f;
                    float neighboring_boids = 0.0f;
                    for (int j = 0; j < N; j++)
                    {
                        if (i == j)
                            continue;
                        Vector2 difference = new(CurrentState[i].x - CurrentState[j].x, CurrentState[i].y - CurrentState[j].y);
                        if (MathF.Abs(difference.X) < visualRange && MathF.Abs(difference.Y) < visualRange)
                        {
                            float SquaredDistance = MathF.Pow(difference.X, 2) + MathF.Pow(difference.Y, 2);
                            if (SquaredDistance < MathF.Pow(protectedRange, 2))
                            {
                                close_dx += difference.X;
                                close_dy += difference.Y;
                            }
                            else if (SquaredDistance < MathF.Pow(visualRange, 2))
                            {
                                xpos_avg += CurrentState[j].x;
                                ypos_avg += CurrentState[j].y;
                                xvel_avg += CurrentState[j].vx;
                                yvel_avg += CurrentState[j].vy;

                                neighboring_boids++;
                            }
                        }
                    }
                    if (neighboring_boids > 0)
                    {
                        xpos_avg = xpos_avg / neighboring_boids;
                        ypos_avg = ypos_avg / neighboring_boids;
                        xvel_avg = xvel_avg / neighboring_boids;
                        yvel_avg = yvel_avg / neighboring_boids;

                        birds[i].vx = (birds[i].vx +
                                   (xpos_avg - birds[i].x) * centeringfactor +
                                   (xvel_avg - birds[i].vx) * matchingfactor);

                        birds[i].vy = (birds[i].vy +
                                   (ypos_avg - birds[i].y) * centeringfactor +
                                   (yvel_avg - birds[i].vy) * matchingfactor);
                    }
                    birds[i].vx = birds[i].vx + (close_dx * avoidfactor);
                    birds[i].vy = birds[i].vy + (close_dy * avoidfactor);

                    // see screen edges (boundaries)
                    /*outside top margin*/
                    if (birds[i].y < h / marginy)
                    {
                        birds[i].vy = birds[i].vy + turnfactor;
                    }
                    /*outside bottom margin*/
                    if (birds[i].y > h - h / marginy)
                    {
                        birds[i].vy = birds[i].vy - turnfactor;
                    }
                    /*outside right margin*/
                    if (birds[i].x > w - w / marginx)
                    {
                        birds[i].vx = birds[i].vx - turnfactor;
                    }
                    /*outside left margin*/
                    if (birds[i].x < w / marginx)
                    {
                        birds[i].vx = birds[i].vx + turnfactor;
                    }
                    float speed = MathF.Sqrt(birds[i].vx * birds[i].vx + birds[i].vy * birds[i].vy);
                    if (speed < minspeed)
                    {
                        birds[i].vx = (birds[i].vx / speed) * minspeed;
                        birds[i].vy = (birds[i].vy / speed) * minspeed;
                    }
                    if (speed > maxspeed)
                    {
                        birds[i].vx = (birds[i].vx / speed) * maxspeed;
                        birds[i].vy = (birds[i].vy / speed) * maxspeed;
                    }

                    birds[i].x += (int)(birds[i].vx * 2);
                    birds[i].y += (int)(birds[i].vy * 2);
                    if (birds[i].x < 0) birds[i].x = 0;
                    if (birds[i].x > w) birds[i].x = w;
                    if (birds[i].y < 0) birds[i].y = 0;
                    if (birds[i].y > h) birds[i].y = h;
                    //Raylib.DrawCircle(birds[i].x, birds[i].y, radius, c);
                }
            }
        }
        static void DrawBird(Boid bird)
        {
            float Angle;
            if (bird.vx == 0)
            {
                Angle = (bird.vy > 0) ? MathF.PI / 2.0f : 3 * MathF.PI / 2.0f;
            }
            else if (bird.vx < 0 && bird.vy < 0)
            {
                Angle = MathF.Atan(bird.vy / bird.vx) + MathF.PI;
            }
            else if (bird.vx < 0)
            {
                Angle = MathF.PI - MathF.Atan(bird.vy / -bird.vx);
            }
            else if (bird.vy < 0)
            {
                Angle = 2 * MathF.PI + MathF.Atan(-bird.vy / bird.vx);
            }
            else
            {
                Angle = MathF.Atan(bird.vy / bird.vx);
            }
            Angle += MathF.PI / 2.0f;
            Vector2 top = new(bird.x + CenterLength * MathF.Sin(Angle), bird.y - CenterLength * MathF.Cos(Angle));
            Vector2 leftend = new(bird.x - (WingWidth * MathF.Cos(Angle) + tail * MathF.Sin(Angle)),
                                  bird.y + (tail * MathF.Cos(Angle) - WingWidth * MathF.Sin(Angle)));
            Vector2 rightend = new(bird.x + (WingWidth * MathF.Cos(Angle) - tail * MathF.Sin(Angle)),
                                  bird.y + (tail * MathF.Cos(Angle) + WingWidth * MathF.Sin(Angle)));
            Raylib.DrawLineEx(new(bird.x, bird.y), leftend, Thickness, c);
            Raylib.DrawLineEx(new(bird.x, bird.y), rightend, Thickness, c);
            Raylib.DrawLineEx(top, leftend, Thickness, c);
            Raylib.DrawLineEx(top, rightend, Thickness, c);
        }
        static void Main(string[] args)
        {
            Raylib.SetConfigFlags(ConfigFlags.AlwaysRunWindow | ConfigFlags.ResizableWindow);
            Raylib.SetTargetFPS(60);
            Raylib.InitWindow(w, h, "Boids");
            for (int i = 0; i < N; i++)
            {
                birds.Add(new(i, h / 2 + random.Next(123), 4, 4));
            }
            int NumberOfThreads = 4;
            while (!Raylib.WindowShouldClose())
            {
                w = Raylib.GetScreenWidth();
                h = Raylib.GetScreenHeight();
                float dt = Raylib.GetFrameTime();
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Raylib.DrawFPS(0, 0);

                int part = 0;
                Thread thread1 = new Thread(() => Update(part++ * N / NumberOfThreads, N / NumberOfThreads));
                Thread thread2 = new Thread(() => Update(part++ * N / NumberOfThreads, N / NumberOfThreads));
                Thread thread3 = new Thread(() => Update(part++ * N / NumberOfThreads, N / NumberOfThreads));
                Thread thread4 = new Thread(() => Update(part++ * N / NumberOfThreads, N / NumberOfThreads));
                thread1.Start();
                thread2.Start();
                thread3.Start();
                thread4.Start();
                for (int i = 0; i < N; i++)
                {
                    DrawBird(birds[i]);
                }
                Raylib.DrawRectangleLinesEx(new(w / marginx, h / marginy, new(w - 2 * w / marginx, h - 2 * h / marginy)), radius, Color.Red);
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}

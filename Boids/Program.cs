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
        public enum State
        {
            Rendering, WelcomeScreen 
        }
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


        static float Defaultmaxspeed = 5.5f;
        static float Defaultminspeed = 3.0f;
        static float Defaultturnfactor = 0.20f;
        static float Defaultcenteringfactor = 0.00005f;
        static float Defaultavoidfactor = 0.05f;
        static float Defaultmatchingfactor = 0.05f;

        static float maxspeed = 5.5f;
        static float minspeed = 3.0f;
        static float turnfactor = 0.20f;
        static float centeringfactor = 0.00005f;
        static float avoidfactor = 0.05f;
        static float matchingfactor = 0.05f;

        static float radius = 2.0f;
        static float visualRange = 40f;
        static float protectedRange = 8f;
        static int marginx = 7;
        static int marginy = 7;
        

        static int f = 100;
        static int w = 9 * f;
        static int h = 9 * f;
        public static void UpdatePrallelFor(int StartIndex, int Count)
        {
            Parallel.For(StartIndex, StartIndex + Count, i =>
            {
                float close_dx = 0.0f;
                float close_dy = 0.0f;
                float xpos_avg = 0.0f;
                float ypos_avg = 0.0f;
                float xvel_avg = 0.0f;
                float yvel_avg = 0.0f;
                float neighboring_boids = 0.0f;
                Parallel.For(0, N, j =>
                {
                    if (i != j)
                    {
                        Vector2 difference = new(birds[i].x - birds[j].x, birds[i].y - birds[j].y);
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
                                xpos_avg += birds[j].x;
                                ypos_avg += birds[j].y;
                                xvel_avg += birds[j].vx;
                                yvel_avg += birds[j].vy;

                                neighboring_boids++;
                            }
                        }
                    }
                });
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
            });
        }

        public static void Update(int StartIndex, int Count)
        {
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
                    if (i != j)
                    {
                        Vector2 difference = new(birds[i].x - birds[j].x, birds[i].y - birds[j].y);
                        if (MathF.Abs(difference.X) < visualRange && MathF.Abs(difference.Y) < visualRange)
                        {
                            float SquaredDistance = difference.LengthSquared();
                            if (SquaredDistance < MathF.Pow(protectedRange, 2))
                            {
                                close_dx += difference.X;
                                close_dy += difference.Y;
                            }
                            else if (SquaredDistance < MathF.Pow(visualRange, 2))
                            {
                                xpos_avg += birds[j].x;
                                ypos_avg += birds[j].y;
                                xvel_avg += birds[j].vx;
                                yvel_avg += birds[j].vy;

                                neighboring_boids++;
                            }
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
        static int N = 5*1000;
        public static void RenderBoids()
        {
#if true
            int part = 0;
            int NumberOfThreads = 12;
            Thread[] threads = new Thread[NumberOfThreads];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new(() => Update(part++ * N / NumberOfThreads, N / NumberOfThreads));
                threads[i].Start();
            }
            for (int i = 0; i < threads.Length; i++)
                threads[i].Join();
#else
            UpdatePrallelFor(0, N);
#endif
            for (int i = 0; i < N; i++)
            {
                DrawBird(birds[i]);
            }
            Raylib.DrawRectangleLinesEx(new(w / marginx, h / marginy, new(w - 2 * w / marginx, h - 2 * h / marginy)), radius, Color.Red);
        }
        public static void ModifyParameterStep(ref float parameter, float step)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.Right))
                parameter += step;
            if (Raylib.IsKeyPressed(KeyboardKey.Left))
                parameter -= step;
        }
        public static void ModifyParameterSweep(ref float parameter, float step)
        {
            if (Raylib.IsKeyDown(KeyboardKey.Right))
                parameter += step;
            if (Raylib.IsKeyDown(KeyboardKey.Left))
                parameter -= step;
        }
        public static void ControlParamters()
        {
            if (Raylib.IsKeyDown(KeyboardKey.T))
            {
                if (Raylib.IsKeyDown(KeyboardKey.Up))
                    ModifyParameterSweep(ref turnfactor, 0.01f);
                else
                    ModifyParameterStep(ref turnfactor, 0.01f);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.C))
            {
                if (Raylib.IsKeyDown(KeyboardKey.Up))
                    ModifyParameterSweep(ref centeringfactor, 0.00001f);
                else
                    ModifyParameterStep(ref centeringfactor, 0.00001f);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.A))
            {
                if (Raylib.IsKeyDown(KeyboardKey.Up))
                    ModifyParameterSweep(ref maxspeed, 0.1f);
                else
                    ModifyParameterStep(ref maxspeed, 0.1f);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.I))
            {
                if (Raylib.IsKeyDown(KeyboardKey.Up))
                    ModifyParameterSweep(ref minspeed, 0.1f);
                else
                    ModifyParameterStep(ref minspeed, 0.1f);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.V))
            {
                if (Raylib.IsKeyDown(KeyboardKey.Up))
                    ModifyParameterSweep(ref avoidfactor, 0.01f);
                else
                    ModifyParameterStep(ref avoidfactor, 0.01f);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.M))
            {
                if (Raylib.IsKeyDown(KeyboardKey.Up))
                    ModifyParameterSweep(ref matchingfactor, 0.01f);
                else
                    ModifyParameterStep(ref matchingfactor, 0.01f);
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                maxspeed = Defaultmaxspeed;
                minspeed = Defaultminspeed;
                turnfactor = Defaultturnfactor;
                centeringfactor = Defaultcenteringfactor;
                avoidfactor = Defaultavoidfactor;
                matchingfactor = Defaultmatchingfactor;
            }
        }
        public static void RenderParamterText(int TextHeight)
        {
            int spacing = 0;
            int margin = 5;
            string TextMaxSpeed = $"Max Speed: {(int)maxspeed}.{((int)(maxspeed * 10)) % 10}{((int)(maxspeed * 100)) % 10}, ";
            Raylib.DrawText(TextMaxSpeed, 10, h + margin, TextHeight - margin, Color.White);

            spacing += Raylib.MeasureText(TextMaxSpeed, TextHeight - margin) + 10;
            string TextMinSpeed = $"Min Speed: {(int)minspeed}.{((int)(minspeed * 10)) % 10}{((int)(minspeed * 100)) % 10}, ";
            Raylib.DrawText(TextMinSpeed, spacing, h + margin, TextHeight - margin, Color.White);

            spacing += Raylib.MeasureText(TextMinSpeed, TextHeight - margin) + 5;
            string TextTurnFactor = $"Turn Factor: {(int)turnfactor}.{((int)(turnfactor * 10)) % 10}{((int)(turnfactor * 100)) % 10}, ";
            Raylib.DrawText(TextTurnFactor, spacing, h + margin, TextHeight - margin, Color.White);

            spacing += Raylib.MeasureText(TextTurnFactor, TextHeight - margin) + 5;
            string TextCenteringFactor = $"Centering Factor: {(int)centeringfactor}.{((int)(centeringfactor * 10)) % 10}{((int)(centeringfactor * 100)) % 10}{((int)(centeringfactor * 1000)) % 10}{((int)(centeringfactor * 10000)) % 10}{((int)(centeringfactor * 100000)) % 10}, ";
            Raylib.DrawText(TextCenteringFactor, spacing, h + margin, TextHeight - margin, Color.White);

            spacing += Raylib.MeasureText(TextCenteringFactor, TextHeight - margin) + 5;
            string TextAvoidFactor = $"Avoid Factor: {(int)avoidfactor}.{((int)(avoidfactor * 10)) % 10}{((int)(avoidfactor * 100)) % 10}, ";
            Raylib.DrawText(TextAvoidFactor, spacing, h + margin, TextHeight - margin, Color.White);

            spacing += Raylib.MeasureText(TextAvoidFactor, TextHeight - margin) + 5;
            string TextMatchingFactor = $"Matching Factor: {(int)matchingfactor}.{((int)(matchingfactor * 10)) % 10}{((int)(matchingfactor * 100)) % 10}";
            Raylib.DrawText(TextMatchingFactor, spacing, h + margin, TextHeight - margin, Color.White);
        }
        public static void DisplayWelcomeScreen()
        {
            int FontSize = 30;
            string TextWelcomeMessage = "Welcom to Boids Simulation";
            int TextWelcomeMessageWidth = Raylib.MeasureText(TextWelcomeMessage, FontSize);
            Raylib.DrawText(TextWelcomeMessage, w / 2 - TextWelcomeMessageWidth / 2, h / 5, FontSize, Color.White);

            string Textmsg1 = "You can control the following parameters:";
            int Textmsg1Width = Raylib.MeasureText(Textmsg1, FontSize);
            Raylib.DrawText(Textmsg1, w / 2 - Textmsg1Width / 2, h / 3, FontSize, Color.White);
            string[] TextParameters =
            [
                "A: maxspeed",
                "I: minspeed",
                "T: turnfactor",
                "C: centeringfactor",
                "V: avoidfactor",
                "M: matchingfactor",
            ];
            for (int i = 0; i < TextParameters.Length; i++)
            {
                string TempText = TextParameters[i];
                Raylib.DrawText(TempText, w / 2 - Textmsg1Width / 2, h / 3 + 100 + i * (int)(1.5f * FontSize), FontSize, Color.White);
            }
            string Textinstruction = "Press and Hold the corresponding letter and then\n\nincrease or decrease the value using\n\nthe right and left arrows respectively";
            int TextinstructionWidth = Raylib.MeasureText(Textinstruction, FontSize);
            Raylib.DrawText(Textinstruction, 20, h - h / 5, FontSize, Color.White);

            string TextContinue = "Press Enter to continue.";
            int TextContinueWidth = Raylib.MeasureText(TextContinue, FontSize / 2);
            Raylib.DrawText(TextContinue, w / 2 - TextContinueWidth / 2, h - 20, FontSize / 2, Color.White);
        }
        public static void Render(int TextHeight)
        {
            ControlParamters();
            RenderBoids();
            RenderParamterText(TextHeight);
        }
        static void Main(string[] args)
        {
            int TextHeight = 21;
            State CurrentState = State.WelcomeScreen;
            Raylib.SetConfigFlags(ConfigFlags.AlwaysRunWindow);
            Raylib.SetTargetFPS(0);
            Raylib.InitWindow(w, h + TextHeight, "Boids");
            for (int i = 0; i < N; i++)
            {
                birds.Add(new(i, h / 2 + random.Next(123), 4, 4));
            }
            
            while (!Raylib.WindowShouldClose())
            {
                float dt = Raylib.GetFrameTime();
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Raylib.DrawFPS(0, 0);

                if (Raylib.IsKeyPressed(KeyboardKey.Enter) && CurrentState == State.WelcomeScreen)
                    CurrentState = State.Rendering;
                if (Raylib.IsKeyPressed(KeyboardKey.H) && CurrentState == State.Rendering)
                    CurrentState = State.WelcomeScreen;

                if (CurrentState == State.WelcomeScreen)
                {
                    DisplayWelcomeScreen();
                }
                else if (CurrentState == State.Rendering)
                {
                    Render(TextHeight);
                }
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}

using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Game
{
    class Program
    {
        public const int Width = 800;
        public const int Height = 600;

        private static List<Particle> particles;

        public const double UpdateTime = 1.0/60.0;

        public static void Main()
        {
            particles = new List<Particle>();
            //initialization
            Raylib.InitWindow(Width, Height, "basic particle sim");

            ResetSprites();

            Raylib.SetTargetFPS(60);

            double lastUpdate = 0;
            while(!Raylib.WindowShouldClose()){
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);
                foreach(Particle sprite in particles){
                    sprite.Render();
                }
                Raylib.EndDrawing();
                double time = Raylib.GetTime();

                while(time - lastUpdate > UpdateTime){
                    lastUpdate += UpdateTime;
                    //update velocities
                    foreach(Particle particle in particles){
                        particle.UpdateVel(particles);

                    }
                    //update positions
                    foreach(Particle particle in particles){
                        particle.UpdatePos(particles);
                    }
                }
            }
            Raylib.CloseWindow();
        }

        private static void ResetSprites(){
            particles.Clear();
            for(int i=0; i<100; i++){
                Vector2 pos = new Vector2(Raylib.GetRandomValue(0, Width), Raylib.GetRandomValue(0, Height));
                float charge = Raylib.GetRandomValue(-1, 1)/100f;
                float mass = 1;//Raylib.GetRandomValue(10, 100)/100f;
                if(charge!=0)particles.Add(new Particle(pos, Vector2.Zero, charge, mass, false));
            }

        }
    }
}

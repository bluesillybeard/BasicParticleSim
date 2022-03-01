using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using System;

namespace Game{
    class Particle{

        public Particle(Vector2 pos, Vector2 vel, float charge, float mass, bool pinned){
            this.pos = pos;
            this.vel = vel;
            this.charge = charge;
            this.mass = mass;
            this.pinned = pinned;
        }
        const float coulombsConstant = 8.99f*1000000000f; //Coulombs constant (k) for meters, coulombs, and newtons\
        const float scaleFactor = 10;
        public bool pinned;
        public Vector2 pos; //the position in pixels.
        public Vector2 vel;
        //meters = pixels/100
        //Yes, i'm using a Vector as a set of coordinates; treat the X and Y components as X and Y coordinates.
        public float charge; //the charge in coulombs
        public float mass; //the mass in kilograms
        public void Render(){
            Color c;
            if(charge > 0) c=Color.RED;
            else if(charge == 0)c=Color.GRAY;
            else c=Color.BLUE;
            Raylib.DrawCircle((int)pos.X, (int)pos.Y, mass*scaleFactor, c);
            //render the particle. Color is red if positive, blue if negative.
        }

        public void UpdateVel(IList<Particle> particles){
            //extremely simplistic algorithm updates the velocity each update.

            if(!pinned){
                Vector2 force = new Vector2(); //this will keep track of the force as it's done for each particle
                foreach(Particle particle in particles){
                    if(particle == this) continue; //skip if it's this sprite
                    if(Vector2.Distance(particle.pos, this.pos) <= this.mass*scaleFactor+particle.mass*scaleFactor){
                        continue; //skip colliding particles
                    }

                    //calculate the signed magnitude of the force
                    float distanceSquared = Vector2.DistanceSquared(particle.pos, this.pos);
                    float magnitude = (particle.charge * this.charge); //q1*q2
                    magnitude /= distanceSquared; // /r^2
                    magnitude *= coulombsConstant; //*k

                    //calculate the unit vector (essentially the direction) of the force
                    Vector2 direction = Vector2.Normalize(this.pos - particle.pos);

                    force += direction*magnitude; //multiply the magnitude and the direction to create the final vector.
                }
                Vector2 acceleration = force/mass;
                this.vel += acceleration * (float)Program.UpdateTime;

                //bounce off edge of screen
                if(this.pos.X < 0 ){
                    this.vel.X*=-1;
                    this.pos.X = 0;
                } else if(this.pos.X >Program.Width){
                    this.vel.X*=-1;
                    this.pos.X = Program.Width;
                }
                if(this.pos.Y < 0){
                    this.vel.Y*=-1;
                    this.pos.Y = 0;
                } else if(this.pos.Y >Program.Height){
                    this.vel*=-1;
                    this.pos.Y = Program.Height;
                }
                //if(Vector2.Distance(this.vel, Vector2.Zero) > 100) this.vel = Vector2.Normalize(vel)*100;

                //this.vel -= vel*0.1f; //10% dampening
            }
        }
        public void UpdatePos(IList<Particle> particles){
            this.pos += vel*(float)Program.UpdateTime;
        }
    }
}
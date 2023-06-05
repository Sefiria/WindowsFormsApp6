using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Numerics;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace gpttest
{
    public partial class Form1 : Form
    {
        private const double G = 9.80665;
        private List<Particle> particles;
        private Random Random = new Random((int)DateTime.UtcNow.Ticks);
        private float particleRadius;
        private float particleDensity;
        private float particleMass;
        private int particleCount = 0;
        private Timer timer = new Timer() { Enabled = true, Interval = 10 };
        private Bitmap img;
        private Graphics g;
        private bool isDown = false;
        private MouseEventArgs e;
        private float defaultmass = 500F;

        private void InitializeParticles()
        {
            particles = new List<Particle>();

            // définir la taille de la particule
            particleRadius = 10f;

            // définir la densité de la particule
            particleDensity = 1000f;

            // calculer la masse de la particule
            particleMass = particleDensity * (4f / 3f) * (float)Math.PI * (float)Math.Pow(particleRadius, 3);

            // générer les particules
            for (int i = 0; i < particleCount; i++)
            {
                Particle particle = new Particle(particleMass, particleRadius)
                {
                    // définir une position aléatoire pour la particule
                    Position = new Vector2(Random.Next(0, ClientSize.Width), Random.Next(0, ClientSize.Height))
                };

                // ajouter la particule à la liste des particules
                particles.Add(particle);
            }
        }

        private void UpdateParticlesPosition(float deltaTime)
        {
            List<Particle> _particles = new List<Particle>(particles);

            // mettre à jour la position des particules en utilisant la formule de la gravité
            foreach (Particle particle in _particles)
            {
                Vector2 acceleration = new Vector2(0, 0);
                float distance;
                Vector2 direction;

                // calculer l'accélération en utilisant la formule de la gravité
                foreach (Particle otherParticle in _particles)
                {
                    if (particle != otherParticle)
                    {
                        distance = Vector2.Distance(particle.Position, otherParticle.Position);
                        if (distance == 0F) continue;

                        direction = Vector2.Normalize(otherParticle.Position - particle.Position == Vector2.Zero ? new Vector2((Random.Next(20) - 10F) / 10F, (Random.Next(20) - 10F) / 10F) : otherParticle.Position - particle.Position);

                        acceleration += direction.Multiply(G * particle.Mass * otherParticle.Mass / (distance * distance));
                        float ax = acceleration.X, ay = acceleration.Y;
                        float threshold = 10F;
                        if (ax < -threshold) ax = -threshold;
                        if (ax >= threshold) ax = threshold;
                        if (ay < -threshold) ay = -threshold;
                        if (ay >= threshold) ay = threshold;
                        acceleration = new Vector2(ax, ay);
                    }
                }

                void CalcAccForWall(float x, float y)
                {
                    Vector2 pos = new Vector2(x, y);
                    distance = Vector2.Distance(particle.Position, pos);
                    if (distance < 1F)
                    {
                        direction = Vector2.Normalize(pos - particle.Position);
                        acceleration = new Vector2(direction.X * -x, direction.Y * -y);

                        float ax = acceleration.X, ay = acceleration.Y;
                        float threshold = 10F;
                        if (ax < -threshold) ax = -threshold;
                        if (ax >= threshold) ax = threshold;
                        if (ay < -threshold) ay = -threshold;
                        if (ay >= threshold) ay = threshold;
                        acceleration = -new Vector2(ax, ay);
                    }
                }
                CalcAccForWall(0F, particle.Position.Y);
                CalcAccForWall(Render.Width - 1, particle.Position.Y);
                CalcAccForWall(particle.Position.X, 0F);
                CalcAccForWall(particle.Position.X, Render.Height - 1);

                acceleration = new Vector2(acceleration.X, acceleration.Y + (float)G);

                // calculer la vitesse et la position de la particule
                particle.Velocity += acceleration * deltaTime;
                particle.Position += particle.Velocity * deltaTime;

                float _x = particle.Position.X;
                float _y = particle.Position.Y;
                float min = 5F, max = 5F;
                if (_x < min) _x = min;
                if (_y < min) _y = min;
                if (_x > Render.Width - max) _x = Render.Width - max;
                if (_y > Render.Height - max) _y = Render.Height - max;
                particle.Position = new Vector2(_x, _y);
                //if (particle.Position.X.CompareTo(float.NaN) == 0 || particle.Position.Y.CompareTo(float.NaN) == 0 || particle.Position.X < 0 || particle.Position.Y < 0 || particle.Position.X >= Render.Width || particle.Position.Y >= Render.Height)
                //{
                //    float x = particle.Position.X, y = particle.Position.Y;
                //    if (particle.Position.X.CompareTo(float.NaN) == 0) x = 0F;
                //    if (particle.Position.Y.CompareTo(float.NaN) == 0) y = 0F;
                //    x = particle.Position.X < 0F ? 1F : (particle.Position.X >= Render.Width ? Render.Width - 1F : x);
                //    y = particle.Position.Y < 0F ? 1F : (particle.Position.Y >= Render.Height ? Render.Height - 1F : y);
                //    particle.Position = new Vector2(x, y);
                //}
            }
        }

        private void DrawParticles(Graphics graphics)
        {
            // dessiner chaque particule en utilisant un cercle rempli
            foreach (Particle particle in particles)
            {
                Brush brush = new SolidBrush(Color.Blue);
                graphics.FillEllipse(brush, particle.Position.X - particle.Radius, particle.Position.Y - particle.Radius, particle.Radius * 2, particle.Radius * 2);
            }
        }

        internal Form1()
        {
            InitializeComponent();

            img = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(img);

            InitializeParticles();

            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.R))
                particles.Clear();

            if (isDown)
                AddParticle();

            UpdateParticlesPosition(0.1F);

            g.Clear(Color.Black);
            DrawParticles(g);
            Render.Image = img;
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                AddParticle();
                return;
            }

            isDown = true;
            Render_MouseMove(sender, e);
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            isDown = false;
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            isDown = false;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            this.e = e;
        }
        private void AddParticle()
        {
            var p = new Particle(defaultmass, 5F)
            {
                Position = new Vector2(this.e.X, this.e.Y),
                Velocity = new Vector2((Random.Next(200) - 100F) / 100F, (Random.Next(200) - 100F) / 100F) };
            particles.Add(p);
        }
    }


    public class Particle
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Mass { get; private set; }
        public float Radius { get; private set; }

        public Particle(float mass, float radius)
        {
            Mass = mass;
            Radius = radius;
        }
    }

    public static class Common
    {
        public static Vector2 Multiply(this Vector2 v, double d) => new Vector2(v.X * (float)d, v.Y * (float)d);
    }
}

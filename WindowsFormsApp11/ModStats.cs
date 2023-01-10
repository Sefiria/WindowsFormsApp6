using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11
{
    public class ModStats
    {
        /// <summary>
        /// BulletSpeedMove [impact C]
        /// </summary>
        public float A { get; private set; }

        /// <summary>
        /// WaveForce [impact Y]
        /// </summary>
        public float B { get; private set; }

        /// <summary>
        /// WaveAmp [impact A]
        /// </summary>
        public float C { get; private set; }

        /// <summary>
        /// ShotCount [impact Z]
        /// </summary>
        public float X { get; private set; }

        /// <summary>
        /// Rnd LookX for each shot [impact X]
        /// </summary>
        public float Y { get; private set; }

        /// <summary>
        /// Rnd LookY for each shot [impact B]
        /// </summary>
        public float Z { get; private set; }


        public ModStats()
        {
            A = 5F;
            B = 20F;
            C = 2F;
            X = 1F;
            Y = 0F;
            Z = -1;
        }

        /// <summary>
        /// BulletSpeedMove [impact C]
        /// </summary>
        public void ModA(float v)
        {
            A += v;
            C -= v / 2F;
        }
        /// <summary>
        /// WaveForce [impact Y]
        /// </summary>
        public void ModB(float v)
        {
            B += v;
            Y -= v / 2F;
        }
        /// <summary>
        /// WaveAmp [impact A]
        /// </summary>
        public void ModC(float v)
        {
            C += v;
            A -= v / 2F;
        }
        /// <summary>
        /// ShotCount [impact Z]
        /// </summary>
        public void ModX(float v)
        {
            X += v;
            Z -= v / 2F;
        }
        /// <summary>
        /// Rnd LookX for each shot [impact X]
        /// </summary>
        public void ModY(float v)
        {
            Y += v;
            X--;
        }
        /// <summary>
        /// Rnd LookY for each shot [impact B]
        /// </summary>
        public void ModZ(float v)
        {
            Z += v;
            B -= v / 2F;
        }
    }
}

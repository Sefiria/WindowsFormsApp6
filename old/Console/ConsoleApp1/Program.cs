using SFXMNG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //SFX.Play("E1B A3D");

            for (int i = 0; i < 4; i++)
            {
                PlayBeep((int)SFX.Tone.E * 1, 50);
                PlayBeep((int)SFX.Tone.E * 3, 50);
                PlayBeep((int)SFX.Tone.E * 5, 50);
                PlayBeep((int)SFX.Tone.E * 3, 50);
            }

            Console.ReadKey(true);
        }
    }
}

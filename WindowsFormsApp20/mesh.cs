using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp20
{
    public struct mesh
    {
        public List<triangle> tris;

        bool LoadFromObjectFile(string sFilename)
        {
            if (!File.Exists(sFilename))
                return false;

            string[] lines = File.ReadAllText(sFilename).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // Local cache of verts
            List<Vector4> verts = new List<Vector4>();

            foreach(string line in lines)
            {
                string[] words = line.Split(' ');

                char junk;

                if (words[0] == "v")
                {
                    Vector4 v = new Vector4(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3]), 1F);
                    verts.Add(v);
                }

                if (words[0] == "f")
                {
                    int[] f = new[] { int.Parse(words[1]), int.Parse(words[1]), int.Parse(words[1]) };
                    tris.Add(new triangle{ p= new[] { verts[f[0] - 1], verts[f[1] - 1], verts[f[2] - 1] } });
                }
            }
            return true;
        }
    };
}

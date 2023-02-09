using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotile
{
    public class Autotile : Tile
    {
        public string FileName = "autotile";
        Bitmap Source;
        Bitmap A, Z, E, Q, S, D, W, X, C, Alone, Horizontal, Vertical;
        int m_curId;
        public int curId
        {
            get => m_curId;
            private set
            {
                m_curId = value;
                while (m_curId > 11) m_curId -= 11;
                while (m_curId < 0) m_curId += 11;
            }
        }
        public void SetCurID(int curId) => this.curId = curId;
        public int AutoType = 0;

        public Autotile(Bitmap img, int autoType)
        {
            AutoType = autoType;
            Source = new Bitmap(img, Data.TileSize * 3, Data.TileSize * 3);
            Source.MakeTransparent();
            Split();
        }

        private void Split()
        {
            var SZ = Source.Width;
            var sz = SZ / 3;
            A = Source.Clone(new Rectangle(sz * 0, sz * 0, sz, sz), Source.PixelFormat);
            Z = Source.Clone(new Rectangle(sz * 1, sz * 0, sz, sz), Source.PixelFormat);
            E = Source.Clone(new Rectangle(sz * 2, sz * 0, sz, sz), Source.PixelFormat);
            Q = Source.Clone(new Rectangle(sz * 0, sz * 1, sz, sz), Source.PixelFormat);
            S = Source.Clone(new Rectangle(sz * 1, sz * 1, sz, sz), Source.PixelFormat);
            D = Source.Clone(new Rectangle(sz * 2, sz * 1, sz, sz), Source.PixelFormat);
            W = Source.Clone(new Rectangle(sz * 0, sz * 2, sz, sz), Source.PixelFormat);
            X = Source.Clone(new Rectangle(sz * 1, sz * 2, sz, sz), Source.PixelFormat);
            C = Source.Clone(new Rectangle(sz * 2, sz * 2, sz, sz), Source.PixelFormat);


            var hsz = sz / 2;

            // Creating Alone image from raw of A, E, W and C
            var a = A.Clone(new Rectangle(hsz * 0, hsz * 0, hsz, hsz), Source.PixelFormat);
            var e = E.Clone(new Rectangle(hsz * 1, hsz * 0, hsz, hsz), Source.PixelFormat);
            var w = W.Clone(new Rectangle(hsz * 0, hsz * 1, hsz, hsz), Source.PixelFormat);
            var c = C.Clone(new Rectangle(hsz * 1, hsz * 1, hsz, hsz), Source.PixelFormat);
            Alone = new Bitmap(sz, sz);
            using(Graphics g = Graphics.FromImage(Alone))
            {
                g.DrawImage(a, hsz * 0, hsz * 0);
                g.DrawImage(e, hsz * 1, hsz * 0);
                g.DrawImage(w, hsz * 0, hsz * 1);
                g.DrawImage(c, hsz * 1, hsz * 1);
            }

            // Creating Horizontal image from raw of Z and X
            var z = Z.Clone(new Rectangle(hsz * 0, hsz * 0, sz, hsz), Source.PixelFormat);
            var x = X.Clone(new Rectangle(hsz * 0, hsz * 1, sz, hsz), Source.PixelFormat);
            Horizontal = new Bitmap(sz, sz);
            using (Graphics g = Graphics.FromImage(Horizontal))
            {
                g.DrawImage(z, hsz * 0, hsz * 0);
                g.DrawImage(x, hsz * 0, hsz * 1);
            }

            // Creating Vertical image from raw of Q and D
            var q = Q.Clone(new Rectangle(hsz * 0, hsz * 0, hsz, sz), Source.PixelFormat);
            var d = D.Clone(new Rectangle(hsz * 1, hsz * 0, hsz, sz), Source.PixelFormat);
            Vertical = new Bitmap(sz, sz);
            using (Graphics g = Graphics.FromImage(Vertical))
            {
                g.DrawImage(q, hsz * 0, hsz * 0);
                g.DrawImage(d, hsz * 1, hsz * 0);
            }

            Current = Alone;// The default Current is Alone.
            m_curId = 9;
        }

        /// <summary>
        /// Get the pre calculated image from around values
        /// </summary>
        /// <returns></returns>
        public Bitmap Get() => Current;
        public Bitmap GetNext() { curId++; Current = GetCurrentFromCurId(); return Current; }
        public Bitmap GetCurrentFromCurId() => GetFromId(curId);
        public Bitmap GetFromId(int ID)
        {
            switch (ID)
            {
                case 0: return A;
                case 1: return Z;
                case 2: return E;
                case 3: return Q;
                case 4: return S;
                case 5: return D;
                case 6: return W;
                case 7: return X;
                case 8: return C;
                default: case 9: return Alone;
                case 10: return Horizontal;
                case 11: return Vertical;
            }
        }
        /// <summary>
        /// int[3, 3] around, int is tile value from palette, the [1, 1] should no be set (the calculated result is defined here)
        /// </summary>
        /// <param name="around"></param>
        /// <returns></returns>
        public Bitmap CalculateAndGet(List<Tile> pal, byte[,] around) => CalculateAndGet(new Palette() { Tiles = pal }, around);
        public Bitmap CalculateAndGet(Palette pal, byte[,] around)
        {
            Calculate(pal, around);
            return Get();
        }

        /// <summary>
        /// int[3, 3] around, int is tile value from palette, the [1, 1] should no be set (the calculated result is defined here)
        /// </summary>
        /// <param name="around"></param>
        /// <returns></returns>
        public void Calculate(List<Tile> pal, byte[,] iaround) => Calculate(new Palette() { Tiles = pal }, iaround);
        public void Calculate(Palette pal, byte[,] iaround)
        {
            bool v(int i, int j) => (((iaround[i, j] < 0 || iaround[i, j] >= pal.Tiles.Count) ? pal.Tiles[0] : pal.Tiles[iaround[i, j]]) as Autotile)?.AutoType == AutoType;

            bool a = v(0, 0);
            bool z = v(1, 0);
            bool e = v(2, 0);
            bool q = v(0, 1);
            bool d = v(2, 1);
            bool w = v(0, 2);
            bool x = v(1, 2);
            bool c = v(2, 2);

            // Corners
            if (a && z && q && !d&& !x && !c)
            {
                Current = C;
                m_curId = 8;
                return;
            }
            if (z && e && !q && d && !w && !x)
            {
                Current = W;
                m_curId = 6;
                return;
            }
            if (!z && !e && q && !d && w && x)
            {
                Current = E;
                m_curId = 2;
                return;
            }
            if (!a && !z && !q && d && x && c)
            {
                Current = A;
                m_curId = 0;
                return;
            }

            // Mid Straights
            if (a && z && e && q && d && !w && !x && !c)
            {
                Current = X;
                m_curId = 7;
                return;
            }
            if (!a && !z && !e && q && d && w && x && c)
            {
                Current = Z;
                m_curId = 1;
                return;
            }
            if (a && z && !e && q && !d && w && x && !c)
            {
                Current = D;
                m_curId = 5;
                return;
            }
            if (!a && z && e && !q && d && !w && x && c)
            {
                Current = Q;
                m_curId = 3;
                return;
            }

            // Empty & Full
            if (z && q && d && x)
            {
                Current = S;
                m_curId = 4;
                return;
            }
            if (!z && !q && !d && !x)
            {
                Current = Alone;
                m_curId = 9;
                return;
            }

            // Straights
            if (z && !q && !d && x)
            {
                Current = Vertical;
                m_curId = 11;
                return;
            }
            if (!z && q && d && !x)
            {
                Current = Horizontal;
                m_curId = 10;
                return;
            }

            //if (!a && !z && !e && !q && !d && !w && !x && !c)
        }

        public List<Bitmap> AllTiles() => new List<Bitmap>()
        {
            A, Z, E,
            Q, S, D,
            W, X, C,
            Alone, Horizontal, Vertical
        };

        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(FileName);
        }
    }
}

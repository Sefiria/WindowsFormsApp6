using console_v2.res.DBResSpeDef;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;

namespace console_v2
{
    public enum Sols { Vide = 0, Pierre = 10, Terre = 15, Herbe = 20, Pave = 30 }
    public enum Murs { Vide = 0, Pierre = 100, PierreFissuree = 110 }
    public enum Outils { Hache = 200, Pioche = 210, Faux = 220, Pelle = 230 }
    public enum Objets { Buche = 500, BoisDeChauffe, PetitBois, Planche = 505, FibreDePlante = 510, Boue, EssenceViolys = 600, EssenceRougeo, EssenceJaunade, EssenceVerdacier, EssenceNoiranite, EssenceBlanchaine }
    public enum Consommables { Fraises = 1000 }
    public enum Plantes { Violys = 2000, Rougeo, Jaunade, Verdacier, Noiranite, Blanchaine }

    public enum GenerationMode
    {
        Mine = 0, Rocailleux, Plaine, Foret
    }

    public static class DB
    {
        public static bool Is<T>(this int value) where T: Enum => Enum.IsDefined(typeof(T), value);
        public static bool Isnt<T>(this int value) where T: Enum => !value.Is<T>();
        public static IEnumerable<int> GetValues<T>() where T: Enum => Enum.GetValues(typeof(T)).Cast<int>();
        /// <summary>
        /// Sols + Murs
        /// </summary>
        public static IEnumerable<int> Blocks => GetValues<Sols>().Concat(GetValues<Murs>());
        /// <summary>
        /// ALL != (Sols && Murs)
        /// </summary>
        public static IEnumerable<int> Entities => GetValues<Outils>().Concat(GetValues<Objets>()).Concat(GetValues<Consommables>());

        public static Type GetEnumTypeOf(int tileValue)
        {
            if (tileValue.Is<Sols>()) return typeof(Sols);
            if (tileValue.Is<Murs>()) return typeof(Murs);
            if (tileValue.Is<Outils>()) return typeof(Outils);
            if (tileValue.Is<Objets>()) return typeof(Objets);
            if (tileValue.Is<Consommables>()) return typeof(Consommables);
            return null;
        }
        public static string GetEnumNameOf(int tileValue)
        {
            if (tileValue.Is<Sols>()) return Enum.GetName(typeof(Sols), tileValue);
            if (tileValue.Is<Murs>()) return Enum.GetName(typeof(Murs), tileValue);
            if (tileValue.Is<Outils>()) return Enum.GetName(typeof(Outils), tileValue);
            if (tileValue.Is<Objets>()) return Enum.GetName(typeof(Objets), tileValue);
            if (tileValue.Is<Consommables>()) return Enum.GetName(typeof(Consommables), tileValue);
            return null;
        }

        public static bool IsBlockingType(this int value) => value == -1 ? true : /*value.Is<Sols>() || */value.Is<Murs>();
        public static bool IsntBlockingType(this int value) => !IsBlockingType(value);

        public static List<Sols> HarvestableGrounds = new List<Sols>
        {
            Sols.Terre, Sols.Herbe, 
        };

        public static Dictionary<int, int> Resources = new Dictionary<int, int>
        {
            [(int)Sols.Pierre] = short.MaxValue + 0,//'ʭ',
            [(int)Sols.Terre] = '░',
            [(int)Sols.Herbe] = '▒',
            [(int)Sols.Pave] = 'Ш',

            [(int)Murs.Pierre] = '█',
            [(int)Murs.PierreFissuree] = '▓',

            [(int)Outils.Hache] = 'Ƥ',
            [(int)Outils.Pioche] = '₼',
            [(int)Outils.Faux] = 'Ҁ',
            [(int)Outils.Pelle] = 'д',

            [(int)Objets.Buche] = '¶',
            [(int)Objets.BoisDeChauffe] = '═',
            [(int)Objets.PetitBois] = '⸗',
            [(int)Objets.Planche] = '║',
            [(int)Objets.FibreDePlante] = 'ˠ',
            [(int)Objets.Boue] = '░',

            [(int)Consommables.Fraises] = 'ꬾ',

        };
        public static Dictionary<Type, Color> Colors = new Dictionary<Type, Color>
        {
            [typeof(Objets)] = Color.Yellow,
            [typeof(Outils)] = Color.Cyan,
        };
        public static Dictionary<int, Color> ResColor = new Dictionary<int, Color>
        {
            [(int)Sols.Pierre] = Color.DimGray,
            [(int)Sols.Terre] = Color.Sienna,//Color.Tan for sand
            [(int)Sols.Herbe] = Color.DarkGreen,
            [(int)Sols.Pave] = Color.Beige,

            [(int)Murs.Pierre] = Color.Gray,
            [(int)Murs.PierreFissuree] = Color.Gray,
        };
        public static Dictionary<GenerationMode, Color> ChunkLayerColor = new Dictionary<GenerationMode, Color>
        {
            [GenerationMode.Mine] = Color.FromArgb(80,80,80),
            [GenerationMode.Rocailleux] = Color.DimGray,
            [GenerationMode.Plaine] = Color.Green,
            [GenerationMode.Foret] = Color.DarkGreen,
        };
        public static Dictionary<int, Bitmap> ResourcesSpecials;

        public static  (int CharToDisplay, Bitmap DBResSpe) RetrieveDBResOrSpe(int _dbref)
        {
            int _CharToDisplay = -1;
            Bitmap _DBResSpe = null;
            if (Resources.ContainsKey(_dbref))
                _CharToDisplay = Resources[_dbref];
            else if (ResourcesSpecials.ContainsKey(_dbref))
                _DBResSpe = ResourcesSpecials[_dbref];
            return (_CharToDisplay, _DBResSpe);
        }

        public static string DefineName(int _dbref) => GetEnumNameOf(_dbref);

        static DB()
        {
            int w = (int)GraphicsManager.CharSize.Width, h = (int)GraphicsManager.CharSize.Height;

            Bitmap create(string strpixels, int color)
            {
                List<vec> pixels = new List<vec>();
                var lines = string.Concat(strpixels.Skip(2).Take(strpixels.Length - 4)).Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int y = 0;
                for (int _y = 0; _y < lines.Length; _y++, y++)
                {
                    if (lines[_y].StartsWith("@"))
                    {
                        y += int.Parse(string.Concat(lines[_y].Skip(1))) - 1;
                        continue;
                    }
                    for (int x = 0; x < lines[_y].Length; x++)
                    {
                        if (lines[_y][x] != ' ')
                            pixels.Add((x, y).V());
                    }
                }
                Bitmap result = new Bitmap(w, y);
                using (var g = Graphics.FromImage(result))
                    pixels.ForEach(px => g.DrawRectangle(new Pen(Color.FromArgb(color)), px.x, px.y, 1, 1));
                return result.Resize(w, h);
            }

            ResourcesSpecials = new Dictionary<int, Bitmap>();
            ResourcesSpecials[(int)Sols.Pierre] = create(DBSpe_Stone.String, Color.DimGray.ToArgb());
            ResourcesSpecials[(int)Plantes.Violys] = ResourcesSpecials[(int)Objets.EssenceViolys] = create(DBSpe_Violys.String, Color.White.ToArgb());
            ResourcesSpecials[(int)Plantes.Rougeo] = ResourcesSpecials[(int)Objets.EssenceRougeo] = create(DBSpe_Rougeo.String, Color.White.ToArgb());
            ResourcesSpecials[(int)Plantes.Jaunade] = ResourcesSpecials[(int)Objets.EssenceJaunade] = create(DBSpe_Jaunade.String, Color.White.ToArgb());
            ResourcesSpecials[(int)Plantes.Verdacier] = ResourcesSpecials[(int)Objets.EssenceVerdacier] = create(DBSpe_Verdacier.String, Color.White.ToArgb());
            ResourcesSpecials[(int)Plantes.Noiranite] = ResourcesSpecials[(int)Objets.EssenceNoiranite] = create(DBSpe_Noiranite.String, Color.White.ToArgb());
            ResourcesSpecials[(int)Plantes.Blanchaine] = ResourcesSpecials[(int)Objets.EssenceBlanchaine] = create(DBSpe_Blanchaine.String, Color.White.ToArgb());
        }
    }
}

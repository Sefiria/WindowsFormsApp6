﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_v2
{
    public enum Sols { Vide = 0, Pierre = 10, Terre = 15, Herbe = 20, Pave = 30 }
    public enum Murs { Vide = 0, Pierre = 100, PierreFissuree = 110 }
    public enum Outils { Hache = 200, Pioche = 210, Faux = 220 }
    public enum Objets { Buche = 500, Planche = 505 }
    public enum Consommables { Fraises = 1000 }
    public enum ____ {  }

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

        public static bool IsBlockingType(this int value) => value == -1 ? true : /*value.Is<Sols>() || */value.Is<Murs>();
        public static bool IsntBlockingType(this int value) => !IsBlockingType(value);

        public static Dictionary<int, int> Resources = new Dictionary<int, int>
        {

            [(int)Sols.Pierre] = 'ʭ',
            [(int)Sols.Terre] = '░',
            [(int)Sols.Herbe] = '▒',
            [(int)Sols.Pave] = 'Ш',

            [(int)Murs.Pierre] = '█',
            [(int)Murs.PierreFissuree] = '▓',

            [(int)Outils.Hache] = 'Ƥ',
            [(int)Outils.Pioche] = '₼',
            [(int)Outils.Faux] = 'ﻞ',

            [(int)Objets.Buche] = '¶',
            [(int)Objets.Planche] = '║',

            [(int)Consommables.Fraises] = 'ꬾ',
        };
    }
}

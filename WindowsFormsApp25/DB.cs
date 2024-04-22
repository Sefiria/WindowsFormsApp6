using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Tooling;
using WindowsFormsApp25.Properties;

namespace WindowsFormsApp25
{
    public class DB
    {
        public enum TextureRenderTypes
        {
            Source, Resized,
        }
        public enum TextureTypes
        {
            _1x1 = 0, _1x2,
        }

        private static DB m_Instance = null;
        public static DB Instance => m_Instance ?? (m_Instance = new DB());

        public Dictionary<TextureRenderTypes, Dictionary<TextureTypes, Bitmap[,]>> Textures;

        public DB()
        {
            Textures = new Dictionary<TextureRenderTypes, Dictionary<TextureTypes, Bitmap[,]>>()
            {
                [TextureRenderTypes.Source] = new Dictionary<TextureTypes, Bitmap[,]>
                {
                    [TextureTypes._1x1] = Resources.textures_1x1.Split2D(32),
                    [TextureTypes._1x2] = Resources.textures_1x2.Split2D(32, 64),
                },
                [TextureRenderTypes.Resized] = new Dictionary<TextureTypes, Bitmap[,]>
                {
                    [TextureTypes._1x1] = Resources.textures_1x1.Split2DAndResize(32, Core.TileSize, true, Color.White),
                    [TextureTypes._1x2] = Resources.textures_1x2.Split2DAndResize(32, 64, Core.TileSize, Core.TileSize * 2, true, Color.White),
                },
            };
        }
        public enum Tex
        {
            // Floors

            Wall = 0x050,

            // Entities

            TableA = 0x000, TableB = 0x010, TableC = 0x020, Floor = 0x030, You = 0x040,
            
            Box = 0x100,
        }
        private static int Get0x100(int tex) => tex / (16 * 16);
        private static int Get0x010(int tex) => tex % (16 * 16) / 16;
        private static int Get0x001(int tex) => tex % (16 * 16) % 16;
        public static Bitmap GetTex(int tex) => Instance.Textures[TextureRenderTypes.Resized][(TextureTypes)Get0x100(tex)][Get0x010(tex), Get0x001(tex)];
        public static Bitmap GetTex(Tex tex) => GetTex((int)tex);
        public static string GetName(int tex) => Enum.GetName(typeof(Tex), tex);
        public static string GetName(Tex tex) => Enum.GetName(typeof(Tex), tex);

        public List<Tex> BlockingFloors = new List<Tex>
        {
            Tex.Wall,
        };
        public static bool IsBlocking(int tex) => Instance.BlockingFloors.Contains((Tex)tex);
        public static bool IsntBlocking(int tex) => !IsBlocking(tex);
        public static bool IsBlocking(Tex tex) => IsBlocking((int)tex);
        public static bool IsntBlocking(Tex tex) => IsntBlocking((int)tex);
    }
}

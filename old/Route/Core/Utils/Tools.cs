using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using static Core.Definitions;

namespace Core.Utils
{
    public class Tools
    {
        public enum Rotation { None = 0, D90 = 90, D180 = 180, D270 = 270 }

        public static Random rnd = new Random((int)DateTime.Now.Ticks);

        public static Color GetPixel(Bitmap img, Point Location)
        {
            return GetPixel(img, Location.X, Location.Y);
        }
        public static Color GetPixel(Bitmap img, int X, int Y)
        {
            Color result;

            using (BmpPixelSnoop bps = new BmpPixelSnoop(img))
            {
                result = bps.GetPixel(X, Y);
            }

            return result;
        }

        static public float Lerp(float firstFloat, float secondFloat, float by)
        {
            return (1F - by) * firstFloat + by * secondFloat;
        }
        static public Bitmap DecodeBitmap(string bytes)
        {
            string[] arr = bytes.Split('-');
            byte[] array = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i++) array[i] = Convert.ToByte(arr[i], 16);
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
            return (Bitmap)tc.ConvertFrom(array);
        }
        static public int Snap(int coord) => Snap(coord, SharedData.TileSize);
        static public Point Snap(Point pos) => Snap(pos, SharedData.TileSize);
        static public int Snap(int coord, int tilesize)
        {
            return (coord / tilesize) * tilesize;
        }
        static public Point Snap(Point pos, int tilesize)
        {
            return new Point(Snap(pos.X, tilesize), Snap(pos.Y, tilesize));
        }
        static public bool CompareColors(Color a, Color b)
        {
            return a.ToArgb() == b.ToArgb();
        }
        static public string GetBitmapEncoded(Bitmap bmp)
        {
            return BitConverter.ToString((byte[])new ImageConverter().ConvertTo(bmp, typeof(byte[])));
        }
        static public Bitmap GetBitmapDecoded(string bytes)
        {
            if (string.IsNullOrWhiteSpace(bytes))
            {
                int TileSize = SharedData.TileSize;
                var img = new Bitmap(TileSize, TileSize);
                using (var g = Graphics.FromImage(img))
                {
                    g.FillRectangle(new SolidBrush(Color.PaleVioletRed), new RectangleF(0F, 0F, TileSize, TileSize));
                    g.DrawString("X", new Font("Arial", 10F), new SolidBrush(Color.Red), new RectangleF(4F, 2F, TileSize - 4, TileSize - 2));
                    g.DrawString("X", new Font("Arial", 10F), new SolidBrush(Color.Black), new RectangleF(0F, 0F, TileSize, TileSize));
                }
                return img;
            }

            string[] arr = bytes.Split('-');
            byte[] array = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i++) array[i] = Convert.ToByte(arr[i], 16);
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
            return (Bitmap)tc.ConvertFrom(array);
        }
        static public Point ClientToWorld(Point point)
        {
            return new Point(point.X / SharedData.TileSize, point.Y / SharedData.TileSize);
        }
        static public List<Bitmap> SplitBitmap(Bitmap img, int size)
        {
            List<Bitmap> result = new List<Bitmap>();
            for (int y = 0; y < img.Size.Height / size; y++)
            {
                for (int x = 0; x < img.Size.Width / size; x++)
                {
                    var splitted = new Bitmap(size, size);
                    using (var g = Graphics.FromImage(splitted))
                        g.DrawImage(img, new Rectangle(0, 0, size, size), new Rectangle(x * size, y * size, size, size), GraphicsUnit.Pixel);
                    splitted.MakeTransparent();
                    result.Add(splitted);
                }
            }
            return result;
        }

        [DllImport("shlwapi.dll")]
        public static extern int ColorHLSToRGB(int H, int L, int S);

        // Convert an object to a byte array
        static public byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        // Convert a byte array to an Object
        static public object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }

        public static void Serialize(object obj, string filename)
        {
            var serializer = new JsonSerializer();

            using (var sw = new StreamWriter(filename))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, obj);
            }
        }
        static public void SerializeJSON(object data, string filename)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filename, json);
        }
        public static object Deserialize(string path)
        {
            var serializer = new JsonSerializer();
            object result = null;

            using (var sw = new StreamReader(path))
            using (var reader = new JsonTextReader(sw))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
        static public T DeserializeJSON<T>(string JSON) where T:class
        {
            return JsonConvert.DeserializeObject<T>(JSON);
        }
        static public T DeserializeJSONFromFile<T>(string path) where T : class
        {
            return DeserializeJSON<T>(File.ReadAllText(path));
        }

        public static T EnumParser<T>(string name) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return default(T);

            return (T)Enum.Parse(typeof(T), name);
        }

        public static void ForEachPathDots(Action<PathDots> action)
        {
            Array list = Enum.GetValues(typeof(PathDots));
            foreach (PathDots pathdot in list)
                action(pathdot);
        }
        public static int GetFilesCount(string ResourcesPath, string extension)
        {
            return Directory.GetFiles(ResourcesPath, extension, SearchOption.TopDirectoryOnly).Length;
        }
        public static bool IsPathDotCorner(PathDots pathDotValue)
        {
            return new[] { PathDots.CA, PathDots.CZ, PathDots.CQ, PathDots.CS }.ToList().Contains(pathDotValue);
        }

        public static void Rotate(ref Rotation Angle)
        {
            Angle += 90;
            if ((int)Angle > 270)
                Angle = 0;
        }
        public static Bitmap ApplyRotation(Bitmap img, Rotation Angle)
        {
            Bitmap rotatedImg = new Bitmap(img);
            switch (Angle)
            {
                case Rotation.D90: rotatedImg.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
                case Rotation.D180: rotatedImg.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
                case Rotation.D270: rotatedImg.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
            }
            return rotatedImg;
        }
        public static Bitmap Rotated(Bitmap b, float degrees)
        {
            int size = (int)Math.Sqrt(b.Width * b.Width + b.Height * b.Height);
            Bitmap returnBitmap = new Bitmap(size, size);

            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                g.TranslateTransform(size / 2, size / 2);
                g.RotateTransform(degrees);
                g.TranslateTransform(-size / 2, -size / 2);
                g.DrawImage(b, new Point((size - b.Width) / 2, (size - b.Height) / 2));
                //g.DrawImage(b, Point.Empty);
            }

            return returnBitmap;
        }

        public static Dictionary<PathDots, PathDotType> GetRotatedPathDots(Dictionary<PathDots, PathDotType> list, Rotation Angle)
        {
            list = new Dictionary<PathDots, PathDotType>(list);
            switch (Angle)
            {
                case Rotation.D90: RotatePathDots(ref list); break;
                case Rotation.D180: RotatePathDots(ref list); RotatePathDots(ref list); break;
                case Rotation.D270: RotatePathDots(ref list); RotatePathDots(ref list); RotatePathDots(ref list); break;
            }

            return list;
        }
        public static void RotatePathDots(ref Dictionary<PathDots, PathDotType> list)
        {
            /*
                        Init
                   12   0   1  13
                    3   8   9   6
                    2  10  11   7
                   14   5   4   15

                       Rotated
                   14   2   3  12
                    5  10   8   0
                    4  11   9   1
                   15   7   6  13
            */

            Dictionary<PathDots, PathDotType> initList = new Dictionary<PathDots, PathDotType>(list);
            list[(PathDots)12] = initList[(PathDots)14];
            list[(PathDots)0] = initList[(PathDots)2];
            list[(PathDots)1] = initList[(PathDots)3];
            list[(PathDots)13] = initList[(PathDots)12];
            list[(PathDots)3] = initList[(PathDots)5];
            list[(PathDots)8] = initList[(PathDots)10];
            list[(PathDots)9] = initList[(PathDots)8];
            list[(PathDots)6] = initList[(PathDots)0];
            list[(PathDots)2] = initList[(PathDots)4];
            list[(PathDots)10] = initList[(PathDots)11];
            list[(PathDots)11] = initList[(PathDots)9];
            list[(PathDots)7] = initList[(PathDots)1];
            list[(PathDots)14] = initList[(PathDots)15];
            list[(PathDots)5] = initList[(PathDots)7];
            list[(PathDots)4] = initList[(PathDots)6];
            list[(PathDots)15] = initList[(PathDots)13];
        }
        public static void RotatePathDots2(ref Dictionary<PathDots, PathDotType> list)
        {
            /*
                        Init
                   12   0   1  13
                    3   8   9   6
                    2  10  11   7
                   14   5   4   15

                       Rotated
                   13   6   7  15
                    1   9  11   4
                    0   8  10   5
                   12   3   2  14
            */

            Dictionary<PathDots, PathDotType> initList = new Dictionary<PathDots, PathDotType>(list);
            list[(PathDots)12] = initList[(PathDots)13];
            list[(PathDots)0] = initList[(PathDots)6];
            list[(PathDots)1] = initList[(PathDots)7];
            list[(PathDots)13] = initList[(PathDots)15];
            list[(PathDots)3] = initList[(PathDots)1];
            list[(PathDots)8] = initList[(PathDots)9];
            list[(PathDots)9] = initList[(PathDots)11];
            list[(PathDots)6] = initList[(PathDots)4];
            list[(PathDots)2] = initList[(PathDots)0];
            list[(PathDots)10] = initList[(PathDots)8];
            list[(PathDots)11] = initList[(PathDots)10];
            list[(PathDots)7] = initList[(PathDots)5];
            list[(PathDots)14] = initList[(PathDots)12];
            list[(PathDots)5] = initList[(PathDots)3];
            list[(PathDots)4] = initList[(PathDots)2];
            list[(PathDots)15] = initList[(PathDots)14];
        }
        public static void Flip180PathDots(ref Dictionary<PathDots, PathDotType> list)
        {
            /*
                        Init
                   12   0   1  13
                    3   8   9   6
                    2  10  11   7
                   14   5   4   15

                       Flipped
                   14   5   4   15
                    2  10  11   7
                    3   8   9   6
                   12   0   1  13
            */

            Dictionary<PathDots, PathDotType> initList = new Dictionary<PathDots, PathDotType>(list);
            list[(PathDots)12] = initList[(PathDots)14];
            list[(PathDots)0] = initList[(PathDots)5];
            list[(PathDots)1] = initList[(PathDots)4];
            list[(PathDots)13] = initList[(PathDots)15];
            list[(PathDots)3] = initList[(PathDots)2];
            list[(PathDots)8] = initList[(PathDots)10];
            list[(PathDots)9] = initList[(PathDots)11];
            list[(PathDots)6] = initList[(PathDots)7];
            list[(PathDots)2] = initList[(PathDots)3];
            list[(PathDots)10] = initList[(PathDots)8];
            list[(PathDots)11] = initList[(PathDots)9];
            list[(PathDots)7] = initList[(PathDots)6];
            list[(PathDots)14] = initList[(PathDots)12];
            list[(PathDots)5] = initList[(PathDots)0];
            list[(PathDots)4] = initList[(PathDots)1];
            list[(PathDots)15] = initList[(PathDots)13];
        }

        public static PathDots? PathDotUp(PathDots dot)
        {
            switch(dot)
            {
                /*
                    ○ ○ ○ ○
                    • • • •
                    • • • •
                    • • • •
                */
                default:
                case PathDots.CA:
                case PathDots.TopLeft:
                case PathDots.TopRight:
                case PathDots.CZ:
                    return null;

                /*
                    • • • •
                    ○ • • •
                    ○ • • •
                    ○ • • •
                */
                case PathDots.CQ: return PathDots.LeftBottom;
                case PathDots.LeftBottom: return PathDots.LeftTop;
                case PathDots.LeftTop: return PathDots.CA;

                /*
                    • • • •
                    • ○ • •
                    • ○ • •
                    • ○ • •
                */
                case PathDots.BottomLeft: return PathDots.Q;
                case PathDots.Q: return PathDots.A;
                case PathDots.A: return PathDots.TopLeft;

                /*
                    • • • •
                    • • ○ •
                    • • ○ •
                    • • ○ •
                */
                case PathDots.BottomRight: return PathDots.S;
                case PathDots.S: return PathDots.Z;
                case PathDots.Z: return PathDots.TopRight;

                /*
                    • • • •
                    • • • ○
                    • • • ○
                    • • • ○
                */
                case PathDots.CS: return PathDots.RightBottom;
                case PathDots.RightBottom: return PathDots.RightTop;
                case PathDots.RightTop: return PathDots.CZ;
            }
        }
        public static PathDots? PathDotDown(PathDots dot)
        {
            switch (dot)
            {
                /*
                    • • • •
                    • • • •
                    • • • •
                    ○ ○ ○ ○
                */
                default:
                case PathDots.CQ:
                case PathDots.BottomLeft:
                case PathDots.BottomRight:
                case PathDots.CS:
                    return null;

                /*
                    ○ • • •
                    ○ • • •
                    ○ • • •
                    • • • •
                */
                case PathDots.CA: return PathDots.LeftTop;
                case PathDots.LeftTop: return PathDots.LeftBottom;
                case PathDots.LeftBottom: return PathDots.CQ;

                /*
                    • ○ • •
                    • ○ • •
                    • ○ • •
                    • • • •
                */
                case PathDots.TopLeft: return PathDots.A;
                case PathDots.A: return PathDots.Q;
                case PathDots.Q: return PathDots.BottomLeft;

                /*
                    • • ○ •
                    • • ○ •
                    • • ○ •
                    • • • •
                */
                case PathDots.TopRight: return PathDots.Z;
                case PathDots.Z: return PathDots.S;
                case PathDots.S: return PathDots.BottomRight;

                /*
                    • • • ○
                    • • • ○
                    • • • ○
                    • • • •
                */
                case PathDots.CZ: return PathDots.RightTop;
                case PathDots.RightTop: return PathDots.RightBottom;
                case PathDots.RightBottom: return PathDots.CS;
            }
        }
        public static PathDots? PathDotRight(PathDots dot)
        {
            switch (dot)
            {
                /*
                    • • • ○
                    • • • ○
                    • • • ○
                    • • • ○
                */
                default:
                case PathDots.CZ:
                case PathDots.RightTop:
                case PathDots.RightBottom:
                case PathDots.CS:
                    return null;

                /*
                    ○ ○ ○ •
                    • • • •
                    • • • •
                    • • • •
                */
                case PathDots.CA: return PathDots.TopLeft;
                case PathDots.TopLeft: return PathDots.TopRight;
                case PathDots.TopRight: return PathDots.CZ;

                /*
                    • • • •
                    ○ ○ ○ •
                    • • • •
                    • • • •
                */
                case PathDots.LeftTop: return PathDots.A;
                case PathDots.A: return PathDots.Z;
                case PathDots.Z: return PathDots.RightTop;

                /*
                    • • • •
                    • • • •
                    ○ ○ ○ •
                    • • • •
                */
                case PathDots.LeftBottom: return PathDots.Q;
                case PathDots.Q: return PathDots.S;
                case PathDots.S: return PathDots.RightBottom;

                /*
                    • • • •
                    • • • •
                    • • • •
                    ○ ○ ○ •
                */
                case PathDots.CQ: return PathDots.BottomLeft;
                case PathDots.BottomLeft: return PathDots.BottomRight;
                case PathDots.BottomRight: return PathDots.CS;
            }
        }
        public static PathDots? PathDotLeft(PathDots dot)
        {
            switch (dot)
            {
                /*
                    ○ • • •
                    ○ • • •
                    ○ • • •
                    ○ • • •
                */
                default:
                case PathDots.CA:
                case PathDots.LeftTop:
                case PathDots.LeftBottom:
                case PathDots.CQ:
                    return null;

                /*
                    • ○ ○ ○
                    • • • •
                    • • • •
                    • • • •
                */
                case PathDots.CZ: return PathDots.TopRight;
                case PathDots.TopRight: return PathDots.TopLeft;
                case PathDots.TopLeft: return PathDots.CA;

                /*
                    • • • •
                    • ○ ○ ○
                    • • • •
                    • • • •
                */
                case PathDots.RightTop: return PathDots.Z;
                case PathDots.Z: return PathDots.A;
                case PathDots.A: return PathDots.LeftTop;

                /*
                    • • • •
                    • • • •
                    • ○ ○ ○
                    • • • •
                */
                case PathDots.RightBottom: return PathDots.S;
                case PathDots.S: return PathDots.Q;
                case PathDots.Q: return PathDots.LeftBottom;

                /*
                    • • • •
                    • • • •
                    • • • •
                    • ○ ○ ○
                */
                case PathDots.CS: return PathDots.BottomRight;
                case PathDots.BottomRight: return PathDots.BottomLeft;
                case PathDots.BottomLeft: return PathDots.CQ;
            }
        }
        public static PathDots? PathDotGetFirstWay(PathDots S)
        {
            switch (S)
            {
                case PathDots.LeftTop:
                case PathDots.TopLeft: return PathDots.A;

                case PathDots.RightTop:
                case PathDots.TopRight: return PathDots.Z;

                case PathDots.LeftBottom:
                case PathDots.BottomLeft: return PathDots.Q;

                case PathDots.RightBottom:
                case PathDots.BottomRight: return PathDots.S;
            }

            return null;
        }
        public static List<PathDots> PathDotGetSecondWay(PathDots FW)
        {
            if (FW == PathDots.A)
            {
                return new List<PathDots>() { PathDots.Q, PathDots.Z };
            }

            if (FW == PathDots.Z)
            {
                return new List<PathDots>() { PathDots.A, PathDots.S };
            }

            if (FW == PathDots.S)
            {
                return new List<PathDots>() { PathDots.Z, PathDots.Q };
            }

            if (FW == PathDots.Q)
            {
                return new List<PathDots>() { PathDots.S, PathDots.A };
            }

            return new List<PathDots>();
        }
        public static List<PathDots> PathDotGetEnd(TileIndex tileIndex, PathDots SW)
        {
            List<PathDots> result = new List<PathDots>();

            if (!TileResMngr.Instance.ResourcesTileInfo.ContainsKey(tileIndex.Index))
                return result;
            Tile tile = TileResMngr.Instance.ResourcesTileInfo[tileIndex.Index].Tile;
            Dictionary<PathDots, PathDotType> dots = GetRotatedPathDots(tile.pathDots, tileIndex.Angle);

            if (SW == PathDots.A)
            {
                if (dots[PathDots.TopLeft] == PathDotType.Out)
                    result.Add(PathDots.TopLeft);
                if (dots[PathDots.LeftTop] == PathDotType.Out)
                    result.Add(PathDots.LeftTop);
            }
            else
            if (SW == PathDots.Z)
            {
                if (dots[PathDots.TopRight] == PathDotType.Out)
                    result.Add(PathDots.TopRight);
                if (dots[PathDots.RightTop] == PathDotType.Out)
                    result.Add(PathDots.RightTop);
            }
            else
            if (SW == PathDots.Q)
            {
                if (dots[PathDots.LeftBottom] == PathDotType.Out)
                    result.Add(PathDots.LeftBottom);
                if (dots[PathDots.BottomLeft] == PathDotType.Out)
                    result.Add(PathDots.BottomLeft);
            }
            else
            if (SW == PathDots.S)
            {
                if (dots[PathDots.RightBottom] == PathDotType.Out)
                    result.Add(PathDots.RightBottom);
                if (dots[PathDots.BottomRight] == PathDotType.Out)
                    result.Add(PathDots.BottomRight);
            }

            return result;
        }
        public static (PathDots DFW, PathDots? DSW, PathDots DE)? GetRandomPath(TileIndex tileIndex, PathDots S)
        {
            if (!TileResMngr.Instance.ResourcesTileInfo.ContainsKey(tileIndex.Index))
                return null;
            Tile tile = TileResMngr.Instance.ResourcesTileInfo[tileIndex.Index].Tile;
            Dictionary<PathDots, PathDotType> dots = GetRotatedPathDots(tile.pathDots, tileIndex.Angle);

            PathDots? FW = PathDotGetFirstWay(S);
            if (FW == null) return null;

            PathDots? SW = null;
            if (rnd.Next(2) == 0)
            {
                List<PathDots> listSW = PathDotGetSecondWay(FW.Value);
                for (int i = 0; i < listSW.Count; i++)
                    if (PathDotGetEnd(tileIndex, listSW[i]).Where(x => dots[x] == PathDotType.Out).Count() == 0)
                        listSW.RemoveAt(i--);
                if (listSW.Count == 0) return null;
                if (listSW.Count > 1) SW = listSW[rnd.Next(listSW.Count)];
                else SW = listSW[0];
            }

            PathDots E;
            List<PathDots>  listE = PathDotGetEnd(tileIndex, SW ?? FW.Value);
            if (listE.Count == 0) return null;
            if (listE.Count > 1) E = listE[rnd.Next(listE.Count)];
            else E = listE[0];

            return (FW.Value, SW, E);
        }
        public static CarPath FindAnyPath(int TX, int TY, TileMap tileMap, PathDots DS)
        {
            var result = GetRandomPath(tileMap[TX, TY, null], DS);
            if (result == null)
                return null;
            (PathDots DFW, PathDots? DSW, PathDots DE) = result.Value;
            return new CarPath(TX, TY, DS, DFW, DSW, DE);
        }
    }
}

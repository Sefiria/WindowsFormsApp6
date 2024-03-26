using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6
{
    static public class Tools
    {
        static public byte[] EncodeRender(Image Render, Point CursorPosition)
        {
            List<byte> data = new List<byte>();

            Bitmap img = new Bitmap(Render);

            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 320; y++)
                {
                    Color px = img.GetPixel(x, y);
                    data.Add(px.R);
                    data.Add(px.G);
                    data.Add(px.B);
                }
            }

            IEnumerable<byte> FragCursorPosition = BitConverter.GetBytes(CursorPosition.X).Concat(BitConverter.GetBytes(CursorPosition.Y));
            data.AddRange(FragCursorPosition);

            return CompressBytes(data.ToArray());
        }
        static public (Bitmap Render, Point CursorPosition) DecodeRender(byte[] msg)
        {
            msg = new List<byte>(UncompressString(msg)).ToArray();

            Bitmap Render = new Bitmap(320, 320);

            for (int x = 0; x < 320 * 3; x += 3)
            {
                for (int y = 0; y < 320; y++)
                {
                    Color px = Color.FromArgb(255, msg[y * 320 * 3 + x + 0], msg[y * 320 * 3 + x + 1], msg[y * 320 * 3 + x + 2]);
                    Render.SetPixel(x / 3, y, px);
                }
            }

            byte[] CursorX = new byte[4];
            byte[] CursorY = new byte[4];
            Buffer.BlockCopy(msg, 320 * 320 * 3, CursorX, 0, 4);
            Buffer.BlockCopy(msg, 320 * 320 * 3, CursorY, 0, 4);
            int iCursorX = BitConverter.ToInt32(CursorX, 0);
            int iCursorY = BitConverter.ToInt32(CursorY, 0);

            return (Render, new Point(iCursorX, iCursorY));
        }
        static public byte[] CompressBytes(byte[] data)
        {
            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    gZipStream.Write(data, 0, data.Length);
                byte[] toto = outputStream.ToArray();
                return toto;
            }
        }
        static public byte[] UncompressString(byte[] msg)
        {
            using (var inputStream = new MemoryStream(msg))
            using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gZipStream))
            {
                byte[] toto = Convert.FromBase64String(streamReader.ReadToEnd());
                return toto;
            }
        }
    }
}

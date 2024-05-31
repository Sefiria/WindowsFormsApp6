using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp27
{
    public static class ByteManipulator
    {
        public static byte RGB64ToByte(this (byte val1, byte val2, byte val3) v)
        {
            return (byte)((v.val1 & 0x3F) | ((v.val2 & 0x3F) << 2) | ((v.val3 & 0x3F) << 4));
        }
        public static byte RGB64ToByte(this (int val1, int val2, int val3) v) => RGB64ToByte(((byte)v.val1, (byte)v.val2, (byte)v.val3));
        public static string rgb64hex(this (int val1, int val2, int val3) v) => RGB64ToByte(((byte)v.val1, (byte)v.val2, (byte)v.val3)).ToString("X2");
        public static string rgb256to64hex(this (int val1, int val2, int val3) v) => RGB64ToByte(((byte)(v.val1/ 256F * 64), (byte)(v.val2 / 256F * 64), (byte)(v.val3 / 256F * 64))).ToString("X2");

        public static (byte r64, byte g64, byte b64) ByteToRGB64(this byte b)
        {
            byte val1 = (byte)(b & 0x3F);
            byte val2 = (byte)((b >> 2) & 0x3F);
            byte val3 = (byte)((b >> 4) & 0x3F);
            return (val1, val2, val3);
        }
        public static (byte r64, byte g64, byte b64) ByteToRGB64(this int b) => ByteToRGB64((byte)b);
        public static byte ByteRToR64(this byte r) => (byte)(r & 0x3F);
        public static byte ByteGToG64(this byte g) => (byte)((g >> 2) & 0x3F);
        public static byte ByteBToB64(this byte b) => (byte)((b >> 4) & 0x3F);


        public static string hex(this int _byte) => _byte.ToString("X2");
        public static string X2(this int _, params byte[] hexs) => string.Concat(hexs.Select(b => b.ToString("X2")));
    }

}

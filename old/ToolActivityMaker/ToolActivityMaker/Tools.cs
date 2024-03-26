using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToolActivityMaker
{
    public static class Tools
    {
        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char GetCharFromKey(System.Windows.Forms.Keys key)
        {
            return GetCharFromKey(WinformsToWPFKey(key));
        }
        public static char GetCharFromKey(System.Windows.Input.Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
                default:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
            }
            return ch;
        }
        public static System.Windows.Input.Key WinformsToWPFKey(System.Windows.Forms.Keys inputKey)
        {
            string strInput = inputKey.ToString();
            if (strInput.Contains("comma"))
                strInput = strInput.Replace("comma", "Comma");
            // Put special case logic here if there's a key you need but doesn't map...   
            try
            {
                return (System.Windows.Input.Key)Enum.Parse(typeof(System.Windows.Input.Key), strInput);
            }
            catch (Exception e)
            {
                // There wasn't a direct mapping...     
                return System.Windows.Input.Key.None;
            }
        }






        static public double Lerp(double v0, double v1, double t)
        {
            return (1D - t) * v0 + t * v1;
        }
    }
}

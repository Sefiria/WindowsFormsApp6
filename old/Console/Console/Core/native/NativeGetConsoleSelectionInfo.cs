using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.native
{
    public class NativeGetConsoleSelectionInfo
    {
        public struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
        public struct CONSOLE_SELECTION_INFO
        {
            /// <summary>
            /// CONSOLE_MOUSE_DOWN 0x0008               Mouse is down
            /// CONSOLE_MOUSE_SELECTION 0x0004          Selecting with the mouse
            /// CONSOLE_NO_SELECTION 0x0000             No selection
            /// CONSOLE_SELECTION_IN_PROGRESS 0x0001    Selection has begun
            /// CONSOLE_SELECTION_NOT_EMPTY 0x0002      Selection rectangle is not empty
            /// </summary>
            public uint dwFlags;
            public COORD dwSelectionAnchor;
            public SMALL_RECT srSelection;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
        }
        [DllImport("kernel32.dll")]
        public static extern bool GetConsoleSelectionInfo(out CONSOLE_SELECTION_INFO lpConsoleSelectionInfo);

        public static (IntPtr Pointer, GCHandle Handle) GetPointerFromObject(object obj)
        {
            GCHandle handle = GCHandle.Alloc(obj);
            IntPtr pointer = (IntPtr)handle;

            return (pointer, handle);

            // call WinAPi and pass the parameter here
            // then free the handle when not needed:
            // handle.Free();
        }
        public static T GetObjectFromPointer<T>(IntPtr pointer)
        {
            GCHandle handle = (GCHandle)pointer;
            T obj = (T)handle.Target;
            return obj;
        }
    }
}

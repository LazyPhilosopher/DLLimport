using System;
using System.Text;
using System.Runtime.InteropServices;

namespace BasePlugin
{
    class Program
    {
        #region Win32

        // Clipboard
        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("User32.dll", SetLastError = true)]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseClipboard();

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern int GlobalSize(IntPtr hMem);

        private const uint CF_UNICODETEXT = 13U;



        // bool Insert (int p1, int p2)
        [DllImport("C:\\Users\\serg\\source\\repos\\App1\\x64\\Debug\\NativeDLL.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Insert(int p1, int p2);

        #endregion

        public static string GetClipboardText()
        {
            if (!IsClipboardFormatAvailable(CF_UNICODETEXT))
            {
                return null;
            }

            try
            {
                if (!OpenClipboard(IntPtr.Zero))
                {
                    return null;
                }

                IntPtr handle = GetClipboardData(CF_UNICODETEXT);

                if (handle == IntPtr.Zero)
                {
                    return null;
                }

                IntPtr pointer = IntPtr.Zero;

                try
                {
                    pointer = GlobalLock(handle);
                    if (pointer == IntPtr.Zero)
                        return null;

                    int size = GlobalSize(handle);
                    byte[] buff = new byte[size];

                    Marshal.Copy(pointer, buff, 0, size);

                    return Encoding.Unicode.GetString(buff).TrimEnd('\0');
                }
                finally
                {
                    if (pointer != IntPtr.Zero)
                        GlobalUnlock(handle);
                }
            }
            finally
            {
                CloseClipboard();
            }
        }

        static void Main(string[] args)
        {
            string val = "";

            try
            {
                val = GetClipboardText();
            }
            catch
            {
                Console.WriteLine("err");
            }
            Console.WriteLine("From clipboard: \"{0}\"", val);


            int ret = 0;
            try
            {
                ret = Insert(100, 200);
            }
            catch
            {
                Console.WriteLine("err");
            }
            Console.WriteLine("From DLL: \"{0}\"", ret);

        }
    }
}

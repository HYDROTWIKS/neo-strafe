using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace NeoStrafe
{
    static class Program
    {
        // Windows API imports
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const int VK_SPACE = 0x20;
        private const int VK_W = 0x57;
        private const int VK_A = 0x41;
        private const int VK_S = 0x53;
        private const int VK_D = 0x44;
        private const int VK_O = 0x4F;

        private const uint KEYEVENTF_KEYDOWN = 0x0000;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        private static bool isActive = false;
        private static bool wasSpaceHeld = false;

        static void Main()
        {
            Console.WriteLine("==================================");
            Console.WriteLine("   NEO STRAFE - Keyboard Macro");
            Console.WriteLine("==================================");
            Console.WriteLine("\nToggle Key: O");
            Console.WriteLine("Spacebar: Auto-press while held");
            Console.WriteLine("WASD Speed: 30ms spam");
            Console.WriteLine("\nPress O to toggle ON/OFF");
            Console.WriteLine("Press ESC to exit");
            Console.WriteLine("\n==================================");

            while (true)
            {
                // Check for toggle key (O)
                if ((GetAsyncKeyState(VK_O) & 0x0001) != 0)
                {
                    isActive = !isActive;
                    Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] Macro {(isActive ? "ACTIVATED" : "DEACTIVATED")}");
                    Thread.Sleep(200); // Debounce
                }

                // Check for exit key (ESC)
                if ((GetAsyncKeyState(27) & 0x0001) != 0)
                {
                    Console.WriteLine("\n[Exit] Program closing...");
                    break;
                }

                if (isActive)
                {
                    bool isSpaceHeld = (GetAsyncKeyState(VK_SPACE) & 0x8000) != 0;

                    if (isSpaceHeld)
                    {
                        // Auto-press spacebar
                        if (!wasSpaceHeld)
                        {
                            keybd_event((byte)VK_SPACE, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);
                            wasSpaceHeld = true;
                        }

                        // Spam WASD keys
                        SpamKey(VK_W);
                        SpamKey(VK_A);
                        SpamKey(VK_S);
                        SpamKey(VK_D);
                    }
                    else
                    {
                        // Release spacebar if it was held
                        if (wasSpaceHeld)
                        {
                            keybd_event((byte)VK_SPACE, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                            wasSpaceHeld = false;
                        }
                    }
                }

                Thread.Sleep(10); // Main loop tick
            }
        }

        static void SpamKey(int vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);
            Thread.Sleep(15); // Half of 30ms for press duration
            keybd_event((byte)vKey, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            Thread.Sleep(15); // Half of 30ms for release duration
        }
    }
}

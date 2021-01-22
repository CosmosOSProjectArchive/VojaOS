using System;
using Sys = Cosmos.System;

namespace VOS
{
    public class Crash
    {
        public static void StopKernel(Exception ex)
        {
            //Kernel.running = false;

            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();

            string ex_message = ex.Message;
            string inner_message = "";

            Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.GS6, 250);

            Console.WriteLine("ERROR! System Halted. Crash log:\n");
            Console.WriteLine("Exception (cause of the crash): ", ex);
            if (ex.InnerException != null)
            {
                inner_message = ex.InnerException.Message;
                Console.WriteLine(inner_message);
            }
            Console.WriteLine("Looks like the system has crashed!\n");
            Console.WriteLine("If this is the first time you've seen this error screen, press any key to restart your computer.\n\nIf this screen appears again, follow these steps:");
            Console.WriteLine("Try to reinstall Vojislav's OS on your computer or Virtual Machine.\n\nYou can also try to reset the filesystem with a blank .vmdk (Virtual Machine Disk) file if you're on a Virtual Machine and if not by formatting your device.");
            Console.WriteLine("If problems continue, you can contact me at example.com");
            Console.WriteLine();
            Console.WriteLine("Press any key to reboot...");

            Console.ReadKey();

            Sys.Power.Reboot();
        }

        public static void StopKernel(string exception, string description, string lastknowaddress, string ctxinterrupt)
        {

            //Kernel.running = false;

            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();

            Console.WriteLine("CPU Exception x" + ctxinterrupt + " occured!:");
            Console.WriteLine("Exception: " + exception);
            Console.WriteLine("Description: " + description);
            if (lastknowaddress != "")
            {
                Console.WriteLine("||||Last known address: 0x" + lastknowaddress);
            }
            Console.WriteLine("Looks like the system has crashed!\n");
            Console.WriteLine("If this is the first time you've seen this error screen, press any key to restart your computer.\n\nIf this screen appears again, follow these steps:");
            Console.WriteLine("Try to reinstall Vojislav's OS on your computer or Virtual Machine.\n\nYou can also try to reset the filesystem with a blank .vmdk (Virtual Machine Disk) file if you're on a Virtual Machine and if not by formatting your device.");
            Console.WriteLine("If problems continue, you can contact me at example.com");
            Console.WriteLine();
            Console.WriteLine("Press any key to reboot...");

            Console.ReadKey();

            Sys.Power.Reboot();
        }
    }
}
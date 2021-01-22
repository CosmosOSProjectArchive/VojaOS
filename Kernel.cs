using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using System.IO;
using Cosmos.System.Graphics;
using System.Threading;

namespace VOS
{

    public class Kernel : Sys.Kernel
    {

        public bool beepoff = false;
        public bool tf = false;
        public string ver = "1.0";
        public string rev = "5";
        public static string file;
        protected VGAScreen screen;
        public int count = 0;
        public bool running = false;
        public bool blink = true;
        public static string current_directory = "0:\\";
        protected override void BeforeRun()
        {
            FS = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(FS);
            FS.Initialize();
            running = true;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\n VojaOS booted succesfully. Enjoy!\n\n NOTICE: VirtualBox does not work properly with VojaOS.\n Version: " + ver + " Revision: " + rev);
            Console.WriteLine("\n System time:\n " + Cosmos.HAL.RTC.Hour + ":" + Cosmos.HAL.RTC.Minute + ":" + Cosmos.HAL.RTC.Second);
            Console.WriteLine(" "+Cosmos.HAL.RTC.DayOfTheMonth + "." + Cosmos.HAL.RTC.Month + "." + "20" + Cosmos.HAL.RTC.Year);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            //screen.SetGraphicsMode();
            Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E5, 100);
            
        }
        public Cosmos.System.FileSystem.CosmosVFS FS = null;
        protected override void Run()
        {
            Console.WriteLine("\n\nThis is Vojislav's OS v. 1.0\n");
            try { 
                while (running) 
                {
                    
                    Console.Write(current_directory + "> ");

                    string input = Console.ReadLine();

                    CMD(input);

                } 
            }
            catch(Exception ex)
            {
                
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E5, 200);
                running = false;
                Crash.StopKernel(ex);
            }

            
        }
        public void CMD(string input)
        {
            string[] args = input.Split(' ');
            
            
            if (args[0] == "clear" || args[0] == "cls")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                
                Console.Clear();
            }
            else if (args[0] == "mkdir")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                string file = args[1];
                Directory.CreateDirectory(current_directory + file);
            }

            else if (args[0] == "cd")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                if (args.Length == 1)
                {
                    current_directory = "0:\\";
                }
                else
                {
                    var newdir = args[1];
                    if (newdir == "..")
                    {
                        if (current_directory != "0:\\")
                        {
                            var dir = FS.GetDirectory(current_directory);
                            string p = dir.mParent.mFullPath;
                            current_directory = p;
                        }
                    }
                    else if (Directory.Exists(current_directory + newdir))
                    {
                        current_directory += newdir + "\\";
                    }
                }
            }

            else if (args[0] == "cat")
            {
                
                if(args.Length == 1)
                {
                    if (beepoff == false)
                    {
                        //Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E1, 100);
                    }
                    
                    Console.WriteLine("Usage: cat [file path]");
                }
                else
                {
                    string file = args[1];
                    if (File.Exists(current_directory + file))
                    {
                        if (beepoff == false)
                        {
                            Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                        }
                        Console.BackgroundColor = ConsoleColor.DarkGreen;

                        Console.WriteLine(File.ReadAllText(current_directory + file));
                    }
                    else
                    {
                        if (beepoff == false)
                        {
                            Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E1, 100);
                        }
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("File doesn't exist.");
                    }
                }

            }

            else if (args[0] == "mkfil")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                string file = args[1];
                File.Create(current_directory + file);
            }

            else if (args[0] == "shutdown")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\nSystem shutting down...");
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E5, 300);
                Sys.Power.Shutdown();
            }

            else if (args[0] == "reboot")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\nSystem rebooting...");
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E5, 250);
                Sys.Power.Reboot();
            }

            else if (args[0] == "dir" || args[0] == "ls")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("Type\tName");
                foreach (var dir in Directory.GetDirectories(current_directory))
                {
                    Console.WriteLine("d\t" + dir);
                }
                foreach (var dir in Directory.GetFiles(current_directory))
                {
                    Console.WriteLine("-\t" + dir);
                }
            }

            else if (args[0] == "sysinfo")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Operating system name:     VOS");
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Operating system version:  " + ver);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Operating system revision: " + rev);
                //Console.WriteLine("Date and time:             " + Utils.Time.MonthString() + "/" + Utils.Time.DayString() + "/" + Utils.Time.YearString() + ", " + Utils.Time.TimeString(true, true, true));
                //Console.WriteLine("System boot time:          " + boottime);
                //Console.WriteLine("Total memory:              " + Core.MemoryManager.TotalMemory + "MB");
                //Console.WriteLine("Used memory:               " + Core.MemoryManager.GetUsedMemory() + "MB");
                //Console.WriteLine("Free memory:               " + Core.MemoryManager.GetFreeMemory() + "MB");
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("File system type:          " + FS.GetFileSystemType("0:\\"));
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Available free space:      " + (FS.GetAvailableFreeSpace("0:\\") / 1024) / 1024 + "MB");
            }

            else if (args[0] == "rm")
            {
                if(args.Length == 1)
                {
                    Console.WriteLine("Usage:\nrm [file or folder path]");
                }
                else
                {
                    string file = args[1];
                    if (File.Exists(current_directory + file))
                    {
                        File.Delete(current_directory + file);
                        if (beepoff == false)
                        {
                            Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                        }
                    }
                    else
                    {
                        if (Directory.Exists(current_directory + file))
                        {
                            Directory.Delete(current_directory + file);
                            if (beepoff == false)
                            {
                                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                            }
                        }
                        else
                        {
                            if (beepoff == false)
                            {
                                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E1, 100);
                            }
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine("File or Directory doesn't exist.");
                        }
                    }
                }
            }

            else if (args[0] == "pwd")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine(current_directory);
            }

            else if (args[0] == "beepoff")
            {
                if (beepoff == false)
                {
                    //Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    
                    Console.WriteLine("Sounds and beeps disabled!");
                    beepoff = true;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                    Console.WriteLine("Sounds and beeps enabled!");
                    beepoff = false;
                }
                
            }

            else if(args[0] == "miv")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                MIV.StartMIV();
            }

            else if (args[0] == "banner")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\n VojaOS booted succesfully. Enjoy!\n\n NOTICE: VirtualBox does not work properly with VojaOS.\n Version: " + ver + " Revision: " + rev);
                Console.WriteLine("\n System time:\n " + Cosmos.HAL.RTC.Hour + ":" + Cosmos.HAL.RTC.Minute + ":" + Cosmos.HAL.RTC.Second);
                Console.WriteLine(" " + Cosmos.HAL.RTC.DayOfTheMonth + "." + Cosmos.HAL.RTC.Month + "." + "20" + Cosmos.HAL.RTC.Year);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }

            else if (args[0] == "beep")
            {
                if (args.Length < 3)
                {
                    Console.WriteLine("Usage: beep [number1] [number2]");
                }
                else
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Convert.ToInt64(args[1]), (uint)Convert.ToInt64(args[2]));
                }
            }

            else if (args[0] == "help")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                Console.WriteLine("Commands:\n[cd] [mkdir] [time] [mkfil] [crash / crash2 / halt]\n[chime] [beep] [miv] [pwd] [help]\n[beepoff] [chime] [rm] [sysinfo] [ls / dir]\n[reboot] [shutdown] [cat] [cls / clear] [info]\n[info] [netstats] [version-check]");
                //Console.WriteLine("Commands:\n\ncd\nmkdir\ntime\nmkfil\ncrash\nchime\nbeep\nmiv\npwd\nhelp\nbeepoff\nchime\nrm\nsysinfo\nls / dir\nreboot\nshutdown\ncat\ncls / clear\ninfo\nnetstats\nversion-check\n");
            }

            else if (args[0] == "info")
            {
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.F4, 300);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.F5, 300);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.F6, 300);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nVOS (Vojislav's OS) is a DOS-like operating system made by Vojislav.\n");
            }

            else if(args[0] == "time")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("Time:");
                Console.WriteLine(Cosmos.HAL.RTC.Hour+":" + Cosmos.HAL.RTC.Minute + ":" + Cosmos.HAL.RTC.Second);
                Console.WriteLine("Date:");
                Console.WriteLine(Cosmos.HAL.RTC.DayOfTheMonth + "." + Cosmos.HAL.RTC.Month + "." + "20"+Cosmos.HAL.RTC.Year);
            }

            else if (args[0] == "netstats")
            {

                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E4, 100);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E5, 100);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E6, 100);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo ethernet driver present.\n");
            }

            else if (args[0] == "chime")
            {
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.B5, 300);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.B6, 300);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.B4, 300);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.B5, 300);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.B6, 300);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.B2, 100);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.B2, 100);
            }

            else if (args[0] == "version-check")
            {
                Console.WriteLine("\nChecking for updates, please wait...\n");
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E2, 300);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 300);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E4, 300);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E5, 300);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E6, 300);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo ethernet driver present so no updates lmao.\n");
            }

            else if (args[0] == "crash")
            {
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E4, 50);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E4, 50);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Clear();
                int[] lel = { 1, 2, 3 };
                Console.WriteLine(lel[10]);
            }

            else if (args[0] == "crash2")
            {
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E4, 50);
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E4, 50);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Clear();
                int[] lel = { 1, 2, 3 };
                int[] lel2 = { 1, 2, 3, 4 };
                Console.WriteLine(lel[10] + lel2[5]);
            }

            else if(args[0] == "halt")
            {
                
                Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E5, 200);
                running = false;
                
            }

            else if (args[0] == "install")
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("Are you sure that you want to install the filesystem? (Y/n):");
                string yn = Console.ReadLine();
                if(yn.Length < 2)
                {
                    if (yn=="Y" || yn == "y")
                    {
                        Console.WriteLine("Trying to install the filesystem...");
                        Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.D1, 200);
                        Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.D2, 200);
                        Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.D3, 200);
                        Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.D4, 200);
                        Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.D5, 300);
                        FS.Initialize();
                    }
                    else
                    {
                        Console.WriteLine("Install aborted.");
                    }
                }
                else
                {
                    if (beepoff == false)
                    {
                        Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E1, 100);
                    }
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Unknown command!");
                }
                
            }

            else if (args[0] == "print")
            {
                tf = false;
                count = 1;
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E3, 100);
                }
                while (tf==false)
                {
                    if(args[count] != null)
                    {
                        Console.Write(args[count]);
                        count = count + 1;
                    }
                    else
                    {
                        count = 0;
                        tf = true;
                        
                    }
                }
                //Console.WriteLine(args[1]);
            }

            else
            {
                if (beepoff == false)
                {
                    Cosmos.System.PCSpeaker.Beep((uint)Cosmos.System.Notes.E1, 100);
                }
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Unknown command!");
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
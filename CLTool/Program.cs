using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActiveHomeScriptLib;

namespace CLTool
{
    class Program
    {
        static ActiveHome home = new ActiveHome();
        static readonly string[] houseCodes = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p" };
        static readonly string[] unitCodes = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };
        static readonly string[] commands = { "on", "off", "dim", "bright" };

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Invalid parameters. Correct usage:\ncltool.exe <houseCode> <unitCode> <command>");
                return;
            }

            string houseCode = args[0].ToLower();
            string unitCode = args[1].ToLower();
            string command = args[2].ToLower();

            if (!houseCodes.Contains(houseCode))
            {
                Console.WriteLine("Error: Invalid houseCode parameter. Valid house codes include: ");
                foreach (var c in houseCodes)
                    Console.Write(c + " ");
                Console.WriteLine("");
                return;
            }

            // check for all units special case
            if (unitCode == "allunits")
            {
                if (command == "off")
                {
                    home.SendAction("sendplc", houseCode + "1 allunitsoff", null, null);
                    Console.WriteLine("All units turned off");
                    return;
                }
                else
                {
                    Console.WriteLine("Error: Invalid command parameter. Valid commands for all units include:\noff");
                    return;
                }
            }

            // check for all lights special cases
            if (unitCode == "alllights")
            {
                if (command == "off" || command == "on")
                {
                    home.SendAction("sendplc", houseCode + "1 alllights" + command, null, null);
                    Console.WriteLine("All lights turned " + command);
                    return;
                }
                else
                {
                    Console.WriteLine("Error: Invalid command parameter. Valid commands for all lights include:\noff on");
                    return;
                }
            }

            if (!unitCodes.Contains(unitCode))
            {
                Console.WriteLine("Error: Invalid unitCode parameter. Valid unit codes include: ");
                foreach (var c in unitCodes)
                    Console.Write(c + " ");
                Console.WriteLine("");
                return;
            }
            if (!commands.Contains(command))
            {
                Console.WriteLine("Error: Invalid command parameter. Valid commands include: ");
                foreach (var c in commands)
                    Console.Write(c + " ");
                Console.WriteLine("");
                return;
            }

            string parameters = houseCode + unitCode + " " + command;

            if (command == "dim" || command == "bright")
                parameters += " 10";

            home.SendAction("sendplc", parameters, null, null);

            Console.WriteLine(houseCode + unitCode + " " + command);
            return;
        }
    }
}

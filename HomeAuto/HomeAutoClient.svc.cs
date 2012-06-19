using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ActiveHomeScriptLib;

namespace HomeAuto
{
    public class HomeAutoClient : IHomeAutoClient
    {
        static ActiveHome home = new ActiveHome();
        static readonly string[] houseCodes = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p" };
        static readonly string[] unitCodes = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };
        static readonly string[] commands = { "on", "off", "dim", "bright" };

        public ResultDTO ExecuteX10Command(string houseCode, string unitCode, string command)
        {
            houseCode = houseCode.ToLower();
            unitCode = unitCode.ToLower();
            command = command.ToLower();

            if (!houseCodes.Contains(houseCode))
                return new ResultDTO { Outcome = "Error", Message = "Invalid house code" };

            // check for all units special case
            if (unitCode == "allunits")
            {
                if (command == "off")
                {
                    home.SendAction("sendplc", houseCode + "1 allunitsoff", null, null);
                    return new ResultDTO { Outcome = "Success", Message = "All units turned off" };
                }
                else
                    return new ResultDTO { Outcome = "Error", Message = "Invalid command" };
            }

            // check for all lights special cases
            if (unitCode == "alllights")
            {
                if (command == "off" || command == "on")
                {
                    home.SendAction("sendplc", houseCode + "1 alllights" + command, null, null);
                    return new ResultDTO { Outcome = "Success", Message = "All lights turned " + command };
                }
                else
                    return new ResultDTO { Outcome = "Error", Message = "Invalid command" };
            }

            if (!unitCodes.Contains(unitCode))
                return new ResultDTO { Outcome = "Error", Message = "Invalid unit code" };

            if (!commands.Contains(command))
                return new ResultDTO { Outcome = "Error", Message = "Invalid command" };

            string parameters = houseCode + unitCode + " " + command;

            if (command == "dim" || command == "bright")
                parameters += " 10";

            home.SendAction("sendplc", parameters, null, null);

            return new ResultDTO { Outcome = "Success", Message = houseCode + unitCode + " " + command };
        }   
    }

    [DataContract]
    public class ResultDTO
    {
        [DataMember]
        public string Outcome { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}

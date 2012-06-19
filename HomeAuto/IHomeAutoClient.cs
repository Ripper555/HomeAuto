using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace HomeAuto
{

    [ServiceContract]
    public interface IHomeAutoClient
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "x10/{houseCode}/{unitCode}/{command}")]
        ResultDTO ExecuteX10Command(string houseCode, string unitCode, string command);
    }


 
    
}

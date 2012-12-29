using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net;

namespace NetWhois.Imp.Settings
{
    public class Settings : ISettings
    {        
        public EndPoint Bind
        {
            get 
            { 
                var localEp = ConfigurationManager.AppSettings["bind"];
                if (localEp == null)
                    throw new ConfigurationErrorsException("AppSettings: bind is not found");

                string[] hostPort = localEp.Split(':');
                if (hostPort.Length != 2)
                    throw new ConfigurationErrorsException("AppSettings: bind value is incorrect. Use host:port format");
                
                int port;
                if (!int.TryParse(hostPort[1], out port))
                    throw new ConfigurationErrorsException("AppSettings: bind value is incorrect. Port parsing error!");

                if (hostPort[0].ToLower(CultureInfo.InvariantCulture) == "any")
                {
                    return new IPEndPoint(IPAddress.Any, port);
                }

                return new IPEndPoint(IPAddress.Parse(hostPort[0]), port);
            }
        }
    }
}
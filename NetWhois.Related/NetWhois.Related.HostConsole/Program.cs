using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetWhois.Components;
using NetWhois.Imp.Protocol;
using NetWhois.Related.Bootsloader;

namespace NetWhois.Related.HostConsole
{
	class Program
	{
		static void Main(string[] args)
		{
		    try
		    {
		        var objectFactory = Ioc.Initialize();
		        var socketFactory = objectFactory.Instance<ISocketAdapterFactory>();
		        var listenningSocket = socketFactory.Create(
                        new Socket(SocketType.Stream, ProtocolType.Tcp)
		            );

		        var server = objectFactory.Instance<IWhoisServer>(new {socket = listenningSocket});
		        server.StartAsync();

		        Console.WriteLine("Server started!");
		        Console.ReadLine();
		    }
		    catch (Exception ex)
		    {
                Console.WriteLine("Exceprion : " + ex.Message);
		        Console.ReadLine();
		    }
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetWhois.Imp.Protocol;
using NetWhois.Related.Bootsloader;

namespace NetWhois.Related.HostConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var objectFactory = Ioc.Initialize();
			var server = objectFactory.Instance<IWhoisServer>();
			server.StartAsync();

			Console.WriteLine("Server started!");
			Console.ReadLine();
		}
	}
}

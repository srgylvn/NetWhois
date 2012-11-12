using System.Threading.Tasks;

namespace NetWhois.Imp.Protocol
{
	public interface IWhoisServer
	{
		Task StartAsync();
		void Stop();
	}
}
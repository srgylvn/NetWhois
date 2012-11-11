using System.Threading.Tasks;

namespace NetWhois.Imp
{
	public interface IWhoisServer
	{
		Task StartAsync();
		void Stop();
	}
}
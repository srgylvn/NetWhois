using System.Threading.Tasks;

namespace NetWhois.Imp.Protocol
{
	public interface IWhoisProtocol
	{
		Task<string> GetRequestAsync();
		Task<int> SendResponseAsync(string response);
	}
}
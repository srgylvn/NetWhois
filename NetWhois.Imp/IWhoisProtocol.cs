using System.Threading.Tasks;

namespace NetWhois.Imp
{
	public interface IWhoisProtocol
	{
		Task<string> GetRequestAsync();
		Task<int> SendResponseAsync(string response);
	}
}
using System.Threading.Tasks;

namespace NetWhois.Imp.Response
{
	public interface IResponseBuilder
	{
		Task<string> BuildResponseAsync(string domainName);
	}
}
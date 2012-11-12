using System.Threading.Tasks;

namespace NetWhois.Imp.Response
{
	public class ResponseBuilder : IResponseBuilder
	{
		public Task<string> BuildResponseAsync(string domainName)
		{
			return new TaskFactory<string>().StartNew(
				() => BuildResponse(domainName)
			);
		}

		private string BuildResponse(string domainName)
		{
			return "Response: " + domainName + System.Environment.NewLine;
		}
	}
}
using System.Threading.Tasks;
using NetWhois.Imp.Response;

namespace NetWhois.Imp.Protocol
{
	public class WhoisRoutine : IWhoisRoutine
	{
		private IResponseBuilder _responseBuilder;
		public WhoisRoutine(IResponseBuilder responseBuilder)
		{
			_responseBuilder = responseBuilder;
		}

		public Task RunAsync(IWhoisProtocol protocol)
		{
			return new TaskFactory().StartNew(
					() => ProcessRoutine(protocol)
				);
		}

		private async void ProcessRoutine(IWhoisProtocol protocol)
		{
			string request = await protocol.GetRequestAsync();
			string response = await _responseBuilder.BuildResponseAsync(request);
			await protocol.SendResponseAsync(response);
		}
	}
}
using System;
using System.Threading.Tasks;
using NetWhois.Components;
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

		public Task RunAsync(IWhoisProtocol protocol, Action<Exception> onSocketError)
		{
			return new TaskFactory().StartNew(
				() => SocketUtilities.Try(
					() => ProcessRoutine(protocol), onSocketError));
		}

		private async void ProcessRoutine(IWhoisProtocol protocol)
		{			
			string request = await protocol.GetRequestAsync();
			string response = await _responseBuilder.BuildResponseAsync(request);
			await protocol.SendResponseAsync(response);				
		}
	}
}
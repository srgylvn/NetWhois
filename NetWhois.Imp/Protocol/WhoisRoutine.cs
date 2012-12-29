using System;
using System.Threading.Tasks;
using NetWhois.Components;
using NetWhois.Imp.Response;

namespace NetWhois.Imp.Protocol
{
    public static class TaskExtension
    {
        public static T Await<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }

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

		private void ProcessRoutine(IWhoisProtocol protocol)
		{    
			string request = protocol.GetRequestAsync().Await<string>();
			string response = _responseBuilder.BuildResponseAsync(request).Await<string>();
			protocol.SendResponseAsync(response).Wait();				
		}
	}
}
using System.Threading.Tasks;
using NetWhois.Components;

namespace NetWhois.Imp.Protocol
{
	public class WhoisServer : IWhoisServer
	{
		private ISocketAsyncAdapter _socket;
		private ISocketAdapterFactory _socketFactory;
		private IProtocolFactory _protocolFactory;
		private IWhoisRoutine _whoisRoutine;
		private bool _bStop;

		public WhoisServer(ISocketAsyncAdapter socket, ISocketAdapterFactory socketFactory, IProtocolFactory protocolFactory, IWhoisRoutine whoisRoutine)
		{
			_socket = socket;
			_socketFactory = socketFactory;
			_protocolFactory = protocolFactory;
			_whoisRoutine = whoisRoutine;
			_bStop = true;
		}

		public Task StartAsync()
		{
			_bStop = false;
			return new TaskFactory().StartNew(
				(s) =>
					{
						while (!_bStop)
						{
							ProcessSingleConnection((ISocketAsyncAdapter)s);
						}
					}, _socket);
		}

		internal async void ProcessSingleConnection(ISocketAsyncAdapter serverSocket)
		{			
			var socket = await serverSocket.AcceptAsync();
			var asyncClientSocket = _socketFactory.Create(socket);
			var protocol = _protocolFactory.CreateWhois(asyncClientSocket);

			_whoisRoutine
				.RunAsync(protocol)
				.ContinueWith((_) => asyncClientSocket.Close());
		}

		public void Stop()
		{
			_bStop = true;
		}
	}
}
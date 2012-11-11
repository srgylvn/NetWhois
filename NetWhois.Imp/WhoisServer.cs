using System.Threading.Tasks;
using NetWhois.Components;

namespace NetWhois.Imp
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

		internal void ProcessSingleConnection(ISocketAsyncAdapter serverSocket)
		{			
			var taskAccept = serverSocket.AcceptAsync();
			taskAccept.Wait();
			var socket = taskAccept.Result;
			var asyncClientSocket = _socketFactory.Create(socket);
			var protocol = _protocolFactory.CreateWhois(asyncClientSocket);

			_whoisRoutine.RunAsync(asyncClientSocket, protocol);
		}

		public void Stop()
		{
			_bStop = true;
		}
	}
}
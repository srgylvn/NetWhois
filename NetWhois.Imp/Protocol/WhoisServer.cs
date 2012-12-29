using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NetWhois.Components;
using NetWhois.Imp.Settings;

namespace NetWhois.Imp.Protocol
{
	public class WhoisServer : IWhoisServer
	{
		private IAsyncSocketAdapter _socket;
		private ISocketAdapterFactory _socketFactory;
		private IProtocolFactory _protocolFactory;
		private IWhoisRoutine _whoisRoutine;
	    private ISettings _settings;
		private bool _bStop;

		public WhoisServer(IAsyncSocketAdapter socket, ISocketAdapterFactory socketFactory, IProtocolFactory protocolFactory, IWhoisRoutine whoisRoutine, ISettings settings)
		{
			_socket = socket;
			_socketFactory = socketFactory;
			_protocolFactory = protocolFactory;
			_whoisRoutine = whoisRoutine;
		    _settings = settings;
		    _bStop = true;
		}

		public Task StartAsync()
		{
			_bStop = false;
            
            var localIp = _settings.Bind;
            _socket.Bind(localIp);
            _socket.Listen();

			return new TaskFactory().StartNew(
				(s) =>
				    {
				        ProcessSingleConnection((IAsyncSocketAdapter) s);
				    }, _socket);
		}

		internal async void ProcessSingleConnection(IAsyncSocketAdapter serverSocket)
		{
			Task<Socket> tSocket = serverSocket.AcceptAsync();
		    tSocket.Wait();
		    var socket = tSocket.Result;

			var asyncClientSocket = _socketFactory.Create(socket);
			var protocol = _protocolFactory.CreateWhois(asyncClientSocket);			

			var cancellationToken = new CancellationTokenSource();
			Action<Exception> onSocketError = (_) => cancellationToken.Cancel();

			await _whoisRoutine
			    .RunAsync(protocol, onSocketError)
			    .ContinueWith(
			        (t) =>
			            {
			                if (!t.IsCanceled)
			                {
			                    SocketUtilities.Try(() => asyncClientSocket.Close(), onSocketError);
			                }
			            },
			        cancellationToken.Token
			    );
		}

		public void Stop()
		{
			_bStop = true;
		}
	}
}
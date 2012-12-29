using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using NetWhois.Components;
using NetWhois.Imp.Protocol;
using NetWhois.Imp.Settings;

namespace NetWhois.Imp.Related.Tests
{
	[TestFixture]
	public class WhoisServerTests
	{
		private ISocketAdapterFactory _socketFactory;
		private IAsyncSocketAdapter _socket;
		private IProtocolFactory _protocolFactory;
		private IWhoisProtocol _protocol;
		private IWhoisRoutine _whoisRoutine;
	    private ISettings _settings;
		private Socket _dummySocket;
	    private EndPoint _localEp;
		
		[SetUp]
		public void Setup()
		{
			_socket = A.Fake<IAsyncSocketAdapter>();
			_socketFactory = A.Fake<ISocketAdapterFactory>();
			_protocolFactory = A.Fake<IProtocolFactory>();
			_protocol = A.Fake<IWhoisProtocol>();
			_whoisRoutine = A.Fake<IWhoisRoutine>();
            _settings = A.Fake<ISettings>();
		    _localEp = new IPEndPoint(IPAddress.Any, 43);
			_dummySocket = new Socket(SocketType.Stream, ProtocolType.Tcp);            

			InitDefaults();
		}

		private void InitDefaults()
		{
			A.CallTo(() => _socket.AcceptAsync())
				.Returns((new TaskFactory<Socket>()).StartNew(() => _dummySocket));
			A.CallTo(() => _socketFactory.Create(A<Socket>._))
				.Returns(_socket);
			A.CallTo(() => _protocolFactory.CreateWhois(A<IAsyncSocketAdapter>._))
				.Returns(_protocol);
		    A.CallTo(() => _settings.Bind).Returns(_localEp);
		}

		private WhoisServer CreateServer()
		{
			return new WhoisServer(_socket, _socketFactory, _protocolFactory, _whoisRoutine, _settings);
		}

        //[Test]
        //public void ProcessSingleConnection_UsesSettigs()
        //{
        //    var server = CreateServer();

        //    server.ProcessSingleConnection(_socket);

        //    A.CallTo(() => _settings.Bind).MustHaveHappened();
        //}        

        //[Test]
        //public void ProcessSingleConnection_BindsSocket()
        //{
        //    var server = CreateServer();

        //    server.ProcessSingleConnection(_socket);

        //    A.CallTo(() => _socket.Bind(_localEp)).MustHaveHappened();
        //}

        //[Test]
        //public void ProcessSingleConnection_CallsListen()
        //{
        //    var server = CreateServer();

        //    server.ProcessSingleConnection(_socket);

        //    A.CallTo(() => _socket.Listen()).MustHaveHappened();
        //}

		[Test]
		public void ProcessSingleConnection_StartsAcceptingConnections()
		{
			var server = CreateServer();

			server.ProcessSingleConnection(_socket);			

			A.CallTo(() => _socket.AcceptAsync()).MustHaveHappened();
		}

		[Test]
		public void ProcessSingleConnection_UsesAsyncFactory()
		{
			var server = CreateServer();

			server.ProcessSingleConnection(_socket);			

			A.CallTo(() => _socketFactory.Create(_dummySocket)).MustHaveHappened();
		}

		[Test]
		public void ProcessSingleConnection_UsesFactoryToCreateWhoisProtocol()
		{
			var server = CreateServer();

			server.ProcessSingleConnection(_socket);

			A.CallTo(() => _protocolFactory.CreateWhois(_socket)).MustHaveHappened();
		}

		[Test]
		public void ProcessSingleConnection_CallsWhoisRequestRoutine()
		{
			var server = CreateServer();

			server.ProcessSingleConnection(_socket);

			A.CallTo(() => _whoisRoutine.RunAsync(_protocol, A<Action<Exception>>._));
		}

		[Test]
		public void Stop_StopsSeverAcceptingConnections()
		{
			var server = CreateServer();

			var task = server.StartAsync();
			System.Threading.Thread.Sleep(1000);
			server.Stop();
			bool res = task.Wait(1000);

			Assert.That(res, Is.True);
			A.CallTo(() => _socket.AcceptAsync()).MustHaveHappened(Repeated.AtLeast.Twice);
		}

		[Test]
		public void ProcessSingleConnection_ClosesSocket()
		{
			A.CallTo(() => _whoisRoutine.RunAsync(A<IWhoisProtocol>._, A<Action<Exception>>._))
				.Returns(new TaskFactory().StartNew(() => { }));

			var server = CreateServer();
			server.ProcessSingleConnection(_socket);			
			System.Threading.Thread.Sleep(1000); // wait for continue with is called

			A.CallTo(() => _socket.Close()).MustHaveHappened();
		}

		[Test]
		public void ProcessSingleConnection_CloseIsNotCalledOnSocketError()
		{
			A.CallTo(() => _whoisRoutine.RunAsync(A<IWhoisProtocol>._, A<Action<Exception>>._))
				.Invokes(x => x.GetArgument<Action<Exception>>("onSocketError").Invoke(new SocketException()))
				.Returns(new TaskFactory().StartNew(() => { }));

			var server = CreateServer();
			server.ProcessSingleConnection(_socket);
			System.Threading.Thread.Sleep(1000); // wait for continue with is called

			A.CallTo(() => _socket.Close()).MustNotHaveHappened();
		}
	}
}
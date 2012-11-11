using System.Net.Sockets;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using NetWhois.Components;

namespace NetWhois.Imp.Related.Tests
{
	[TestFixture]
	public class WhoisServerTests
	{
		private ISocketAdapterFactory _socketFactory;
		private ISocketAsyncAdapter _socket;
		private IProtocolFactory _protocolFactory;
		private IWhoisProtocol _protocol;
		private IWhoisRoutine _whoisRoutine;
		private Socket _dummySocket;
		
		[SetUp]
		public void Setup()
		{
			_socket = A.Fake<ISocketAsyncAdapter>();
			_socketFactory = A.Fake<ISocketAdapterFactory>();
			_protocolFactory = A.Fake<IProtocolFactory>();
			_protocol = A.Fake<IWhoisProtocol>();
			_whoisRoutine = A.Fake<IWhoisRoutine>();
			_dummySocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

			InitDefaults();
		}

		private void InitDefaults()
		{
			A.CallTo(() => _socket.AcceptAsync())
				.Returns((new TaskFactory<Socket>()).StartNew(() => _dummySocket));
			A.CallTo(() => _socketFactory.Create(A<Socket>._))
				.Returns(_socket);
			A.CallTo(() => _protocolFactory.CreateWhois(A<ISocketAsyncAdapter>._))
				.Returns(_protocol);
		}

		private WhoisServer CreateServer()
		{
			return new WhoisServer(_socket, _socketFactory, _protocolFactory, _whoisRoutine);
		}

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

			A.CallTo(() => _whoisRoutine.RunAsync(_socket, _protocol));
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
	}
}
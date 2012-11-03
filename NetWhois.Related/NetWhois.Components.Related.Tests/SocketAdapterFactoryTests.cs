using System.Net.Sockets;
using NUnit.Framework;
using NetWhois.Related.Bootsloader;

namespace NetWhois.Components.Related.Tests
{
	[TestFixture]
	public class SocketAdapterFactoryTests
	{
		private IObjectFactory _of;
		public SocketAdapterFactoryTests()
		{
			_of = Ioc.Initialize();
		}

		private SocketAdapterFactory CreateFactory()
		{
			return new SocketAdapterFactory(_of);
		}

		[Test]
		public void Create_Socket_ReturnsAsyncSockeytAdapterIntance()
		{
			var s = new Socket(SocketType.Stream, ProtocolType.Tcp);
			
			var adapter = CreateFactory().Create(s);

			Assert.That(adapter, Is.TypeOf<AsyncSocketAdapter>());
			Assert.That(adapter.Socket, Is.EqualTo(s));
		}
	}
}
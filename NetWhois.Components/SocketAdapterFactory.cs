using System.Net.Sockets;

namespace NetWhois.Components
{
	public class SocketAdapterFactory : ISocketAdapterFactory
	{
		private IObjectFactory _objectFactory;
		public SocketAdapterFactory(IObjectFactory objectFactory)
		{
			_objectFactory = objectFactory;
		}

		public IAsyncSocketAdapter Create(Socket socket)
		{
			return _objectFactory.Instance<IAsyncSocketAdapter>(new {socket });
		}
	}
}
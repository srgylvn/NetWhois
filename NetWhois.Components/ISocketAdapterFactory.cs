using System.Net.Sockets;

namespace NetWhois.Components
{
	public interface ISocketAdapterFactory
	{
		IAsyncSocketAdapter Create(Socket socket);
	}
}
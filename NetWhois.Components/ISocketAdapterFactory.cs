using System.Net.Sockets;

namespace NetWhois.Components
{
	public interface ISocketAdapterFactory
	{
		ISocketAsyncAdapter Create(Socket socket);
	}
}
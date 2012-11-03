using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetWhois.Components
{
	public interface ISocketAsyncAdapter
	{
		Task AcceptAsync();
		Task<int> ReceiveAsync(byte[] buffer, int offset, int size);
		Task<int> SendAsync(byte[] buffer, int offset, int size);
		Socket Socket { get; }
	}
}
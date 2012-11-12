using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetWhois.Components
{
	public interface ISocketAsyncAdapter
	{
		Task<Socket> AcceptAsync();
		Task<int> ReceiveAsync(byte[] buffer, int offset, int size);
		Task<int> SendAsync(byte[] buffer, int offset, int size);
		Socket Socket { get; }
		Task Close();
	}
}
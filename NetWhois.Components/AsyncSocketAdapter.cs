using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetWhois.Components
{
	public class AsyncSocketAdapter : IAsyncSocketAdapter
	{		
        public Socket Socket { get; private set; }
	    public AsyncSocketAdapter(Socket socket)
	    {
	        Socket = socket;
	    }

	    public Task<Socket> AcceptAsync()
		{
			return Task<Socket>.Factory.FromAsync(Socket.BeginAccept, Socket.EndAccept, null);
		}

		public Task<int> ReceiveAsync(byte[] buffer, int offset, int size)
		{
			return Task<int>.Factory.FromAsync(
				(callback, state) => Socket.BeginReceive(buffer, offset, size, SocketFlags.None, callback, state),
				Socket.EndReceive,
				null);
		}

		public Task<int> SendAsync(byte[] buffer, int offset, int size)
		{
			return Task<int>.Factory.FromAsync(
				(callback, state) => Socket.BeginSend(buffer, offset, size, SocketFlags.None, callback, state),
				Socket.EndSend,
				null
			);
		}

		public Task Close()
		{
			return new TaskFactory().StartNew(
				() =>
					{
						Socket.Shutdown(SocketShutdown.Both);
						Socket.Close();
					});
		}

	    public void Bind(EndPoint lockalEp)
	    {
	        Socket.Bind(lockalEp);
	    }

	    public void Listen()
	    {
	        Socket.Listen(256);
	    }
	}
}
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetWhois.Components;

namespace NetWhois.Imp.Related.Tests.Fakes
{
	public class CustomAsyncAdapterFake : ISocketAsyncAdapter
	{
		private List<byte[]> _sequence;
		private int _pointer;

		public CustomAsyncAdapterFake(params string[] sequence)
		{
			_sequence = new List<byte[]>();

			foreach (var data in sequence)
			{
				_sequence.Add(
					Encoding.ASCII.GetBytes(data)
				);
			}
		}

		public Task<Socket> AcceptAsync()
		{
			return (new TaskFactory<Socket>().StartNew(
				() => new Socket(SocketType.Unknown, ProtocolType.Unknown)
				));
		}

		public Task<int> ReceiveAsync(byte[] buffer, int offset, int size)
		{
			var bytes = Math.Min(size - offset, _sequence[_pointer].Length);
			for (int i = offset; i < bytes; i++)
			{
				buffer[i] = (_sequence[_pointer])[i];
			}
			_pointer ++;
			return (new TaskFactory<int>().StartNew(() => bytes));
		}

		public Task<int> SendAsync(byte[] buffer, int offset, int size)
		{
			return (new TaskFactory<int>().StartNew(
				() => Math.Min(buffer.Length, size - offset))
			       );
		}

		public Task Close()
		{
			return new TaskFactory().StartNew(() => { });
		}

		public Socket Socket { get; set; }
	}
}
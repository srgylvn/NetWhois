using System.Text;
using System.Threading.Tasks;
using NetWhois.Components;

namespace NetWhois.Imp
{
	public class WhoisProtocol : IWhoisProtocol
	{
		private const string Pattern = "\r\n";

		private readonly ISocketAsyncAdapter _asyncAdapter;
		public WhoisProtocol(ISocketAsyncAdapter asyncAdapter)
		{
			_asyncAdapter = asyncAdapter;
		}

		public async Task<string> GetRequestAsync()
		{
			var sb = new StringBuilder();
			var buffer = new byte[1024];

			int receivedSize = 0;
			int patternPosion = -1;
			do
			{
				receivedSize = await _asyncAdapter.ReceiveAsync(buffer, 0, 1024);
				if (receivedSize == 0)
					return string.Empty;

				var str = Encoding.ASCII.GetChars(buffer, 0, receivedSize);
				sb.Append(str);
				patternPosion = sb.ToString().IndexOf(Pattern);
				if (patternPosion != -1)
					break;					

			} while (receivedSize > 0);
			
			return sb.ToString().Substring(0, patternPosion);
		}

		public async Task<int> SendResponseAsync(string response)
		{
		    byte[] buffer = Encoding.ASCII.GetBytes(response);
			int offset = 0;
			while(offset < buffer.Length - 1)
			{				
				int sent = await _asyncAdapter.SendAsync(
					buffer, offset, buffer.Length - offset
					);
				
				if (sent == 0)
					return offset;

				offset += sent;
			}

			return response.Length;
		}		
	}
}
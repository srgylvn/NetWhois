using System.Threading.Tasks;
using NetWhois.Components;

namespace NetWhois.Imp
{
	public class WhoisRoutine : IWhoisRoutine
	{
		public Task RunAsync(ISocketAsyncAdapter socket, IWhoisProtocol protocol)
		{
			throw new System.NotImplementedException();
		}
	}
}
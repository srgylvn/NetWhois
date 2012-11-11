using System.Threading.Tasks;
using NetWhois.Components;

namespace NetWhois.Imp
{
	public interface IWhoisRoutine
	{
		Task RunAsync(ISocketAsyncAdapter socket, IWhoisProtocol protocol);
	}
}
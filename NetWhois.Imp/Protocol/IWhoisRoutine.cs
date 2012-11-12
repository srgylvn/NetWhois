using System.Threading.Tasks;

namespace NetWhois.Imp.Protocol
{
	public interface IWhoisRoutine
	{
		Task RunAsync(IWhoisProtocol protocol);
	}
}
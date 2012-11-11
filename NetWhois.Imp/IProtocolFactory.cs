using NetWhois.Components;

namespace NetWhois.Imp
{
	public interface IProtocolFactory
	{
		IWhoisProtocol CreateWhois(ISocketAsyncAdapter boundSocket);
	}
}
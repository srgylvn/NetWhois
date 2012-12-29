using NetWhois.Components;

namespace NetWhois.Imp.Protocol
{
	public class ProtocolFactory : IProtocolFactory
	{
		private IObjectFactory _objectFactory;
		public ProtocolFactory(IObjectFactory objectFactory)
		{
			_objectFactory = objectFactory;
		}

		public IWhoisProtocol CreateWhois(IAsyncSocketAdapter boundSocket)
		{
			return _objectFactory.Instance<IWhoisProtocol>(new {asyncAdapter = boundSocket});
		}
	}
}
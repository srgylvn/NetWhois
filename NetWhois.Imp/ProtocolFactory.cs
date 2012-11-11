using NetWhois.Components;

namespace NetWhois.Imp
{
	public class ProtocolFactory : IProtocolFactory
	{
		private IObjectFactory _objectFactory;
		public ProtocolFactory(IObjectFactory objectFactory)
		{
			_objectFactory = objectFactory;
		}

		public IWhoisProtocol CreateWhois(ISocketAsyncAdapter boundSocket)
		{
			return _objectFactory.Instance<IWhoisProtocol>(new {asyncAdapter = boundSocket});
		}
	}
}
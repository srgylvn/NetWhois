using Castle.Windsor;

namespace NetWhois.Components
{
	public class ObjectFactory : IObjectFactory
	{
		private readonly WindsorContainer _container;
		public ObjectFactory(WindsorContainer container)
		{
			_container = container;
		}

		public T Instance<T>()
		{
			return Container.Resolve<T>();
		}

		public T Instance<T>(object parameters)
		{
			return Container.Resolve<T>(parameters);
		}

		public WindsorContainer Container
		{
			get { return _container; }
		}
	}
}
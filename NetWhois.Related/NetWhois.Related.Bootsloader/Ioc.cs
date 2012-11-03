using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NetWhois.Components;

namespace NetWhois.Related.Bootsloader
{
	public static class Ioc
	{
		public static IObjectFactory Initialize()
		{
			var wcontainer = new WindsorContainer();
			wcontainer.Install(FromAssembly.This());

			wcontainer.Register(
				Component.For<IObjectFactory>().ImplementedBy<ObjectFactory>().DependsOn(new { container = wcontainer })
				);

			return wcontainer.Resolve<IObjectFactory>();
		}
	}
}
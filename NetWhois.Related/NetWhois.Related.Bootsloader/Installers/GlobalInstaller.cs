using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NetWhois.Components;

namespace NetWhois.Related.Bootsloader.Installers
{
	public class GlobalInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component.For<ISocketAsyncAdapter>()
					.ImplementedBy<AsyncSocketAdapter>()
				);
		}
	}
}
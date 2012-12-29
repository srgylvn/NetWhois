using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NetWhois.Components;
using NetWhois.Imp.Protocol;
using NetWhois.Imp.Response;
using NetWhois.Imp.Settings;

namespace NetWhois.Related.Bootsloader.Installers
{
	public class GlobalInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IAsyncSocketAdapter>().ImplementedBy<AsyncSocketAdapter>().LifestyleTransient());
			container.Register(Component.For<ISocketAdapterFactory>().ImplementedBy<SocketAdapterFactory>().LifestyleTransient());
			container.Register(Component.For<IProtocolFactory>().ImplementedBy<ProtocolFactory>().LifestyleTransient());
			container.Register(Component.For<IWhoisProtocol>().ImplementedBy<WhoisProtocol>().LifestyleTransient());
			container.Register(Component.For<IWhoisRoutine>().ImplementedBy<WhoisRoutine>().LifestyleTransient());
			container.Register(Component.For<IWhoisServer>().ImplementedBy<WhoisServer>().LifestyleTransient());
			container.Register(Component.For<IResponseBuilder>().ImplementedBy<ResponseBuilder>().LifestyleTransient());
		    container.Register(Component.For<ISettings>().ImplementedBy<Settings>().LifestyleTransient());
		}
	}
}
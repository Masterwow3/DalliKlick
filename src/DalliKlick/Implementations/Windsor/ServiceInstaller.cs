using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DalliKlick.Implementations.Interfaces;
using DalliKlick.Implementations.Interfaces.Services;
using DalliKlick.Implementations.Services;
using DalliKlick.Implementations.Window;

namespace DalliKlick.Implementations.Windsor
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IGameImageService>().ImplementedBy<GameImageService>().LifestyleSingleton()
            );
        }
    }
}
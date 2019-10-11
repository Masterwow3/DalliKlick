using System.Windows.Controls;
using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DalliKlick.Implementations.Interfaces;
using DalliKlick.Implementations.Window;

namespace DalliKlick.Implementations.Windsor
{
    public class WindowInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            InstallViews(container);
            InstallViewModels(container);

            container.Register(Component.For<IWindowManager>().ImplementedBy<CustomWindowManager>().LifestyleSingleton(),
                Component.For<IMessageManager>().ImplementedBy<MessageManager>().LifestyleSingleton());
        }

        private void InstallViewModels(IWindsorContainer container)
        {
            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<PropertyChangedBase>()
                .LifestyleTransient());
        }
        private void InstallViews(IWindsorContainer container)
        {
            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<UserControl>()
                .LifestyleTransient());

            var defaultLocator = ViewLocator.LocateTypeForModelType;
            ViewLocator.LocateTypeForModelType = (modelType, displayLocation, context) => {
                var viewType = defaultLocator(modelType, displayLocation, context);
                while (viewType == null && modelType != typeof(object))
                {
                    modelType = modelType.BaseType;
                    viewType = defaultLocator(modelType, displayLocation, context);
                }
                return viewType;
            };
        }
    }
}
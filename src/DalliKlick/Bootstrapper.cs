using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using DalliKlick.Implementations.Window;
using DalliKlick.Views.Menu;
using DevExpress.Mvvm.Native;

namespace DalliKlick
{
    public class Bootstrapper : BootstrapperBase
    {
        private WindsorContainer _container;


        public Bootstrapper()
        {
            Initialize();
        }


        #region Methods

        protected override void Configure()
        {
            _container = new WindsorContainer();
            _container.Install(FromAssembly.This());
            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel));
        }


        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] {
                Assembly.GetAssembly(typeof(Bootstrapper))
            };
        }


        /// <summary>
        ///     Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <param name="key">The key to locate.</param>
        protected override object GetInstance(Type service, string key)
        {
            if (_container.Kernel.HasComponent(service) == false)
            {
                return base.GetInstance(service, key);
            }
            return _container.Resolve(service);
        }


        /// <summary>
        ///     Override this to provide an IoC specific implementation
        /// </summary>
        /// <param name="service">The service to locate.</param>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            if (_container.Kernel.HasComponent(service) == false)
            {
                return base.GetAllInstances(service);
            }
            return _container.ResolveAll(service).Cast<object>();
        }


        /// <summary>
        ///     Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            var propertiesToInject = instance
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => f.CanWrite && _container.Kernel.HasComponent(f.PropertyType));
            foreach (var property in propertiesToInject)
            {
                property.SetValue(instance, _container.Resolve(property.PropertyType));
            }
        }


        /// <summary>
        ///     Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            Application.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            ShowMainMenu();
            Application.Shutdown();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>False if the Application has to close, True for login was success</returns>
        private bool? ShowMainMenu()
        {
            var windowManager = _container.Resolve<IWindowManager>();
            var viewModel = _container.Resolve<MenuViewModel>();
            return windowManager.ShowDialog(viewModel, null, WindowSettings.With().FixedSize(800,400));
        }

        #endregion Methods
    }
}
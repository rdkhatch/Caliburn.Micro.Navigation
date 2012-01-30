using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Autofac;
using Caliburn.Micro.Navigation;
using Caliburn.Micro.Navigation.Helpers;

namespace Caliburn.Micro.SimpleNavigation {
    public class AppBootstrapper : Bootstrapper<ShellViewModel>
    {
        private ContainerBuilder builder;
        public static Action<object, DependencyObject, object> OldBind;
        protected override void Configure()
        {
            builder = new ContainerBuilder();
            builder.RegisterType<ScreenFactory>().AsSelf();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(x => typeof(Screen).IsAssignableFrom(x)).AsSelf();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(x => typeof(Screen).IsAssignableFrom(x)).Named<Screen>(x => x.GetNavigationName());
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(x => typeof(FrameworkElement).IsAssignableFrom(x)).AsSelf();
            NavigationConvention.AddNavigationConventions();
            Container = builder.Build();
        }

        public IContainer Container { get; private set; }


        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] { GetType().Assembly, typeof(ShellViewModel).Assembly };
        }

        protected override void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // a ChildWindow control.
            if (System.Diagnostics.Debugger.IsAttached) return;
            // NOTE: This will allow the application to continue running after an exception has been thrown
            // but not handled. 
            // For production applications this error handling should be replaced with something that will 
            // report the error to the website and stop the application.
            e.Handled = true;
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            base.OnExit(sender, e);
            Container.Dispose();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            object instance;
            if (string.IsNullOrWhiteSpace(key))
            {
                if (Container.TryResolve(serviceType, out instance))
                {
                    return instance;
                }
            }
            else
            {
                if (Container.TryResolveNamed(key, serviceType, out instance))
                {
                    return instance;
                }
            }
            throw new Exception(string.Format("Could not locate any instances of contract {0}.", key ?? serviceType.Name));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return Container.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            Container.InjectProperties(instance);
        }
    }
}
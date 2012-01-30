using Autofac;
using Caliburn.Micro.Navigation;

namespace Caliburn.Micro.SimpleNavigation
{
    public class ScreenFactory:IScreenFactory<Screen>
    {
        private readonly IComponentContext container;

        public ScreenFactory(IComponentContext container)
        {
            this.container = container;

        }

        public Screen GetScreen(string navigationName)
        {
           return  container.ResolveNamed<Screen>(navigationName);
        }
    }
}

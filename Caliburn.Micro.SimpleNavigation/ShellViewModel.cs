using Caliburn.Micro.Navigation;

namespace Caliburn.Micro.SimpleNavigation {
    public class ShellViewModel : NavigationConductor {
        public ShellViewModel(ScreenFactory factory):base(factory)
        {

        }

        public void ShowPageOne() {
            ActivateItem<PageOneViewModel>();
        }

        public void ShowPageTwo() {
            ActivateItem<PageTwoViewModel>();
        }
    }
}
namespace Caliburn.Micro.Navigation
{
    public interface IScreenFactory<out T>
    {
        T GetScreen(string navigationName);
    }
}

namespace TPA_Desktop.Core.Interfaces
{
    public interface IDirector<out T>
    {
        void Reset();
        T Build();
    }
}
namespace TPA_Desktop.Core.Interfaces
{
    public interface IMediator
    {
        void Notify(object sender, string @event);
    }
}
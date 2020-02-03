namespace CustomFramework.Http.Contracts
{
    using System.Threading.Tasks;

    public interface IServerEntity
    {
        Task StartAsync();

        void Stop();
    }
}

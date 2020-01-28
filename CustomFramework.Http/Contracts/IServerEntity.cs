using System.Threading.Tasks;

namespace CustomFramework.Http.Contracts
{
    public interface IServerEntity
    {
        Task StartAsync();

        void Stop();
    }
}

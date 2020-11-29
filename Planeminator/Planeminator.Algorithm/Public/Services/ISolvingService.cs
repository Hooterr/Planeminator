using System.Threading.Tasks;

namespace Planeminator.Services
{
    public interface ISolvingService
    {
        Task<bool> StartAsync();
    }
}

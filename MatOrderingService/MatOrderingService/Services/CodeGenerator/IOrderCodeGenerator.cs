using System.Threading.Tasks;

namespace MatOrderingService.Services
{
    public interface IOrderCodeGenerator
    {
        Task<string> Get(int id);
    }
}
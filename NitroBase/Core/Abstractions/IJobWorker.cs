using System.Threading.Tasks;

namespace Core.Abstractions;

public interface IJobWorker
{
    Task Run();
}

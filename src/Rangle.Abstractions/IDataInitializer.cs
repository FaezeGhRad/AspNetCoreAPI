using System.Threading;
using System.Threading.Tasks;

namespace Rangle.Abstractions
{
    public interface IDataInitializer
    {
        public Task Initialize(CancellationToken cancellationToken = default);
    }
}

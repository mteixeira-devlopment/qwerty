using System.Threading.Tasks;

namespace EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task<bool> Handle(dynamic eventData);
    }
}

using Sms.Entity;

namespace Sms.Adapter
{
    public interface ICustomerBuilderFactory
    {
        ICustomerBuilder Create(Guest source, int clientId, int sourceSystemCode);
    }
}
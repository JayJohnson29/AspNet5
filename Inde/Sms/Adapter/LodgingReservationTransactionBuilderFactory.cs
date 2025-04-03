using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using Sms.MC;

namespace Sms.Adapter
{
    public class LodgingReservationTransactionBuilderFactory : ILodgingReservationTransactionBuilderFactory
    {
        private readonly ILogger<LodgingReservationTransactionBuilderFactory> _logger;
        private readonly IServiceProvider _serviceProvider;

        public LodgingReservationTransactionBuilderFactory(ILogger<LodgingReservationTransactionBuilderFactory> logger, IServiceProvider serviceProvider)
        {
            this._logger = logger;
            this._serviceProvider = serviceProvider;
        }

        public LodgingReservationTransactionBuilder Create(AppConfig config, Reservation reservation, List<LodgingReservationTransaction> lodgingReservationTransactions)
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<LodgingReservationTransactionBuilder>>();
            return new LodgingReservationTransactionBuilder(logger, config, reservation,lodgingReservationTransactions);
        }
    }
}

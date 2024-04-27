using System;
using SampleShop.Events;
using SampleShop.Interfaces;

namespace SampleShop.Services
{
    public class AuditService : IAuditService
    {
        private readonly ILogger _logger;

        public AuditService(ILogger log)
        {
            _logger = log;
        }

        /// <summary>
        /// Subscribes to TransactionService's OnTransactionProcessed and writes to log.
        /// </summary>
        public void Subscribe(ITransactionService transactionService)
        {
            transactionService.OnTransactionProcessed += (sender, args) =>
            {
                if (args != null)
                {
                    _logger.WriteToLog($"{args.TransactionType.ToString()} for {args.Amount} processed");
                }
            };
        }

       
     
    
    }
}

using System;
using SampleShop.Events;
using SampleShop.Interfaces;

namespace SampleShop.Services
{
    public class TransactionService : ITransactionService
    {
   
        public event EventHandler<TransactionProcessedEventArgs> OnTransactionProcessed;

        /// <summary>
        /// Processes a deposit and sends an event to every subsciber holding the amount and transactiontype.
        /// amount must be larger than 0.
        /// </summary>
        public void MakeDeposit(decimal amount) {
            
            if (amount <= 0)
            {
                throw new ArgumentException("amount");
            }

            var eventArgs = new TransactionProcessedEventArgs(amount, TransactionType.Deposit);

            OnTransactionProcessed?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Processes a withdrawal and sends an event to every subsciber holding the amount and transactiontype.
        /// amount must be larger than 0.
        /// </summary>
        public void MakeWithdrawal(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("amount");
            }

            var eventArgs = new TransactionProcessedEventArgs(amount, TransactionType.Withdrawal);

            OnTransactionProcessed?.Invoke(this, eventArgs);
        }

        private void ProcessDeposit(decimal amount)
        {
            // Processing logic not necessary for exam
        }

        private void ProcessWithdrawal(decimal amount)
        {
            // Processing logic not necessary for exam
        }
    }
}

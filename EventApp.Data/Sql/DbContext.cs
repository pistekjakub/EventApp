using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace EventApp.Data.Sql
{
    public class DbContext : IDbContext
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<DbContext> _logger;
        private Stack<TransactionScope> _transactionStack = new Stack<TransactionScope>();

        public event OnCommitNotificationDelegate? OnCommitNotification;
        public event OnBeforeCommitNotificationDelegate? OnBeforeCommitNotification;

        private bool _isDisposing;

        public DbContext(ILogger<DbContext> logger, IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<IDbConnection> GetConnection()
        {
            return await _connectionFactory.OpenConnection();
        }

        public TransactionScope StartTransaction(
            TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TransactionScopeAsyncFlowOption transactionScopeAsyncFlowOption = TransactionScopeAsyncFlowOption.Enabled
        )
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = isolationLevel
            };

            var tran = new TransactionScope(transactionScopeOption, transactionOptions, transactionScopeAsyncFlowOption);

            _transactionStack.Push(tran);

            return tran;
        }

        public async Task Commit()
        {
            if (_transactionStack.Count == 0)
            {
                throw new Exception("No transaction to commit.");
            }

            if (_transactionStack.Count == 1 && OnBeforeCommitNotification != null)
            { // execute before commit notification when there is only one transaction in the stack
                try
                {
                    await OnBeforeCommitNotification();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in OnBeforeCommitNotification happened");
                    throw;
                }
            }

            var tran = _transactionStack.Pop();
            tran.Complete();
            SilentDisposeTransaction(tran);

            if (_transactionStack.Count == 0 && OnCommitNotification != null)
            {
                try
                {
                    await OnCommitNotification();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in OnCommitNotification happened");
                }
            }
        }

        public void Rollback()
        {
            while (_transactionStack.Count != 0)
            {
                var tran = _transactionStack.Pop();
                SilentDisposeTransaction(tran);
                _logger.LogDebug($"Disposed transaction, {_transactionStack.Count} left to dispose");
            }
        }

        public void Dispose()
        {
            if (!_isDisposing)
            {
                _isDisposing = true;

                Rollback();
            }

        }

        public bool AnyOngoingTransaction()
        {
            return _transactionStack.Any();
        }

        private void SilentDisposeTransaction(TransactionScope transaction)
        {
            try
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
            }
            catch (TransactionAbortedException)
            {
                _logger.LogDebug("Caught TransactionAbortedException, most likely happened because an internal transaction got disposed, we can safely eat this up.");
            }
        }
    }
}

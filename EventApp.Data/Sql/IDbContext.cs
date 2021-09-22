using System;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;

using IsolationLevel = System.Transactions.IsolationLevel;

namespace EventApp.Data.Sql
{
    public interface IDbContext : IDisposable
    {
        Task<IDbConnection> GetConnection();
        TransactionScope StartTransaction(
            TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TransactionScopeAsyncFlowOption transactionScopeAsyncFlowOption = TransactionScopeAsyncFlowOption.Enabled
        );
        Task Commit();
        void Rollback();
        event OnCommitNotificationDelegate OnCommitNotification;

        event OnBeforeCommitNotificationDelegate OnBeforeCommitNotification;

        bool AnyOngoingTransaction();
    }
}

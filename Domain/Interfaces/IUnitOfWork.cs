namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDeveloperRepository Developers { get; }
        IProjectRepository Projects { get; }
        IUserRepository Users { get; }
        IAccountRepository Accounts { get; }
        int Commit();
    }
}

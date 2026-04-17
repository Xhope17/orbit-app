using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.DataBase.SqlServer;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer
{
    public class UnitOfWork(XcloneContext context, IUserRepository usersRepository, IEmailTemplateRepository emailTemplateRepository) : IUnitOfWork
    {
        private readonly XcloneContext _context = context;
        public IUserRepository userRepository { get; set; } = usersRepository;
        public IEmailTemplateRepository emailTemplateRepository { get; set; } = emailTemplateRepository;

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

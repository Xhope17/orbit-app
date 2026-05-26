using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.DataBase.SqlServer;
using XClone.Domain.Interfaces.Repositories;

namespace XClone.Infrastructure.Persistence.SqlServer
{
    public class UnitOfWork(
        XcloneContext context,
        IUserRepository usersRepository,
        IEmailTemplateRepository emailTemplateRepository,
        IRoleRepository roleRepository,
        IPostRepository postRepository,
        IChatRepository chatRepository,
        IMessageRepository messageRepository,
        IFollowingRepository followingRepository,
        IBlockRepository blockRepository,
        ICommunityRepository communityRepository,
        ICommunityMemberRepository communityMemberRepository,
        ILikeRepository likeRepository,
        IReplyRepository replyRepository,
        IRepostRepository repostRepository,
        IQuoteRepository quoteRepository,
        IHashtagRepository hashtagRepository,
        IPostHashtagRepository postHashtagRepository,
        IMediaFileRepository mediaFileRepository
    ) : IUnitOfWork
    {
        private readonly XcloneContext _context = context;
        public IUserRepository userRepository { get; set; } = usersRepository;
        public IEmailTemplateRepository emailTemplateRepository { get; set; } = emailTemplateRepository;
        public IRoleRepository roleRepository { get; set; } = roleRepository;
        public IPostRepository postRepository { get; set; } = postRepository;
        public IChatRepository chatRepository { get; set; } = chatRepository;
        public IMessageRepository messageRepository { get; set; } = messageRepository;
        public IFollowingRepository followingRepository { get; set; } = followingRepository;
        public IBlockRepository blockRepository { get; set; } = blockRepository;
        public ICommunityRepository communityRepository { get; set; } = communityRepository;
        public ICommunityMemberRepository communityMemberRepository { get; set; } = communityMemberRepository;
        public ILikeRepository likeRepository { get; set; } = likeRepository;
        public IReplyRepository replyRepository { get; set; } = replyRepository;
        public IRepostRepository repostRepository { get; set; } = repostRepository;
        public IQuoteRepository quoteRepository { get; set; } = quoteRepository;
        public IHashtagRepository hashtagRepository { get; set; } = hashtagRepository;
        public IPostHashtagRepository postHashtagRepository { get; set; } = postHashtagRepository;
        public IMediaFileRepository mediaFileRepository { get; set; } = mediaFileRepository;
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

using XClone.Domain.Interfaces.Repositories;

namespace XClone.Domain.DataBase.SqlServer
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; set; }
        IEmailTemplateRepository emailTemplateRepository { get; set; }
        IRoleRepository roleRepository { get; set; }
        IPostRepository postRepository { get; set; }
        IChatRepository chatRepository { get; set; }
        IMessageRepository messageRepository { get; set; }
        IFollowingRepository followingRepository { get; set; }
        IBlockRepository blockRepository { get; set; }
        ICommunityRepository communityRepository { get; set; }
        ICommunityMemberRepository communityMemberRepository { get; set; }
        ILikeRepository likeRepository { get; set; }
        IReplyRepository replyRepository { get; set; }
        IRepostRepository repostRepository { get; set; }
        IQuoteRepository quoteRepository { get; set; }
        IHashtagRepository hashtagRepository { get; set; }
        IPostHashtagRepository postHashtagRepository { get; set; }
        IMediaFileRepository mediaFileRepository { get; set; }
        Task SaveChangesAsync();
    }
}

using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using GreyAnatomyFanSite.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GreyAnatomyFanSite.Application.Comments.Commands.AddArticleComment
{
    public sealed record AddArticleCommentCommand(
        int ArticleId,
        string Title,
        string Text) : IRequest<IdentityOperationResult>;

    public sealed class AddArticleCommentCommandHandler : IRequestHandler<AddArticleCommentCommand, IdentityOperationResult>
    {
        private readonly IApplicationDbContext dbContext;
        private readonly ICurrentUser currentUser;

        public AddArticleCommentCommandHandler(IApplicationDbContext dbContext, ICurrentUser currentUser)
        {
            this.dbContext = dbContext;
            this.currentUser = currentUser;
        }

        public async Task<IdentityOperationResult> Handle(AddArticleCommentCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            {
                return IdentityOperationResult.Failure("Vous devez être connecté pour poster un commentaire.");
            }

            if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Text))
            {
                return IdentityOperationResult.Failure("Le titre et le texte du commentaire sont obligatoires.");
            }

            bool articleExists = await dbContext.Articles
                .AsNoTracking()
                .AnyAsync(article => article.Id == request.ArticleId, cancellationToken);

            if (!articleExists)
            {
                return IdentityOperationResult.Failure("L'article demandé est introuvable.");
            }

            ArticleComment comment = new ArticleComment
            {
                ArticleId = request.ArticleId,
                AuthorMemberProfileId = currentUser.UserId.Value,
                Title = request.Title.Trim(),
                Content = request.Text.Trim(),
                CreatedAtUtc = DateTime.UtcNow
            };

            await dbContext.ArticleComments.AddAsync(comment, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return IdentityOperationResult.Success();
        }
    }
}

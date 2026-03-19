using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using GreyAnatomyFanSite.Application.Comments.Commands.AddArticleComment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers
{
    public sealed class CommentairesController : AppControllerBase
    {
        private readonly ISender sender;

        public CommentairesController(
            IIdentityService identityService,
            ISender sender)
            : base(identityService)
        {
            this.sender = sender;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(
            string titre,
            string text,
            string typePubli,
            int idPubli,
            CancellationToken cancellationToken = default)
        {
            if (!string.Equals(typePubli, "article", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Home");
            }

            IdentityOperationResult result = await sender.Send(
                new AddArticleCommentCommand(idPubli, titre, text),
                cancellationToken);

            if (!result.Succeeded)
            {
                TempData["CommentErrors"] = string.Join("||", result.Errors);
            }

            return RedirectToAction("ViewArticle", "Home", new { id = idPubli });
        }
    }
}

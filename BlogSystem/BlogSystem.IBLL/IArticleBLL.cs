using BlogSystem.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    public interface IArticleBLL
    {
        bool CreateArticle(string title, string content, Guid[] categoryIds, Guid userId);
        bool CreateCategory(string name, Guid userId);
        List<CategoryDTO> GetAllCategories(Guid userId);
        List<ArticleDTO> GetAllArticlesByUserId(Guid userId, int pageIndex, int pageSize, out int totalCount);
        List<ArticleDTO> GetAllArticlesByEmail(string email);
        List<ArticleDTO> GetAllArticlesByCategoryId(Guid categoryId);
        bool RemoveCategory(string name);
        bool EditCategory(Guid categoryId, string newCategoryName);
        bool RemoveArticle(Guid articleId);
        bool EditArticle(Guid articleId, string title, string content, Guid[] categoryIds);
        bool ExistArticle(Guid articleId);
        ArticleDTO GetOneArticleById(Guid articleId);
        void Like(Guid articleId);
        void UnLike(Guid articleId);
        bool CreateComment(Guid userId, Guid articleId, string Content);
        List<CommentDTO> GetCommentsByArticleId(Guid articleId);
    }
}

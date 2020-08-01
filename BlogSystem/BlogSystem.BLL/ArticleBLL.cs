using BlogSystem.DAL;
using BlogSystem.DTO;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class ArticleBLL : IArticleBLL
    {
        private IUserDAL _userDAL = new UserDAL();
        private IArticleDAL _articleDAL = new ArticleDAL();
        private ICategoryDAL _categoryDAL = new CategoryDAL();
        private IArticleToCategoryDAL _articleToCategoryDAL = new ArticleToCategoryDAL();
        private ICommentDAl _commentDAl = new CommentDAL();

        public bool CreateArticle(string title, string content, Guid[] categoryIds, Guid userId)
        {
            Article article = new Article()
            {
                ArticleId = Guid.NewGuid(),
                ArticleCreateDateTime = DateTime.Now,
                ArticleIsRemoved = 0,
                ArticleTitle = title,
                ArticleContent = content,
                ArticleUserId = userId,
                ArticleLikeCount = 0,
                ArticleUnlikeCount = 0
            };

            // create an artile
            bool articleCreated = _articleDAL.CreateEntity(article);

            // add relationship between the article and categories
            bool articleToCategoryCreated = false;
            foreach(Guid categoryId in categoryIds)
            {
                articleToCategoryCreated = _articleToCategoryDAL.CreateEntity(new ArticleToCategory()
                {
                    ArticleToCategoryId = Guid.NewGuid(),
                    ArticleToCategoryCreateDateTime = DateTime.Now,
                    ArticleToCategoryIsRemoved = 0,
                    ArticleToCategoryCategoryId = categoryId,
                    ArticleToCategoryArticleId = article.ArticleId
                });

                if(!articleToCategoryCreated)
                {
                    articleToCategoryCreated = false;
                    break;
                }
            }
            if(articleCreated && articleToCategoryCreated)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CreateCategory(string name, Guid userId)
        {
            return _categoryDAL.CreateEntity(new Category()
            {
                CategoryId = Guid.NewGuid(),
                CategoryCreateDateTime = DateTime.Now,
                CategoryIsRemoved = 0,
                CategoryName = name,
                CategoryUserId = userId

            });
        }

        public bool CreateComment(Guid userId, Guid articleId, string Content)
        {
            _commentDAl.CreateEntity(new Comment
            {
                CommentId = Guid.NewGuid(),
                CommentCreateDateTime = DateTime.Now,
                CommentIsRemoved = 0,
                CommentContent = Content,
                CommentUserId = userId,
                CommentArticleId = articleId
            });

            return true;
        }

        public List<CommentDTO> GetCommentsByArticleId(Guid articleId)
        {
            List<CommentDTO> commentDTOs = new List<CommentDTO>();
            foreach(Comment comment in _commentDAl.GetEntities(m => m.CommentArticleId == articleId))
            {
                User user = _userDAL.GetUserById(comment.CommentUserId);
                commentDTOs.Add(new CommentDTO
                {
                    Id = comment.CommentId,
                    UserId = comment.CommentUserId,
                    ArticleId = comment.CommentArticleId,
                    Content = comment.CommentContent,
                    CreateDateTime = comment.CommentCreateDateTime,
                    Email = user.UserEmail
                });
            }
            return commentDTOs;
        }

        public bool EditArticle(Guid articleId, string title, string content, Guid[] categoryIds)
        {
            Article article = _articleDAL.GetArticleById(articleId);

            article.ArticleTitle = title;
            article.ArticleContent = content;
            _articleDAL.EditEntity(article);

            //IQueryable<ArticleToCategory> articleCategories = _articleToCategoryDAL.GetEntities(m => m.ArticleToCategoryArticleId == articleId).ToList();
             var articleCategories = _articleToCategoryDAL.GetEntities(m => m.ArticleToCategoryArticleId == articleId).ToList();
            foreach (ArticleToCategory articleToCategory in articleCategories)
            {
                _articleToCategoryDAL.DeleteEntity(articleToCategory);
            }
            // add new categories
            foreach (Guid categoryId in categoryIds)
            {
                _articleToCategoryDAL.CreateEntity(new ArticleToCategory()
                {
                    ArticleToCategoryId = Guid.NewGuid(),
                    ArticleToCategoryCreateDateTime = DateTime.Now,
                    ArticleToCategoryIsRemoved = 0,
                    ArticleToCategoryCategoryId = categoryId,
                    ArticleToCategoryArticleId = article.ArticleId
                });
            }

            return true;
        }

        public bool EditCategory(Guid categoryId, string newCategoryName)
        {
            Category category = _categoryDAL.GetCategoryById(categoryId);
            if(category != null)
            {
                category.CategoryName = newCategoryName;
                _categoryDAL.EditEntity(category);
            }
            return true;
        }

        public bool ExistArticle(Guid articleId)
        {
            var article = _articleDAL.GetArticleById(articleId);
            if(article == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<ArticleDTO> GetAllArticlesByCategoryId(Guid categoryId)
        {
            IQueryable<ArticleToCategory> articleToCategories = _articleToCategoryDAL.GetEntities(m => m.ArticleToCategoryCategoryId == categoryId);
            List<ArticleDTO> articleDTOs = new List<ArticleDTO>();
            foreach(ArticleToCategory articleToCategory in articleToCategories)
            {
                Article article = _articleDAL.GetArticleById(articleToCategory.ArticleToCategoryArticleId);
                User user = _userDAL.GetUserById(article.ArticleUserId);
                Category category = _categoryDAL.GetCategoryById(categoryId);
                
                
                articleDTOs.Add(new ArticleDTO()
                {
                    Title = article.ArticleTitle,
                    Content = article.ArticleContent,
                    CreateDateTime = article.ArticleCreateDateTime,
                    UserEmail = user.UserEmail,
                    LikeCount = article.ArticleLikeCount,
                    UnlikeCount = article.ArticleUnlikeCount,
                    ImagePath = user.UserImagePath,
                    CategoryNames = new string[] { category.CategoryName},
                    CategoryIds = new string[] { categoryId.ToString() }
                });
            }
            return articleDTOs;
        }

        public List<ArticleDTO> GetAllArticlesByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public List<ArticleDTO> GetAllArticlesByUserId(Guid userId, int pageIndex, int pageSize, out int totalCount)
        {
            // IQueryable<Article> articles = _articleDAL.GetEntities(m => m.ArticleUserId == userId);
            IQueryable<Article> articles = _articleDAL.GetPageEntitiesOrdered(pageIndex, pageSize, out totalCount, m => m.ArticleUserId == userId, m => m.ArticleCreateDateTime, false);
            List<ArticleDTO> articleDTOs = new List<ArticleDTO>();
            foreach (Article article in articles)
            {
                var categories = _articleToCategoryDAL.GetCategoriesByArticleId(article.ArticleId).ToList();
                User user = _userDAL.GetUserById(article.ArticleUserId);
                List<string> cateNames = new List<string>();
                foreach(ArticleToCategory articleToCategory in categories)
                {
                    cateNames.Add(_categoryDAL.GetCategoryById(articleToCategory.ArticleToCategoryCategoryId).CategoryName);
                }
                
                articleDTOs.Add(new ArticleDTO()
                {
                    Id = article.ArticleId,
                    Title = article.ArticleTitle,
                    Content = article.ArticleContent,
                    CreateDateTime = article.ArticleCreateDateTime,
                    UserEmail = user.UserEmail,
                    LikeCount = article.ArticleLikeCount,
                    UnlikeCount = article.ArticleUnlikeCount,
                    ImagePath = user.UserImagePath,
                    CategoryIds = categories.Select(m => m.ArticleToCategoryCategoryId.ToString()).ToArray(),
                    CategoryNames = cateNames.ToArray()
                });
            }
            return articleDTOs;
        }

        public List<CategoryDTO> GetAllCategories(Guid userId)
        {
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            IQueryable<Category> categories = _categoryDAL.GetEntities(m => m.CategoryUserId == userId);
            foreach(Category category in categories)
            {
                categoryDTOs.Add(new CategoryDTO() 
                { 
                    Id = category.CategoryId,
                    CategoryName = category.CategoryName
                });
            }
            return categoryDTOs;
        }

        public ArticleDTO GetOneArticleById(Guid articleId)
        {
            Article article = _articleDAL.GetArticleById(articleId);
            IQueryable<ArticleToCategory> articleToCategories = _articleToCategoryDAL.GetCategoriesByArticleId(articleId);
            User user = _userDAL.GetUserById(article.ArticleUserId);
            List<String> categoryIds = new List<String>();
            List<string> categoryNames = new List<string>();
            foreach(ArticleToCategory articleToCategory in articleToCategories)
            {
                categoryIds.Add(articleToCategory.ArticleToCategoryCategoryId.ToString());
                categoryNames.Add(_categoryDAL.GetCategoryById(articleToCategory.ArticleToCategoryCategoryId).CategoryName);
            }

            return new ArticleDTO()
            {
                Id = article.ArticleId,
                Title = article.ArticleTitle,
                Content = article.ArticleContent,
                CreateDateTime = article.ArticleCreateDateTime,
                UserEmail = user.UserEmail,
                LikeCount = article.ArticleLikeCount,
                UnlikeCount = article.ArticleUnlikeCount,
                ImagePath = user.UserImagePath,
                CategoryIds = categoryIds.ToArray(),
                CategoryNames = categoryNames.ToArray()

            };
        }

        public void Like(Guid articleId)
        {
            Article article = _articleDAL.GetArticleById(articleId);
            article.ArticleLikeCount++;
            _articleDAL.EditEntity(article);

        }

        public bool RemoveArticle(Guid articleId)
        {
            return _articleDAL.DeleteEntity(_articleDAL.GetArticleById(articleId));
        }

        public bool RemoveCategory(string name)
        {
            return _categoryDAL.DeleteEntity(_categoryDAL.GetCategoryByName(name));
        }

        public void UnLike(Guid articleId)
        {
            Article article = _articleDAL.GetArticleById(articleId);
            article.ArticleUnlikeCount++;
            _articleDAL.EditEntity(article);
        }
    }
}

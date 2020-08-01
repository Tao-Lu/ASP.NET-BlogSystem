using BlogSystem.BLL;
using BlogSystem.DTO;
using BlogSystem.IBLL;
using BlogSystem.WebUI.Filters;
using BlogSystem.WebUI.Models.ArticleViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace BlogSystem.WebUI.Controllers
{
    [BlogSystemAuth]
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(CreateCategoryViewModel createCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                IArticleBLL articleBLL = new ArticleBLL();
                articleBLL.CreateCategory(createCategoryViewModel.CategoryName, Guid.Parse(Session["loginUserId"].ToString()));
                // redirect to category list page
                return RedirectToAction("CategoryList");
            }
            ModelState.AddModelError("", "invalid entry");
            return View(createCategoryViewModel);
        }

        [HttpGet]
        public ActionResult CategoryList()
        {
            IArticleBLL articleBLL = new ArticleBLL();
            return View(articleBLL.GetAllCategories(Guid.Parse(Session["loginUserId"].ToString())));
        }

        [HttpGet]
        public ActionResult CreateArticle()
        {
            var userId = Guid.Parse(Session["loginUserId"].ToString());
            ViewBag.CategoryIds = new ArticleBLL().GetAllCategories(userId);
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateArticle(CreateArticleViewModel createArticleViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = Guid.Parse(Session["loginUserId"].ToString());
                new ArticleBLL().CreateArticle(createArticleViewModel.Titile, createArticleViewModel.Content, createArticleViewModel.CategoryIds, userId);
                return RedirectToAction("ArticleList");
            }
            ModelState.AddModelError("", "failed");
            return View(createArticleViewModel);
        }

        [HttpGet]
        public ActionResult ArticleList(int pageIndex = 1)
        {
            // paging
            // total pages, current page, page showing
            const int showingPages = 7;
            const int pageSize = 3;
            int totalCount;
            var userId = Guid.Parse(Session["loginUserId"].ToString());
            var articles = new ArticleBLL().GetAllArticlesByUserId(userId, pageIndex, pageSize, out totalCount);
            ViewBag.totalPages = totalCount % pageSize == 0 ? totalCount/pageSize : totalCount/pageSize+1;
            ViewBag.currentPageIndex = pageIndex;
            ViewBag.showingPages = showingPages;
            return View(articles);
        }

        // using third party pager
        [HttpGet]
        public ActionResult ArticleList2(int pageIndex = 1)
        {
            // paging
            // total pages, current page, page showing
            const int showingPages = 7;
            const int pageSize = 3;
            int totalCount;
            var userId = Guid.Parse(Session["loginUserId"].ToString());
            var articles = new ArticleBLL().GetAllArticlesByUserId(userId, pageIndex, pageSize, out totalCount);
            ViewBag.totalPages = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;
            ViewBag.currentPageIndex = pageIndex;
            ViewBag.showingPages = showingPages;
            return View(new PagedList<ArticleDTO>(articles, pageIndex, pageSize, totalCount));
        }

        [HttpGet]
        public ActionResult ArticleDetails(Guid? id)
        {
            IArticleBLL articleBLL = new ArticleBLL();
            if(id == null || !articleBLL.ExistArticle(id.Value))
            {
                return RedirectToAction(nameof(ArticleList));
            }

            ViewBag.Comments = articleBLL.GetCommentsByArticleId(id.Value);

            return View(articleBLL.GetOneArticleById(id.Value));

            
        }

        [HttpGet]
        public ActionResult EditArticle(Guid id)
        {
            IArticleBLL articleBLL = new ArticleBLL();
            var article = articleBLL.GetOneArticleById(id);
            EditArticleViewModel editArticleViewModel = new EditArticleViewModel
            {
                Title = article.Title,
                Content = article.Content,
                CategoryIds = article.CategoryIds,
                ArticleId = article.Id
            };

            // all user's categories
            var userId = Guid.Parse(Session["loginUserId"].ToString());
            ViewBag.CategoryIds = new ArticleBLL().GetAllCategories(userId);

            return View(editArticleViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditArticle(EditArticleViewModel editArticleViewModel)
        {
            if (ModelState.IsValid)
            {
                IArticleBLL articleBLL = new ArticleBLL();

                List<Guid> categoryIds = new List<Guid>();
                foreach(string categoryId in editArticleViewModel.CategoryIds)
                {
                    categoryIds.Add(Guid.Parse(categoryId));
                }

                articleBLL.EditArticle(editArticleViewModel.ArticleId, editArticleViewModel.Title, editArticleViewModel.Content, categoryIds.ToArray());
                return RedirectToAction("ArticleList2");
            }
            else
            {
                var userId = Guid.Parse(Session["loginUserId"].ToString());
                ViewBag.CategoryIds = new ArticleBLL().GetAllCategories(userId);
                return View(editArticleViewModel);
            }
            
            
        }

        [HttpGet]
        public  ActionResult Like(Guid articleId)
        {
            IArticleBLL articleBLL = new ArticleBLL();
            articleBLL.Like(articleId);
            return RedirectToAction("ArticleDetails", new { id = articleId });
            //return Json(new { result = "OK" });
        }

        [HttpGet]
        public ActionResult UnLike(Guid articleId)
        {
            IArticleBLL articleBLL = new ArticleBLL();
            articleBLL.UnLike(articleId);
            return RedirectToAction("ArticleDetails", new { id = articleId });
        }

        //[HttpGet]
        //[ValidateInput(false)]
        //public ActionResult NewComment(Guid articleId, string content)
        //{
        //    var userId = Guid.Parse(Session["loginUserId"].ToString());
        //    IArticleBLL articleBLL = new ArticleBLL();
        //    articleBLL.CreateComment(userId, articleId, content);
        //    return RedirectToAction("ArticleDetails", new { id = articleId });
        //}

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewComment(CreateCommentViewModel createCommentViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = Guid.Parse(Session["loginUserId"].ToString());
                IArticleBLL articleBLL = new ArticleBLL();
                articleBLL.CreateComment(userId, createCommentViewModel.ArticleId, createCommentViewModel.Content);
                return RedirectToAction("ArticleDetails", new { id = createCommentViewModel.ArticleId });
            }
            ModelState.AddModelError("", "invalid entry");
            return RedirectToAction("ArticleDetails", new { id = createCommentViewModel.ArticleId });


        }
    }
}
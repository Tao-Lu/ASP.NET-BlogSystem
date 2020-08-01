using BlogSystem.BLL;
using BlogSystem.DTO;
using BlogSystem.IBLL;
using BlogSystem.WebUI.Filters;
using BlogSystem.WebUI.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [BlogSystemAuth]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                IUserBLL userBLL = new UserBLL();
                if(userBLL.Register(registerViewModel.Email, registerViewModel.Password))
                {
                    Guid userId;
                    userBLL.Login(registerViewModel.Email, registerViewModel.Password, out userId);

                    // create default category and article in the account

                    IArticleBLL articleBLL = new ArticleBLL();
                    articleBLL.CreateCategory("default", userId);

                    List<Guid> categoryIds = new List<Guid>();
                    foreach( CategoryDTO categoryDTO in articleBLL.GetAllCategories(userId))
                    {
                        categoryIds.Add(categoryDTO.Id);
                    }
                    articleBLL.CreateArticle("default title", "default content", categoryIds.ToArray(), userId);


                    return RedirectToAction(nameof(Login));
                }
                return View(registerViewModel);
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                IUserBLL userBLL = new UserBLL();
                Guid userId;
                if(userBLL.Login(loginViewModel.Email, loginViewModel.Password, out userId))
                {
                    // remember me?
                    // yes, using cookie
                    if (loginViewModel.RememberMe)
                    {
                        Response.Cookies.Add(new HttpCookie("loginEmail"){
                            Value = loginViewModel.Email,
                            Expires = DateTime.Now.AddDays(7)
                        });

                        Response.Cookies.Add(new HttpCookie("loginUserId")
                        {
                            Value = userId.ToString(),
                            Expires = DateTime.Now.AddDays(7)
                        });
                    }
                    // no, using session
                    else
                    {
                        Session["loginEmail"] = loginViewModel.Email;
                        Session["loginUserId"] = userId;
                    }

                    // redirect to homepage
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "wrong Email or Password");
                }
            }
            return View(loginViewModel);
        }
    }
}
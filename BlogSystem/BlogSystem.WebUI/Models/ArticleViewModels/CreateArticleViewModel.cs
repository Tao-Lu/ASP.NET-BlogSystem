using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogSystem.WebUI.Models.ArticleViewModels
{
    public class CreateArticleViewModel
    {
        [Required]
        public string Titile { get; set; }
        [Required]
        public string Content { get; set; }
        [Display(Name = "Categories")]
        public Guid[] CategoryIds { get; set; }
    }
}
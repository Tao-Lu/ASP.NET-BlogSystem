using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogSystem.WebUI.Models.ArticleViewModels
{
    public class EditArticleViewModel
    {
        public Guid ArticleId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string[] CategoryIds { get; set; }
    }
}
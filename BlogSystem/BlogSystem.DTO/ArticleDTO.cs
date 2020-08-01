﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DTO
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string UserEmail { get; set; }
        public int LikeCount { get; set; }
        public int UnlikeCount { get; set; }
        public string ImagePath { get; set; }
        public string[] CategoryNames { get; set; }
        public string[] CategoryIds { get; set; }
    }
}

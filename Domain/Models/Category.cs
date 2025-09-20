﻿using APICoursePlatform.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Category:BaseEntity
    {

        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(250)]
        public string ImageUrl { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}

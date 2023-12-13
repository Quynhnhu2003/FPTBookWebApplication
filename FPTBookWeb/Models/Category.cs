using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FPTBookWeb.Models;


public partial class Category
{

    public int CategoryId { get; set; }

    [Display(Name = "Category Name")]
    public string CategoryName { get; set; } = null!;
    [Display(Name = "Category Details")]
    public string? CatDetails { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}


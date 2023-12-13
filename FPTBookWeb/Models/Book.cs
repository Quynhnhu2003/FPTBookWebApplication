
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FPTBookWeb.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string BookTitle { get; set; } = null!;

    public int BookPrice { get; set; }

    public string? BookDescription { get; set; }

    public string BookImage1 { get; set; } = null!;

    public string? BookImage2 { get; set; }

    public string? BookImage3 { get; set; }

    public int? PublishedYear { get; set; }

    public int Quantity { get; set; }

    public int? BookPages { get; set; }

    public int PublisherId { get; set; }

    public int CategoryId { get; set; }

    public int AuthorId { get; set; }

    public string UserId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Publisher Publisher { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

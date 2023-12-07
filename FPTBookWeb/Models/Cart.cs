using System;
using System.Collections.Generic;

namespace FPTBookWeb.Models;

public partial class Cart
{
    public string CartId { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}

using System;
using System.Collections.Generic;

namespace FPTBookWeb.Models;

public partial class CartItem
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int BookId { get; set; }

    public string CartId { get; set; } = null!;
    /*public string CustomerId { get; set; }*/
    public virtual Book Book { get; set; } = null!;
   /* public virtual User Customer { get; set; } = null!;*/

    /*public virtual Cart Cart { get; set; } = null!;*/
}

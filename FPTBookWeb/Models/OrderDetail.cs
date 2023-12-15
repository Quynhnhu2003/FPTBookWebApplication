using FPTBookWeb.Models;
using System;
using System.Collections.Generic;

namespace FPTBookWeb.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? BookId { get; set; }

    public int Quantity { get; set; }
    
    public string UserId { get; set; }

    /*    public decimal Price { get; set; }*/
    public virtual Book? Book { get; set; }

    public virtual Order? Order { get; set; }
    public virtual User User { get; set; }
}

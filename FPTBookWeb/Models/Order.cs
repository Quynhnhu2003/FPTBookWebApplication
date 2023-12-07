using System;
using System.Collections.Generic;

namespace FPTBookWeb.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int OrderTotal { get; set; }

    public DateTime OrderDate { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

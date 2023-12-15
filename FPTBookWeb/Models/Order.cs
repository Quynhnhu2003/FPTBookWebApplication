﻿
using System;
using System.Collections.Generic;

namespace FPTBookWeb.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual User? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
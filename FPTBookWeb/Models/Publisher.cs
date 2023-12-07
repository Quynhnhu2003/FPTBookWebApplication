using System;
using System.Collections.Generic;

namespace FPTBookWeb.Models;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string PublisherName { get; set; } = null!;

    public string PublisherEmail { get; set; } = null!;

    public string? PublisherPhone { get; set; }

    public string? PublisherAddress { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}

using System;
using System.Collections.Generic;

namespace FPTBookWeb.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserEmail { get; set; } = null!;

    public string UserPass { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserPhone { get; set; } = null!;

    public string UserAddress { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual Role Role { get; set; } = null!;
}

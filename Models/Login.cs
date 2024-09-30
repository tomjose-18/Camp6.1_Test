using System;
using System.Collections.Generic;

namespace Assetmanagementsystem.Models;

public partial class Login
{
    public int LoginId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Usertype { get; set; } = null!;

   /* public virtual ICollection<UserRegistration> UserRegistrations { get; set; } = new List<UserRegistration>();*/
}

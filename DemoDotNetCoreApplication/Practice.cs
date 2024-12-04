using System;
using System.Collections.Generic;

namespace DemoDotNetCoreApplication;

public partial class Practice
{
    public int Id { get; set; }

    public string? Practice1 { get; set; }

    public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();
}

using System;
using System.Collections.Generic;

namespace DemoDotNetCoreApplication;

public partial class Group
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();
}

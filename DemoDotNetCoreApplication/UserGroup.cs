using System;
using System.Collections.Generic;

namespace DemoDotNetCoreApplication;

public partial class UserGroup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateOnly? ModifiedOnDt { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();
}

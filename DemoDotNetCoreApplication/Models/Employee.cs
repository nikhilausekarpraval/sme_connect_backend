using System;
using System.Collections.Generic;

namespace DemoDotNetCoreApplication.Models;

public partial class Employee
{
    public int Id { get; set; }

    public DateOnly? CreatedOnDt { get; set; }

    public string? CreatedBy { get; set; }

    public string? Designation { get; set; }

    public string? Email { get; set; }

    public string? MobileNo { get; set; }

    public string? Name { get; set; }

    public string? Position { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}

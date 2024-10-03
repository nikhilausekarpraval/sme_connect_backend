using System;
using System.Collections.Generic;

namespace DemoDotNetCoreApplication.Modals;

public partial class Task
{
    public int Id { get; set; }

    public DateOnly? AssignedOnDt { get; set; }

    public DateOnly? CreatedOnDt { get; set; }

    public int? EmployeeId { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? CreatedBy { get; set; }

    public string? Description { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public virtual Employee? Employee { get; set; }
}

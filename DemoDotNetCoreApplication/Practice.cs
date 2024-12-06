using System;
using System.Collections.Generic;

namespace DemoDotNetCoreApplication;

public partial class Practice
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateOnly? ModifiedOnDt { get; set; }

    public string? ModifiedBy { get; set; }

}

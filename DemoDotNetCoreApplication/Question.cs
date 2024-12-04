using System;
using System.Collections.Generic;

namespace DemoDotNetCoreApplication;

public partial class Question
{
    public int Id { get; set; }

    public string Question1 { get; set; } = null!;

    public byte[] AnswerHash { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}

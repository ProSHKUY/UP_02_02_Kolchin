using System;
using System.Collections.Generic;

namespace Kolchin_up_02._02.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int RoleId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual EmployeeRole Role { get; set; } = null!;
}

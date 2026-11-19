using System;
using System.Collections.Generic;

namespace Kolchin_up_02._02.Models;

public partial class EmployeeRole
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

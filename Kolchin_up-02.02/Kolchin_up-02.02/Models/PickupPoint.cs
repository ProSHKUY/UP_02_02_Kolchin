using System;
using System.Collections.Generic;

namespace Kolchin_up_02._02.Models;

public partial class PickupPoint
{
    public int PickupPointId { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

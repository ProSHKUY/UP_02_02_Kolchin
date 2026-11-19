using System;
using System.Collections.Generic;

namespace Kolchin_up_02._02.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public int PickupPointId { get; set; }

    public string? ClientFullName { get; set; }

    public int PickupCode { get; set; }

    public int StatusId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual PickupPoint PickupPoint { get; set; } = null!;

    public virtual OrderStatus Status { get; set; } = null!;
}

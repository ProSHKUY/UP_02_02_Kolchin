using System;
using System.Collections.Generic;

namespace Kolchin_up_02._02.Models;

public partial class Manufacturer
{
    public int ManufacturerId { get; set; }

    public string ManufacturerName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

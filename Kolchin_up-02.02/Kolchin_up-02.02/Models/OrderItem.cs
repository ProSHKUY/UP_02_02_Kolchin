using System;
using System.Collections.Generic;

namespace Kolchin_up_02._02.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public string ProductArticle { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product ProductArticleNavigation { get; set; } = null!;
}

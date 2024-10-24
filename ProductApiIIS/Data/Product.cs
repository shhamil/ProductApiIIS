using System;
using System.Collections.Generic;

namespace ProductApiIIS.Data;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ProductVersion> ProductVersions { get; set; } = new List<ProductVersion>();
}

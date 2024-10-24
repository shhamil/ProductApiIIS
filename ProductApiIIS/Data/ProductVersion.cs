using System;
using System.Collections.Generic;

namespace ProductApiIIS.Data;

public partial class ProductVersion
{
    public Guid Id { get; set; }

    public Guid? ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatingDate { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public double Length { get; set; }

    public virtual Product? Product { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Web.Models;

public sealed class CustomerDto
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}

public sealed class CustomerUpsertRequest
{
    [Required]
    public string CustomerName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}

public sealed class CustomerFormModel
{
    public int? CustomerId { get; set; }

    [Required]
    public string CustomerName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}

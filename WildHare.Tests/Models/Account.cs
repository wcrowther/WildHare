using System.ComponentModel.DataAnnotations;

namespace WildHare.Tests.Models;

public class Account
{
    public int AccountId { get; set; } 

    [Required, MaxLength(50)]
    public string AccountName { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required,Phone] 
    public string PhoneNumber { get; set; } = "";

    [Required, MaxLength(50)]
    public string StreetAddress { get; set; } = "";

    [Required, MaxLength(50)]
    public string City { get; set; } = "";

    [Required, MaxLength(2)]
    public string State { get; set; } = "";

    [Required, MaxLength(10)]
    public string PostalCode { get; set; } = "";

}

using System.ComponentModel.DataAnnotations;

namespace APBD25_CW8.Models.DTOs;

public class ClientDTO
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    [Length(9,9)]
    public string Telephone { get; set; }
    [Required]
    [Length(11,11)]
    public string Pesel { get; set; }
    
}
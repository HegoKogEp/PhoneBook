using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PhoneBook.PhoneBookDbContext;

public partial class Contact
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;
    
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Name) && Regex.IsMatch(PhoneNumber, @"^\+7\d{10}$");
    }
}

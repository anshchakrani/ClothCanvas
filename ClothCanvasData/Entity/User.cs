using Microsoft.AspNetCore.Identity;

namespace ClothCanvas.Entity;

public class User: IdentityUser
{
    public bool IsActive { get; set; }

    public bool isBusiness { get; set; } = false;
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public List<Address> Addresses { get; set; }
    public List<Order> Orders { get; set; }
    
}

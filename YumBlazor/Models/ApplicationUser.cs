using Microsoft.AspNetCore.Identity;
namespace YumBlazor.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2094:Remove this empty class...", Justification = "Required for Identity customization")]
    public class ApplicationUser : IdentityUser
    {
    }

}

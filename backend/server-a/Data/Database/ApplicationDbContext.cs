using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace server_a.Data.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
 IdentityUserContext<IdentityUser>(options)
{
}
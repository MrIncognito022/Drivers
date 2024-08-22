
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Drivers.API.Data
{
    public class ApiDbContext:IdentityDbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            :base(options) { }
        
    }
}

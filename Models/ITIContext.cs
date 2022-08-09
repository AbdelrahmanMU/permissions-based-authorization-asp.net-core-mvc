using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace permissions.Models
{

    public class ITIContext: IdentityDbContext
    {
        public ITIContext():base()
        {
                
        }

        public ITIContext(DbContextOptions options): base(options)
        {

        }
    }
}

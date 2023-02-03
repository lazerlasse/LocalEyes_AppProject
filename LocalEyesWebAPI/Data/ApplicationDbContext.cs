using LocalEyesWebAPI.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LocalEyesWebAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext, IDataProtectionKeyContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PushoverSenderAPIModel> PushoverSenderAPIs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SubscriberModel> Subscribers { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
    }
}
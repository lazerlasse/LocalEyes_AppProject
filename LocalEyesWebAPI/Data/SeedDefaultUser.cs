using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LocalEyesWebAPI.Data
{
    public class SeedDefaultUser
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            // Create logger and Db context to use for seeding users and roles.
            var logger = serviceProvider.GetRequiredService<ILogger<SeedDefaultUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            var identityUser = new IdentityUser
            {
                UserName = "localeyes@localeyes.dk",
                NormalizedUserName = "LOCALEYES@LOCALEYES.DK",
                Email = "localeyes@localeyes.dk",
                NormalizedEmail = "LOCALEYES@LOCALEYES.DK",
                EmailConfirmed = true
            };

            if (!context.Users.Any())
            {
                var password = new PasswordHasher<IdentityUser>();
                var hashed = password.HashPassword(identityUser, "Localeyes.dk+123");
                identityUser.PasswordHash = hashed;

                // Make a user store and save the user to the store.
                var userStore = new UserStore<IdentityUser>(context);
                var result = await userStore.CreateAsync(identityUser);

                // If user created succesfull.
                if (result.Succeeded)
                {
                    try
                    {
                        // Try saving context to Db before continue.
                        await context.SaveChangesAsync();
                        logger.LogInformation("Brugeren blev oprettet og gemt i databasen.");
                    }
                    catch (Exception ex)
                    {
                        // Log error message to console on failure.
                        logger.LogError("Brugeren blev ikke gemt i databasen: ", ex.Message);
                    }
                }
                else
                {
                    logger.LogWarning("Der opstod en uventet fejl og brugeren blev ikke oprettet");
                }
            }
        }
    }
}

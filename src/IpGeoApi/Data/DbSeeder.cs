using Microsoft.AspNetCore.Identity;
namespace IpGeoApi.Data
{
    public static class DbSeeder
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (!userManager.Users.Any())
            {
                for (int i = 1; i <= 5; i++)
                {
                    var user = new IdentityUser
                    {
                        UserName = $"user{i}@example.com",
                        Email = $"user{i}@example.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(user, $"Password{i}!");
                }
            }
        }
    }
}

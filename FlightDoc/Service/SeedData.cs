using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Flight_Doc_Manager_Systems.Utils
{
    public class SeedData
    {
        public static async Task InitializeRoles(RoleManager<IdentityRole> roleManager)
        {
            // Tạo các roles
            var roles = new[]
            {
            "Admin",
            "Manager",
            "User"
        };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task InitializePermissions(RoleManager<IdentityRole> roleManager)
        {
            // Gán permissions cho roles
            await AssignPermissionToRole(roleManager, "Admin", "users.create");
            await AssignPermissionToRole(roleManager, "Admin", "users.read");
            await AssignPermissionToRole(roleManager, "Admin", "users.update");
            await AssignPermissionToRole(roleManager, "Admin", "users.delete");

            await AssignPermissionToRole(roleManager, "Pilot", "users.read");
            await AssignPermissionToRole(roleManager, "Staff_Manager", "users.update");

            await AssignPermissionToRole(roleManager, "crew", "users.read");
            await AssignPermissionToRole(roleManager, "crew", "users.update");
        }

        private static async Task AssignPermissionToRole(RoleManager<IdentityRole> roleManager, string roleName, string permission)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var claims = await roleManager.GetClaimsAsync(role);
                if (!claims.Any(c => c.Type == "permission" && c.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("permission", permission));
                }
            }
        }
    }

}

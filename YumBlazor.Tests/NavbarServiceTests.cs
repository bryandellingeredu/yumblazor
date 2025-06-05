using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using YumBlazor.Services;
using YumBlazor.Utility;

namespace YumBlazor.Tests
{
    public class NavbarServiceTests
    {
        private AuthenticationStateProvider CreateAuthProvider(bool isAuthenticated, string? role = null)
        {
            var identity = isAuthenticated
                ? new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, "TestUser"),
                        new Claim(ClaimTypes.Role, role ?? string.Empty)
                    }, "TestAuth")
                : new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(user);

            return new TestAuthenticationStateProvider(authState);
        }

        [Fact]
        public async Task ShowNavBarAsync_ReturnsTrue_WhenUserIsNotAuthenticated()
        {
            var provider = CreateAuthProvider(false);
            var service = new NavBarService(provider);

            var result = await service.ShowNavBarAsync();

            Assert.True(result);
        }

        [Fact]
        public async Task ShowNavBarAsync_ReturnsTrue_WhenUserIsNotAdmin()
        {
            var provider = CreateAuthProvider(true, "User");
            var service = new NavBarService(provider);

            var result = await service.ShowNavBarAsync();

            Assert.True(result);
        }

        [Fact]
        public async Task ShowNavBarAsync_ReturnsFalse_WhenUserIsAdmin()
        {
            var provider = CreateAuthProvider(true, SD.Role_Admin);
            var service = new NavBarService(provider);

            var result = await service.ShowNavBarAsync();

            Assert.False(result);
        }
    }
}

using Microsoft.AspNetCore.Components.Authorization;
using YumBlazor.Utility;

namespace YumBlazor.Services
{
    
        public class NavBarService
        {
            private readonly AuthenticationStateProvider _authProvider;

            public NavBarService(AuthenticationStateProvider authProvider)
            {
                _authProvider = authProvider;
            }

            public async Task<bool> ShowNavBarAsync()
            {
                var authState = await _authProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                var authenticated = user.Identity is not null && user.Identity.IsAuthenticated;

                if (!authenticated)
                {
                    return true;
                }

                if (!user.IsInRole(SD.Role_Admin))
                {
                    return true;
                }

                return false;
            }
        }
    }

    

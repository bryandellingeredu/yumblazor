using Microsoft.AspNetCore.Components.Authorization;


namespace YumBlazor.Tests
{
    public class TestAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthenticationState _authenticationState;
        public TestAuthenticationStateProvider(AuthenticationState authenticationState) => _authenticationState = authenticationState;

        public override Task<AuthenticationState> GetAuthenticationStateAsync() => Task.FromResult(_authenticationState);
        
    }   
 
}

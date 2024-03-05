Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin
Imports Microsoft.Owin.Security.Cookies
Imports Microsoft.Owin.Security.OAuth
Imports Owin

Partial Public Class Startup
	Private Shared _oAuthOptions As OAuthAuthorizationServerOptions
	Private Shared _publicClientId As String

	' Enable the application to use OAuthAuthorization. You can then secure your Web APIs
	Shared Sub New()
		PublicClientId = "web"

		OAuthOptions = New OAuthAuthorizationServerOptions() With {
			.TokenEndpointPath = New PathString("/Token"),
			.AuthorizeEndpointPath = New PathString("/Account/Authorize"),
			.Provider = New AppOAuthProvider(PublicClientId),
			.AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
			.AllowInsecureHttp = Environment.GetEnvironmentVariable("IN_DEBUG_MODE").Equals("1")
		}
	End Sub

	Public Shared Property OAuthOptions() As OAuthAuthorizationServerOptions
		Get
			Return _oAuthOptions
		End Get
		Private Set
			_oAuthOptions = Value
		End Set
	End Property

	Public Shared Property PublicClientId() As String
		Get
			Return _publicClientId
		End Get
		Private Set
			_publicClientId = Value
		End Set
	End Property

	' For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
	Public Sub ConfigureAuth(appBuilder As IAppBuilder)
		' Configure the db context, user manager and signin manager to use a single instance per request
		appBuilder.CreatePerOwinContext(AddressOf AppDbContext.Create)
		appBuilder.CreatePerOwinContext(Of AppUserManager)(AddressOf AppUserManager.Create)
		appBuilder.CreatePerOwinContext(Of AppSignInManager)(AddressOf AppSignInManager.Create)

		appBuilder.UseCookieAuthentication(New CookieAuthenticationOptions() With {
			.AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
			.Provider = New CookieAuthenticationProvider() With {
				.OnValidateIdentity = SecurityStampValidator.OnValidateIdentity(Of AppUserManager, AppUser)(
					validateInterval:=TimeSpan.FromMinutes(30),
					regenerateIdentity:=Function(manager, user) user.GenerateUserIdentityAsync(manager))},
			.LoginPath = New PathString("/Admin/Login")})
		
		appBuilder.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie)

		' Enable the application to use bearer tokens to authenticate users
		appBuilder.UseOAuthBearerTokens(OAuthOptions)
	End Sub

End Class

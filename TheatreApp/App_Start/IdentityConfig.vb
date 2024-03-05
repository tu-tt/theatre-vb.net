Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin
Imports Microsoft.Owin.Security

' Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

Public Class AppUserManager
	Inherits UserManager(Of AppUser)

	Public Sub New(store As IUserStore(Of AppUser))
		MyBase.New(store)
	End Sub

	Public Shared Function Create(options As IdentityFactoryOptions(Of AppUserManager), context As IOwinContext) As AppUserManager
		Dim manager = New AppUserManager(New UserStore(Of AppUser)(context.Get(Of AppDbContext)()))

		' Configure validation logic for usernames
		manager.UserValidator = New UserValidator(Of AppUser)(manager) With {
			.AllowOnlyAlphanumericUserNames = False,
			.RequireUniqueEmail = True
		}

		' Configure validation logic for passwords
		manager.PasswordValidator = New PasswordValidator With {
			.RequiredLength = 6,
			.RequireNonLetterOrDigit = True,
			.RequireDigit = True,
			.RequireLowercase = True,
			.RequireUppercase = True
		}

		' Configure user lockout defaults
		manager.UserLockoutEnabledByDefault = True
		manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5)
		manager.MaxFailedAccessAttemptsBeforeLockout = 5

		Dim dataProtectionProvider = options.DataProtectionProvider
		If (dataProtectionProvider IsNot Nothing) Then
			manager.UserTokenProvider = New DataProtectorTokenProvider(Of AppUser)(dataProtectionProvider.Create("ASP.NET Identity"))
		End If

		Return manager
	End Function

End Class

' Configure the application sign-in manager which is used in this application.
Public Class AppSignInManager
	Inherits SignInManager(Of AppUser, String)
	Public Sub New(userManager As AppUserManager, authenticationManager As IAuthenticationManager)
		MyBase.New(userManager, authenticationManager)
	End Sub

	Public Overrides Function CreateUserIdentityAsync(user As AppUser) As Task(Of ClaimsIdentity)
		Return user.GenerateUserIdentityAsync(DirectCast(UserManager, AppUserManager))
	End Function

	Public Shared Function Create(options As IdentityFactoryOptions(Of AppSignInManager), context As IOwinContext) As AppSignInManager
		Return New AppSignInManager(context.GetUserManager(Of AppUserManager)(), context.Authentication)
	End Function
End Class
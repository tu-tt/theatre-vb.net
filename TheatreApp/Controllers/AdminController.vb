Imports System.Data.Entity
Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security

Namespace Controllers
	<Authorize>
	Public Class AdminController
		Inherits Controller

		Private _signInManager As AppSignInManager
		Private _userManager As AppUserManager
		Private _authManager As IAuthenticationManager
		Private dbContext As New AppDbContext

		Public Sub New()
		End Sub

		Public Sub New(appUserMan As AppUserManager, signInMan As AppSignInManager)
			UserManager = appUserMan
			SignInManager = signInMan
		End Sub

		Public Property SignInManager() As AppSignInManager
			Get
				Return If(_signInManager, HttpContext.GetOwinContext().[Get](Of AppSignInManager)())
			End Get
			Private Set
				_signInManager = Value
			End Set
		End Property

		Public Property UserManager() As AppUserManager
			Get
				Return If(_userManager, HttpContext.GetOwinContext().GetUserManager(Of AppUserManager)())
			End Get
			Private Set
				_userManager = Value
			End Set
		End Property

		Public Property AuthManager() As IAuthenticationManager
			Get
				Return HttpContext.GetOwinContext().Authentication
			End Get
			Private Set
				_authManager = Value
			End Set
		End Property

		' GET: /Admin
		<AllowAnonymous>
		Function Index() As ActionResult
			If Not User.Identity.IsAuthenticated
				Return RedirectToAction("Login")
			End If

			Return RedirectToAction("Movie")
		End Function

		' GET: /Admin/Login
		<AllowAnonymous>
		Function Login() As ActionResult
			Return View()
		End Function

		' POST: /Admin/Login
		<HttpPost>
		<AllowAnonymous>
		<ValidateAntiForgeryToken>
		Async Function Login(model As LoginPayload) As Task(Of ActionResult)
			If Not ModelState.IsValid Then
				Return View(model)
			End If

			' This doesn't count login failures towards account lockout
			' To enable password failures to trigger account lockout, change to shouldLockout := True
			Dim signinStatus = Await SignInManager.PasswordSignInAsync(model.Email, model.Password, False, shouldLockout:=False)
			Select Case signinStatus
				Case SignInStatus.Success
					Dim user = dbContext.Users.Where(Function(u) u.Email = model.Email).First()
					Dim identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie)
					AuthManager.SignIn(identity)

					Return RedirectToAction("Index")
				Case Else
					ModelState.AddModelError("", "Invalid login attempt.")
					Return View(model)
			End Select
		End Function

		'
		' GET: /Admin/Register
		<AllowAnonymous>
		Public Function Register() As ActionResult
			Return View()
		End Function

		'
		' POST: /Admin/Register
		<HttpPost>
		<AllowAnonymous>
		<ValidateAntiForgeryToken>
		Public Async Function Register(model As RegisterPayload) As Task(Of ActionResult)
			If ModelState.IsValid Then
				Dim user = New AppUser() With {
					.UserName = model.Email,
					.Email = model.Email
				}

				Dim result = Await UserManager.CreateAsync(user, model.Password)
				If result.Succeeded Then
					Await SignInManager.SignInAsync(user, isPersistent:=False, rememberBrowser:=False)

					' For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
					' Send an email with this link
					' Dim code = Await UserManager.GenerateEmailConfirmationTokenAsync(user.Id)
					' Dim callbackUrl = Url.Action("ConfirmEmail", "Account", New With { .userId = user.Id, code }, protocol := Request.Url.Scheme)
					' Await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=""" & callbackUrl & """>here</a>")

					Return RedirectToAction("Index")
				End If
				AddErrors(result)
			End If

			' If we got this far, something failed, redisplay form
			Return View(model)
		End Function

		' GET: /Admin/Movie
		Function Movie() As ActionResult
			Return View(dbContext.Movies.ToList())
		End Function

		' POST: /Admin/CreateMovie
		<HttpPost()>
		<ValidateAntiForgeryToken()>
		Function CreateMovie(ByVal movie As Movie) As ActionResult
			dbContext.Movies.Add(movie)
			dbContext.SaveChanges()

			Return RedirectToAction("Movie")
		End Function

		' POST: /Admin/UpdateMovie
		<HttpPost()>
		<ValidateAntiForgeryToken()>
		Function UpdateMovie(ByVal movie As Movie) As ActionResult
			If ModelState.IsValid Then
				dbContext.Entry(movie).State = EntityState.Modified
				dbContext.SaveChanges()
			End If

			Return RedirectToAction("Movie")
		End Function

		' GET: /Admin/DeleteMovie/{id}
		<HttpGet()>
		Function DeleteMovie(ByVal id As Integer?) As ActionResult
			dbContext.Movies.Remove(dbContext.Movies.Find(id))
			dbContext.SaveChanges()

			Return RedirectToAction("Movie")
		End Function

		' GET: /Admin/Show
		<HttpGet()>
		Function Show(ActionMsg As String) As ActionResult
			ViewData("Shows") = dbContext.Shows.ToList()
			ViewData("Rooms") = dbContext.Rooms.ToList()
			ViewData("ActionMsg") = ActionMsg

			Return View(dbContext.Movies.Include("Shows").ToList())
		End Function

		' POST: /Admin/CreateShow
		<HttpPost()>
		Function CreateShow(ByVal payloads As List(Of CreateShowPayload)) As ActionResult
			For Each payload In payloads
				If payload.Id > 0 And Not payload.Changed
					Continue For
				End If

				Dim movie = dbContext.Movies.Find(payload.MovieId)
				Dim room = dbContext.Rooms.Find(payload.RoomId)

				If payload.Id > 0
					Dim show = dbContext.Shows.Find(payload.Id)
					show.Movie = movie
					show.Room = room
					show.ShowTime = payload.ShowTime
					dbContext.Entry(show).State = EntityState.Modified
				Else
					Dim show As New Show With {
						.Movie = movie,
						.Room = room,
						.ShowTime = payload.ShowTime
					}
					dbContext.Shows.Add(show)
				End If
			Next

			dbContext.SaveChanges()
			Return RedirectToAction("Show", New With {.ActionMsg = "Movie shows are updated successfully."})
		End Function

		Private Sub AddErrors(result As IdentityResult)
			For Each [error] In result.Errors
				ModelState.AddModelError("", [error])
			Next
		End Sub

	End Class
End Namespace
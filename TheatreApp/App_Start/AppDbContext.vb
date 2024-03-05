Imports System.Data.Entity
Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework

' You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
Public Class AppUser
	Inherits IdentityUser

	Public Async Function GenerateUserIdentityAsync(manager As UserManager(Of AppUser)) As Task(Of ClaimsIdentity)
		' Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
		Dim userIdentity = Await manager.CreateIdentityAsync(Me, DefaultAuthenticationTypes.ExternalBearer)
		' Add custom user claims here
		Return userIdentity
	End Function
End Class

Public Class AppDbContext
	Inherits IdentityDbContext(Of AppUser)

	Public Sub New()
		MyBase.New("Server=localhost,1433;UID=sa;PWD=P@ssw0rd;database=theatre")
	End Sub

	Public Shared Function Create() As AppDbContext
		Return New AppDbContext()
	End Function

	Public Property Movies() As DbSet(Of Movie)
	Public Property Rooms() As DbSet(Of Room)
	Public Property Seats() As DbSet(Of Seat)
	Public Property Shows() As DbSet(Of Show)
	Public Property Tickets() As DbSet(Of Ticket)
	Public Property Payments() As DbSet(Of Payment)
End Class

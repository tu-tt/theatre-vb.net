Imports System.Net

Namespace Controllers
    Public Class MovieController
        Inherits Controller

        Private dbContext As New AppDbContext

        ' GET: /Movie/Detail/{id}
        Function Detail(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim movie As Movie = dbContext.Movies.Find(id)
            If IsNothing(movie) Then
                Return HttpNotFound()
            End If
            Return View(movie)
        End Function

        ' GET /Movie/Shows/{id}
        Function Shows(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
			
            Dim showList = dbContext.Shows _
				.Include("Room").Include("Room.Seats").Include("Movie") _
				.Where(Function(show) show.Movie.Id = id) _
				.ToList()
			
            If IsNothing(showList) Then
                Return HttpNotFound()
            End If

			Dim ticketList = dbContext.Tickets _
				.Where(Function(ticket) ticket.Show.Movie.Id = id) _
				.Where(Function(ticket) ticket.Payment.Status = "SUCCESS") _
				.ToList()

			Dim boughtSeatList = ticketList.ConvertAll(Function(ticket) ticket.Seat)
			ViewData("boughtSeatList") = boughtSeatList

            Return View(showList)
        End Function
    End Class
End Namespace
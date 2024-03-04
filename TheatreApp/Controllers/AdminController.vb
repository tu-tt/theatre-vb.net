Imports System.Data.Entity

Namespace Controllers
	Public Class AdminController
		Inherits Controller

		Private dbContext As New AppDbContext

		' GET: /Admin
		Function Index() As ActionResult
			Return RedirectToAction("Movie")
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
	End Class
End Namespace
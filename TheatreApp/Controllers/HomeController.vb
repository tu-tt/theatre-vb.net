Public Class HomeController
    Inherits Controller

    Private dbContext As New AppDbContext

    ' GET /
    Function Index() As ActionResult
        Return View(dbContext.Movies.ToList())
    End Function
End Class

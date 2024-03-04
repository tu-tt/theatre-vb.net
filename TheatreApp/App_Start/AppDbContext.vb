Imports System.Data.Entity

Public Class AppDbContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("Server=localhost,1433;UID=sa;PWD=P@ssw0rd;database=theatre")
    End Sub

    Public Property Movies() As DbSet(Of Movie)
    Public Property Rooms() As DbSet(Of Room)
    Public Property Seats() As DbSet(Of Seat)
    Public Property Shows() As DbSet(Of Show)
    Public Property Tickets() As DbSet(Of Ticket)
	Public Property Payments() As DbSet(Of Payment)
End Class

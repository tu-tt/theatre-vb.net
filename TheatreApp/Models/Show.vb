Imports System.ComponentModel.DataAnnotations

Public Class Show
	<Key()>
	Public Property Id As Integer
	Public Property Movie As Movie
	Public Property Room As Room
	Public Property ShowTime As DateTime
	Public Property Tickets As List(Of Ticket)
End Class

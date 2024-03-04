Public Class Payment
	Public Property Id As Integer
	Public Property CardHolder As String
	Public Property CardNumber As String
	Public Property Amount As Integer
	Public Property Status As String
	Public Property Tickets As List(Of Ticket)
End Class

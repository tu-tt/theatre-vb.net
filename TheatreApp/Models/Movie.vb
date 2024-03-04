Imports System.ComponentModel.DataAnnotations
Imports Newtonsoft.Json

Public Class Movie
	<Key()>
	Public Property Id As Integer
	Public Property Name As String
	Public Property ImageUrl As String
	Public Property TicketPrice As Integer
	<JsonIgnore()>
	Public Property Shows As List(Of Show)
End Class

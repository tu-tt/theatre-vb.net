Imports System.ComponentModel.DataAnnotations
Imports Newtonsoft.Json

Public Class Room
	<Key()>
	Public Property Id As Integer
	Public Property Number As Integer
	<JsonIgnore()>
	Public Property Shows As List(Of Show)
	<JsonIgnore()>
	Public Property Seats As List(Of Seat)
End Class

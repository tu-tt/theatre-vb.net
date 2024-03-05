Imports System.ComponentModel.DataAnnotations

Public Class RegisterPayload
	<Required>
	<EmailAddress>
	<Display(Name:="Email")>
	Public Property Email As String

	<Required>
	<StringLength(100, ErrorMessage:="The {0} must be at least {2} characters long.", MinimumLength:=6)>
	<DataType(DataType.Password)>
	<Display(Name:="Password")>
	Public Property Password As String

	<DataType(DataType.Password)>
	<Display(Name:="Confirm password")>
	<Compare("Password", ErrorMessage:="The password and confirmation password do not match.")>
	Public Property ConfirmPassword As String
End Class

Public Class LoginPayload
	<Required>
	<EmailAddress>
	<Display(Name:="Email")>
	Public Property Email As String

	<Required>
	<DataType(DataType.Password)>
	<Display(Name:="Password")>
	Public Property Password As String
End Class
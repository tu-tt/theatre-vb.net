@Code
    Dim url = HttpContext.Current.Request.Url.AbsolutePath
    If url.StartsWith("/Admin") Then
        Layout = "~/Views/Shared/_AdminLayout.vbhtml"
    Else Layout = "~/Views/Shared/_Layout.vbhtml"
    End If
End Code
@Code
    ViewData("Title") = "Result"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="w-100 text-center">
    @Code
        If (ViewBag.Success)
            @<b class="fs-4">Lovely! Enjoy the movie &#9829;</b>
            @<br />
            @<a class="btn btn-success mt-4" href="/">Back to Home Page</a>
        Else
            @<b class="fs-4">Oops! Something isn't right</b>
            @<br />
            @<span class="mt-4">Please try again, or make a call for support!</span>
        End If
    End Code
</div>

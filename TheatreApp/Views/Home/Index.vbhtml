@ModelType IEnumerable(Of Movie)
@Code
    ViewData("Title") = "Welcome"
End Code

<div class="row">
    @For Each movie In Model
    @<div class="col-3 d-flex flex-column gap-2 my-2">
         <a href="/Movie/Detail/@movie.Id" class="d-flex flex-column gap-2 text-center text-decoration-none">
             <img alt="" src="@movie.ImageUrl" class="w-100"/>
             <b class="fs-6 text-white">@movie.Name</b>
         </a>
    </div>
    Next
</div>

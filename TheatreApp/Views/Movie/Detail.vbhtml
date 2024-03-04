@ModelType Movie
@Code
    ViewData("Title") = "Details"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="row">
    <div class="col-5">
        <img alt="" src="@Model.ImageUrl" class="w-100 rounded shadow"/>
    </div>
    <div class="col-7">
        <h1>@Model.Name</h1>
        <b class="fs-6 me-4">Ticket Price:</b><span>@Model.TicketPrice</span><br />
        <a class="btn btn-primary mt-4" href="/Movie/Shows/@Model.Id">Book a Ticket</a>
    </div>
</div>

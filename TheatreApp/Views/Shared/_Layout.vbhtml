<!DOCTYPE html>
<html lang="en" data-bs-theme="dark">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title</title>
    @Styles.Render("~/Content/css")
    @Styles.Render("~/Content/font-awesome")
    @Scripts.Render("~/bundles/modernizr")
</head>
<body>
    <nav class="navbar navbar-expand-lg bg-body-tertiary position-fixed w-100 z-3">
        <div class="container">
            <a class="navbar-brand" href="/">Movie Theatre</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                </ul>
                <form class="d-flex" role="search">
                    <input class="form-control me-2" type="search" placeholder="Search" aria-label="Search">
                    <button class="btn btn-outline-success" type="submit">Search</button>
                </form>
            </div>
        </div>
    </nav>
    <div class="container body-content py-4" style="padding-top: 64px !important">
        @RenderBody()
    </div>

    <footer class="container">
        <p>&copy; @DateTime.Now.Year - AnoD</p>
    </footer>
    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/umbrellajs"></script>
    @RenderSection("scripts", required:=False)
</body>
</html>

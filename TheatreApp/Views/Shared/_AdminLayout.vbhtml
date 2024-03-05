<!DOCTYPE html>

<html lang="en" data-bs-theme="dark">
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Theatre Management</title>
    @Styles.Render("~/Content/css")
    @Styles.Render("~/Content/font-awesome")
    @Scripts.Render("~/bundles/modernizr")
</head>
<body class="overflow-y-hidden">
    <div class="container-fluid body-content">
        <div class="row">
            <div class="col-2 vh-100 position-fixed">
                <div class="w-100 h-100 position-relative">
                    <img alt="" src="~/Static/vox_cine.png" class="w-75 d-block m-auto my-4" style="filter: invert(1)" />
                    <ul class="list-group list-group-flush mt-2 fs-6">
                        <a class="list-group-item list-group-item-action d-inline-flex align-items-center gap-4 py-3" href="/Admin/Movie">
                            <i class="fa fa-film" aria-hidden="true"></i>
                            <span>Movie</span>
                        </a>
                        <a class="list-group-item list-group-item-action d-inline-flex align-items-center gap-4 py-3" href="/Admin/Show">
                            <i class="fa fa-video-camera" aria-hidden="true"></i>
                            <span>Show</span>
                        </a>
                    </ul>
                    <p class="position-absolute bottom-0 left-0">&copy; @DateTime.Now.Year - AnoD </p>
                </div>
            </div>
            <div class="col-2"></div>
            <div class="col-10">
                <div class="w-100 row py-2 position-fixed bg-dark z-3">
                    <h2>@ViewBag.Title</h2>
                </div>
                <div class="w-100 mb-5 pb-2" style="margin-top: 64px; max-height: calc(100vh - 48px)">
                    @RenderBody()
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/umbrellajs"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/luxon@3.4.4/build/global/luxon.min.js"></script>
    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
    @RenderSection("scripts", required:=False)
</body>
</html>

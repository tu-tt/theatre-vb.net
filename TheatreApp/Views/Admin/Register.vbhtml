@Code
    Layout = Nothing
End Code

<!DOCTYPE html>
<html lang="en" data-bs-theme="dark">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Admin Dashboard - Login</title>
    @Styles.Render("~/Content/css")
    @Styles.Render("~/Content/font-awesome")
    @Scripts.Render("~/bundles/modernizr")
</head>
<body>
    <div class="container body-content py-4" style="padding-top: 64px !important">
        <form action="/Admin/Register" method="post" class="m-auto" style="width: fit-content">
            @Html.AntiForgeryToken
            <img alt="" src="/Static/Wicked-Movie-Logo-PNG.png" width="384px" />
            <div class="mb-3">
                <label for="email-input" class="form-label">Email address</label>
                <input type="email" class="form-control" id="email-input" aria-describedby="emailHelp" name="Email">
                <div id="emailHelp" class="form-text">We'll never share your email with anyone else.</div>
            </div>
            <div class="mb-3">
                <label for="password-input" class="form-label">Password</label>
                <input type="password" class="form-control" id="password-input" name="Password">
            </div>
            <div class="mb-3">
                <label for="confirm-password-input" class="form-label">Confirm Password</label>
                <input type="password" class="form-control" id="confirm-password-input" name="ConfirmPassword">
            </div>
            <button type="submit" class="btn btn-primary">Submit</button>
            <a href="/Admin/Login" class="btn btn-outline-primary btn-link">Login</a>
        </form>
    </div>

    <footer class="container-fluid position-absolute bottom-0 start-0">
        <p>&copy; @DateTime.Now.Year - AnoD</p>
    </footer>

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
</body>
</html>

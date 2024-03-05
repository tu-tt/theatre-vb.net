Imports System.Security.Claims
Imports System.Web.Helpers
Imports System.Web.Optimization

Public Class MvcApplication
	Inherits HttpApplication

	Sub Application_Start()
		AreaRegistration.RegisterAllAreas()
		RegisterGlobalFilters(GlobalFilters.Filters)
		RegisterRoutes(RouteTable.Routes)
		RegisterBundles(BundleTable.Bundles)
		AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier
	End Sub
End Class

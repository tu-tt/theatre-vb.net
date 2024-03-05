Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class ModelChanged
        Inherits DbMigration
    
        Public Overrides Sub Up()
            DropColumn("dbo.AspNetUsers", "Hometown")
        End Sub
        
        Public Overrides Sub Down()
            AddColumn("dbo.AspNetUsers", "Hometown", Function(c) c.String())
        End Sub
    End Class
End Namespace

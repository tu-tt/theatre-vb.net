Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class AddPaymentCardHolder
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Payments", "CardHolder", Function(c) c.String())
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Payments", "CardHolder")
        End Sub
    End Class
End Namespace

Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class RenameToSeat
        Inherits DbMigration
    
        Public Overrides Sub Up()
            RenameTable(name := "dbo.Seets", newName := "Seats")
            RenameColumn(table := "dbo.Tickets", name := "Seet_Id", newName := "Seat_Id")
            RenameIndex(table := "dbo.Tickets", name := "IX_Seet_Id", newName := "IX_Seat_Id")
        End Sub
        
        Public Overrides Sub Down()
            RenameIndex(table := "dbo.Tickets", name := "IX_Seat_Id", newName := "IX_Seet_Id")
            RenameColumn(table := "dbo.Tickets", name := "Seat_Id", newName := "Seet_Id")
            RenameTable(name := "dbo.Seats", newName := "Seets")
        End Sub
    End Class
End Namespace

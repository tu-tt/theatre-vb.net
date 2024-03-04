Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class Initial
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Movies",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .Name = c.String(),
                        .ImageUrl = c.String(),
                        .TicketPrice = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.Shows",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .ShowTime = c.DateTime(nullable := False),
                        .Movie_Id = c.Int(),
                        .Room_Id = c.Int()
                    }) _
                .PrimaryKey(Function(t) t.Id) _
                .ForeignKey("dbo.Movies", Function(t) t.Movie_Id) _
                .ForeignKey("dbo.Rooms", Function(t) t.Room_Id) _
                .Index(Function(t) t.Movie_Id) _
                .Index(Function(t) t.Room_Id)
            
            CreateTable(
                "dbo.Rooms",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .Number = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.Seats",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .Row = c.String(),
                        .Number = c.Int(nullable := False),
                        .Room_Id = c.Int()
                    }) _
                .PrimaryKey(Function(t) t.Id) _
                .ForeignKey("dbo.Rooms", Function(t) t.Room_Id) _
                .Index(Function(t) t.Room_Id)
            
            CreateTable(
                "dbo.Payments",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .CardNumber = c.String(),
                        .Amount = c.Int(nullable := False),
                        .Status = c.String()
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.Tickets",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .Price = c.Int(nullable := False),
                        .Payment_Id = c.Int(),
                        .Seat_Id = c.Int(),
                        .Show_Id = c.Int()
                    }) _
                .PrimaryKey(Function(t) t.Id) _
                .ForeignKey("dbo.Payments", Function(t) t.Payment_Id) _
                .ForeignKey("dbo.Seats", Function(t) t.Seat_Id) _
                .ForeignKey("dbo.Shows", Function(t) t.Show_Id) _
                .Index(Function(t) t.Payment_Id) _
                .Index(Function(t) t.Seat_Id) _
                .Index(Function(t) t.Show_Id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.Tickets", "Show_Id", "dbo.Shows")
            DropForeignKey("dbo.Tickets", "Seat_Id", "dbo.Seats")
            DropForeignKey("dbo.Tickets", "Payment_Id", "dbo.Payments")
            DropForeignKey("dbo.Shows", "Room_Id", "dbo.Rooms")
            DropForeignKey("dbo.Seats", "Room_Id", "dbo.Rooms")
            DropForeignKey("dbo.Shows", "Movie_Id", "dbo.Movies")
            DropIndex("dbo.Tickets", New String() { "Show_Id" })
            DropIndex("dbo.Tickets", New String() { "Seat_Id" })
            DropIndex("dbo.Tickets", New String() { "Payment_Id" })
            DropIndex("dbo.Seats", New String() { "Room_Id" })
            DropIndex("dbo.Shows", New String() { "Room_Id" })
            DropIndex("dbo.Shows", New String() { "Movie_Id" })
            DropTable("dbo.Tickets")
            DropTable("dbo.Payments")
            DropTable("dbo.Seats")
            DropTable("dbo.Rooms")
            DropTable("dbo.Shows")
            DropTable("dbo.Movies")
        End Sub
    End Class
End Namespace

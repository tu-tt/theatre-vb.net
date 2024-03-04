Namespace Controllers
    Public Class TicketController
        Inherits Controller

        Private dbContext As New AppDbContext

        ' POST: /Ticket/Pay
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Pay(ByVal payloads As List(Of TicketPayload)) As ActionResult
			Dim amount = payloads.Sum(Function(payload) payload.Price)
			Dim payment As New Payment With {
				.Amount = amount,
				.Status = "PENDING",
				.Tickets = New List(Of Ticket)
			}

			dbContext.Payments.Add(payment)

			For Each payload In payloads
				Dim ticket As New Ticket With {
					.Show = dbContext.Shows.Find(payload.ShowId),
					.Seat = dbContext.Seats.Find(payload.SeatId),
					.Price = payload.Price,
					.Payment = payment
				}

				payment.Tickets.Add(ticket)
				dbContext.Tickets.Add(ticket)
			Next

			dbContext.SaveChanges()
			Return View(payment)
        End Function

		' POST: /Ticket/ProcessPayment
		<HttpPost()>
		<ValidateAntiForgeryToken()>
		Function ProcessPayment(ByVal payment As Payment) As ActionResult
			Dim success = False
			If ModelState.IsValid Then
				Dim userPayment = dbContext.Payments.Find(payment.Id)
				userPayment.CardHolder = payment.CardHolder
				userPayment.CardNumber = payment.CardNumber
				userPayment.Status = "SUCCESS"

				dbContext.Entry(userPayment).State = Entity.EntityState.Modified
				dbContext.SaveChanges()
				
				success = True
			End If

			ViewData("Success") = success
			Return View("PaymentResult")
		End Function
    End Class
End Namespace
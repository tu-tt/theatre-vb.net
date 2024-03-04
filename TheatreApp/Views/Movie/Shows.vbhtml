@Imports Newtonsoft.Json
@ModelType IEnumerable(Of Show)
@Code
    ViewData("Title") = "Shows"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="row" style="margin-top: 48px">
    <div class="col-9">
        <h4>Show time:</h4>
        <div class="my-4 list-group list-group-horizontal">
            @For Each show In Model
                @<button class="list-group-item btn-time" data-show-id="@show.Id">@show.ShowTime (Room @show.Room.Number)</button>
            Next
        </div>
        <h4>Seats:</h4>
        <div class="w-75 m-auto text-center border border-2 rounded py-4 mb-4">
            Screen
        </div>
        <table class="m-auto mt-4">
            <tbody id="seat-table"></tbody>
        </table>
    </div>
    <div class="col-3 text-center position-sticky">
        @Code
            Dim movie = Model.ElementAt(0).Movie
        End Code
        <img alt="" src="@movie.ImageUrl" class="w-100 rounded shadow" />
        <h4 class="mt-4">@movie.Name</h4>
        <div class="d-flex justify-content-between align-items-center my-4">
            <b>Ticket Price:</b>
            <b id="price-text">VND @movie.TicketPrice</b>
        </div>
        <div class="d-flex justify-content-between align-items-center my-4">
            <b>Amount:</b>
            <b id="amount-text"></b>
        </div>
        <div class="d-flex justify-content-between align-items-center my-4">
            <b>Total Price:</b>
            <b id="total-text"></b>
        </div>
        <button class="btn btn-success w-100" disabled id="btn-pay">Pay</button>
    </div>
</div>

<form action="/Ticket/Pay" method="post" id="ticket-form">
</form>

@Section Scripts
    <script>
        @Code
            Dim jsonSettings As New JsonSerializerSettings With { .ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
        End Code
        const shows = @Html.Raw(JsonConvert.SerializeObject(Model, Formatting.None, jsonSettings));

        const showSeats = {
            @For Each show In Model
                @Html.Raw(show.Id & ": " & JsonConvert.SerializeObject(show.Room.Seats) & "," & System.Environment.NewLine)
            Next
        };

        const boughtSeats = @Html.Raw(JsonConvert.SerializeObject(ViewBag.boughtSeatList, Formatting.None, jsonSettings));

        const price = @movie.TicketPrice;
        const token = '@Html.AntiForgeryToken()';
        const ticketForm = u('#ticket-form')
        const selectedSeats = []

        let selectedShow = null
        u('.btn-time').each(el => u(el).on('click', event => {
            if (selectedShow) {
                if (selectedShow.first() === event.target) return
                selectedShow.removeClass('active')
            }

            const el = u(event.target).addClass('active')
            const showId = el.data('show-id')
            selectedShow = el

            const table = u('#seat-table').empty()
            const seats = showSeats[showId]
            for (const row of 'ABCDEFGHIJ') {
                const seatRow = u('<tr>')
                const rowSeats = seats.filter(seat => seat.Row === row)
                rowSeats.forEach(seat => seatRow.append(seatCell(seat).on('click', takeSeat)))
                table.append(seatRow)
            }

            selectedSeats.splice(0, selectedSeats.length)
            renderSelectedSeats()
        }))

        u('#btn-pay').on('click', event => ticketForm.append(token).first().submit())

        const takeSeat = event => {
            const buttonEl = u(event.target).filter(el => u(el).is('button')).first()
            if (!buttonEl) return

            const button = u(buttonEl)
            const seatId = parseInt(button.data('seat-id'))
            const showId = parseInt(selectedShow.data('show-id'))

            if (!button.hasClass('active')) {
                button.addClass('active')
                const seat = showSeats[showId].find(s => s.Id === seatId)
                selectedSeats.push(seat)
            } else {
                button.removeClass('active')
                const seatIndex = selectedSeats.findIndex(s => s.Id === seatId)
                selectedSeats.splice(seatIndex, 1)
            }

            renderSelectedSeats()
        }

        const renderSelectedSeats = () => {
            ticketForm.empty()

            const showId = selectedShow.data('show-id')
            selectedSeats.forEach((seat, i) => {
                if (!seat) return
                const prefix = '[' + i + '].'
                ticketForm
                    .append(hiddenInput(prefix + "ShowId", showId))
                    .append(hiddenInput(prefix + "SeatId", seat.Id))
                    .append(hiddenInput(prefix + "Price", price))
            })

            const amount = selectedSeats.length
            u('#amount-text').text(amount)
            u('#total-text').text('VND ' + (amount * price))
            u('#btn-pay').attr('disabled', amount === 0)
        }

        const seatCell = seat => {
            const seatBought = !!boughtSeats.find(s => s.Id === seat.Id)
            const btnClass = 'btn w-100 ' + (seatBought ? 'btn-secondary' : '')

            return u('<button>')
                .addClass(btnClass).text(seat.Row + seat.Number)
                .data('seat-id', seat.Id).attr('disabled', seatBought)
                .wrap('<td>');
        }

        const hiddenInput = (name, value) => u('<input>').attr({type: 'hidden', name, value})
    </script>
End Section

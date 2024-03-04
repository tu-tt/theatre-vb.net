@Imports Newtonsoft.Json
@ModelType IEnumerable(Of Movie)
@Code
    ViewData("Title") = "Show"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<div class="row ms-1 position-relative">
    <div class="col-3 ps-0">
        <h4>Movies:</h4>
        <div class="list-group list-group-flush overflow-y-scroll pb-4" style="height: calc(100vh - 96px)">
            @For Each movie In Model
                @<a class="list-group-item d-flex flex-column gap-2 px-2 py-4 rounded movie-card" 
                    href="javascript:void(0);" data-movie-id="@movie.Id">
                    <h6>@movie.Name</h6>
                    <img alt="" src="@movie.ImageUrl" class="object-fit-cover rounded" style="height: 128px" />
                </a>
            Next
        </div>
    </div>
    <div class="col-9">
        <h4 id="schedule-title">Scheduled Shows:</h4>
        <form class="w-100 mt-4" id="shows-form" style="max-height: calc(100vh - 120px)"
              action="/Admin/CreateShow" method="post">
            <table class="table">
                <thead>
                    <tr>
                        <th class="col-4">Room</th>
                        <th class="col-4">Show Time</th>
                        <th class="col-4">Action</th>
                    </tr>
                </thead>
                <tbody id="show-table">
                </tbody>
            </table>
        </form>
        <div class="w-100 mt-4 d-flex gap-2">
            <button class="btn btn-primary d-none" id="btn-add-schedule">New Schedule</button>
            <button class="btn btn-success d-none" id="btn-apply">Apply</button>
        </div>
    </div>
</div>

<div class="toast-container position-fixed top-0 end-0 p-3">
    <div class="toast align-items-center text-bg-primary border-0" role="alert" aria-live="assertive" aria-atomic="true" id="action-toast">
        <div class="d-flex">
            <div class="toast-body">
                <span id="action-msg">@ViewBag.ActionMsg</span>
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
    </div>
</div>

@Section Scripts
    <script>
        const movies = @Html.Raw(JsonConvert.SerializeObject(Model));
        const shows = @Html.Raw(JsonConvert.SerializeObject(ViewBag.Shows));
        const rooms = @Html.Raw(JsonConvert.SerializeObject(ViewBag.Rooms));

        const addScheduleBtn = u('#btn-add-schedule')
        const appliedBtn = u('#btn-apply')
        const showsForm = u('#shows-form')
        const showTable = u('#show-table')
        const actionToast = bootstrap.Toast.getOrCreateInstance(u('#action-toast').first())

        if (u('#action-msg').text()) actionToast.show()

        addScheduleBtn.on('click', event => {
            const movieId = parseInt(addScheduleBtn.data('movie-id'))
            const nextId = showTable.children().length
            showTable.append(showCell(nextId, null, movieId, null, null))
        })

        appliedBtn.on('click', event => showsForm.trigger('submit'))
        showsForm.handle('submit', event => showsForm.first().submit())

        let selectedCard = null
        u('.movie-card').each(el => u(el).on('click', event => {
            if (selectedCard) selectedCard.removeClass('active')

            let el = u(event.target)
            while (!el.is('a')) el = el.parent();
            selectedCard = el.addClass('active')

            const movieId = parseInt(el.data('movie-id'))
            showTable.empty()
            shows.filter(s => s.Movie.Id === movieId)
                .map((show, index) => showCell(index, show.Id, movieId, show.Room.Id, show.ShowTime))
                .forEach(el => showTable.append(el))

            addScheduleBtn.data('movie-id', movieId).removeClass('d-none')
            u('#schedule-title').text('Scheduled Shows for "' + movies.find(m => m.Id === movieId).Name + '":')
        }))

        const checkCell = (index, feedback) => {
            const showData = el => {
                const node = u(el);
                const RoomId = node.find('select').first().value
                const ShowTime = node.find('input[type="datetime-local"]').first().value
                return { RoomId, ShowTime };
            }

            const shows = showTable.children('tr').array(el => showData(el))
            const indexedShow = showData(showTable.children().filter((node, i) => i === index))
            const duplicated = shows.find((s, i) => i !== index
                && s.RoomId === indexedShow.RoomId
                && s.ShowTime === indexedShow.ShowTime);
            
            if (duplicated) {
                appliedBtn.attr('disabled', true)
                feedback.removeClass('d-none').text('This show is already set!')
            } else {
                appliedBtn.attr('disabled', null)
                feedback.addClass('d-none').text(null)
            }
        }

        const showCell = (index, showId, movieId, roomId, showTime) => {
            const prefix = '[' + index + '].'
            const idInput = hiddenInput(prefix + 'Id', showId)
            const movieIdInput = hiddenInput(prefix + 'MovieId', movieId)
            const changedInput = hiddenInput(prefix + 'Changed', false)
            const validFeedback = u('<div>').attr('id', prefix + 'feedback').addClass('mt-2 text-danger d-none')

            const changedEvent = event => {
                changedInput.attr('value', true);
                appliedBtn.removeClass('d-none');
                checkCell(index, validFeedback)
            }

            const options = rooms.map(room => u('<option>')
                .attr({ value: room.Id, selected: room.Id === roomId })
                .text(room.Number))
            const select = u('<select>').addClass('form-control')
                .attr('name', prefix + 'RoomId')
                .on('change', changedEvent)
            options.forEach(option => select.append(option))

            return u('<tr>')
                .append(u('<td>').addClass('col-4').append(select).append(validFeedback))
                .append(u('<td>').addClass('col-4')
                    .append(u('<input>').addClass('form-control').attr({
                        'type': 'datetime-local',
                        'name': prefix + 'ShowTime',
                        'value': showTime,
                    }).on('change', changedEvent)))
                .append(u('<td>')
                    .append(idInput).append(movieIdInput).append(changedInput))
        }

        const hiddenInput = (name, value) => u('<input>').attr({ type: 'hidden', name, value })
    </script>
End Section

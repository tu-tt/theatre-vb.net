@ModelType IEnumerable(Of Movie)
@Code
    ViewData("Title") = "Movie"
    Layout = "~/Views/Shared/_AdminLayout.vbhtml"
End Code

<button class="btn btn-primary my-2" data-bs-toggle="modal" data-bs-target="#movie-modal" id="btn-add">Add Movie</button>
<div class="row overflow-y-scroll" style="max-height: calc(100vh - 120px)">
    @For Each movie In Model
        @<div class="col-3 d-flex flex-column gap-2 my-2">
            <div class="d-flex flex-column gap-2 text-center movie-block" style="cursor: pointer" 
                 data-movie-id="@movie.Id">
                <img alt="" src="@movie.ImageUrl" class="w-100 rounded shadow" />
                <b class="fs-6 text-white">@movie.Name</b>
            </div>
        </div>
    Next
</div>


<!-- Modal -->
<div class="modal fade" id="movie-modal" tabindex="-1" aria-labelledby="movie-modal-label" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <form class="modal-content" id="form" action="/Admin/CreateMovie" method="post">
            @Html.AntiForgeryToken()
            <input type="hidden" name="Id" id="id-input" />
            <div class="modal-header">
                <h1 class="modal-title fs-5" id="movie-modal-label">New Movie</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body row">
                <div class="col-7">
                    <div class="d-flex flex-column gap-2">
                        <div class="mb-3">
                            <label for="name-input" class="form-label">Name:</label>
                            <input class="form-control" id="name-input" name="Name">
                        </div>
                        <div class="mb-3">
                            <label for="image-input" class="form-label">Image:</label>
                            <div class="d-flex gap-2">
                                <input class="form-control" id="image-input" type="url" name="ImageUrl">
                                <button type="button" class="btn btn-outline-primary" id="btn-paste">
                                    <i class="fa fa-clipboard" aria-hidden="true"></i>
                                </button>
                            </div>
                        </div>
                        <div class="mb-3">
                            <label for="price-input" class="form-label">Ticket Price:</label>
                            <input class="form-control" id="price-input" name="TicketPrice">
                        </div>
                    </div>
                </div>
                <div class="col-5">
                    <div class="w-100 h-100 border-1 border-secondary text-center">
                        <p class="fs-6 text-secondary align-middle" id="preview-placeholder">Image Preview</p>
                        <img id="preview-img" alt="Preview Image" src="" class="w-100 d-none rounded shadow" />
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <div class="d-flex justify-content-between w-100">
                    <a class="btn btn-danger" href="#" id="btn-delete">Delete Movie</a>
                    <div class="d-flex gap-2">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        <button type="submit" class="btn btn-primary">Save</button>
                    </div>
                </div>
            </div>
        </form>
    </div>
</div>

@Section Scripts
    <script>
        const movies = @Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(Model));

        const deleteBtn = u('#btn-delete')
        const imgHolder = u('#preview-placeholder')
        const imgPreview = u('#preview-img')
        const modalTitle = u('#movie-modal-label')
        const form = u('#form')

        const nullMovie = { Id: null, Name: null, ImageUrl: null, TicketPrice: null }
        const movieModal = bootstrap.Modal.getOrCreateInstance(u('#movie-modal').first())

        u('#btn-add').on('click', event => {
            modalTitle.text('New Movie')
            deleteBtn.attr('href', '#')
            form.attr('action', '/Admin/CreateMovie')

            showMovie(nullMovie)
            hideImage()
            hide(deleteBtn)
            toggleClass(deleteBtn.parent(), 'justify-content-end', 'justify-content-between')
        })

        u('.movie-block').each(el => u(el).on('click', event => {
            let element = u(event.target)
            while (!element.is('div')) element = element.parent();

            const movieId = parseInt(element.data('movie-id'))
            const movie = movies.find(m => m.Id === movieId)

            modalTitle.text('Edit')
            deleteBtn.attr('href', '/Admin/DeleteMovie/' + movie.Id)

            showMovie(movie)
            showImage(movie.ImageUrl)
            show(deleteBtn)
            toggleClass(deleteBtn.parent(), 'justify-content-between', 'justify-content-end')

            form.attr('action', '/Admin/UpdateMovie/' + movie.Id)
            movieModal.show()
        }))

        u('#btn-paste').on('click', async event => {
            try {
                const imgUrl = await navigator.clipboard.readText()
                new URL(imgUrl) // Validate imgUrl
                showImage(imgUrl)
            } catch (error) {
                console.Error(error)
            }
        })

        const showMovie = movie => {
            u('#id-input').attr('value', movie.Id)
            u('#name-input').attr('value', movie.Name)
            u('#image-input').attr('value', movie.ImageUrl)
            u('#price-input').attr('value', movie.TicketPrice)
        }

        const showImage = imageUrl => {
            u('#image-input').attr('value', imageUrl)
            imgPreview.attr('src', imageUrl)

            show(imgPreview)
            hide(imgHolder)
        }

        const hideImage = () => {
            u('#image-input').attr('value', null)
            imgPreview.attr('src', null)

            hide(imgPreview)
            show(imgHolder)
        }

        const toggleClass = (element, classAdd, classRemove) => element.addClass(classAdd).removeClass(classRemove);
        const show = element => toggleClass(element, 'd-block', 'd-none')
        const hide = element => toggleClass(element, 'd-none', 'd-block')
    </script>
End Section


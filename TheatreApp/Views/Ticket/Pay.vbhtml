@ModelType Payment
@Code
    ViewData("Title") = "Pay"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="d-flex flex-column gap-4 m-auto" style="width: fit-content">
    <div class="position-relative rounded shadow-lg payment-card">
        @Code
            Dim space = "16px"
        End Code
        <img alt="" src="" id="card-type-img" class="position-absolute"
             style="width: 64px; top: @space; right: @space" />
        <span id="card-holder" class="position-absolute text-uppercase fs-4" style="bottom: 64px; left: @space"></span>
        <span id="card-number" class="position-absolute fs-5" style="bottom: @space; left: @space"></span>
    </div>
    <form action="/Ticket/ProcessPayment" method="post">
        @Html.AntiForgeryToken()
        <input type="hidden" name="Id" value="@Model.Id" />
        <div class="mb-3">
            <label for="card-input" class="form-label">Card Holder:</label>
            <input class="form-control text-uppercase" id="name-input" name="CardHolder">
        </div>
        <div class="mb-3">
            <label for="card-input" class="form-label">Card Number:</label>
            <input class="form-control" id="card-input" name="CardNumber">
        </div>
        <div class="mb-3 d-flex justify-content-between" id="info-input">
            <div>
                <label for="month-input" class="form-label">Expiry Month:</label>
                <input class="form-control" id="month-input" type="number" min="1" max="12">
            </div>
            <div>
                <label for="year-input" class="form-label">Expiry Year:</label>
                <input class="form-control" id="year-input" type="number" maxlength="4">
            </div>
            <div>
                <label for="pass-input" class="form-label">CVV / CVC:</label>
                <input class="form-control" id="pass-input" type="number" maxlength="3">
            </div>
        </div>
        <button type="submit" class="btn btn-success w-100" disabled id="btn-submit">Submit</button>
    </form>
</div>

@Section Scripts
    @Scripts.Render("~/bundles/card-validator")
    <script>
        const cardHolder = u('#card-holder')
        const cardNumber = u('#card-number')

        const nameInput = u('#name-input')
        const cardInput = u('#card-input')
        const monthInput = u('#month-input')
        const yearInput = u('#year-input')
        const passInput = u('#pass-input')

        u('input').each(el => u(el).on('input', event => {
            const validName = cardValidator.cardholderName(nameInput.first().value)
            const validCard = cardValidator.number(cardInput.first().value.replaceAll(' ', ''))
            const validMonth = cardValidator.expirationMonth(monthInput.first().value)
            const validYear = cardValidator.expirationYear(yearInput.first().value, 15)
            const validPass = cardValidator.cvv(passInput.first().value, 3)

            const el = u(event.target)
            const val = event.target.value
            if (el.attr('id') === 'name-input') setCardHolder(val)
            if (el.attr('id') === 'card-input') {
                setCardNumber(val.replaceAll(' ', ''))
                setCardLogo(validCard.card.type)
            }

            const validList = [validName, validCard, validMonth, validYear, validPass]
            const invalid = !!validList.find(v => !v.isValid)

            console.log(validList)
            u('#btn-submit').attr('disabled', invalid)
        }))

        const setCardHolder = name => cardHolder.text(name)
        const setCardLogo = cardType => u('#card-type-img').attr('src', '/Static/' + cardType + '.png')
        const setCardNumber = number => {
            let str = "";
            for (const i in number) {
                if (i !== 0 && i % 4 === 0) str += " "
                str += number[i]
            }

            cardNumber.text(str)
        }
    </script>
End Section

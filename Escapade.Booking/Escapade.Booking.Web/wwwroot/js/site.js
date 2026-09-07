document.addEventListener("DOMContentLoaded", function () {

    const calendarContainer = document.getElementById("inline-calendar");
    if (calendarContainer) {
        flatpickr(calendarContainer, {
            inline: true,
            mode: "range",
            minDate: "today",
            locale: {
                firstDayOfWeek: 1
            },
            dateFormat: "Y-m-d",
            onChange: function (selectedDates) {
                const startDateInput = document.getElementById("StartDate");
                const endDateInput = document.getElementById("EndDate");

                if (selectedDates.length === 2 && startDateInput && endDateInput) {
                    startDateInput.value = selectedDates[0].toISOString().split('T')[0];
                    endDateInput.value = selectedDates[1].toISOString().split('T')[0];
                } else if (startDateInput && endDateInput) {
                    startDateInput.value = "";
                    endDateInput.value = "";
                }
            }
        });
    }
});

document.addEventListener("DOMContentLoaded", () => {
    const inputs = document.querySelectorAll('.form-input, .form-control');

    function checkValue(input) {
        if (input.value.trim() !== "") {
            input.classList.add('is-filled');
        } else {
            input.classList.remove('is-filled');
        }
    }

    inputs.forEach(input => {
        checkValue(input);
        input.addEventListener('input', () => checkValue(input));
    });
});

function openConfirmationModal() {
    const emailInput = document.getElementById('Email');
    const confirmEmailDisplay = document.getElementById('confirmEmailDisplay');

    const emailValue = emailInput ? emailInput.value.trim() : '';

    if (!emailValue) {
        alert('Vul een geldig e-mailadres in voordat u de aanvraag verstuurt.');
        if (emailInput) emailInput.focus();
        return;
    }

    if (confirmEmailDisplay) {
        confirmEmailDisplay.textContent = emailValue;
    }

    const modalElement = document.getElementById('confirmationModal');
    if (modalElement) {
        const modal = new bootstrap.Modal(modalElement);
        modal.show();
    }
}

function submitForm() {
    const form = document.getElementById('booking');
    if (form) {
        form.submit();
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("booking");
    if (form) {
        form.addEventListener("submit", function (e) {
            e.preventDefault();
            openConfirmationModal();
        });
    }
});

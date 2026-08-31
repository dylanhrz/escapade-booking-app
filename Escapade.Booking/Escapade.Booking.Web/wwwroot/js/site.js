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
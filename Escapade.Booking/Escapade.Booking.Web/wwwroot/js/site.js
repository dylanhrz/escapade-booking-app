document.addEventListener("DOMContentLoaded", function () {

    const calendarContainer = document.getElementById("inline-calendar");
    if (calendarContainer) {
        flatpickr(calendarContainer, {
            inline: true,
            mode: "range",
            minDate: "today",
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
function openLightbox(src) {
    const modal = document.getElementById('lightbox');
    document.getElementById('lightbox-img').src = src;
    modal.showModal();
}

const cursor = document.getElementById('custom-cursor');
const imgContainers = document.querySelectorAll('.img-container');

document.addEventListener('mousemove', (e) => {
    cursor.style.left = `${e.clientX}px`;
    cursor.style.top = `${e.clientY}px`;
});

imgContainers.forEach(container => {
    container.addEventListener('mouseenter', () => cursor.classList.add('active'));
    container.addEventListener('mouseleave', () => cursor.classList.remove('active'));
});

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
    const emailError = document.getElementById('emailValidationError');

    const emailValue = emailInput ? emailInput.value.trim() : '';

    if (!emailValue) {
        if (emailError) emailError.classList.remove('d-none');
        if (emailInput) emailInput.focus();
        return;
    }

    if (emailError) emailError.classList.add('d-none');

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

/* ACCORDION GALLERY */
document.addEventListener("DOMContentLoaded", () => {
    const accordionHeaders = document.querySelectorAll(".accordion-header");

    accordionHeaders.forEach(header => {
        header.addEventListener("click", () => {
            const accordionItem = header.parentElement;
            const isExpanded = header.getAttribute("aria-expanded") === "true";

            document.querySelectorAll(".accordion-item").forEach(item => {
                item.classList.remove("active");
                item.querySelector(".accordion-header").setAttribute("aria-expanded", "false");
            });

            if (!isExpanded) {
                accordionItem.classList.add("active");
                header.setAttribute("aria-expanded", "true");
            }
        });
    });

    const galleries = document.querySelectorAll('.room-gallery');

    galleries.forEach(gallery => {
        const images = gallery.querySelectorAll('.image-wrapper img');
        const prevBtn = gallery.querySelector('.prev-btn');
        const nextBtn = gallery.querySelector('.next-btn');
        let currentIndex = 0;

        function showImage(index) {
            images.forEach(img => img.classList.remove('active'));
            images[index].classList.add('active');
        }

        if (nextBtn && prevBtn) {
            nextBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                currentIndex = (currentIndex + 1) % images.length;
                showImage(currentIndex);
            });

            prevBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                currentIndex = (currentIndex - 1 + images.length) % images.length;
                showImage(currentIndex);
            });
        }
    });
});

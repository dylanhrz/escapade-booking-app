function openLightbox(src) {
    const modal = document.getElementById('lightbox');
    const img = document.getElementById('lightbox-img');
    if (modal && img) {
        img.src = src;
        modal.showModal();
    }
}

document.addEventListener('click', e => {
    const tile = e.target.closest('[data-lightbox]');
    if (tile) openLightbox(tile.dataset.lightbox);
});

document.addEventListener('keydown', e => {
    if (e.key !== 'Enter') return;
    const tile = e.target.closest('[data-lightbox]');
    if (tile) openLightbox(tile.dataset.lightbox);
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
    if (modalElement && typeof bootstrap !== 'undefined') {
        const modal = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
        modal.show();
    }
}

function submitForm() {
    const form = document.getElementById('booking');
    if (form) form.submit();
}

document.addEventListener("DOMContentLoaded", function () {

    const cursor = document.getElementById('custom-cursor');
    if (cursor) {
        let mouseX = 0, mouseY = 0;
        let ticking = false;

        document.addEventListener('mousemove', (e) => {
            mouseX = e.clientX;
            mouseY = e.clientY;

            if (!ticking) {
                requestAnimationFrame(() => {
                    cursor.style.left = `${mouseX}px`;
                    cursor.style.top = `${mouseY}px`;
                    ticking = false;
                });
                ticking = true;
            }
        });

        document.addEventListener('mouseover', (e) => {
            if (e.target.closest('.img-container')) {
                cursor.classList.add('active');
            }
        });

        document.addEventListener('mouseout', (e) => {
            if (e.target.closest('.img-container')) {
                cursor.classList.remove('active');
            }
        });
    }

    const calendarContainer = document.getElementById("inline-calendar");
    if (calendarContainer && typeof flatpickr !== 'undefined') {
        flatpickr(calendarContainer, {
            inline: true,
            mode: "range",
            minDate: "today",
            locale: { firstDayOfWeek: 1 },
            dateFormat: "Y-m-d",
            onChange: function (selectedDates, dateStr, instance) {
                const startDateInput = document.getElementById("StartDate");
                const endDateInput = document.getElementById("EndDate");

                if (startDateInput && endDateInput) {
                    if (selectedDates.length === 2) {
                        startDateInput.value = instance.formatDate(selectedDates[0], "Y-m-d");
                        endDateInput.value = instance.formatDate(selectedDates[1], "Y-m-d");
                    } else {
                        startDateInput.value = "";
                        endDateInput.value = "";
                    }
                }
            }
        });
    }

    const inputs = document.querySelectorAll('.form-input, .form-control');
    const checkValue = (input) => {
        input.classList.toggle('is-filled', input.value.trim() !== "");
    };

    inputs.forEach(input => {
        checkValue(input);
        input.addEventListener('input', () => checkValue(input));
    });

    const bookingForm = document.getElementById("booking");
    if (bookingForm) {
        bookingForm.addEventListener("submit", function (e) {
            e.preventDefault();
            openConfirmationModal();
        });
    }

    const accordionHeaders = document.querySelectorAll(".accordion-header");
    accordionHeaders.forEach(header => {
        header.addEventListener("click", () => {
            const accordionItem = header.parentElement;
            const isExpanded = header.getAttribute("aria-expanded") === "true";

            document.querySelectorAll(".accordion-item").forEach(item => {
                item.classList.remove("active");
                const itemHeader = item.querySelector(".accordion-header");
                if (itemHeader) itemHeader.setAttribute("aria-expanded", "false");
            });

            if (!isExpanded) {
                accordionItem.classList.add("active");
                header.setAttribute("aria-expanded", "true");
            }
        });
    });

    const initCarousels = (selector, imgSelector) => {
        document.querySelectorAll(selector).forEach(container => {
            const images = container.querySelectorAll(imgSelector);
            const prevBtn = container.querySelector('.prev-btn');
            const nextBtn = container.querySelector('.next-btn');

            if (images.length <= 1) {
                if (prevBtn) prevBtn.style.display = "none";
                if (nextBtn) nextBtn.style.display = "none";
                return;
            }

            let currentIndex = 0;

            const showImage = (index) => {
                images.forEach((img, i) => {
                    img.classList.toggle('active', i === index);
                });
            };

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
    };

    initCarousels('.room-gallery', '.image-wrapper img');
    initCarousels('.room-carousel', '.carousel-img');

    const topBtn = document.getElementById("scrollToTopBtn");
    if (topBtn) {
        window.addEventListener("scroll", function () {
            topBtn.classList.toggle("visible", window.scrollY > 400);
        });
    }

    const track = document.querySelector(".ticker-track");
    if (track) {
        const originalHTML = track.innerHTML;
        while (track.offsetWidth < window.innerWidth * 2) {
            track.innerHTML += originalHTML;
        }
        track.innerHTML += track.innerHTML;
    }

});
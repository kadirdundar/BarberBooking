window.bookingInterop = {
    scroll: function (element, direction) {
        if (element) {
            const scrollAmount = 200; // Adjust as needed
            element.scrollBy({
                left: direction === 'left' ? -scrollAmount : scrollAmount,
                behavior: 'smooth'
            });
        }
    },
    enableDragScroll: function (element) {
        if (!element) return;

        let isDown = false;
        let startX;
        let scrollLeft;

        element.addEventListener('mousedown', (e) => {
            isDown = true;
            element.classList.add('active');
            startX = e.pageX - element.offsetLeft;
            scrollLeft = element.scrollLeft;
        });

        element.addEventListener('mouseleave', () => {
            isDown = false;
            element.classList.remove('active');
        });

        element.addEventListener('mouseup', () => {
            isDown = false;
            element.classList.remove('active');
        });

        element.addEventListener('mousemove', (e) => {
            if (!isDown) return;
            e.preventDefault();
            const x = e.pageX - element.offsetLeft;
            const walk = (x - startX) * 2; //scroll-fast
            element.scrollLeft = scrollLeft - walk;
        });
    },
    scrollToElement: function (element, elementId) {
        if (element) {
            const target = document.getElementById(elementId);
            if (target) {
                target.scrollIntoView({ behavior: 'smooth', inline: 'center', block: 'nearest' });
            }
        }
    }
};
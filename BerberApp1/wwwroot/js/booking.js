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
    },
    copyToClipboard: async function (text) {
        try {
            await navigator.clipboard.writeText(text);
            return true;
        } catch (err) {
            // Fallback for older browsers or non-secure contexts
            const textArea = document.createElement("textarea");
            textArea.value = text;
            textArea.style.position = "fixed";
            document.body.appendChild(textArea);
            textArea.focus();
            textArea.select();
            try {
                document.execCommand('copy');
                document.body.removeChild(textArea);
                return true;
            } catch (fallbackErr) {
                console.error('Fallback copy failed: ', fallbackErr);
                document.body.removeChild(textArea);
                return false;
            }
        }
    },
    shareLink: async function (title, text, url) {
        if (navigator.share) {
            try {
                await navigator.share({
                    title: title,
                    text: text,
                    url: url
                });
                return true;
            } catch (err) {
                console.log('User cancelled or sharing failed: ', err);
                return false;
            }
        }
        return false;
    },
    cropperInstance: null,
    initCropper: function (imageElementId, aspectRatio) {
        // Destroy existing cropper if active
        if (window.bookingInterop.cropperInstance) {
            window.bookingInterop.cropperInstance.destroy();
            window.bookingInterop.cropperInstance = null;
        }

        const image = document.getElementById(imageElementId);
        if (!image) {
            console.error("Image element not found for Cropper:", imageElementId);
            return;
        }

        window.bookingInterop.cropperInstance = new Cropper(image, {
            aspectRatio: aspectRatio ? parseFloat(aspectRatio) : NaN,
            viewMode: 1,
            dragMode: 'move',
            autoCropArea: 0.9,
            restore: false,
            guides: true,
            center: true,
            highlight: false,
            cropBoxMovable: true,
            cropBoxResizable: true,
            toggleDragModeOnDblclick: false,
        });
    },
    getCroppedImage: function (targetWidth, targetHeight) {
        if (!window.bookingInterop.cropperInstance) {
            console.error("No active Cropper instance found.");
            return null;
        }

        const canvasOptions = {};
        if (targetWidth && targetHeight) {
            canvasOptions.width = parseInt(targetWidth);
            canvasOptions.height = parseInt(targetHeight);
        }

        try {
            return window.bookingInterop.cropperInstance.getCroppedCanvas(canvasOptions).toDataURL('image/jpeg', 0.9);
        } catch (e) {
            console.error("Failed to get cropped canvas:", e);
            return null;
        }
    },
    destroyCropper: function () {
        if (window.bookingInterop.cropperInstance) {
            window.bookingInterop.cropperInstance.destroy();
            window.bookingInterop.cropperInstance = null;
        }
    },
    saveSessionState: function (key, stateJson) {
        sessionStorage.setItem(key, stateJson);
    },
    getSessionState: function (key) {
        return sessionStorage.getItem(key);
    },
    clearSessionState: function (key) {
        sessionStorage.removeItem(key);
    }
};
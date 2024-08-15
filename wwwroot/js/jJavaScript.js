document.addEventListener("DOMContentLoaded", function () {
    const themeToggleButton = document.getElementById("bd-theme");
    const themeText = document.getElementById("bd-theme-text");
    const themeButtons = document.querySelectorAll("[data-bs-theme-value]");
    const themeIcon = document.querySelector(".theme-icon-active use");

    // Function to set the theme
    function setTheme(theme) {
        document.documentElement.setAttribute("data-bs-theme", theme);
        themeButtons.forEach((button) => {
            const isActive = button.getAttribute("data-bs-theme-value") === theme;
            button.classList.toggle("active", isActive);
            button.setAttribute("aria-pressed", isActive);
            button.querySelector(".ms-auto").classList.toggle("d-none", !isActive);
        });

        // Update the SVG icon
        let iconHref;
        switch (theme) {
            case "light":
                iconHref = "#sun-fill";
                break;
            case "dark":
                iconHref = "#moon-stars-fill";
                break;
            case "auto":
                iconHref = "#circle-half";
                break;
            default:
                iconHref = "#sun-fill";
        }
        themeIcon.setAttribute("href", iconHref);
    }

    // Event listener for theme buttons
    themeButtons.forEach((button) => {
        button.addEventListener("click", function () {
            const theme = this.getAttribute("data-bs-theme-value");
            setTheme(theme);
            localStorage.setItem("theme", theme);
        });
    });

    // Initialize theme based on saved preference or default to light
    const savedTheme = localStorage.getItem("theme") || "light";
    setTheme(savedTheme);
});

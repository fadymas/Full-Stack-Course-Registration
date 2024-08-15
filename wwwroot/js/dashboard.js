document.addEventListener("DOMContentLoaded", function () {
  const themeToggleButton = document.getElementById("bd-theme");
  const themeText = document.getElementById("bd-theme-text");
  const themeButtons = document.querySelectorAll("[data-bs-theme-value]");

    // Function to set the theme
    debugger;
  function setTheme(theme) {
    document.documentElement.setAttribute("data-bs-theme", theme);
    themeButtons.forEach((button) => {
      const isActive = button.getAttribute("data-bs-theme-value") === theme;
      button.classList.toggle("active", isActive);
      button.setAttribute("aria-pressed", isActive);
      button.querySelector(".ms-auto").classList.toggle("d-none", !isActive);
    });
  }

  // Event listener for theme buttons
  themeButtons.forEach((button) => {
    button.addEventListener("click", function () {
      const theme = this.getAttribute("data-bs-theme-value");
      setTheme(theme);
    });
  });

  // Initialize theme based on saved preference or default to light
  const savedTheme = localStorage.getItem("theme") || "light";
  setTheme(savedTheme);

  // Save theme preference to localStorage
  themeButtons.forEach((button) => {
    button.addEventListener("click", function () {
      const theme = this.getAttribute("data-bs-theme-value");
      localStorage.setItem("theme", theme);
    });
  });
});
$("#createstudent").submit(function (e) {
    
});

    let date_of_birth = $("#date").val();

    try {
        date_of_birth = parseInt(date_of_birth);
    } catch (error) {
        alert("Invalid date of birth");
    }
    if (date_of_birth <= 1999 || date_of_birth >= 2008) {
        alert("you must be 24 years old at least");
        e.preventDefault();
    }

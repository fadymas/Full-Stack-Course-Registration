$(document).ready(function () {
  $(".main").addClass("visible");
  $(".smain").addClass("visible");
  $(".formain").addClass("visible");
});

$("#signupform").on("input", function () {
  if ($("#nameerror").hasClass("field-validation-error")) {
    $("#name").removeClass("email");
  }
  let pass = $("#password").val();
  let email = $("#email").val();

  if (email === "") {
    $("#email").addClass("email");
  } else {
    $("#email").removeClass("email");
  }

  if ($("#passworderror").hasClass("field-validation-valid")) {
    if (pass === "") {
      $("#password").addClass("password");
    } else if ($("#password-error").val() == undefined) {
      $("#password").removeClass("password");
    }
  } else if (pass === "") {
    $("#password").addClass("password");
  } else {
    $("#password").addClass("password");
  }

  // if (email === "") {
  //     $("#email").addClass("email");
  // } else {
  //     $("#email").removeClass("email");
  // }
  let confpass = $("#confpass").val();
  if (pass === confpass && confpass !== "") {
    $("#confpass").removeClass("confpass");
  } else {
    $("#confpass").addClass("confpass");
  }
  let dropdown = $("#dropdown").val();
  if (dropdown !== null) {
    $("#dropdown").removeClass("dropdown");
  } else {
    $("#dropdown").addClass("dropdown");
  }
});
$("#resetpass").submit(function (e) {
  let confpass = $("#confpass").val();
  let pass = $("#password").val();
  if (pass !== confpass) {
    alert(" The Passwords did not Match");
    e.preventDefault();
  }
});
$("#resetpass").on("input", function (e) {
  let pass = $("#password").val();
  let confpass = $("#confpass").val();
  if (pass === confpass && confpass !== "") {
    $("#confpass").removeClass("confpass");
  } else {
    $("#confpass").addClass("confpass");
  }
});
$("#signupform").submit(function (e) {
  if ($("#passworderror").hasClass("field-validation-valid")) {
    $("#password").removeClass("password");
  }

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
});
$("#loginform").submit(function (e) {
  const Email = $(".email").val();

  const Password = $(".password").val();

  if (toString(Email) == "" && toString(Password) == "") {
    alert("Login Failure");
    e.preventDefault();
  }
});

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

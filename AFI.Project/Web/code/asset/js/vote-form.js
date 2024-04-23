window.addEventListener("DOMContentLoaded", () => {
  // remove db-content class
  const dvContent = document.querySelector(".vote-login-form").closest(".dv-content");
  dvContent?.classList.remove("dv-content");

  // logo replacement
  const logo = document.querySelector("#logo_container img");
  const logoContainer = document.querySelector(".left__side .logo-container");

  logoContainer?.appendChild(logo);
});

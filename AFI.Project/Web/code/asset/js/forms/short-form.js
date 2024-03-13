window.addEventListener("load", () => {
  const shortForm = document.getElementById("short-form");

  // *********************  Functionality *********************
  // *********************  Functionality *********************
  const referPhone = shortForm && shortForm?.querySelector("#phone");
  if (referPhone) phoneNumberPattern(referPhone); // phoneNumberPattern comes from formCommon.js file's

  const showFieldError = (selector, msg = "This field is required.") => {
    const fieldWrapper = selector.closest(".field");
    fieldWrapper?.classList.add("has-errors");

    const errorField = fieldWrapper?.querySelector(".validation-field__errors");
    if (errorField) errorField.innerHTML = msg;
  };

  // Email validation
  function shortEmailVal(selector) {
    const regEx =
      /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (regEx.test(selector?.value)) {
      return true;
    } else {
      showFieldError(selector, "Please enter a valid email address");
      return false;
    }
  }

  // Phone Number validation
  function shortPhoneVal(selector) {
    const regEx = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;
    if (regEx.test(selector?.value)) {
      return true;
    } else {
      showFieldError(selector, "Please enter a valid phone number");
      return false;
    }
  }

  // isFilled validation
  function isFilled(selector) {
    if (selector?.value) {
      return true;
    } else {
      showFieldError(selector, "This field is required.");
      return false;
    }
  }

  // remove error messages on change
  shortForm?.querySelectorAll(".field")?.forEach((fieldWrapper) => {
    const removeFieldError = () => {
      fieldWrapper?.classList.remove("has-errors");

      const errorField = fieldWrapper?.querySelector(".validation-field__errors");
      if (errorField) errorField.innerHTML = "";
    };

    fieldWrapper?.querySelector(".input")?.addEventListener("input", removeFieldError);
  });

  // *********************  Validation *********************
  // *********************  Validation *********************

  function shortFormValidation() {
    let result = {};

    // Fill Validation Check
    const fields = shortForm?.querySelectorAll(".input.field__input");
    fields.forEach((field) => {
      if (field.type === "email") {
        const isEmail = shortEmailVal(field);
        if (!isEmail) result = false;
      }

      if (field.name.toLocaleLowerCase() === "phone") {
        const isPhone = shortPhoneVal(field);
        if (!isPhone) result = false;
      }

      if (field.required) {
        const isFill = isFilled(field);
        if (!isFill) result = false;
      }

      // add finally store value if no error
      result[field.name] = field.value;
    });

    // token
    result.token = document.querySelector("input[name=token]")?.value;

    return result;
  }

  //  ********************* Submit *********************
  //  ********************* Submit *********************
  async function shortFormSubmit(e) {
    e.preventDefault();

    const result = shortFormValidation(); // return data or false
    if (!result) return;

    // if all OK
    try {
      btnSubmitting(true);

      const url = "/api/sitecore/ShortForm/Index";

      const options = {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(result),
      };

      const res = await fetch(url, options);
      if (!res.ok) throw new Error("Something is wrong! Please try again.");

      const data = await res.json();
      if (!data?.success) throw new Error("Something is wrong! Please try again.");
      if (data?.success === "failed") throw new Error("Something is wrong! Please try again.");

      // clear fields value
      const clearFieldsEl = document.querySelectorAll(".input.field__input");
      clearFieldsEl?.forEach((field) => (field.value = ""));

      // Redirect to thank you page
      window.location.href = "/short-form-confirmation";
    } catch (error) {
      showResponseErrorMessage();
    } finally {
      btnSubmitting(false);
    }
  }

  // Event Trigger for Submit
  const shortFormEl = document.querySelector(".short-form__form");
  if (shortFormEl) {
    shortFormEl.noValidate = true;
    shortFormEl?.addEventListener("submit", shortFormSubmit);
  }
});

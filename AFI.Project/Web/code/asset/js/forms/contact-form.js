window.addEventListener("DOMContentLoaded", () => {
  const contactForm = document.getElementById("contact-us");
  if (!contactForm) return;
  const formToggle = contactForm.querySelector(".js-formtoggle-wrapper");

  // *********************  Functionality *********************
  // Submit button functionality
  function btnSubmitting(isSubmitting) {
    const submitBtn = contactForm.querySelector(".js-submit-button");

    if (isSubmitting === true) {
      submitBtn.innerText = "Submitting";
      submitBtn.disabled = true;
    } else {
      submitBtn.innerText = "Submit";
      submitBtn.disabled = false;
    }
  }

  // phone pattern
  const contactPhone = contactForm?.querySelector(".phone");
  if (contactPhone) phoneNumberPattern(contactPhone); // phoneNumberPattern comes from formCommon.js file's

  // submitAgainBtn button functionality
  const submitAgainBtn = document.querySelector(".js-submitAgain-button");
  submitAgainBtn?.addEventListener("click", () => {
    const messageDiv = submitAgainBtn.closest(".message");
    if (messageDiv) messageDiv.style.display = "none";
    formToggle.style.display = "flex";
  });

  // reset field on change
  contactForm?.querySelectorAll(".field")?.forEach((fieldWrapper) => {
    const removeFieldError = () => {
      const errorField = fieldWrapper?.querySelector(".field_message");
      errorField?.classList.remove("error");
      fieldWrapper?.classList.remove("has-errors");
    };

    fieldWrapper
      ?.querySelectorAll(".field__input")
      .forEach((inputField) => inputField?.addEventListener("input", removeFieldError));
  });

  // *********************  Validation *********************
  function contactFormValidation() {
    const isValid = validateForm("contact-us-form", false); // validateForm comes from formCommon.js file's

    if (!isValid) {
      // If not valid
      const errorMsgs = document.querySelectorAll(".field_message.error");
      errorMsgs.forEach((errMsg) => errMsg.closest(".field").classList.add("has-errors"));

      return false;
    }

    // After validation Pass
    const tokenValue = contactForm.querySelector(".form__token")?.value;
    const formData = {
      token: tokenValue,
    };

    const InputFields = contactForm.querySelectorAll(".field__input");
    InputFields.forEach((field) => (formData[field.name] = field.value));

    return formData;
  }

  //  ********************* Submit *********************
  async function contactFormSubmit(e) {
    e.preventDefault();

    const result = contactFormValidation(); // return json or false
    if (!result) return;

    const submitErrMsg = document.querySelector(".js-submit-message .is-error");
    const submitSuccessMsg = document.querySelector(".js-submit-message .is-success");

    // if all OK
    try {
      btnSubmitting(true);

      const url = "/api/contactUs";

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
      if (!data?.success) throw new Error(data?.message);

      // show success message and hide form and error message
      submitSuccessMsg.style.display = "block";
      submitErrMsg.style.display = "none";
      formToggle.style.display = "none";

      // clear subject & message fields
      const clearFieldsEl = document.querySelectorAll(".js-clearoncomplete-input");
      clearFieldsEl.forEach((field) => (field.value = ""));

      // TRIGGER reCaptcha TO GET NEW TOKEN
      const form = document.querySelector(".js-recaptcha-form");
      const formWithRecaptcha = recaptchaForm(form);
      formWithRecaptcha.executeRecaptcha();
    } catch (error) {
      // error message
      submitErrMsg.textContent = error.message;
      submitErrMsg.style.display = "block";
    } finally {
      btnSubmitting(false);
    }
  }
  contactForm?.addEventListener("submit", contactFormSubmit);
});

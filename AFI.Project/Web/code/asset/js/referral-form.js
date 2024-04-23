window.addEventListener("DOMContentLoaded", () => {
  const referForm = document.getElementById("referForm");

  // *********************  Functionality *********************
  // *********************  Functionality *********************
  const referPhone = referForm && referForm?.querySelector("#phone");
  if (referPhone) phoneNumberPattern(referPhone); // phoneNumberPattern comes from formCommon.js file's

  const showFieldError = (selector, msg = "This field is required.") => {
    const fieldWrapper = selector.closest(".field");
    fieldWrapper?.classList.add("has-errors");

    const errorField = fieldWrapper?.querySelector(".validation-field__errors");
    errorField.innerHTML = msg;
  };

  // Email validation
  function referEmailVal(selector) {
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
  function referPhoneVal(selector) {
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

  // Input Alphabet Only at name
  function nameValidation(input) {
    if (!input) return;

    input?.addEventListener("input", (e) => {
      e.target.value = e.target?.value.replace(/[^a-zA-Z\']+/g, ""); // /[^a-zA-z]/g
    });
  }

  nameValidation(referForm.querySelector("#first-name"));
  nameValidation(referForm.querySelector("#last-name"));
  nameValidation(referForm.querySelector("#member-first-name"));
  nameValidation(referForm.querySelector("#member-first-name"));
  nameValidation(referForm.querySelector("#member-last-name"));

  // check error is field parents
  referForm?.querySelectorAll(".field")?.forEach((fieldWrapper) => {
    const removeFieldError = () => {
      fieldWrapper?.classList.remove("has-errors");

      const errorField = fieldWrapper?.querySelector(".validation-field__errors");
      if (errorField) errorField.innerHTML = "";
    };

    fieldWrapper?.querySelector(".input")?.addEventListener("input", removeFieldError);
  });

  // TOGGLE "YOUR INFORMATION" FIELDS
  const memNumber = referForm?.querySelector(".refer-a-member__number-fields");
  const memName = referForm?.querySelector(".refer-a-member__name-fields");

  const notNumBtn = referForm?.querySelector(".not__memNumber_btn");
  const hasNumBtn = referForm?.querySelector(".has__memNumber_btn");

  notNumBtn?.addEventListener("click", () => {
    memNumber?.classList.add("is-hide");
    memNumber?.querySelectorAll(".input-field").forEach((field) => (field.disabled = true));

    memName?.classList.remove("is-hide");
    memName?.querySelectorAll(".input-field").forEach((field) => (field.disabled = false));
  });

  hasNumBtn?.addEventListener("click", () => {
    memNumber?.classList.remove("is-hide");
    memNumber?.querySelectorAll(".input-field").forEach((field) => (field.disabled = false));

    memName?.classList.add("is-hide");
    memName?.querySelectorAll(".input-field").forEach((field) => (field.disabled = true));
  });

  // TOGGLE "contact info" INFORMATION" FIELDS
  const radioFields = referForm?.querySelectorAll(".input.input-radio");
  const emailField = referForm?.querySelector(".field.js-email-field");
  const numberField = referForm?.querySelector(".field.js-phone-field");

  radioFields?.forEach((radio) => {
    radio?.addEventListener("change", () => {
      const radioValue = referForm?.querySelector(".input.input-radio:checked").value;

      if (radioValue === "email") {
        numberField?.classList.add("is-hide");
        numberField?.querySelectorAll(".input-field").forEach((field) => (field.disabled = true));

        emailField?.classList.remove("is-hide");
        emailField?.querySelectorAll(".input-field").forEach((field) => (field.disabled = false));
      } else {
        emailField?.classList.add("is-hide");
        emailField?.querySelectorAll(".input-field").forEach((field) => (field.disabled = true));

        numberField?.classList.remove("is-hide");
        numberField?.querySelectorAll(".input-field").forEach((field) => (field.disabled = false));
      }
    });
  });

  // Rank Functionality
  const referRank = referForm?.querySelector("#rank-field");
  const referPrefix = referForm?.querySelector(".js-prefix-input");

  if (referRank) {
    referRank.disabled = true;
    referPrefix.addEventListener("change", (e) => (referRank.disabled = e.target.value !== "Rank"));
  }

  // *********************  Validation *********************
  // *********************  Validation *********************

  function referFormValidation() {
    let result = {
      scController: "Forms",
      scAction: "SubmitReferralForm",
    };

    // Fill Validation Check
    const fields = referForm?.querySelectorAll(".input.input-field");
    fields.forEach((field) => {
      const isDisabled = field.disabled;

      if (!isDisabled) {
        if (field.type === "email") {
          const isEmail = referEmailVal(field);
          if (!isEmail) result = false;
        }

        if (field.name === "phone") {
          const isPhone = referPhoneVal(field);
          if (!isPhone) result = false;
        }

        const isFill = isFilled(field);
        if (!isFill) result = false;

        // add finally store value if no error
        result[field.name] = field.value;
      }
    });

    // bestContactType
    const bestContactValue = document.querySelector(".input[name=bestContactType]:checked")?.value;
    result.bestContactType = bestContactValue;

    // token
    result.token = document.querySelector("input[name=token]")?.value;

    return result;
  }

  //  ********************* Submit *********************
  //  ********************* Submit *********************
  async function referFormSubmit(e) {
    e.preventDefault();

    const result = referFormValidation(); // return data or false
    if (!result) return;

    // if all OK
    try {
      btnSubmitting(true);
      const url = "/api/sitecore/AFIReport/SubmitReferralForm";

      const options = {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(result),
      };

      // AJAX call
      const res = await fetch(url, options);
      if (!res.ok) throw new Error("Something is wrong! Please try again.");

      const data = await res?.json();

      // // check Success from response
      // if (!data?.Success) {
      //   showResponseErrorMessage(data?.Message);
      //   btnSubmitting(false);
      //   return console.error(data?.Message);
      // }

      // clear subject & message fields
      const clearFieldsEl = document.querySelectorAll(".input.input-field");
      clearFieldsEl?.forEach((field) => (field.value = ""));

      // Redirect to thank you page
      window.location.href = "/referral-form-submission-page";
    } catch (error) {
      showResponseErrorMessage();
    } finally {
      btnSubmitting(false);
    }
  }

  // Event Trigger for Submit
  const referFormEl = document.querySelector(".refer-a-member__form");
  if (referFormEl) {
    referFormEl.noValidate = true;
    referFormEl?.addEventListener("submit", referFormSubmit);
  }
});

// *********************************************
//       FORM SUBMISSION AND STEP HANDLING
// *********************************************

window.addEventListener("DOMContentLoaded", () => {
  const lead_next_btn = document.querySelector("#lead_next_btn");
  const lead_back_btn = document.querySelector("#lead_back_btn");

  if (!lead_next_btn) return;
  pressEnterToSubmit(lead_next_btn);

  // *********************************************
  //         LEAD FORM SUBMISSION
  // *********************************************
  lead_next_btn?.addEventListener("click", async () => {
    if (!validateForm("lead__form")) return false;

    const leadCheckbox = document.querySelector(".leadSameAsMailing--true");
    formData[leadCheckbox.name] = leadCheckbox.checked;

    // Save Data
    const resResult = await saveLeadForm("lead__form", "submit");
    if (!resResult) return false;

    // alert("Submit");
    // Go to Thank You Page
    window.location.href = leadSuccessRedirection;
  });

  async function saveLeadForm(form, action = "submit", addressValid = null) {
    const resData = await saveData("/sc-api/forms/save-quotelead", formData, lead_next_btn, form, action, addressValid);

    if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;

    return resData;
  }

  // *********************************************
  //       SAME MAILING FUNCTIONALITY
  // *********************************************
  const leadSameAsMailingTrue = document.querySelector(".leadSameAsMailing--true");

  const leadFormFieldsClient = document.querySelectorAll(".lead__form .field__input.property__field");
  const leadFormFieldsProperty = document.querySelectorAll(".lead__form .field__input.client__field");

  function setMatchedData(disability) {
    leadFormFieldsProperty?.forEach((element) => {
      const elementMatch = element.getAttribute("data-match");

      leadFormFieldsClient.forEach((el) => {
        const elMatch = el.getAttribute("data-match");

        if (elementMatch && elMatch === elementMatch) el.value = element.value;
        el.disabled = disability;
        leadSameAsMailingTrue.disabled = false;
      });
    });
  }

  // Same Mailing CheckBox Functionality
  leadSameAsMailingTrue?.addEventListener("change", () => {
    if (leadSameAsMailingTrue.checked) {
      setMatchedData(true);

      leadFormFieldsClient.forEach((formField) => {
        formField.classList.remove("required");

        formField.parentElement.querySelector(".field_message.error")?.classList.remove("error");
      });

      document.getElementById("policyHolderPrimaryResidenceZip").classList.remove("zip");
    } else {
      leadFormFieldsClient.forEach((el, i) => {
        el.value = "";
        el.disabled = false;

        document.querySelector(".leadPropertyAddress").focus();

        if (el.id !== "propertyAddress2") el.classList.add("required");
      });

      document.getElementById("policyHolderPrimaryResidenceZip").classList.add("zip");
    }
  });

  // onchange of client fields, uncheck checkbox for same mailing address
  leadFormFieldsProperty?.forEach((clientField) => {
    clientField?.addEventListener("change", () => {
      leadSameAsMailingTrue.checked = false;

      leadFormFieldsClient.forEach((el) => {
        el.disabled = false;
        if (el.id !== "propertyAddress2") el.classList.add("required");
      });

      document.getElementById("policyHolderPrimaryResidenceZip").classList.add("zip");
    });
  });
});

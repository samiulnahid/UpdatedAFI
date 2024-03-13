window.addEventListener("DOMContentLoaded", () => {
  // Forms
  const floodFormSteps = ["policyholder_form", "property_quoted_form", "property_overview_form", "property_details_form"];

  let floodStep = 0;
  let floodMaxStep = formList?.length - 1;

  const floodNextBtn = document.querySelector("#floodNextBtn");
  const floodBackBtn = document.querySelector("#floodBackBtn");

  // *********************************************
  //       FORM SUBMISSION AND STEP HANDLING
  // *********************************************

  // ***** NEXT FUNCTIONALITY *****
  pressEnterToSubmit(floodNextBtn);
  floodNextBtn?.addEventListener("click", async () => {
    if (floodStep === 0) {
      const isSelectEligibility = eligibilityValidation(floodFormSteps);
      if (!Boolean(isSelectEligibility)) return false;

      militaryFormFunc();
    }

    //  HANDLE ALL FORM SUBMISSIONS AND STEP VALIDATION
    const submitResult = await handleFloodForms(floodStep);
    if (!submitResult) return false;

    // Step Increment
    floodMaxStep = formList?.length - 1;
    floodStep >= floodMaxStep ? floodStep : floodStep++;

    // Show Form
    showActiveForm(floodStep, floodBackBtn);
  });

  // Back
  floodBackBtn?.addEventListener("click", () => {
    // Step Decrement
    floodStep <= 0 ? floodStep : floodStep--;

    showActiveForm(floodStep, floodBackBtn);
  });

  // =*********************************************
  //       HANDLING MULTI-STEP FORMS
  // =*********************************************
  async function handleFloodForms(step) {
    // =*********************************************************
    if (step === formList?.indexOf("military_information")) {
      if (!militaryValidation()) return false;
    }

    if (step === formList?.indexOf("parent_information")) {
      if (!validateForm("parent_information")) return false;
    }

    if (step === formList?.indexOf("child_information")) {
      if (!validateForm("child_information")) return false;
    }

    if (step === formList?.indexOf("policyholder_form")) {
      if (!policyholderValidation(step, false)) return false;

      // Save Data
      const resData = await saveFlood("policyholder_form", "send", "flood");
      //const resData = await saveFlood("policyholder_form");
      if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;

      floodPropertyQuotedFormFunc();
    }
    if (step === formList?.indexOf("spouse_information")) {
      if (!validateForm("spouse_information")) return false;

      // Save Data
      const resData = await saveFlood("spouse_information");
      if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;
    }

    if (step === formList?.indexOf("property_quoted_form")) {
      if (!floodPropertyQuotedValidation()) return false;

      // Save Data
      const resData = await saveFlood("property_quoted_form", "send", "flood");
      if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;

      floodOverviewFunc();
    }

    if (step === formList?.indexOf("property_overview_form")) {
      if (!floodOverviewValidation()) return false;

      // Save Data
      const resData = await saveFlood("property_overview_form");
      if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;

      floodDetailsFunc();
    }

    //
    if (step === formList?.indexOf("property_details_form")) {
      if (!floodDetailsValidation()) return false;

      // Save Data
      const resData = await saveFlood("property_details_form", "submit");
      if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;

      // Go to Thank You Page
      window.location.href = floodSuccessRedirection;
    }

    return true;
  }

  async function saveFlood(form, action = "send", addressValid = null) {
    const resData = await saveData("/sc-api/forms/save-flood", formData, floodNextBtn, form, action, addressValid);

    return resData;
  }

  // *********************************************
  //             STEP-1 VALIDATION
  // *********************************************

  // Policy Holder validation from formCommon.js (policyholderValidation)

  // Spouse validate by default validateForm in handleFloodForms function

  // *********************************************
  // STEP-2 "Property to be Quoted" FUNCTIONALITY & VALIDATION
  // *********************************************
  const isFloodSameAddressEl = document.getElementById("propertyAddressSameAsMailing--true");

  function floodPropertyQuotedFormFunc() {
    isFloodSameAddressEl.checked = false;
    //
    const floodQuotedMatchEl = document.querySelectorAll(".property_quoted_form .field__input");

    function setMatchedData(disability) {
      const floodHolderMatchEl = document.querySelectorAll(".policyholder_form .field__input");

      floodHolderMatchEl.forEach((element) => {
        const elementMatch = element.getAttribute("data-match");

        floodQuotedMatchEl.forEach((el) => {
          const elMatch = el.getAttribute("data-match");

          if (elMatch === elementMatch && elMatch && elementMatch) el.value = element.value;
          el.disabled = disability;
          isFloodSameAddressEl.disabled = false;

          // clear error if checked true
          if (disability) {
            removeErrorOnDisabled(el);
          }
        });
      });
    }

    setMatchedData(false);
    document.getElementById("propertyAddress").value = "";

    // Same Mailing CheckBox Functionality
    isFloodSameAddressEl?.addEventListener("change", () => {
      if (isFloodSameAddressEl.checked) {
        setMatchedData(true);
      } else {
        floodQuotedMatchEl.forEach((el, i) => {
          el.value = "";
          el.disabled = false;

          if (i === 1) el.focus();
        });
      }
    });
  }

  function floodPropertyQuotedValidation() {
    const isValidate = validateForm("property_quoted_form");
    if (isFloodSameAddressEl?.checked) {
      formData[isFloodSameAddressEl.name] = true;
      return true;
    } else {
      formData[isFloodSameAddressEl.name] = false;
    }
    return isValidate;
  }

  // *********************************************
  // STEP-2 "Property Overview" FUNCTIONALITY & VALIDATION
  // *********************************************

  function floodOverviewFunc() {
    const awareOfFloodLossesOnProperty = document.querySelector(".field__input[name=awareOfFloodLossesOnProperty]:checked");

    const howManyLossesHaveOccurred = document.getElementById("howManyLossesHaveOccurred");

    document.querySelectorAll(".field__input[name=awareOfFloodLossesOnProperty]").forEach((field) => {
      field.addEventListener("change", () => {
        if (field.checked && field?.value === "Yes") {
          howManyLossesHaveOccurred.disabled = false;
          howManyLossesHaveOccurred?.classList.add("required");
        } else {
          howManyLossesHaveOccurred.disabled = true;
          howManyLossesHaveOccurred?.classList.remove("required");
          removeErrorOnDisabled(howManyLossesHaveOccurred);
        }
      });
    });
  }

  function floodOverviewValidation() {
    const isValidate = validateForm("property_overview_form");

    //
    const awareOfFloodLossesOnProperty = document.querySelector(".field__input[name=awareOfFloodLossesOnProperty]:checked");
    if (!awareOfFloodLossesOnProperty) {
      const awareOfFloodError = document.querySelector(".awareOfFloodError");
      awareOfFloodError.style.display = "block";

      document
        .querySelectorAll(".field__input[name=awareOfFloodLossesOnProperty]")
        .forEach((el) => el.addEventListener("change", () => (awareOfFloodError.style.display = "none")));
    } else {
      formData[awareOfFloodLossesOnProperty?.name] = awareOfFloodLossesOnProperty?.value;
    }

    return isValidate && Boolean(awareOfFloodLossesOnProperty);
  }

  // *********************************************
  // STEP-3 "Property Details" FUNCTIONALITY & VALIDATION
  // *********************************************
  function floodDetailsFunc() {
    const isStructureACondominium = document.querySelector(".field__input[name=isStructureACondominium]:checked");

    const whatFloorIsYourCondominiumOn = document.getElementById("whatFloorIsYourCondominiumOn");

    document.querySelectorAll(".field__input[name=isStructureACondominium]").forEach((field) => {
      field.addEventListener("change", () => {
        if (field.checked && field?.value === "Yes") {
          whatFloorIsYourCondominiumOn.disabled = false;
          whatFloorIsYourCondominiumOn?.classList.add("required");
        } else {
          whatFloorIsYourCondominiumOn.disabled = true;
          whatFloorIsYourCondominiumOn?.classList.remove("required");
          removeErrorOnDisabled(whatFloorIsYourCondominiumOn);
        }
      });
    });

    //
    const garageType = document.getElementById("garageType");
    const garageValue = document.getElementById("garageValue");

    garageType.addEventListener("change", (e) => {
      if (e.target.value === "Detached") {
        garageValue.disabled = false;
        garageValue?.classList.add("required");
      } else {
        garageValue.disabled = true;
        garageValue?.classList.remove("required");
        removeErrorOnDisabled(garageValue);
      }
    });
  }

  function floodDetailsValidation() {
    const isValidate = validateForm("property_details_form");

    //
    const isStructureACondominium = document.querySelector(".field__input[name=isStructureACondominium]:checked");
    if (!isStructureACondominium) {
      const structureACondomError = document.querySelector(".structureACondomError");
      structureACondomError.style.display = "block";

      document
        .querySelectorAll(".field__input[name=isStructureACondominium]")
        .forEach((el) => el.addEventListener("change", () => (structureACondomError.style.display = "none")));
    } else {
      formData[isStructureACondominium?.name] = isStructureACondominium?.value;
    }

    return isValidate && Boolean(isStructureACondominium);
  }
});

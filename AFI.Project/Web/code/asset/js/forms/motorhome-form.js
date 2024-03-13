window.addEventListener("DOMContentLoaded", () => {
  // Forms
  const MHForms = [
    "policyholder_form",
    "add_vehicle_form",
    "driver_summary_form",
    "violations_form",
    "coverage_limits_form",
    "physical_damage_form",
    "coverage_history_form",
  ];

  let MHdriverArr = [];
  let MHdriverId = 0;
  let MHeditDriverIndex = -9999;
  const MHmaxDriverItem = 3;

  // *********************************************
  //       FORM SUBMISSION AND STEP HANDLING
  // *********************************************
  const MHNextBtn = document.querySelector("#MHNextBtn");
  const MHBackBtn = document.querySelector("#MHBackBtn");

  let MHStep = 0;
  let MHMaxStep = formList?.length - 1;

  // ***** NEXT FUNCTIONALITY *****
  pressEnterToSubmit(MHNextBtn);
  MHNextBtn?.addEventListener("click", async () => {
    if (MHStep === 0) {
      const isSelectEligibility = eligibilityValidation(MHForms);
      if (!Boolean(isSelectEligibility)) return false;

      militaryFormFunc();
    }
    //  HANDLE ALL FORM SUBMISSIONS AND STEP VALIDATION
    const submitResult = await handleMotorStepForm(MHStep);
    if (!submitResult) return false;

    // Step Increment
    MHMaxStep = formList?.length - 1;
    MHStep >= MHMaxStep ? MHStep : MHStep++;

    // Show Form
    showActiveForm(MHStep, MHBackBtn);
  });

  // Back
  MHBackBtn?.addEventListener("click", () => {
    // Step Decrement
    MHStep <= 0 ? MHStep : MHStep--;

    // 2 side back for additional_driver form
    if (MHStep + 1 === formList?.indexOf("additional_driver")) {
      formList = formList?.filter((item) => item != "additional_driver");
      MHStep = formList?.indexOf("driver_summary_form");
      MHeditDriverIndex = -9999;
    }
    showActiveForm(MHStep, MHBackBtn);
  });

  // =*********************************************
  //       HANDLING MULTI-STEP FORMS
  // =*********************************************
  async function handleMotorStepForm(step) {
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
      if (!policyholderValidation(step)) return false;
      MHspouseOperatorFunc();

      // Save Data
      const resResult = await saveMotorhome("policyholder_form", "send", "travel trailer");
      // const resResult = await saveMotorhome("policyholder_form");
      if (!resResult) return false;
    }

    if (step === formList?.indexOf("spouse_information")) {
      if (!validateForm("spouse_information")) return false;

      // Save Data
      const resResult = await saveMotorhome("spouse_information");
      if (!resResult) return false;
    }

    if (step === formList?.indexOf("driver_summary_form") || step === formList?.indexOf("driver_summary_form") - 1) {
      MHdriverSummaryFunc();
      MHviolationDriverFunc();
    }

    if (step === formList?.indexOf("additional_driver")) {
      if (!MHaddDriverValidation("additional_driver")) return false;

      updateArrToFormData(MHdriverArr);

      // Save Data
      const resResult = await saveMotorhome("additional_driver");
      if (!resResult) return false;

      MHdriverSummaryFunc();
    }

    // add_vehicle_form
    if (step === formList?.indexOf("add_vehicle_form")) {
      if (!validateForm("add_vehicle_form")) return false;
      // disableViolationInputs(false);

      // Save Data
      const resResult = await saveMotorhome("add_vehicle_form");
      if (!resResult) return false;
    }

    // ****
    if (step === formList?.indexOf("violations_form")) {
      if (!MHviolationsValidation()) return false;

      // Save Data
      const resResult = await saveMotorhome("violations_form");
      if (!resResult) return false;
    }

    if (step === formList?.indexOf("coverage_limits_form")) {
      if (!validateForm("coverage_limits_form")) return false;
      MHfuncDamageForm();

      // Save Data
      const resResult = await saveMotorhome("coverage_limits_form");
      if (!resResult) return false;
    }

    if (step === formList?.indexOf("physical_damage_form")) {
      if (!MHphysicalDamageValidation()) return false;
      coverageHistoryFunc();

      // Save Data
      const resResult = await saveMotorhome("physical_damage_form");
      if (!resResult) return false;
    }
    if (step === formList?.indexOf("coverage_history_form")) {
      if (!validateForm("coverage_history_form")) return false;

      // Save Data
      const resResult = await saveMotorhome("coverage_history_form", "submit");
      if (!resResult) return false;

      // alert("Done");

      // Go to Thank You Page
      window.location.href = successRedirection;
    }

    // Run after every submission

    return true;
  }

  async function saveMotorhome(form, action = "send") {
    const resData = await saveData("/sc-api/forms/save-motorhome", formData, MHNextBtn, form, action);

    if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;

    return resData;
  }

  // *********************************************
  //              STEP-1 FUNCTIONALITY
  // *********************************************
  function MHspouseOperatorFunc() {
    const cohabOperator = document.getElementById("SpouseMotorHomeOperator");
    const cohaExp = document.getElementById("cohabitantYearsExperience");

    cohabOperator.addEventListener("change", () => {
      if (cohabOperator.value === "Yes") {
        cohaExp.disabled = false;
        cohaExp.classList.add("required");
      } else {
        cohaExp.disabled = true;
        cohaExp.classList.remove("required");
        removeErrorOnDisabled(cohaExp);
      }
    });
  }

  // Add driver functionality
  const MHaddDriver = document.getElementById("MHaddDriver");
  MHaddDriver?.addEventListener("click", function () {
    if (!formList?.includes("additional_driver")) {
      const summaryIndex = formList?.indexOf("driver_summary_form");
      formList?.splice(summaryIndex, 0, "additional_driver");
    }
    showActiveForm(MHStep, MHBackBtn);

    // Set VehicleId dynamically
    MHdriverId = MHdriverArr.length;
    for (let i = 0; i < MHmaxDriverItem; i++) {
      const vId = MHdriverArr[i]?.MHdriverId;

      if (i != vId) {
        MHdriverId = i;
        break;
      }

      MHdriverId;
    }

    // if (MHdriverArr.length >= MHmaxDriverItem) MHaddDriver.disabled = true;

    const fields = document.querySelectorAll(".additional_driver .field__input");
    fields.forEach((field) => {
      field.value = "";

      const fieldName = field.getAttribute("data-field");
      const property = `additionalDriver${MHdriverId}${fieldName}`;

      field.id = field.name = property;
    });
  });

  // Driver Functionality
  function MHdriverSummaryFunc() {
    // Driver Summary Heading
    const summaryHeading = document.querySelector(".driver_summary_form .quote_request_heading");
    let driverCount = 1;

    // Policyholder info in driver summary
    const { policyHolderFirstName, policyHolderLastName, policyHolderDob } = formData;
    const phAge = `${calculateAge(policyHolderDob)} years old`;

    document.querySelector(
      ".quote_request__summary_policyholder_item_info"
    ).innerHTML = `${policyHolderFirstName} ${policyHolderLastName}, ${phAge} <br> <p>policyholder</p>`;

    // Spouse info in driver summary
    const cohabOperatorVal = document.getElementById("SpouseMotorHomeOperator")?.value;
    const spouseItemInfo = document.querySelector(".quote_request__summary_spouse_item_info");

    if (cohabOperatorVal === "Yes") {
      const { spouseFirstName, spouseLastName, spouseDob } = formData; // different name & id in spouse form
      const cohAge = `${calculateAge(spouseDob)} years old`;

      spouseItemInfo.parentElement.classList.remove("__hide");
      spouseItemInfo.innerHTML = `${spouseFirstName} ${spouseLastName}, ${cohAge}`;
      driverCount = 2;
    } else {
      spouseItemInfo.parentElement.classList.add("__hide");
    }

    // Add all data to moreVehicles sections
    MHdriverArr = MHdriverArr.filter((item) => item !== "deleted");

    // if all data not appended then Append Data to #MHaddDriversList
    if (MHdriverArr.length > 0) {
      const MHaddDriversList = document.querySelector("#MHaddDriversList");

      MHaddDriversList.innerHTML = "";
      const driverDemoItem = document.querySelector(".quote_request__summary_item.driverDemoItem");

      // Clone the demo, create and append
      MHdriverArr.forEach((info) => {
        const clonedItem = driverDemoItem.cloneNode(true);
        clonedItem.classList.remove("__hide", "driverDemoItem");
        clonedItem.setAttribute("data-id", info.MHdriverId);

        let dFirstName = "";
        let dLastName = "";
        let dDob = "";

        for (const k in info) {
          if (String(k).includes("FirstName")) dFirstName = info[k];
          if (String(k).includes("LastName")) dLastName = info[k];
          if (String(k).includes("Dob")) dDob = info[k];
        }

        const dAge = `${calculateAge(dDob)} years old`;

        clonedItem.querySelector(".quote_request__summary_item_info").innerHTML = `${dFirstName} ${dLastName}, ${dAge}`;

        // append clone element in Summary
        MHaddDriversList.appendChild(clonedItem);
      });
    }

    MHdriverArr.forEach((info) => (formData = { ...formData, ...info }));
    delete formData?.MHdriverId;

    // Print Driver Summary Heading
    driverCount = driverCount + MHdriverArr.length;
    summaryHeading.innerHTML = `Your Policy Has ${driverCount} ${driverCount > 1 ? "Drivers" : "Driver"}`;

    // Disable Add Button if max
    if (MHdriverArr.length >= MHmaxDriverItem) MHaddDriver.disabled = true;

    MHrunDriverItemsFunctionality();
  }

  // ********** FUNCTIONALITY OF MORE VEHICLE FORMS : Edit, Delete ***********
  function MHrunDriverItemsFunctionality() {
    // policyholderEditBtn Functionality
    const policyholderEditBtn = document.getElementById("policyholderEditBtn");
    policyholderEditBtn.addEventListener("click", () => {
      MHStep = formList?.indexOf("policyholder_form");
      showActiveForm(MHStep, MHBackBtn);
    });

    // spouseEditBtn Functionality
    const spouseEditBtn = document.getElementById("spouseEditBtn");
    spouseEditBtn.addEventListener("click", () => {
      MHStep = formList?.indexOf("spouse_information");
      showActiveForm(MHStep, MHBackBtn);
    });

    // Others Driver Functionality
    const MHaddDriversList = document.getElementById("MHaddDriversList");
    const driverItemEl = MHaddDriversList.querySelectorAll(".quote_request__summary_item.driverItem");

    driverItemEl.forEach((item, itemIndex) => {
      const MHdriverId = item.getAttribute("data-id");

      const editBtn = item.querySelector(".editBtn");
      const deleteBtn = item.querySelector(".deleteBtn");
      const deleteYes = item.querySelector(".deleteYes");
      const deleteNo = item.querySelector(".deleteNo");

      editBtn?.addEventListener("click", () => {
        MHeditDriverIndex = MHdriverId;

        if (!formList?.includes("additional_driver")) {
          const summaryIndex = formList?.indexOf("driver_summary_form");
          formList?.splice(summaryIndex, 0, "additional_driver");

          showActiveForm(MHStep, MHBackBtn);

          // Assign the values
          const aDrFields = document.querySelectorAll(".additional_driver .field__input");
          aDrFields.forEach((f) => {
            const property = "additionalDriver" + MHdriverId + f.getAttribute("data-field");
            f.id = f.name = property;
            f.value = formData[property];
          });
        }
      });

      deleteBtn?.addEventListener("click", () => {
        item.querySelector(".yes_no")?.classList.remove("__hide");
        item.querySelector(".delete_edit")?.classList.add("__hide");
      });

      deleteNo.addEventListener("click", () => {
        item.querySelector(".yes_no")?.classList.add("__hide");
        item.querySelector(".delete_edit")?.classList.remove("__hide");
      });

      deleteYes.addEventListener("click", () => {
        for (const k in MHdriverArr[itemIndex]) {
          delete formData[k];
        }

        // MHdriverArr[itemIndex + 1] = "deleted";
        MHdriverArr = MHdriverArr.filter((item, i) => i !== itemIndex);

        item.classList.add("__hide");
        item.remove(); // delete elements

        MHaddDriver.disabled = false;
      });
    });
  }

  // *********************************************
  //              STEP-1 Validation
  // *********************************************

  function MHaddDriverValidation() {
    const isValidate = validateForm("additional_driver", false);

    if (isValidate) {
      const driverData = {};

      const allFields = document.querySelectorAll(`.additional_driver .field__input`);

      allFields.forEach((field) => {
        driverData[field.name] = field.value;
      });

      // UPDATE or CREATE Vehicle Data
      if (Number(MHeditDriverIndex) >= 0) {
        const matchId = MHdriverArr.filter((v) => v.MHdriverId == MHeditDriverIndex);
        const updatedData = { ...matchId[0], ...driverData };

        MHdriverArr = MHdriverArr.map((vData) => (vData.MHdriverId == Number(MHeditDriverIndex) ? updatedData : vData));

        // MHdriverArr[Number(MHdriverId)] = driverData;
        MHeditDriverIndex = -1;
      } else {
        driverData.MHdriverId = MHdriverId;

        MHdriverArr.push(driverData);
      }

      // REDUCE MHStep and REMOVE driver_summary_form from the formList
      const summaryIndex = formList?.indexOf("driver_summary_form");
      MHStep = summaryIndex - 2;
      formList = formList?.filter((item) => item != "additional_driver");
    }

    return isValidate;
  }

  function MHviolationDriverFunc() {
    const vioDriver = document.querySelectorAll(".select.householdViolationsDriver");
    if (!vioDriver) return;

    function driverOptionAdd(element, value, name) {
      element.insertAdjacentHTML("beforeend", `<option value="${value}">${name}</option>`);
    }

    // reset options
    vioDriver.forEach((driverEl) => {
      driverEl.innerHTML = "";
      driverOptionAdd(driverEl, "", "Choose One");

      // policyHolder
      if (formData.policyHolderFirstName)
        driverOptionAdd(driverEl, "policyHolder", `${formData?.policyHolderFirstName} ${formData?.policyHolderLastName}`);
      // cohabitant
      if (formData.spouseFirstName)
        driverOptionAdd(driverEl, "spouse", `${formData?.spouseFirstName} ${formData?.spouseLastName}`);

      // driver 0
      if (formData.additionalDriver0FirstName)
        driverOptionAdd(
          driverEl,
          "additionalDriver0",
          `${formData?.additionalDriver0FirstName} ${formData?.additionalDriver0LastName}`
        );
      // driver 1
      if (formData.additionalDriver1FirstName)
        driverOptionAdd(
          driverEl,
          "additionalDriver1",
          `${formData?.additionalDriver1FirstName} ${formData?.additionalDriver1LastName}`
        );
      // driver 2
      if (formData.additionalDriver2FirstName)
        driverOptionAdd(
          driverEl,
          "additionalDriver2",
          `${formData?.additionalDriver2FirstName} ${formData?.additionalDriver2LastName}`
        );
    });
  }

  // *********************************************
  //              STEP-3 FUNCTIONALITY
  // *********************************************

  // ********* FUNCTIONALITY physical_damage_form *********
  function MHfuncDamageForm() {
    const damageForm = document.querySelector(".damage__form");
    const vehicleName = damageForm.querySelector(".vehicle_name");
    vehicleName.innerHTML = `${formData?.vehicleYear} ${formData?.vehicleMake} ${formData?.vehicleModel} `;

    const liability = document.querySelectorAll("input[name=vehicleLiabilityOnlyCoverage]");
    liability.forEach((radio) => {
      radio.addEventListener("change", (e) => {
        document.querySelector(".liabilityError").style.display = "none";
        const otherFields = document.querySelectorAll(".field__input.damage");

        if (e.target.value === "No") {
          otherFields.forEach((field) => {
            field.disabled = false;
            field.classList.add("required");
          });
        } else {
          otherFields.forEach((field) => {
            field.disabled = true;
            field.classList.remove("required");
          });
        }
      });
    });
  }

  // *********************************************
  //              STEP-3 VALIDATION
  // *********************************************
  function MHviolationsValidation() {
    if (getViolationsValue() === "No") {
      formData.householdViolationsPreviousClaims = "No";
      return true;
    } else if (getViolationsValue() === "Yes") {
      formData.householdViolationsPreviousClaims = "Yes";

      const fieldsWrapper = document.querySelectorAll(".violation_info_fields");
      const results = [];

      fieldsWrapper.forEach((field, i) => {
        const driverField = field.querySelector(".householdViolationsDriver");
        const typeField = field.querySelector(".householdViolationsType");
        const dateField = field.querySelector(".householdViolationsDate");

        const validationFields = [
          alphabeticOnly(driverField),
          isValueEmpty(driverField),
          isValueEmpty(typeField),
          minValue(dateField, 10, "Please enter a valid Date"),
          isValueEmpty(dateField),
        ];

        const isValidate = validationFields.every((result) => result === true);

        if (isValidate) {
          formData[driverField.name] = driverField.value;
          formData[typeField.name] = typeField.value;
          formData[dateField.name] = dateField.value;

          results.push(true);
        } else {
          results.push(false);
        }
      });

      const isAllValid = results.every((result) => result === true);

      return isAllValid;
    } else {
      const fieldContainer = document.querySelector(".has_violation_inputs_container");
      isValueEmpty(fieldContainer);

      return false;
    }
  }

  function MHphysicalDamageValidation() {
    const fieldChecked = document.querySelector(`input[name=vehicleLiabilityOnlyCoverage]:checked`);
    if (!fieldChecked) {
      document.querySelector(".liabilityError").style.display = "block";
      return false;
    }

    //
    const isValidate = validateForm("physical_damage_form");
    formData[fieldChecked.name] = fieldChecked.value;

    return isValidate;
  }

  // *********************************************
  //              STEP-4 VALIDATION
  // *********************************************

  // Note: Step 1, 4 is in formCommon.js file
});

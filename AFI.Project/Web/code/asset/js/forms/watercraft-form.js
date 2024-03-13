window.addEventListener("DOMContentLoaded", () => {
  // Forms
  const WCForms = [
    "policyholder_form",
    "driver_summary_form",
    "summary__form",
    "violations_form",
    "coverage_limits_form",
    "physical_damage_form",
    "coverage_history_form",
  ];

  // driver states
  let WCdriverArr = [];
  let WCdriverId = 0;
  let WCeditDriverIndex = -9999;
  const WCmaxDriverItem = 3;

  // vehicle states
  let WCvehicles = [];
  let WCvehicleId = 0;
  let WCeditVehicleIndex = -9999;
  const WCmaxVehicleItem = 4;
  let singleVehicle = {};

  // *********************************************
  //       FORM SUBMISSION AND STEP HANDLING
  // *********************************************
  const WCNextBtn = document.querySelector("#WCNextBtn");
  const WCBackBtn = document.querySelector("#WCBackBtn");

  let WCStep = 0;
  let WCMaxStep = formList?.length - 1;

  // ***** NEXT FUNCTIONALITY *****
  pressEnterToSubmit(WCNextBtn);
  WCNextBtn?.addEventListener("click", async () => {
    if (WCStep === 0) {
      const isSelectEligibility = eligibilityValidation(WCForms);
      if (!Boolean(isSelectEligibility)) return false;

      militaryFormFunc();
    }
    //  HANDLE ALL FORM SUBMISSIONS AND STEP VALIDATION
    const submitResult = await handleWaterStepForm(WCStep);
    if (!submitResult) return false;

    // Step Increment
    WCMaxStep = formList?.length - 1;
    WCStep >= WCMaxStep ? WCStep : WCStep++;

    // Show Form
    showActiveForm(WCStep, WCBackBtn);
  });

  // Back
  WCBackBtn?.addEventListener("click", () => {
    // Step Decrement
    WCStep <= 0 ? WCStep : WCStep--;

    // 2 side back for add_vehicle_form
    const notFirst = WCvehicles.length > 0 && WCvehicles[0].vehicle0HullMaterials;
    if (notFirst && WCStep + 1 === formList?.indexOf("add_vehicle_form")) {
      formList = formList?.filter((form) => form !== "add_vehicle_form" && form !== "add_vehicle_details_form");
      WCStep = formList?.indexOf("summary__form");
      WCeditVehicleIndex = -9999;
    }

    // back from add_vehicle_details_form
    if (WCStep + 1 === formList?.indexOf("add_vehicle_details_form")) {
      WCStep = formList?.indexOf("add_vehicle_form");
      WCeditVehicleIndex = -9999;
    }

    // 2 side back for additional_driver form
    if (WCStep + 1 === formList?.indexOf("additional_driver")) {
      formList = formList?.filter((item) => item != "additional_driver");
      WCStep = formList?.indexOf("driver_summary_form");
      WCeditDriverIndex = -9999;
    }

    showActiveForm(WCStep, WCBackBtn);
  });

  // =*********************************************
  //       HANDLING MULTI-STEP FORMS
  // =*********************************************
  async function handleWaterStepForm(step) {
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
      WCspouseOperatorFunc();

      // Save Data
       const resResult = await saveWatercraft("policyholder_form", "send", "watercraft");
     // const resResult = await saveWatercraft("policyholder_form");
      if (!resResult) return false;
    }
    if (step === formList?.indexOf("spouse_information")) {
      if (!validateForm("spouse_information")) return false;

      // Save Data
      const resResult = await saveWatercraft("spouse_information");
      if (!resResult) return false;
    }

    // Driver
    if (step === formList?.indexOf("driver_summary_form") || step === formList?.indexOf("driver_summary_form") - 1) {
      WCdriverSummaryFunc();
      WCaddVehicleFunc();
    }

    if (step === formList?.indexOf("additional_driver")) {
      if (!WCaddDriverValidation("additional_driver")) return false;

      updateArrToFormData(WCdriverArr);

      // Save Data
      const resResult = await saveWatercraft("additional_driver");
      if (!resResult) return false;

      WCdriverSummaryFunc();
    }

    // Vehicle
    if (step === formList?.indexOf("add_vehicle_form")) {
      if (!WCaddVehicleValidation("add_vehicle_form")) return false;

      updateArrToFormData(WCvehicles);

      // Save Data
      const resResult = await saveWatercraft("add_vehicle_form");
      if (!resResult) return false;
    }
    if (step === formList?.indexOf("add_vehicle_details_form")) {
      if (!WCaddVehicleValidation("add_vehicle_details_form")) return false;

      updateArrToFormData(WCvehicles);

      // Save Data
      const resResult = await saveWatercraft("add_vehicle_details_form");
      if (!resResult) return false;

      WCsummaryFunctionality(true);

      // REDUCE WCStep and REMOVE add_vehicle_form from the formList
      return true;
      // const summaryIndex = formList?.indexOf("summary__form");
      // WCStep = summaryIndex;
      // showActiveForm(WCStep, WCBackBtn);
      // WCStep = summaryIndex - 2;
      // formList = formList?.filter((form) => form != "add_vehicle_form");
      // formList = formList?.filter((form) => form != "add_vehicle_details_form");
    }

    if (step === formList?.indexOf("summary__form") || step === formList?.indexOf("summary__form") - 1) {
      WCsummaryFunctionality();
      // disableViolationInputs(false);
      violationDriverFunc();
    }

    // Coverage
    if (step === formList?.indexOf("violations_form")) {
      if (!WCviolationsValidation()) return false;

      // Save Data
      const resResult = await saveWatercraft("violations_form");
      if (!resResult) return false;
    } else removeViolationError();

    if (step === formList?.indexOf("coverage_limits_form")) {
      if (!validateForm("coverage_limits_form")) return false;
      WCfuncDamageForm();

      // Save Data
      const resResult = await saveWatercraft("coverage_limits_form");
      if (!resResult) return false;
    }

    if (step === formList?.indexOf("physical_damage_form")) {
      if (!WCphysicalDamageValidation()) return false;
      coverageHistoryFunc();

      // Save Data
      const resResult = await saveWatercraft("physical_damage_form");
      if (!resResult) return false;
    }

    // Final
    if (step === formList?.indexOf("coverage_history_form")) {
      if (!validateForm("coverage_history_form")) return false;

      // Save Data
      const resResult = await saveWatercraft("coverage_history_form", "submit");
      if (!resResult) return false;

      // Go to Thank You Page
      window.location.href = successRedirection;
    }

    return true;
  }

  async function saveWatercraft(form, action = "send", addressValid = null) {
    const resData = await saveData("/sc-api/forms/save-watercraft", formData, WCNextBtn, form, action, addressValid);

    if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;

    return resData;
  }

  // *********************************************
  //              STEP-1 FUNCTIONALITY
  // *********************************************
  function WCspouseOperatorFunc() {
    const cohabOperator = document.getElementById("cohabitantWatercraftOperator");
    const cohaExp = document.getElementById("cohabitantOperatingExperience");

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

  // ===================
  // Add driver functionality
  const WCaddDriver = document.getElementById("WCaddDriver");
  WCaddDriver?.addEventListener("click", function () {
    if (!formList?.includes("additional_driver")) {
      const summaryIndex = formList?.indexOf("driver_summary_form");
      formList?.splice(summaryIndex, 0, "additional_driver");
    }
    showActiveForm(WCStep, WCBackBtn);

    // Set VehicleId dynamically
    WCdriverId = WCdriverArr.length;
    for (let i = 0; i < WCmaxDriverItem; i++) {
      const vId = WCdriverArr[i]?.WCdriverId;

      if (i != vId) {
        WCdriverId = i;
        break;
      }

      WCdriverId;
    }

    // if (WCdriverArr.length >= WCmaxDriverItem) WCaddDriver.disabled = true;

    const fields = document.querySelectorAll(".additional_driver .field__input");
    fields.forEach((field) => {
      field.value = "";

      const fieldName = field.getAttribute("data-field");
      const property = `additionalDriver${WCdriverId}${fieldName}`;

      field.id = field.name = property;
    });
  });

  // Driver Functionality
  function WCdriverSummaryFunc() {
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
    const cohabOperatorVal = document.getElementById("cohabitantWatercraftOperator")?.value;
    const spouseItemInfo = document.querySelector(".quote_request__summary_spouse_item_info");

    if (cohabOperatorVal === "Yes") {
      const { cohabitantFirstName, cohabitantLastName, cohabitantDob } = formData;
      const cohAge = `${calculateAge(cohabitantDob)} years old`;

      spouseItemInfo.parentElement.classList.remove("__hide");
      spouseItemInfo.innerHTML = `${cohabitantFirstName} ${cohabitantLastName}, ${cohAge}`;
      driverCount = 2;
    } else {
      spouseItemInfo.parentElement.classList.add("__hide");
    }

    // Add all data to moreVehicles sections
    WCdriverArr = WCdriverArr.filter((item) => item !== "deleted");

    // if all data not appended then Append Data to #WCaddDriversList
    if (WCdriverArr.length > 0) {
      const WCaddDriversList = document.querySelector("#WCaddDriversList");

      WCaddDriversList.innerHTML = "";
      const driverDemoItem = document.querySelector(".quote_request__summary_item.driverDemoItem");

      // Clone the demo, create and append
      WCdriverArr.forEach((info) => {
        const clonedItem = driverDemoItem.cloneNode(true);
        clonedItem.classList.remove("__hide", "driverDemoItem");
        clonedItem.setAttribute("data-id", info.WCdriverId);

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
        WCaddDriversList.appendChild(clonedItem);
      });
    }

    WCdriverArr.forEach((info) => (formData = { ...formData, ...info }));
    delete formData?.WCdriverId;

    // Print Driver Summary Heading
    driverCount = driverCount + WCdriverArr.length;
    summaryHeading.innerHTML = `Your Policy Has ${driverCount} ${driverCount > 1 ? "Operators" : "Operator"} `;

    // Disable Add Button if max
    if (WCdriverArr.length >= WCmaxDriverItem) WCaddDriver.disabled = true;

    WCrunDriverItemsFunctionality();
  }

  // ********** FUNCTIONALITY OF MORE VEHICLE FORMS : Edit, Delete ***********
  function WCrunDriverItemsFunctionality() {
    // policyholderEditBtn Functionality
    const policyholderEditBtn = document.getElementById("policyholderEditBtn");
    policyholderEditBtn.addEventListener("click", () => {
      WCStep = formList?.indexOf("policyholder_form");
      showActiveForm(WCStep, WCBackBtn);
    });

    // spouseEditBtn Functionality
    const spouseEditBtn = document.getElementById("spouseEditBtn");
    spouseEditBtn.addEventListener("click", () => {
      WCStep = formList?.indexOf("spouse_information");
      showActiveForm(WCStep, WCBackBtn);
    });

    // Others Driver Functionality
    const WCaddDriversList = document.getElementById("WCaddDriversList");
    const driverItemEl = WCaddDriversList.querySelectorAll(".quote_request__summary_item.driverItem");

    driverItemEl.forEach((item, itemIndex) => {
      const WCdriverId = item.getAttribute("data-id");

      const editBtn = item.querySelector(".editBtn");
      const deleteBtn = item.querySelector(".deleteBtn");
      const deleteYes = item.querySelector(".deleteYes");
      const deleteNo = item.querySelector(".deleteNo");

      editBtn?.addEventListener("click", () => {
        WCeditDriverIndex = WCdriverId;

        if (!formList?.includes("additional_driver")) {
          const summaryIndex = formList?.indexOf("driver_summary_form");
          formList?.splice(summaryIndex, 0, "additional_driver");

          showActiveForm(WCStep, WCBackBtn);

          // Assign the values
          const aDrFields = document.querySelectorAll(".additional_driver .field__input");
          aDrFields.forEach((f) => {
            const property = "additionalDriver" + WCdriverId + f.getAttribute("data-field");
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
        for (const k in WCdriverArr[itemIndex]) {
          delete formData[k];
        }

        // WCdriverArr[itemIndex + 1] = "deleted";
        WCdriverArr = WCdriverArr.filter((item, i) => i !== itemIndex);

        item.classList.add("__hide");
        item.remove(); // delete elements

        WCaddDriver.disabled = false;
      });
    });
  }

  // *********************************************
  //              STEP-1 Validation
  // *********************************************

  function WCaddDriverValidation() {
    const isValidate = validateForm("additional_driver", false);

    if (isValidate) {
      const driverData = {};

      const allFields = document.querySelectorAll(`.additional_driver .field__input`);

      allFields.forEach((field) => {
        driverData[field.name] = field.value;
      });

      // UPDATE or CREATE Vehicle Data
      if (Number(WCeditDriverIndex) >= 0) {
        const matchId = WCdriverArr.filter((v) => v.WCdriverId == WCeditDriverIndex);
        const updatedData = { ...matchId[0], ...driverData };

        WCdriverArr = WCdriverArr.map((vData) => (vData.WCdriverId == Number(WCeditDriverIndex) ? updatedData : vData));

        // WCdriverArr[Number(WCdriverId)] = driverData;
        WCeditDriverIndex = -1;
      } else {
        driverData.WCdriverId = WCdriverId;

        WCdriverArr.push(driverData);
      }

      // REDUCE WCStep and REMOVE driver_summary_form from the formList
      const summaryIndex = formList?.indexOf("driver_summary_form");
      WCStep = summaryIndex - 2;
      formList = formList?.filter((item) => item != "additional_driver");
    }

    return isValidate;
  }

  // *********************************************
  //              STEP-2 FUNCTIONALITY
  // *********************************************

  // ********** "+ Add Vehicle" BUTTON FUNCTIONALITY  ***********
  const WCaddVehicle = document.getElementById("WCaddVehicle");

  WCaddVehicle?.addEventListener("click", function () {
    WCeditVehicleIndex = -9999;
    // Remove all Error messages
    const errMsgs = document.querySelectorAll(".vehicle__field .error");
    errMsgs.forEach((err) => err.remove());

    // Fill Bank the Form
    const fields = document.querySelectorAll(".vehicle__field .field__input");

    fields.forEach((field) => {
      field.value = "";

      setMatchedData(false);
      document.querySelector(".vehicleAddress").value = "";

      const isSameAddressEl = document.querySelector(".AddressSameAsMailing--true");
      isSameAddressEl.value = "true";
      isSameAddressEl.checked = false;
    });

    // Set VehicleId dynamically
    WCvehicleId = WCvehicles.length;
    for (let i = 0; i < WCmaxVehicleItem; i++) {
      const vId = WCvehicles[i]?.WCvehicleId;

      if (i != vId) {
        WCvehicleId = i;
        break;
      }

      WCvehicleId;
    }

    if (WCvehicles.length >= WCmaxVehicleItem) this.disabled = true;

    fields.forEach((field) => {
      const fieldName = field.getAttribute("data-field");
      const property = `vehicle${WCvehicleId}${fieldName}`;

      field.id = property;
      field.name = property;
    });

    // make Dynamic vehicleSameAsMailingLabel for
    const vehicleSameAsMailing = document.querySelector(".add_vehicle_form .AddressSameAsMailing--true");
    const vehicleSameAsMailingLabel = document.querySelector(".add_vehicle_form .AddressSameAsMailing--label");
    vehicleSameAsMailingLabel.setAttribute("for", vehicleSameAsMailing.id);

    // ==== SHOW FORM =====
    if (!formList?.includes("add_vehicle_form")) {
      const summaryIndex = formList?.indexOf("summary__form");
      formList?.splice(summaryIndex, 0, "add_vehicle_form", "add_vehicle_details_form");
      WCStep = formList?.indexOf("add_vehicle_form");
    }
    showActiveForm(WCStep, WCBackBtn);
  });

  // ********** FUNCTIONALITY OF MORE VEHICLE FORMS : Edit, Delete ***********
  function WCrunVehicleItemsFunctionality() {
    const moreVehicles = document.getElementById("moreVehicles");
    const moreVehicleItems = moreVehicles.querySelectorAll(".quote_request__summary_item");

    moreVehicleItems.forEach((item, itemIndex) => {
      const WCvehicleId = item.getAttribute("data-id");

      const editBtn = item.querySelector(".editBtn");
      const deleteBtn = item.querySelector(".deleteBtn");
      const deleteYes = item.querySelector(".deleteYes");
      const deleteNo = item.querySelector(".deleteNo");

      editBtn?.addEventListener("click", () => {
        WCeditVehicleIndex = Number(WCvehicleId);

        if (!formList?.includes("add_vehicle_form")) {
          const summaryIndex = formList?.indexOf("summary__form");
          formList?.splice(summaryIndex, 0, "add_vehicle_form", "add_vehicle_details_form");

          showActiveForm(WCStep, WCBackBtn);

          // Remove all Error messages
          const errMsgs = document.querySelectorAll(".vehicle__field .error");
          errMsgs.forEach((err) => err.remove());

          // Fill Bank the Form
          const fields = document.querySelectorAll(".vehicle__field .field__input");
          // const vclFields = document.querySelectorAll(".add_vehicle_form .field__input");
          fields.forEach((f) => {
            const property = "vehicle" + WCvehicleId + f.getAttribute("data-field");
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
        for (const k in WCvehicles[itemIndex]) {
          delete formData[k];
        }

        // WCvehicles[itemIndex] = "deleted";
        WCvehicles = WCvehicles.filter((item, i) => i !== itemIndex);

        item.classList.add("__hide");
        item.remove(); // delete elements

        WCaddVehicle.disabled = false;
      });
    });
  }

  // *********************************************
  //              STEP-2 VALIDATION
  // *********************************************

  function WCsummaryFunctionality(shouldUpdateStep) {
    const summaryHeading = document.querySelector(".summary__form .quote_request_heading");
    summaryHeading.innerHTML = `Your Policy Has ${WCvehicles.length} Watercraft`;

    // If Main Vehicle Data OKK then direct show SUMMARY neither show add_vehicle_form
    if (!WCvehicles.length > 0) {
      if (!formList?.includes("add_vehicle_form")) {
        const summaryIndex = formList?.indexOf("summary__form");
        formList?.splice(summaryIndex, 0, "add_vehicle_form", "add_vehicle_details_form");
      }
      showActiveForm(WCStep, WCBackBtn);
    } else {
      formList = formList?.filter((form) => form !== "add_vehicle_form" && form !== "add_vehicle_details_form");
      if (shouldUpdateStep === true) {
        WCStep = formList?.indexOf("summary__form") - 1;
        // showActiveForm(WCStep, WCBackBtn);
      }
    }

    // Add all data to moreVehicles sections
    WCvehicles = WCvehicles.filter((item) => item !== "deleted");

    const addedSummary = document.querySelector("#moreVehicles");
    // const totalAdded = addedSummary.children?.length;

    // if all data not appended then Append Data to #moreVehicles
    if (WCvehicles.length > 0) {
      addedSummary.innerHTML = "";
      const demoItem = document.querySelector(".quote_request__summary_item.demoItem");
      // Clone the demo, create and append
      WCvehicles.forEach((info, i) => {
        const clonedItem = demoItem.cloneNode(true);
        clonedItem.classList.remove("__hide", "demoItem");
        clonedItem.setAttribute("data-id", info.WCvehicleId);

        if (info.WCvehicleId == 0) clonedItem.querySelector("#deleteBtn").remove();

        let vYear = "";
        let vMake = "";
        let vModel = "";

        for (const k in info) {
          if (String(k).includes("Year")) vYear = info[k];
          if (String(k).includes("Make")) vMake = info[k];
          if (String(k).includes("Model")) vModel = info[k];
        }

        clonedItem.querySelector(".quote_request__summary_item_info").innerHTML = `${vYear} ${vMake} ${vModel}`;

        // append clone element in Summary
        addedSummary.appendChild(clonedItem);
      });
    }

    // ****************************************************
    // const filterCVehicles = WCvehicles.map((data) => {
    //   delete data.WCvehicleId;
    //   return data;
    // });

    WCvehicles.forEach((info) => (formData = { ...formData, ...info }));
    delete formData.WCvehicleId;

    WCrunVehicleItemsFunctionality();
  }

  function WCaddVehicleValidation(form) {
    const isValidate = validateForm(form, false);

    if (isValidate) {
      const allFields = document.querySelectorAll(`.${form} .field__input`);

      allFields.forEach((field) => {
        if (field.type === "checkbox") singleVehicle[field.name] = field.checked;
        else singleVehicle[field.name] = field.value;
      });

      // UPDATE or CREATE Vehicle Data
      if (Number(WCeditVehicleIndex) >= 0) {
        const matchId = WCvehicles.filter((v) => v.WCvehicleId == Number(WCeditVehicleIndex));
        const updatedData = { ...matchId[0], ...singleVehicle };

        WCvehicles = WCvehicles.map((vData) => (vData.WCvehicleId == WCeditVehicleIndex ? updatedData : vData));

        // Reset Edit
        if (form === "add_vehicle_details_form") WCeditVehicleIndex = -999;
      } else {
        singleVehicle.WCvehicleId = WCvehicleId;

        if (form === "add_vehicle_details_form") {
          // WCvehicles = WCvehicles.map((vData) =>
          //   vData.WCvehicleId == singleVehicle.WCvehicleId ? { ...vData, ...singleVehicle } : vData
          // );
          WCvehicles.push(singleVehicle);
          singleVehicle = {};
        } else {
          // WCvehicles.push(singleVehicle);
        }
      }

      // REDUCE WCStep and REMOVE add_vehicle_form from the formList
      // const summaryIndex = formList?.indexOf("summary__form");
      // WCStep = summaryIndex - 2;
      // formList = formList?.filter((form) => form != "add_vehicle_form");
      // formList = formList?.filter((form) => form != "add_vehicle_details_form");
    }

    return isValidate;
  }

  // Same Mailing Address Functionality
  function setMatchedData(disability) {
    const isSameAddressEl = document.querySelector(".AddressSameAsMailing--true");

    const WCaddVehicleFields = document.querySelectorAll(".vehicleFullAddress .field__input");

    WCaddVehicleFields.forEach((el) => {
      el.disabled = disability;
      isSameAddressEl.disabled = false;

      const dataMatch = el.getAttribute("data-match");
      if (dataMatch) el.value = formData[dataMatch];
    });

    if (disability === true) {
      const WCerrMsg = document.querySelectorAll(".vehicleFullAddress .error");
      WCerrMsg.forEach((el) => el.remove());
    }
  }

  function WCaddVehicleFunc() {
    // Same AddressSameAsMailing checkbox Functionality
    const isSameAddressEl = document.querySelector(".AddressSameAsMailing--true");
    isSameAddressEl.checked = false;

    setMatchedData(false);
    document.querySelector(".vehicleAddress").value = "";

    // Same Mailing CheckBox Functionality
    isSameAddressEl?.addEventListener("change", () => {
      if (isSameAddressEl.checked) {
        setMatchedData(true);
      } else {
        const WCaddVehicleFields = document.querySelectorAll(".vehicleFullAddress .field__input");

        WCaddVehicleFields.forEach((el, i) => {
          el.value = "";
          el.disabled = false;

          isSameAddressEl.value = "true";

          if (i === 1) el.focus();
        });
      }
    });

    // vehicleTrailerIncluded change functionality (vehicleTrailerValue)
    const vehicleTrailerIncluded = document.querySelector(".vehicleTrailerIncluded");
    vehicleTrailerIncluded?.addEventListener("change", (e) => {
      const v = e.target.value;
      const vehicleTrailerValue = document.querySelector(".vehicleTrailerValue");

      if (v === "Yes") {
        vehicleTrailerValue.disabled = false;
        vehicleTrailerValue.classList.add("required");
      } else {
        vehicleTrailerValue.value = "";
        vehicleTrailerValue.disabled = true;
        vehicleTrailerValue.classList.remove("required");
        removeErrorOnDisabled(vehicleTrailerValue);
      }
    });
  }

  function yearPurchasedFunctionality() {
    const vehicleYear = document.querySelector(".field__input.vehicleYear");
    const vehicleYearPurchased = document.querySelector(".field__input.vehicleYearPurchased");

    if (!vehicleYearPurchased) return; // safe return if field is not found

    const maxYear = new Date().getFullYear() + 1;

    if (!vehicleYear) return; // safe return to prevent errors

    const createOption = (value, isSelected = false) => {
      const option = document.createElement("option");
      option.textContent = value;
      option.value = value;

      isSelected ? (option.selected = isSelected) : "";

      vehicleYearPurchased?.appendChild(option);
    };

    vehicleYear?.addEventListener("change", (e) => {
      const yearValue = parseInt(e.target.value);

      vehicleYearPurchased.innerHTML = ""; // reset all options
      createOption("Choose One", true); // Default and Selected option

      // Update options by Year Value
      for (let year = maxYear; year >= yearValue; year--) {
        createOption(year);
      }
    });
  }
  yearPurchasedFunctionality();
  // *********************************************
  //              STEP-3 FUNCTIONALITY
  // *********************************************

  // ********* FUNCTIONALITY physical_damage_form *********
  function WCfuncDamageForm() {
    const damageForm = document.querySelector(".damage__form.__hide");
    const DamageFormWrapper = document.getElementById("physical_damage_form_wrapper");

    // Clear DamageFormWrapper Children
    DamageFormWrapper.innerHTML = "";

    // Add Vehicle data to DamageFormWrapper with other fields
    WCvehicles.forEach((vData, index) => {
      const vId = vData.WCvehicleId;
      const year = vData[`vehicle${vId}Year`];
      const make = vData[`vehicle${vId}Make`];
      const model = vData[`vehicle${vId}Model`];

      const clonedItem = damageForm.cloneNode(true);

      clonedItem.classList.remove("__hide");
      clonedItem.querySelector(".vehicle_name").innerHTML = `${year} ${make} ${model}`;

      // comprehensiveDeductible fields id name dynamic
      const comprehensiveDeductible = clonedItem.querySelector(".vehicleComprehensiveDeductible");
      comprehensiveDeductible.id = comprehensiveDeductible.name = `vehicle${vId}ComprehensiveDeductible`;

      // collisionDeductible fields id name dynamic
      const collisionDeductible = clonedItem.querySelector(".vehicleCollisionDeductible");
      collisionDeductible.id = collisionDeductible.name = `vehicle${vId}CollisionDeductible`;

      // liability radio fields functionality
      const liabilityYes = clonedItem.querySelector("#liability--Yes");
      const liabilityNo = clonedItem.querySelector("#liability--No");
      const liabilityYesLabel = clonedItem.querySelector(".liability_Yes_label");
      const liabilityNoLabel = clonedItem.querySelector(".liability_No_label");

      const nameOfLiability = `vehicle${vId}LiabilityOnlyCoverage`;
      const noIdOfLiability = nameOfLiability + "--No";
      const yesIdOfLiability = nameOfLiability + "--Yes";

      liabilityYes.name = nameOfLiability;
      liabilityNo.name = nameOfLiability;

      liabilityYes.id = yesIdOfLiability;
      liabilityNo.id = noIdOfLiability;

      liabilityYesLabel.setAttribute("for", yesIdOfLiability);
      liabilityNoLabel.setAttribute("for", noIdOfLiability);

      liabilityNo?.addEventListener("change", toggleDisability);
      liabilityYes?.addEventListener("change", toggleDisability);

      function toggleDisability() {
        const disabledFields = clonedItem.querySelectorAll(".field__input.damage");

        if (liabilityNo.checked) {
          disabledFields.forEach((field) => {
            field.disabled = false;
            field.classList.add("required");
          });
        } else {
          disabledFields.forEach((field) => {
            inputErrorMessage(field, "", true);
            field.disabled = true;
            field.classList.remove("required");

            // clean value comprehensive and collision on disable
            // comprehensiveDeductible.value = "";
            // collisionDeductible.value = "";

            // formData[comprehensiveDeductible.name] = "";
            // formData[collisionDeductible.name] = "";
          });
        }
      }

      DamageFormWrapper.appendChild(clonedItem);
    });

    //
    removeErrorOnChange();
  }

  // *********************************************
  //              STEP-3 VALIDATION
  // *********************************************
  function WCviolationsValidation() {
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

  function removeViolationError() {
    const vioRadioError = document.querySelector(".has_violation_row .error");
    if (vioRadioError) vioRadioError.remove();
  }

  function WCphysicalDamageValidation() {
    const fieldError = WCvehicles.map((vData) => {
      const vId = vData.WCvehicleId;

      const radioFields = document.querySelectorAll(`input[name=vehicle${vId}LiabilityOnlyCoverage]`);

      const fieldChecked = document.querySelector(`input[name=vehicle${vId}LiabilityOnlyCoverage]:checked`);

      // const fieldParent = document.getElementById(`vehicle${vId}LiabilityOnlyCoverage--Yes`).closest(".fields-row");
      const fieldParent = radioFields[0].closest(".fields-row");
      const fieldError = fieldParent.querySelector(".field__error-message");

      radioFields.forEach((field) => {
        field.addEventListener("change", () => {
          fieldError.style.display = "none";
        });
      });

      if (!fieldChecked) {
        fieldError.style.display = "block";
        return false;
      } else {
        return true;
      }
    });

    const isRadioChecked = fieldError.every((b) => b === true);
    if (!isRadioChecked) return false;

    //
    const isValidate = validateForm("physical_damage_form", false);

    if (isValidate) {
      const damageForms = document.querySelectorAll("#physical_damage_form_wrapper .damage__form");

      damageForms.forEach((damageForm, i) => {
        const vId = WCvehicles[i].WCvehicleId;

        const liaCoVal = damageForm.querySelector(`input[name=vehicle${vId}LiabilityOnlyCoverage]:checked`)?.value;

        WCvehicles[i][`vehicle${vId}LiabilityOnlyCoverage`] = liaCoVal;

        if (liaCoVal === "No") {
          const comVal = damageForm.querySelector(".field__input.vehicleComprehensiveDeductible")?.value;
          const colVal = damageForm.querySelector(".field__input.vehicleCollisionDeductible")?.value;

          WCvehicles[i][`vehicle${vId}ComprehensiveDeductible`] = comVal;
          WCvehicles[i][`vehicle${vId}CollisionDeductible`] = colVal;
        }
      });
    }

    WCsummaryFunctionality();

    return isValidate;
  }

  // *********************************************
  //              STEP-4 VALIDATION
  // *********************************************

  // Note: Step 1, 4 is in formCommon.js file
});

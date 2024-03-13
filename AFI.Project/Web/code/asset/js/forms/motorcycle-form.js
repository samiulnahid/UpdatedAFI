window.addEventListener("DOMContentLoaded", () => {
  // Forms 11
  const MCForms = [
    "policyholder_form",
    "driver_summary_form",
    "summary__form",
    "violations_form",
    "coverage_limits_form",
    "physical_damage_form",
    "coverage_history_form",
  ];

  // driver state
  let MCdriverArr = [];
  let MCdriverId = 0;
  let MCeditDriverIndex = -9999;
  const MCmaxDriverItem = 3;

  // vehicle state
  let MCvehicles = [];
  let MCvehicleId = 0;
  let MCeditVehicleIndex = -9999;
  const MCmaxVehicleItem = 4;

  // *********************************************
  //       FORM SUBMISSION AND STEP HANDLING
  // *********************************************
  const MCNextBtn = document.querySelector("#MCNextBtn");
  const MCBackBtn = document.querySelector("#MCBackBtn");

  let MCStep = 0;
  let MCMaxStep = formList?.length - 1;

  // ***** NEXT FUNCTIONALITY *****
  pressEnterToSubmit(MCNextBtn);
  MCNextBtn?.addEventListener("click", async () => {
    if (MCStep === 0) {
      const isSelectEligibility = eligibilityValidation(MCForms);
      if (!Boolean(isSelectEligibility)) return false;

      militaryFormFunc();
    }
    //  HANDLE ALL FORM SUBMISSIONS AND STEP VALIDATION
    const submitResult = await handleMotorcycleStepForm(MCStep);
    if (!submitResult) return false;

    // Step Increment
    MCMaxStep = formList?.length - 1;
    MCStep >= MCMaxStep ? MCStep : MCStep++;

    // Show Form
    showActiveForm(MCStep, MCBackBtn);
  });

  // Back
  MCBackBtn?.addEventListener("click", () => {
    // Step Decrement
    MCStep <= 0 ? MCStep : MCStep--;

    // 2 side back for add_vehicle_form
    const notFirst = MCvehicles.length > 0;
    if (notFirst && MCStep + 1 === formList?.indexOf("add_vehicle_form")) {
      formList = formList?.filter((item) => item != "add_vehicle_form");
      MCStep = formList?.indexOf("summary__form");
      MCeditVehicleIndex = -9999;
    }

    // 2 side back for additional_driver form
    if (MCStep + 1 === formList?.indexOf("additional_driver")) {
      formList = formList?.filter((item) => item != "additional_driver");
      MCStep = formList?.indexOf("driver_summary_form");
      MCeditDriverIndex = -9999;
    }
    showActiveForm(MCStep, MCBackBtn);
  });

  // =*********************************************
  //       HANDLING MULTI-STEP FORMS
  // =*********************************************
  async function handleMotorcycleStepForm(step) {
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
      MCspouseOperatorFunc();

      // Save Data
      const resResult = await saveMotorcycle("policyholder_form", "send", "motorcycle");
      // const resResult = await saveMotorcycle("policyholder_form");
      if (!resResult) return false;
    }
    if (step === formList?.indexOf("spouse_information")) {
      if (!validateForm("spouse_information")) return false;

      // Save Data
      const resResult = await saveMotorcycle("spouse_information");
      if (!resResult) return false;
    }

    if (step === formList?.indexOf("driver_summary_form") || step === formList?.indexOf("driver_summary_form") - 1) {
      MCdriverSummaryFunc();
      MCaddVehicleFunc();
    }

    if (step === formList?.indexOf("additional_driver")) {
      if (!MCaddDriverValidation("additional_driver")) return false;

      updateArrToFormData(MCdriverArr);
      // Save Data
      const resResult = await saveMotorcycle("additional_driver");
      if (!resResult) return false;

      MCdriverSummaryFunc();
    }

    //
    if (step === formList?.indexOf("add_vehicle_form")) {
      if (!MCaddVehicleValidation()) return false;

      updateArrToFormData(MCvehicles);
      // Save Data
      const resResult = await saveMotorcycle("add_vehicle_form", "send", "motorcycle");
      if (!resResult) return false;

      MCdriverSummaryFunc();
    }

    if (step === formList?.indexOf("summary__form") || step === formList?.indexOf("summary__form") - 1) {
      MCsummaryFunctionality();
      // disableViolationInputs(false);
      violationDriverFunc();
    }

    // ****
    if (step === formList?.indexOf("violations_form")) {
      if (!MCviolationsValidation()) return false;

      // Save Data
      const resResult = await saveMotorcycle("violations_form");
      if (!resResult) return false;
    }

    if (step === formList?.indexOf("coverage_limits_form")) {
      if (!validateForm("coverage_limits_form")) return false;
      MCfuncDamageForm();

      // Save Data
      const resResult = await saveMotorcycle("coverage_limits_form");
      if (!resResult) return false;
    }

    if (step === formList?.indexOf("physical_damage_form")) {
      if (!MCphysicalDamageValidation()) return false;
      coverageHistoryFunc();

      // Save Data
      const resResult = await saveMotorcycle("physical_damage_form");
      if (!resResult) return false;
    }
    if (step === formList?.indexOf("coverage_history_form")) {
      if (!validateForm("coverage_history_form")) return false;

      // Save Data
      const resResult = await saveMotorcycle("coverage_history_form", "submit");
      if (!resResult) return false;

      // alert("Done");

      // Go to Thank You Page
      window.location.href = successRedirection;
    }

    // Run after every submission

    return true;
  }

  async function saveMotorcycle(form, action = "send", addressValid = null) {
    const resData = await saveData("/sc-api/forms/save-motorcycle", formData, MCNextBtn, form, action, addressValid);

    if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;

    return resData;
  }

  // *********************************************
  //              STEP-1 FUNCTIONALITY
  // *********************************************
  function MCspouseOperatorFunc() {
    const cohabOperator = document.getElementById("cohabitantIsOperator");
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

  // ===================
  // Add driver functionality
  const MCaddDriver = document.getElementById("MCaddDriver");
  MCaddDriver?.addEventListener("click", function () {
    if (!formList?.includes("additional_driver")) {
      const summaryIndex = formList?.indexOf("driver_summary_form");
      formList?.splice(summaryIndex, 0, "additional_driver");
    }
    showActiveForm(MCStep, MCBackBtn);

    // Set VehicleId dynamically
    MCdriverId = MCdriverArr.length;
    for (let i = 0; i < MCmaxDriverItem; i++) {
      const vId = MCdriverArr[i]?.MCdriverId;

      if (i != vId) {
        MCdriverId = i;
        break;
      }

      MCdriverId;
    }

    // if (MCdriverArr.length >= MCmaxDriverItem) MCaddDriver.disabled = true;

    const fields = document.querySelectorAll(".additional_driver .field__input");
    fields.forEach((field) => {
      field.value = "";

      const fieldName = field.getAttribute("data-field");
      const property = `additionalDriver${MCdriverId}${fieldName}`;

      field.id = field.name = property;
    });
  });

  // Driver Functionality
  function MCdriverSummaryFunc() {
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
    const cohabOperatorVal = document.getElementById("cohabitantIsOperator")?.value;
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
    MCdriverArr = MCdriverArr.filter((item) => item !== "deleted");

    // if all data not appended then Append Data to #MCaddDriversList
    if (MCdriverArr.length > 0) {
      const MCaddDriversList = document.querySelector("#MCaddDriversList");

      MCaddDriversList.innerHTML = "";
      const driverDemoItem = document.querySelector(".quote_request__summary_item.driverDemoItem");

      // Clone the demo, create and append
      MCdriverArr.forEach((info) => {
        const clonedItem = driverDemoItem.cloneNode(true);
        clonedItem.classList.remove("__hide", "driverDemoItem");
        clonedItem.setAttribute("data-id", info.MCdriverId);

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
        MCaddDriversList.appendChild(clonedItem);
      });
    }

    MCdriverArr.forEach((info) => (formData = { ...formData, ...info }));
    delete formData?.MCdriverId;

    // Print Driver Summary Heading
    driverCount = driverCount + MCdriverArr.length;
    summaryHeading.innerHTML = `Your Policy Has ${driverCount} ${driverCount > 1 ? "Drivers" : "Driver"}`;

    // Disable Add Button if max
    if (MCdriverArr.length >= MCmaxDriverItem) MCaddDriver.disabled = true;

    MCrunDriverItemsFunctionality();
  }

  // ********** FUNCTIONALITY OF MORE VEHICLE FORMS : Edit, Delete ***********
  function MCrunDriverItemsFunctionality() {
    // policyholderEditBtn Functionality
    const policyholderEditBtn = document.getElementById("policyholderEditBtn");
    policyholderEditBtn.addEventListener("click", () => {
      MCStep = formList?.indexOf("policyholder_form");
      showActiveForm(MCStep, MCBackBtn);
    });

    // spouseEditBtn Functionality
    const spouseEditBtn = document.getElementById("spouseEditBtn");
    spouseEditBtn.addEventListener("click", () => {
      MCStep = formList?.indexOf("spouse_information");
      showActiveForm(MCStep, MCBackBtn);
    });

    // Others Driver Functionality
    const MCaddDriversList = document.getElementById("MCaddDriversList");
    const driverItemEl = MCaddDriversList.querySelectorAll(".quote_request__summary_item.driverItem");

    driverItemEl.forEach((item, itemIndex) => {
      const MCdriverId = item.getAttribute("data-id");

      const editBtn = item.querySelector(".editBtn");
      const deleteBtn = item.querySelector(".deleteBtn");
      const deleteYes = item.querySelector(".deleteYes");
      const deleteNo = item.querySelector(".deleteNo");

      editBtn?.addEventListener("click", () => {
        MCeditDriverIndex = MCdriverId;

        if (!formList?.includes("additional_driver")) {
          const summaryIndex = formList?.indexOf("driver_summary_form");
          formList?.splice(summaryIndex, 0, "additional_driver");

          showActiveForm(MCStep, MCBackBtn);

          // Assign the values
          const aDrFields = document.querySelectorAll(".additional_driver .field__input");
          aDrFields.forEach((f) => {
            const property = "additionalDriver" + MCdriverId + f.getAttribute("data-field");
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
        for (const k in MCdriverArr[itemIndex]) {
          delete formData[k];
        }

        // MCdriverArr[itemIndex + 1] = "deleted";
        MCdriverArr = MCdriverArr.filter((item, i) => i !== itemIndex);

        item.classList.add("__hide");
        item.remove(); // delete elements

        MCaddDriver.disabled = false;
      });
    });
  }

  // *********************************************
  //              STEP-1 Validation
  // *********************************************

  function MCaddDriverValidation() {
    const isValidate = validateForm("additional_driver", false);

    if (isValidate) {
      const driverData = {};

      const allFields = document.querySelectorAll(`.additional_driver .field__input`);

      allFields.forEach((field) => {
        driverData[field.name] = field.value;
      });

      // UPDATE or CREATE Vehicle Data
      if (Number(MCeditDriverIndex) >= 0) {
        const matchId = MCdriverArr.filter((v) => v.MCdriverId == MCeditDriverIndex);
        const updatedData = { ...matchId[0], ...driverData };

        MCdriverArr = MCdriverArr.map((vData) => (vData.MCdriverId == Number(MCeditDriverIndex) ? updatedData : vData));

        // MCdriverArr[Number(MCdriverId)] = driverData;
        MCeditDriverIndex = -1;
      } else {
        driverData.MCdriverId = MCdriverId;

        MCdriverArr.push(driverData);
      }

      // REDUCE MCStep and REMOVE driver_summary_form from the formList
      const summaryIndex = formList?.indexOf("driver_summary_form");
      MCStep = summaryIndex - 2;
      formList = formList?.filter((item) => item != "additional_driver");
    }

    return isValidate;
  }

  // *********************************************
  //              STEP-2 FUNCTIONALITY
  // *********************************************
  // ********** "+ Add Vehicle" BUTTON FUNCTIONALITY  ***********
  const MCaddVehicle = document.getElementById("MCaddVehicle");

  MCaddVehicle?.addEventListener("click", function () {
    const fields = document.querySelectorAll(".add_vehicle_form .field__input");
    const errMsgs = document.querySelectorAll(".add_vehicle_form .error");

    // Fill Bank the Form
    fields.forEach((field) => {
      field.value = "";

      setMatchedData(false);
      document.querySelector(".vehicleAddress").value = "";

      const isSameAddressEl = document.querySelector(".AddressSameAsMailing--true");
      isSameAddressEl.value = "true";
      isSameAddressEl.checked = false;
    });

    // Remove all Error messages
    errMsgs.forEach((err) => err.remove());

    if (!formList?.includes("add_vehicle_form")) {
      const summaryIndex = formList?.indexOf("summary__form");
      formList?.splice(summaryIndex, 0, "add_vehicle_form");
    }
    showActiveForm(MCStep, MCBackBtn);

    // Set VehicleId dynamically
    MCvehicleId = MCvehicles.length;
    for (let i = 0; i < MCmaxVehicleItem; i++) {
      const vId = MCvehicles[i]?.MCvehicleId;

      if (i != vId) {
        MCvehicleId = i;
        break;
      }

      MCvehicleId;
    }

    if (MCvehicles.length >= MCmaxVehicleItem) this.disabled = true;

    //set field name and id
    const allFields = document.querySelectorAll(`.add_vehicle_form .field__input`);

    allFields.forEach((field) => {
      const fieldName = field.getAttribute("data-field");
      const property = `vehicle${MCvehicleId}${fieldName}`;

      field.id = property;
      field.name = property;
    });

    // make Dynamic vehicleSameAsMailingLabel for
    const vehicleSameAsMailing = document.querySelector(".add_vehicle_form .AddressSameAsMailing--true");
    const vehicleSameAsMailingLabel = document.querySelector(".add_vehicle_form .AddressSameAsMailing--label");
    vehicleSameAsMailingLabel.setAttribute("for", vehicleSameAsMailing.id);
  });

  // ********** FUNCTIONALITY OF MORE VEHICLE FORMS : Edit, Delete ***********
  function MCrunVehicleItemsFunctionality() {
    const moreVehicles = document.getElementById("moreVehicles");
    const moreVehicleItems = moreVehicles.querySelectorAll(".quote_request__summary_item");

    moreVehicleItems.forEach((item, itemIndex) => {
      const MCvehicleId = item.getAttribute("data-id");

      const editBtn = item.querySelector(".editBtn");
      const deleteBtn = item.querySelector(".deleteBtn");
      const deleteYes = item.querySelector(".deleteYes");
      const deleteNo = item.querySelector(".deleteNo");

      editBtn?.addEventListener("click", () => {
        MCeditVehicleIndex = Number(MCvehicleId);

        if (!formList?.includes("add_vehicle_form")) {
          const summaryIndex = formList?.indexOf("summary__form");
          formList?.splice(summaryIndex, 0, "add_vehicle_form");

          showActiveForm(MCStep, MCBackBtn);

          // Assign the values
          const vclFields = document.querySelectorAll(".add_vehicle_form .field__input");
          vclFields.forEach((f) => {
            const property = "vehicle" + MCvehicleId + f.getAttribute("data-field");
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
        for (const k in MCvehicles[itemIndex]) {
          delete formData[k];
        }

        // MCvehicles[itemIndex] = "deleted";
        MCvehicles = MCvehicles.filter((item, i) => i !== itemIndex);

        item.classList.add("__hide");
        item.remove(); // delete elements

        MCaddVehicle.disabled = false;
      });
    });
  }

  // *********************************************
  //              STEP-2 VALIDATION
  // *********************************************

  function MCsummaryFunctionality() {
    const summaryHeading = document.querySelector(".summary__form .quote_request_heading");
    summaryHeading.innerHTML = `Your Policy Has ${MCvehicles.length} ${MCvehicles.length > 1 ? "Vehicles" : "Vehicle"} `;

    // If Main Vehicle Data OKK then direct show SUMMARY neither show add_vehicle_form
    if (!MCvehicles.length > 0) {
      if (!formList?.includes("add_vehicle_form")) {
        const summaryIndex = formList?.indexOf("summary__form");

        formList?.splice(summaryIndex, 0, "add_vehicle_form");
      }

      showActiveForm(MCStep, MCBackBtn);
    } else {
      formList = formList?.filter((form) => form != "add_vehicle_form");
    }

    // Add all data to moreVehicles sections
    MCvehicles = MCvehicles.filter((item) => item !== "deleted");

    const addedSummary = document.querySelector("#moreVehicles");
    // const totalAdded = addedSummary.children?.length;

    // if all data not appended then Append Data to #moreVehicles
    if (MCvehicles.length > 0) {
      addedSummary.innerHTML = "";
      const demoItem = document.querySelector(".quote_request__summary_item.demoItem");
      // Clone the demo, create and append
      MCvehicles.forEach((info, i) => {
        const clonedItem = demoItem.cloneNode(true);
        clonedItem.classList.remove("__hide", "demoItem");
        clonedItem.setAttribute("data-id", info.MCvehicleId);

        if (info.MCvehicleId == 0) clonedItem.querySelector("#deleteBtn").remove();

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
    // const filterCVehicles = MCvehicles.map((data) => {
    //   delete data.MCvehicleId;
    //   return data;
    // });

    MCvehicles.forEach((info) => (formData = { ...formData, ...info }));
    delete formData.MCvehicleId;

    MCrunVehicleItemsFunctionality();
  }

  function MCaddVehicleValidation() {
    const isValidate = validateForm("add_vehicle_form", false);

    if (isValidate) {
      const vehicleData = {};

      const allFields = document.querySelectorAll(`.add_vehicle_form .field__input`);

      allFields.forEach((field) => {
        if (field.type === "checkbox") vehicleData[field.name] = field.checked;
        else vehicleData[field.name] = field.value;
      });

      // UPDATE or CREATE Vehicle Data
      if (Number(MCeditVehicleIndex) >= 0) {
        const matchId = MCvehicles.filter((v) => v.MCvehicleId == Number(MCeditVehicleIndex));
        const updatedData = { ...matchId[0], ...vehicleData };

        MCvehicles = MCvehicles.map((vData) => (vData.MCvehicleId == MCeditVehicleIndex ? updatedData : vData));

        // MCvehicles[Number(MCvehicleId)] = vehicleData;
        MCeditVehicleIndex = -1;
      } else {
        vehicleData.MCvehicleId = MCvehicleId;

        MCvehicles.push(vehicleData);
      }

      // REDUCE MCStep and REMOVE add_vehicle_form from the formList
      const summaryIndex = formList?.indexOf("summary__form");
      MCStep = summaryIndex - 2;
      formList = formList?.filter((item) => item != "add_vehicle_form");
    }

    return isValidate;
  }

  // Same Mailing Address Functionality
  function setMatchedData(disability) {
    const isSameAddressEl = document.querySelector(".AddressSameAsMailing--true");

    const MCaddVehicleFields = document.querySelectorAll(".vehicleFullAddress .field__input");

    MCaddVehicleFields.forEach((el) => {
      el.disabled = disability;
      isSameAddressEl.disabled = false;

      const dataMatch = el.getAttribute("data-match");
      if (dataMatch) el.value = formData[dataMatch];

      if (disability === true) {
        const WCerrMsg = document.querySelectorAll(".vehicleFullAddress .error");
        WCerrMsg.forEach((el) => el.remove());
      }
    });
  }

  function MCaddVehicleFunc() {
    const isSameAddressEl = document.querySelector(".AddressSameAsMailing--true");
    isSameAddressEl.checked = false;
    //

    setMatchedData(false);
    document.querySelector(".vehicleAddress").value = "";

    // Same Mailing CheckBox Functionality
    isSameAddressEl?.addEventListener("change", () => {
      if (isSameAddressEl.checked) {
        setMatchedData(true);
      } else {
        const MCaddVehicleFields = document.querySelectorAll(".vehicleFullAddress .field__input");

        MCaddVehicleFields.forEach((el, i) => {
          el.value = "";
          el.disabled = false;

          isSameAddressEl.value = "true";

          if (i === 1) el.focus();
        });
      }
    });
  }

  // // *********************************************
  // //              STEP-3 FUNCTIONALITY
  // // *********************************************
  // const MCaddViolationBtn = document.getElementById("add_violation_btn");
  // const MCviolationsFields = document.querySelector(".violation_info_fields");
  // const MCviolationWrapper = document.getElementById("violation_info_fields_wrapper");

  // let MCvioSerial = 0;

  // // ******************* Violation Form Functionality *******************
  // // ADD MORE VIOLATIONS FIELDS
  // MCaddViolationBtn?.addEventListener("click", function () {
  //   MCvioSerial++;

  //   const newFields = MCviolationsFields.cloneNode(true);
  //   newFields.querySelectorAll(".field__input").forEach((field) => {
  //     field.value = "";

  //     const newId = field.id.replace("0", MCvioSerial);
  //     field.id = field.name = newId;
  //   });

  //   // for new fields : clearFieldErrorMsg
  //   newFields.querySelectorAll(".error").forEach((errField) => errField.remove());

  //   MCviolationWrapper?.appendChild(newFields);

  //   // Data Validator added
  //   document.querySelectorAll(".householdViolationsDate").forEach((vDate) => dateValidation(vDate, thisYear));

  //   if (MCviolationWrapper?.children.length >= 5) {
  //     this.disabled = true;
  //   }

  //   removeErrorOnChange();
  // });

  // // IF householdViolationsPreviousClaims value not== Yes, then disable all
  // function MCdisableViolationInputs(disable = true) {
  //   const violationInputs = MCviolationWrapper?.querySelectorAll(".field__input");
  //   violationInputs.forEach((input) => (input.disabled = disable));
  //   MCaddViolationBtn.disabled = disable;
  // }

  // const MChasViolationsFields = document.getElementsByName("householdViolationsPreviousClaims");
  // const MCgetViolationsValue = () => {
  //   let value = "";
  //   MChasViolationsFields?.forEach((field) => {
  //     if (field?.checked) value = field.value;
  //   });

  //   return value;
  // };

  // // Get every violation Radio field's value
  // MChasViolationsFields.forEach((fields) => {
  //   fields?.addEventListener("change", () => {
  //     let getValue = MCgetViolationsValue();

  //     if (getValue === "Yes") {
  //       MCdisableViolationInputs(false);
  //     } else {
  //       MCdisableViolationInputs(true);
  //     }

  //     const fieldContainer = document.querySelector(".violations_form");
  //     if (fieldContainer) {
  //       const errors = fieldContainer.querySelectorAll(".field_message.error");
  //       errors.forEach((error) => error.remove());
  //     }
  //   });
  // });

  // // **** coverageLimitsValidation 'qrf-accordion' Functionality ****
  // const MCaccordionButtons = document.querySelectorAll(".qrf-accordion__trigger");

  // MCaccordionButtons?.forEach((button) => {
  //   button.addEventListener("click", () => {
  //     const accordion = button.closest(".qrf-accordion");
  //     accordion.classList.toggle("qrf-accordion--active");

  //     const symbol = button.querySelector(".qrf_accordion");
  //     if (accordion.classList.contains("qrf-accordion--active")) symbol.innerHTML = "-";
  //     else symbol.innerHTML = "+";
  //   });
  // });

  // ********* FUNCTIONALITY physical_damage_form *********
  function MCfuncDamageForm() {
    const damageForm = document.querySelector(".damage__form.__hide");
    const DamageFormWrapper = document.getElementById("physical_damage_form_wrapper");

    // Clear DamageFormWrapper Children
    DamageFormWrapper.innerHTML = "";

    // Add Vehicle data to DamageFormWrapper with other fields
    MCvehicles.forEach((vData, index) => {
      const vId = vData.MCvehicleId;
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
      const vehicleCollisionDeductible = clonedItem.querySelector(".vehicleCollisionDeductible");
      vehicleCollisionDeductible.id = vehicleCollisionDeductible.name = `vehicle${vId}CollisionDeductible`;

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
  function MCviolationsValidation() {
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
          // result = true;
          results.push(true);
        } else {
          // result = false;
          results.push(false);
        }
      });

      const isAllValid = results.every((result) => result === true);

      // return result;
      return isAllValid;
    } else {
      const fieldContainer = document.querySelector(".has_violation_inputs_container");
      isValueEmpty(fieldContainer);

      return false;
    }
  }

  function MCphysicalDamageValidation() {
    const fieldError = MCvehicles.map((vData) => {
      const vId = vData.MCvehicleId;

      const radioFields = document.querySelectorAll(`input[name=vehicle${vId}LiabilityOnlyCoverage]`);

      const fieldChecked = document.querySelector(`input[name=vehicle${vId}LiabilityOnlyCoverage]:checked`);

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
        const vId = MCvehicles[i].MCvehicleId;

        const liaCoVal = damageForm.querySelector(`input[name=vehicle${vId}LiabilityOnlyCoverage]:checked`)?.value;

        MCvehicles[i][`vehicle${vId}LiabilityOnlyCoverage`] = liaCoVal;

        if (liaCoVal === "No") {
          const comVal = damageForm.querySelector(".field__input.vehicleComprehensiveDeductible")?.value;
          const colVal = damageForm.querySelector(".field__input.vehicleCollisionDeductible")?.value;

          MCvehicles[i][`vehicle${vId}ComprehensiveDeductible`] = comVal;
          MCvehicles[i][`vehicle${vId}CollisionDeductible`] = colVal;
        }
      });
    }

    MCsummaryFunctionality();

    return isValidate;
  }

  // *********************************************
  //              STEP-4 VALIDATION
  // *********************************************

  // Note: Step 1, 4 is in formCommon.js file
});

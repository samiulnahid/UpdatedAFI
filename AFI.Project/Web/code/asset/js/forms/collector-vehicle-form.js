window.addEventListener("load", () => {
  // Forms
  const vehicleForms = [
    "policyholder_form",
    // "add_vehicle__form", // dynamically added
    "summary__form",
    "violations__form",
    "coverage_limits_form",
    "physical_damage_form",
    "coverage_history_form",
  ];

  let collectorVehicles = [];
  let vehicleId = 0;
  let editVehicleIndex = -9999;
  const maxVehicleItem = 4;

  // *********************************************
  //       FORM SUBMISSION AND STEP HANDLING
  // *********************************************
  const vehicleNextBtn = document.querySelector("#vehicleNextBtn");
  const vehicleBackBtn = document.querySelector("#vehicleBackBtn");

  let vehicleStep = 0;
  let vehicleMaxStep = formList?.length - 1;

  // ***** NEXT FUNCTIONALITY *****
  pressEnterToSubmit(vehicleNextBtn);
  vehicleNextBtn?.addEventListener("click", async () => {
    if (vehicleStep === 0) {
      const isSelectEligibility = eligibilityValidation(vehicleForms);
      if (!Boolean(isSelectEligibility)) return false;

      militaryFormFunc();
    }
    //  HANDLE ALL FORM SUBMISSIONS AND STEP VALIDATION
    const submitResult = await handleVehicleStepForm(vehicleStep);
    if (!submitResult) return false;

    // Step Increment
    vehicleMaxStep = formList?.length - 1;
    vehicleStep >= vehicleMaxStep ? vehicleStep : vehicleStep++;

    // Show Form
    showActiveForm(vehicleStep, vehicleBackBtn);
  });

  // Back
  vehicleBackBtn?.addEventListener("click", () => {
    // Step Decrement
    vehicleStep <= 0 ? vehicleStep : vehicleStep--;

    // 2 side back for add_more_vehicle_form
    if (vehicleStep + 1 === formList?.indexOf("add_more_vehicle_form")) {
      formList = formList?.filter((item) => item != "add_more_vehicle_form");
      vehicleStep = formList?.indexOf("summary__form");
      editVehicleIndex = -9999;
    }

    // 2 side back for add_vehicle__form
    if (isEditMainVehicle && vehicleStep + 1 === formList?.indexOf("add_vehicle__form")) {
      formList = formList?.filter((item) => item != "add_vehicle__form");
      vehicleStep = formList?.indexOf("summary__form");
      editVehicleIndex = -9999;
    }
    showActiveForm(vehicleStep, vehicleBackBtn);
  });

  // =*********************************************
  //       HANDLING MULTI-STEP FORMS
  // =*********************************************
  async function handleVehicleStepForm(step) {
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

      // Save Data
      const resData = await saveCollectorVehicle("policyholder_form", "send", "collector vehicle");
      //const resData = await saveCollectorVehicle("policyholder_form");
      if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;
    }
    if (step === formList?.indexOf("spouse_information")) {
      if (!validateForm("spouse_information")) return false;

      // Save Data
      const resData = await saveCollectorVehicle("spouse_information");
      if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;
    }

    //
    if (step === formList?.indexOf("add_vehicle__form")) {
      if (!addVehicleValidation()) return false;

      updateArrToFormData(collectorVehicles);

      // Save Data
      const resData = await saveCollectorVehicle("add_vehicle__form");
      if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;

      // REDUCE vehicleStep cz add_vehicle__form will remove from the formList
      const summaryIndex = formList?.indexOf("summary__form");
      vehicleStep = summaryIndex - 2;

      // show summary
      summaryFunctionality();
    }

    if (step === formList?.indexOf("add_more_vehicle_form")) {
      if (!addMoreVehicleValidation()) return false;

      updateArrToFormData(collectorVehicles);

      // Save Data
      const resData = await saveCollectorVehicle("add_more_vehicle_form");
      if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;

      // REDUCE vehicleStep and REMOVE add_more_vehicle_form from the formList
      const summaryIndex = formList?.indexOf("summary__form");
      vehicleStep = summaryIndex - 2;
      formList = formList?.filter((item) => item != "add_more_vehicle_form");

      // show summary
      summaryFunctionality();
    }

    if (step === formList?.indexOf("summary__form") || step === formList?.indexOf("summary__form") - 1) {
      summaryFunctionality();
      // disableViolationInputs(true);
    }

    // ****
    if (step === formList?.indexOf("violations__form")) {
      if (!violationsValidation()) return false;

      // Save Data
      const resData = await saveCollectorVehicle("violations__form");
      if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;
    }

    if (step === formList?.indexOf("coverage_limits_form")) {
      if (!validateForm("coverage_limits_form")) return false;
      functionalityForEachDamageForm();

      // Save Data
      const resData = await saveCollectorVehicle("coverage_limits_form");
      if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;
    }

    if (step === formList?.indexOf("physical_damage_form")) {
      if (!physicalDamageValidation()) return false;
      coverageHistoryFunc();

      // Save Data
      const resData = await saveCollectorVehicle("physical_damage_form");
      if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;
    }
    if (step === formList?.indexOf("coverage_history_form")) {
      if (!validateForm("coverage_history_form")) return false;

      // Save Data
      const resData = await saveCollectorVehicle("coverage_history_form", "submit");
      if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;

      // Go to Thank You Page
      window.location.href = successRedirection;
    }

    // Run after every submission

    return true;
  }

  async function saveCollectorVehicle(form, action = "send", addressValid = null) {
    const resData = await saveData("/sc-api/forms/save-collectorvehicle", formData, vehicleNextBtn, form, action, addressValid);

    return resData;
  }

  // *********************************************
  //              STEP-2 FUNCTIONALITY
  // *********************************************

  // ********** "+ Add Vehicle" BUTTON FUNCTIONALITY  ***********
  const CVaddVehicle = document.getElementById("CVaddVehicle");

  CVaddVehicle?.addEventListener("click", function () {
    const fields = document.querySelectorAll(".add_more_vehicle_form .field__input");
    fields.forEach((field) => (field.value = ""));

    if (!formList?.includes("add_more_vehicle_form")) {
      const summaryIndex = formList?.indexOf("summary__form");
      formList?.splice(summaryIndex, 0, "add_more_vehicle_form");
    }
    showActiveForm(vehicleStep, vehicleBackBtn);

    // Set VehicleId dynamically
    vehicleId = collectorVehicles.length;
    for (let i = 0; i < maxVehicleItem; i++) {
      const vId = collectorVehicles[i]?.vehicleId;

      if (i != vId) {
        vehicleId = i;
        break;
      }

      vehicleId;
    }

    if (collectorVehicles.length >= maxVehicleItem) this.disabled = true;

    //set field name and id
    const allFields = document.querySelectorAll(`.add_more_vehicle_form .field__input`);

    allFields.forEach((field) => {
      const fieldName = field.getAttribute("data-field");
      const property = `vehicle${vehicleId}${fieldName}`;

      field.id = property;
      field.name = property;
    });
  });

  // ********** FUNCTIONALITY OF VEHICLE FORM : Edit ***********
  const mainVehicleEditBtn = document.getElementById("mainVehicleEditBtn");
  let isEditMainVehicle = false;
  mainVehicleEditBtn?.addEventListener("click", () => {
    const summaryIndex = formList?.indexOf("summary__form");

    if (!formList?.includes("add_vehicle__form")) {
      formList?.splice(summaryIndex, 0, "add_vehicle__form");
    }

    isEditMainVehicle = true;
    showActiveForm(vehicleStep, vehicleBackBtn);
  });

  // ********** FUNCTIONALITY OF MORE VEHICLE FORMS : Edit, Delete ***********
  function runVehicleItemsFunctionality() {
    const moreVehicles = document.getElementById("moreVehicles");
    const moreVehicleItems = moreVehicles.querySelectorAll(".quote_request__summary_item");

    moreVehicleItems.forEach((item, itemIndex) => {
      const vehicleId = item.getAttribute("data-id");

      const editBtn = item.querySelector(".editBtn");
      const deleteBtn = item.querySelector(".deleteBtn");
      const deleteYes = item.querySelector(".deleteYes");
      const deleteNo = item.querySelector(".deleteNo");

      editBtn?.addEventListener("click", () => {
        editVehicleIndex = vehicleId;

        if (!formList?.includes("add_more_vehicle_form")) {
          const summaryIndex = formList?.indexOf("summary__form");
          formList?.splice(summaryIndex, 0, "add_more_vehicle_form");

          showActiveForm(vehicleStep, vehicleBackBtn);

          //set field name and id
          const allFields = document.querySelectorAll(`.add_more_vehicle_form .field__input`);
          allFields.forEach((field) => {
            const fieldName = field.getAttribute("data-field");
            const property = `vehicle${vehicleId}${fieldName}`;

            field.id = field.name = property;
          });

          // Assign the values
          function editFormWithValue(type) {
            document.getElementById(`vehicle${vehicleId}${type}`).value = formData[`vehicle${vehicleId}${type}`];
          }

          editFormWithValue("Year");
          editFormWithValue("Make");
          editFormWithValue("Model");
          editFormWithValue("Type");
          editFormWithValue("EstimatedValue");
          editFormWithValue("Storage");
          editFormWithValue("DriveDescription");
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
        for (const k in collectorVehicles[itemIndex + 1]) {
          delete formData[k];
        }

        // collectorVehicles[itemIndex + 1] = "deleted";
        collectorVehicles = collectorVehicles.filter((item, i) => i !== itemIndex + 1);

        item.classList.add("__hide");
        item.remove(); // delete elements

        CVaddVehicle.disabled = false;
      });
    });
  }

  // *********************************************
  //              STEP-2 VALIDATION
  // *********************************************

  function summaryFunctionality() {
    const summaryHeading = document.querySelector(".summary__form .quote_request_heading");

    summaryHeading.innerHTML = `Your Policy Has ${collectorVehicles.length} ${
      collectorVehicles.length > 1 ? "Vehicles" : "Vehicle"
    } `;
    //
    // Check Main Vehicle data OKK or Not
    const mainVehicleFields = document.querySelectorAll(".add_vehicle__form .field__input");

    const mainVehicleValues = [];
    mainVehicleFields.forEach((field) => mainVehicleValues.push(field.value));

    const haveAllMainVehicleValues = mainVehicleValues.every((v) => Boolean(v) === true);

    // If Main Vehicle Data OKK then direct show SUMMARY neither show add_vehicle__form
    if (!haveAllMainVehicleValues) {
      if (!formList?.includes("add_vehicle__form")) {
        const summaryIndex = formList?.indexOf("summary__form");

        formList?.splice(summaryIndex, 0, "add_vehicle__form");
      }

      showActiveForm(vehicleStep, vehicleBackBtn);
    } else {
      formList = formList?.filter((form) => form != "add_vehicle__form");
      // show data in Summary
      if (collectorVehicles.length > 0) {
        const { vehicle0Year, vehicle0Make, vehicle0Model } = formData;
        document.querySelector(
          ".quote_request__summary_main_item_info"
        ).innerText = `${vehicle0Year} ${vehicle0Make} ${vehicle0Model}`;
      }
    }

    // Add all data to moreVehicles sections
    collectorVehicles = collectorVehicles.filter((item) => item !== "deleted");

    const moreVehicles = collectorVehicles.filter((item, index) => index > 0);

    const addedSummary = document.querySelector("#moreVehicles");
    // const totalAdded = addedSummary.children?.length;

    // if all data not appended then Append Data to #moreVehicles
    if (moreVehicles.length > 0) {
      addedSummary.innerHTML = "";
      const demoItem = document.querySelector(".quote_request__summary_item.demoItem");
      // Clone the demo, create and append
      moreVehicles.forEach((info, i) => {
        const clonedItem = demoItem.cloneNode(true);
        clonedItem.classList.remove("__hide", "demoItem");
        clonedItem.setAttribute("data-id", info.vehicleId);

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
    // const filterCVehicles = collectorVehicles.map((data) => {
    //   delete data.vehicleId;
    //   return data;
    // });

    collectorVehicles.forEach((info) => (formData = { ...formData, ...info }));
    delete formData.vehicleId;

    runVehicleItemsFunctionality();
  }

  function addVehicleValidation() {
    const isValidate = validateForm("add_vehicle__form", false);

    if (isValidate) {
      collectorVehicles[0] = {};

      const allFields = document.querySelectorAll(`.add_vehicle__form .field__input`);

      allFields.forEach((field) => {
        collectorVehicles[0][field.name] = field.value;
        collectorVehicles[0].vehicleId = 0;
      });
    }

    return isValidate;
  }

  function addMoreVehicleValidation() {
    const isValidate = validateForm("add_more_vehicle_form", false);

    if (isValidate) {
      const vehicleData = {};

      const allFields = document.querySelectorAll(`.add_more_vehicle_form .field__input`);

      allFields.forEach((field) => {
        vehicleData[field.name] = field.value;
      });

      // UPDATE or CREATE Vehicle Data
      if (editVehicleIndex > 0) {
        const matchId = collectorVehicles.filter((v) => v.vehicleId == editVehicleIndex);
        const updatedData = { ...matchId[0], ...vehicleData };

        collectorVehicles = collectorVehicles.map((vData) => (vData.vehicleId == editVehicleIndex ? updatedData : vData));

        // collectorVehicles[Number(vehicleId)] = vehicleData;
        editVehicleIndex = -1;
      } else {
        vehicleData.vehicleId = vehicleId;

        collectorVehicles.push(vehicleData);
      }
    }

    return isValidate;
  }

  // *********************************************
  //              STEP-3 FUNCTIONALITY
  // *********************************************
  // ********* FUNCTIONALITY physical_damage_form *********
  function functionalityForEachDamageForm() {
    const damageForm = document.querySelector(".damage__form.__hide");
    const DamageFormWrapper = document.getElementById("physical_damage_form_wrapper");

    // Clear DamageFormWrapper Children
    DamageFormWrapper.innerHTML = "";

    // const vehicleList = collectorVehicles;
    // Add Vehicle data to DamageFormWrapper with other fields
    collectorVehicles.forEach((vData, index) => {
      const vId = vData.vehicleId;
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

      liabilityYesLabel.setAttribute("for", liabilityYesLabel);
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
  function violationsValidation() {
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

      // return result;
      return isAllValid;
    } else {
      const fieldContainer = document.querySelector(".has_violation_inputs_container");
      isValueEmpty(fieldContainer);

      return false;
    }
  }

  function physicalDamageValidation() {
    const fieldError = collectorVehicles.map((vData) => {
      const vId = vData.vehicleId;

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
        const vId = collectorVehicles[i].vehicleId;

        const liaCoVal = damageForm.querySelector(`input[name=vehicle${vId}LiabilityOnlyCoverage]:checked`)?.value;

        collectorVehicles[i][`vehicle${vId}LiabilityOnlyCoverage`] = liaCoVal;

        if (liaCoVal === "No") {
          const comVal = damageForm.querySelector(".field__input.vehicleComprehensiveDeductible")?.value;
          const colVal = damageForm.querySelector(".field__input.vehicleCollisionDeductible")?.value;

          collectorVehicles[i][`vehicle${vId}ComprehensiveDeductible`] = comVal;
          collectorVehicles[i][`vehicle${vId}CollisionDeductible`] = colVal;
        }
      });
    }

    summaryFunctionality();

    return isValidate;
  }

  // *********************************************
  //              STEP-4 VALIDATION
  // *********************************************

  // Note: Step 1, 4 is in formCommon.js file
});

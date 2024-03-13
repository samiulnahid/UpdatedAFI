// Forms
const smallFormList = ["about_yourself", "email_confirmation"];

// *********************************************
//       FORM SUBMISSION AND STEP HANDLING
// *********************************************
const small_next_btn = document.querySelector("#small_next_btn");
const small_back_btn = document.querySelector("#small_back_btn");
const small_submit_btn = document.querySelector("#small_submit_btn");
const small_back_btn2 = document.querySelector("#small_back_btn2");

let smallStep = 0;
let smallMaxStep = smallFormList.length - 1;

// ***** NEXT FUNCTIONALITY *****
pressEnterToSubmit(small_next_btn);

small_next_btn?.addEventListener("click", async () => {
  if (smallStep === smallFormList.indexOf("about_yourself")) {
    // Step 1
    if (!validateForm("about_yourself")) return false;

    document.querySelector("#submitQuoteContactInfo").value = formData.policyHolderEmail;
    document.querySelector(".quote_request__action_buttons").classList.add("hide");

    // API Call
    const resData = await saveData("/sc-api/forms/save-hillafb", formData, small_next_btn, "about_yourself", "send");
    if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;
  }

  // Step 2
  if (smallStep === smallFormList.indexOf("email_confirmation")) {
    if (!validateForm("email_confirmation")) return false;

    // extra data addition
    formData.submitQuoteContactMethod = "Email";
    formData.policyHolderPrimaryResidenceZip = null;
    formData.policyHolderMaritalStatus = "";
    formData.IsRecaptchaValid = true;

    // API Call
    const resData = await saveData("/sc-api/forms/save-hillafb", formData, small_submit_btn, "email_confirmation", "submit");

    // small_submit_btn back to prev innerText
    const afbSubmitBtn = document.getElementById("small_submit_btn");
    afbSubmitBtn.innerText = "Submit Request for Quote";

    if (!resData || !resData.QuoteId || resData.QuoteId <= 0) return false;

    // Go to Thank You Page
    window.location.href = "/quote-forms-submit-success-hill";

    return;
  }

  // Step Increment
  smallStep >= smallMaxStep ? smallStep : smallStep++;

  // Show Form
  smallActiveForm(smallMaxStep);
});

// Back
small_back_btn?.addEventListener("click", () => {
  // Step Decrement
  smallStep <= 0 ? smallStep : smallStep--;

  smallActiveForm(smallStep);
});

small_back_btn2?.addEventListener("click", () => {
  small_back_btn.click();
  document.querySelector(".quote_request__action_buttons").classList.remove("hide");
});

small_submit_btn?.addEventListener("click", () => small_next_btn.click());

function smallActiveForm(step) {
  console.log(formData);

  // remove active_section class from everywhere
  document.querySelector(".active_section")?.classList.remove("active_section");

  // set active_section class
  document.querySelector(`.${smallFormList[step]}`)?.classList.add("active_section");

  // Conditionally Hide Back Btn
  step <= 0 ? small_back_btn.classList.add("hide") : small_back_btn?.classList.remove("hide");
}

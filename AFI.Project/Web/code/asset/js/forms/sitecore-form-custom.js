const eligibilityFields  = document.querySelectorAll(".Eligibility__Form .fIeld__radio input");
eligibilityFields?.forEach(field =>{
    field.addEventListener("change", ()=> {
        document.querySelector(".checked__label")?.classList.remove("checked__label")
        field.parentElement.classList.add("checked__label");
    })
})
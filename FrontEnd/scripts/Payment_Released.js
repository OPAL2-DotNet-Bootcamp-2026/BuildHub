"use strict";
const agreementValue = 1200;
const feePercentage = 0.02;
const fee = agreementValue * feePercentage;
const total = agreementValue + fee;
const agreementElement = document.querySelector("#agreementValue");
const feeElement = document.querySelector("#buildHubFee");
const totalElement = document.querySelector("#totalPaid");
if (agreementElement) {
    agreementElement.textContent = `OMR ${agreementValue}`;
}
if (feeElement) {
    feeElement.textContent = `OMR ${fee}`;
}
if (totalElement) {
    totalElement.textContent = `OMR ${total}`;
}
const reviewButton = document.querySelector("#reviewButton");
reviewButton?.addEventListener("click", () => {
    window.location.href = "leave-review.html";
});

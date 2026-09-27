


const agreementValue: number = 1200;
const feePercentage: number = 0.02;

const fee: number = agreementValue * feePercentage;
const total: number = agreementValue + fee;

const agreementElement = document.querySelector<HTMLElement>("#agreementValue");

const feeElement = document.querySelector<HTMLElement>("#buildHubFee");

const totalElement = document.querySelector<HTMLElement>("#totalPaid");

if (agreementElement) { agreementElement.textContent = `OMR ${agreementValue}`;}

if (feeElement) { feeElement.textContent = `OMR ${fee}`;}

if (totalElement) {totalElement.textContent = `OMR ${total}`;}

const reviewButton = document.querySelector<HTMLButtonElement>("#reviewButton");

reviewButton?.addEventListener("click", () => { window.location.href = "leave-review.html";
});
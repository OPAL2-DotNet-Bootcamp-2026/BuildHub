const confirmPaymentBtn =
  document.getElementById("confirmPaymentBtn") as HTMLButtonElement;

  
// For testing with the existing Agreement in the database
const agreementId = 2;

confirmPaymentBtn.addEventListener("click", () => {
  window.location.href =
    `Release_payment.html?agreementId=${agreementId}`;
});


  const params = new URLSearchParams(window.location.search);

const confirmPaymentBtn =
  document.getElementById("confirmPaymentBtn") as HTMLButtonElement;

  
// For testing with the existing Agreement in the database
const agreementId = params.get("agreementId");

confirmPaymentBtn.addEventListener("click", () => {
  window.location.href =
    `Release_payment.html?agreementId=${agreementId}`;
});


const confirmPaymentBtn =
  document.getElementById("confirmPaymentBtn") as HTMLButtonElement;

confirmPaymentBtn.addEventListener("click", () => {
  window.location.href = "Release_payment.html";
});
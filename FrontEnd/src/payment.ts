 import { base_url } from "./base_url.js";
 

 interface PaymentCheckoutResponse {
  paymentId: number;
  checkoutUrl: string;
}
const token = localStorage.getItem("token");

const payButton =
  document.querySelector<HTMLButtonElement>("#payButton");

 const params = new URLSearchParams(window.location.search);

const confirmPaymentBtn =
  document.getElementById("confirmPaymentBtn") as HTMLButtonElement;

  
// For testing with the existing Agreement in the database
const agreementId = params.get("agreementId");

confirmPaymentBtn.addEventListener("click", () => {
  window.location.href =
    `Release_payment.html?agreementId=${agreementId}`;
});


// Start the Thawani payment
payButton?.addEventListener("click", async () => {
  if (!agreementId) {
    alert("Agreement ID is missing.");
    return;
  }

  if (!token) {
    alert("Please log in first.");
    return;
  }

  try {
    payButton.disabled = true;
    payButton.textContent = "Opening Thawani...";

    const response = await fetch(
      `${base_url}/api/Payments/${agreementId}/checkout`,
      {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`
        }
      }
    );

    if (!response.ok) {
      throw new Error(
        `Could not start payment. Status: ${response.status}`
      );
    }

    const checkout =
      await response.json() as PaymentCheckoutResponse;

    // Keep the payment ID for verification after Thawani returns
    localStorage.setItem(
      "pendingPaymentId",
      checkout.paymentId.toString()
    );

    // Redirect the homeowner to Thawani
    window.location.href = checkout.checkoutUrl;
  } catch (error: unknown) {
    const message =
      error instanceof Error
        ? error.message
        : "An unexpected error occurred.";

    alert(message);

    payButton.disabled = false;
    payButton.textContent =
      "Pay OMR 1,224 with Thawani";
  }
});

// Open the Release Payment page
confirmPaymentBtn?.addEventListener("click", () => {
  if (!agreementId) {
    alert("Agreement ID is missing.");
    return;
  }

  window.location.href =
    `Release_payment.html?agreementId=${agreementId}`;
});

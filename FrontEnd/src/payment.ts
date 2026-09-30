 import { base_url } from "./base_url.js";
 

 interface PaymentCheckoutResponse {
  paymentId: number;
  checkoutUrl: string;
}
const token = localStorage.getItem("token");
const params = new URLSearchParams(window.location.search);
const payButton = document.querySelector<HTMLButtonElement>("#payButton");


const confirmPaymentBtn = document.getElementById("confirmPaymentBtn") as HTMLButtonElement;

const checkoutSection =
  document.querySelector<HTMLElement>("#checkoutSection");

const releaseSection =
  document.querySelector<HTMLElement>("#releaseSection");

  
// For testing with the existing Agreement in the database
const agreementId = params.get("agreementId");

confirmPaymentBtn.addEventListener("click", () => {
  window.location.href =
    `Release_payment.html?agreementId=${agreementId}`;
});

// This function gets the payment status from the backend and
//  shows either the Pay with Thawani button or the Confirm and Release Payment button.
interface AgreementPaymentResponse {
  paymentStatus: number | string;
}

async function loadPaymentStatus(): Promise<void> {
  if (!agreementId || !token) {
    return;
  }

  try {
    const response = await fetch(
      `${base_url}/api/Agreements/${agreementId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`
        }
      }
    );

    if (!response.ok) {
      throw new Error("Could not get payment status.");
    }

    const agreement =
      await response.json() as AgreementPaymentResponse;

    const isPending =
      agreement.paymentStatus === 4 ||
      agreement.paymentStatus === "Pending";

    const isHeld =
      agreement.paymentStatus === 1 ||
      agreement.paymentStatus === "Held";

    if (checkoutSection) {
      checkoutSection.hidden = !isPending;
    }

    if (releaseSection) {
      releaseSection.hidden = !isHeld;
    }
  } catch (error) {
    console.error("Payment status error:", error);
  }
}


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

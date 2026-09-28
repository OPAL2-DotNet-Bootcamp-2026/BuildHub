import { base_url } from "./base_url.js";


const releasePaymentBtn =
  document.getElementById("releasePaymentBtn") as HTMLButtonElement;

  // Read the query parameters from the URL
  const params = new URLSearchParams(window.location.search);
const releaseAgreementId = params.get("agreementId");

// Get the login token to send it with the API request
const token = localStorage.getItem("token");

releasePaymentBtn.addEventListener("click", async () => {
   
      try {
    const response = await fetch(
      `${base_url}/api/Agreements/${releaseAgreementId}`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          paymentStatus: 2,
        }),
      }
    );

    if (!response.ok) {
  throw new Error("Failed to release payment");
}

window.location.href = "Payment_Released.html";
  } catch (error) {
    console.error(error);
  }
});
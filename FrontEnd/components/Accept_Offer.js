// Read the quote ID from the page URL
const parameters = new URLSearchParams(window.location.search);
const offerId = 4;

// Get the saved token from login
const token = localStorage.getItem("token");

// find the HTML buttons by ids
const acceptButton = document.querySelector("#acceptButton");
const cancelButton = document.querySelector("#cancelButton");
const agreedPrice =document.querySelector("#agreedPrice");
const timeline =document.querySelector("#timeline");

// GET 
async function getOfferInformation() {
if (!offerId) {
    alert("Offer ID is missing.");
    return;
    }

if (!token) {
     alert("Please log in first.");
     window.location.href = "login.html";
     return;
   }

try {
    const response = await fetch(
    `https://localhost:7101/api/offers/${offerId}`,
    {
        method: "GET",

        headers: {
            "Authorization": `Bearer ${token}`
                }
    }
    );

if (!response.ok) {
    throw new Error("Could not get the offer.");
    }

// Convert backend JSON into a JavaScript object
    const offer  = await response.json();

 // Show the result in the Console , because i want to check the get work or not
    console.log("GET successful:", offer);


    // Display backend information in HTML
    agreedPrice.textContent =`OMR ${offer.price}`;
    timeline.textContent =`${offer.durationDays} days`;

} catch (error) {
    console.error("GET error:", error);
    alert(error.message);
}
}

//POST: Accept offer and create agreement
// accept button for event (click)
acceptButton.addEventListener("click", async () => {
if (!offerId) {
    alert("Offer ID is missing.");
    return;
}

if (!token) {
    alert("Please log in first.");
    window.location.href = "login.html";
    return;
  }


acceptButton.disabled = true;
acceptButton.textContent = "Accepting...";

//POST
try {
    const response = await fetch("https://localhost:7101/api/Agreements",
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
        },

            body: JSON.stringify({offerId: Number(offerId)})
            }
        );

if (!response.ok) {
throw new Error("Could not accept the offer.");
    }

// Get the newly created agreement
    const agreement = await response.json();

    console.log("Agreement created:", agreement);
    alert("Offer accepted successfully.");


    // to send the agreement ID to the next page
    window.location.href =`offer-accepted.html?agreementId=${agreement.agreementId}`;
} catch (error) {
    console.error("POST error:", error);
    alert(error.message);

    acceptButton.disabled = false;
    acceptButton.textContent =
    "✓ Accept & Create Agreement";
}
});


// Return to the offers page
cancelButton.addEventListener("click", () => {
window.location.href = "View_Job.html";
});

// Run GET when the page opens
getOfferInformation();
// Read the quote ID from the page URL
const parameters = new URLSearchParams(window.location.search);
const quoteId = parameters.get("quoteId");

// find the HTML buttons by ids
const acceptButton = document.querySelector("#acceptButton");
const cancelButton = document.querySelector("#cancelButton");

// accept button for event (click)
acceptButton.addEventListener("click", async () => {
if (!quoteId) {
    alert("Quote ID is missing.");
    return;
}

acceptButton.disabled = true;
acceptButton.textContent = "Accepting...";

try {
    const response = await fetch(`https://localhost:5153/api/quotes/${quoteId}/accept`,
{
    method: "POST"
}
    );

if (!response.ok) {
throw new Error("Could not accept the offer.");
    }

    alert("Offer accepted successfully.");

    window.location.href =`offer-accepted.html?quoteId=${quoteId}`;
} catch (error) {
    console.log(error);
    alert(error.message);

    acceptButton.disabled = false;
    acceptButton.textContent =
    "✓ Accept & Create Agreement";
}
});

// Cancel button
cancelButton.addEventListener("click", () => {history.back();});
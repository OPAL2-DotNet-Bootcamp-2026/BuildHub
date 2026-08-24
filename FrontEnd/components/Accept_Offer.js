// Read the quote ID from the page URL
const parameters = new URLSearchParams(window.location.search);
const quoteId = 2;

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
    const response = await fetch(`https://localhost:7102/api/quotes/${quoteId}/accept`,
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


// Return to the offers page
cancelButton.addEventListener("click", () => {
window.location.href = "View_Job.html";
});


const sleep = (milliseconds) => {
    return new Promise(resolve => setTimeout(resolve, milliseconds));
};
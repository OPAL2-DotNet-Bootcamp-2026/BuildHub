import { base_url } from "../scripts/base_url.js";

interface Offer {
    price: number;
    durationDays: number;
}

interface Agreement {
    agreementId: number;
}

// Fixed ID for testing
const offerId: number = 5;

// Get the saved token from login
const token: string | null = localStorage.getItem("token");

// Find the HTML elements by ID
const acceptButton =
    document.querySelector<HTMLButtonElement>("#acceptButton")!;

const cancelButton =
    document.querySelector<HTMLButtonElement>("#cancelButton")!;

const agreedPrice =
    document.querySelector<HTMLElement>("#agreedPrice")!;

const timeline =
    document.querySelector<HTMLElement>("#timeline")!;

// GET offer information
async function getOfferInformation(): Promise<void>  // Promise<void> means the function finishes later but does not return any data
{
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
        const response: Response = await fetch(
            `${base_url}/api/offers/${offerId}`,
            {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }
        );

        if (!response.ok) {
            throw new Error("Could not get the offer.");
        }

        // Convert the JSON response into an Offer object
        const offer: Offer = await response.json();

        console.log("GET successful:", offer);

        // Display the offer information
        agreedPrice.textContent = `OMR ${offer.price}`;
        timeline.textContent = `${offer.durationDays} days`;

    } catch (error: unknown) {
        console.error("GET error:", error);

        if (error instanceof Error) {
            alert(error.message);
        }
    }
}

// POST: Accept offer and create agreement
acceptButton.addEventListener("click", async (): Promise<void> => {
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

    try {
        const response: Response = await fetch(
            `${base_url}/api/Agreements`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`
                },
                body: JSON.stringify({
                    offerId: offerId
                })
            }
        );

        if (!response.ok) {
            throw new Error("Could not accept the offer.");
        }

        // Get the newly created agreement
        const agreement: Agreement = await response.json();

        console.log("Agreement created:", agreement);
        alert("Offer accepted successfully.");

        // Send the agreement ID to the next page
        window.location.href =
            `offer-accepted.html?agreementId=${agreement.agreementId}`;

    } catch (error: unknown) {
        console.error("POST error:", error);

        if (error instanceof Error) {
            alert(error.message);
        }

        acceptButton.disabled = false;
        acceptButton.textContent = "✓ Accept & Create Agreement";
    }
});


// Return to the offers page
cancelButton.addEventListener("click", (): void => {
    window.location.href = "View_Job.html";
});


// Run GET when the page opens
getOfferInformation();
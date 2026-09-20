import { base_url } from "./base_url.js";
// to read the agreement ID from the page URL
const parameters = new URLSearchParams(window.location.search);
const agreementId = Number(parameters.get("agreementId"));
// Find the HTML elements
const stars = document.querySelectorAll(".star-rating .star");
const submitButton = document.querySelector(".btn-submit-review");
const skipButton = document.querySelector(".btn-skip");
const textarea = document.querySelector(".review-textarea");
const counter = document.querySelector(".char-counter");
// Make sure the important HTML elements exist
if (!submitButton || !skipButton || !textarea || !counter) {
    throw new Error("Some review page elements were not found.");
}
// The selected rating starts at zero
let selectedRating = 0;
// Add a click event to every star
stars.forEach((star, index) => {
    star.addEventListener("click", () => {
        // Convert the index into a rating from 1 to 5
        selectedRating = index + 1;
        // Fill the selected stars
        stars.forEach((currentStar, currentIndex) => {
            const isFilled = currentIndex < selectedRating;
            currentStar.classList.toggle("is-filled", isFilled);
            currentStar.setAttribute("aria-checked", currentIndex === index ? "true" : "false");
        });
        // Enable the Submit Review button
        submitButton.disabled = false;
        submitButton.classList.add("is-active");
    });
});
// Update the character counter when the user writes
textarea.addEventListener("input", () => {
    counter.textContent = `${textarea.value.length}/500`;
});
// Submit the review
submitButton.addEventListener("click", async () => {
    // Check if the URL contains a valid agreement ID
    if (!Number.isInteger(agreementId) || agreementId <= 0) {
        alert("Agreement ID is missing or invalid.");
        return;
    }
    // Check if the user selected a rating
    if (selectedRating === 0) {
        alert("Please select a rating.");
        return;
    }
    // Get the JWT token saved after login
    const token = localStorage.getItem("token");
    // Stop if the user is not logged in
    if (!token) {
        alert("Please log in before submitting a review.");
        return;
    }
    // Prepare the information that will be sent
    const reviewRequest = {
        agreementId: agreementId,
        rating: selectedRating,
        comment: textarea.value.trim()
    };
    try {
        // Disable the button while waiting for the backend
        submitButton.disabled = true;
        submitButton.textContent = "Submitting...";
        // Send the review to the backend
        const response = await fetch(`${base_url}/api/Reviews`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify(reviewRequest)
        });
        // Check if the backend returned an error
        if (!response.ok) {
            const errorMessage = await response.text();
            console.error("Review error:", response.status, errorMessage);
            if (response.status === 400) {
                throw new Error("Please check the review information.");
            }
            if (response.status === 401) {
                throw new Error("Your login token is missing or expired.");
            }
            if (response.status === 403) {
                throw new Error("Only the homeowner can submit this review.");
            }
            if (response.status === 404) {
                throw new Error("The agreement could not be found.");
            }
            if (response.status === 409) {
                throw new Error("You have already reviewed this agreement.");
            }
            throw new Error(errorMessage || "Could not submit the review.");
        }
        // Convert the successful JSON response into an object
        const createdReview = await response.json();
        // Show the created review in the Console
        console.log("Review created successfully:", createdReview);
        // Tell the user that the review was submitted
        alert("Your review was submitted successfully.");
        // Return to the previous page
        history.back();
    }
    catch (error) {
        // Create a simple error message
        const message = error instanceof Error
            ? error.message
            : "An unexpected error occurred.";
        console.error(error);
        alert(message);
        // Enable the button so the user can try again
        submitButton.disabled = false;
        submitButton.textContent = "Submit Review";
    }
});
// Return to the previous page when Skip is clicked
skipButton.addEventListener("click", () => {
    history.back();
});

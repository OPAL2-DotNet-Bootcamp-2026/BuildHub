import { base_url } from "./base_url.js";

const jobsContainer = document.getElementById("jobs-container");

(async function login() {
    try {
        const response = await fetch(`${base_url}/api/Jobs`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        });

        if (response.ok) {
            const jobs = await response.json();
            jobs.forEach(job => {
                const jobCard = document.createElement("div");
                jobCard.innerHTML = `
                    <div class="card mx-auto my-3 px-4 py-2" style="max-width: 80%">
                        <div class="card-body">
                            <h3 class="card-title">
                                ${job.title}
                                <span class="badge bg-primary badge-color">${job.status}</span>
                            </h3>
                            <p class="card-text">
                                ${job.description} | ${job.location} | ${job.budget} | Due: ${job.dueDate}
                            </p>
                            <div class="text-end">
                                <span class="fs-3 fw-bold d-block lh-1">${job.offerCount}</span>
                                <small class="text-muted">offers</small>
                            </div>
                        </div>
                        <div class="d-flex justify-content-between align-items-center pt-3">
                            <span class="text-muted small"> ${job.newOffers} new offers waiting for review </span>
                            <button
                                class="btn btn-navy px-4 py-2"
                                id="review-button"
                                onclick="window.location.href = '../pages/View_Job.html'"
                            >
                                > Review Offers &rarr;
                            </button>
                        </div>
                    </div>
                `;
                jobsContainer.appendChild(jobCard);
            });
        } else {
            console.error("Fetching jobs failed with status:", response.status);
            alert("Failed to fetch jobs.");
        }
    } catch (networkError) {
        // Handle network/connection failure
        console.error("Network error occurred:", networkError);
        alert("Unable to connect to the server. Please check your connection.");
    }
})();
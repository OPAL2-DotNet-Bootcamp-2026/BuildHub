import { base_url } from "./base_url.js";

const jobsContainer = document.getElementById("jobs-container");
const token = localStorage.getItem("token");

(async function loadDashboard() {
    try {
        const response = await fetch(`${base_url}/api/Jobs`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        });

        const response2 = await fetch(`${base_url}/api/Offers`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            }
        });

        const offers = response2.ok ? await response2.json() : [];
        localStorage.setItem("offers", JSON.stringify(offers));

        if (response.ok) {
            const jobs = await response.json();
            const currentUserId = localStorage.getItem("userId");

            jobsContainer.innerHTML = ""; // Clear container first

            jobs
                .filter(job => job.homeownerId == currentUserId)
                .forEach(job => {
                    const matchingOffers = offers.filter(offer => offer.jobId == job.jobId);
                    const offerCount = matchingOffers.length;

                    const jobCard = document.createElement("div");
                    jobCard.innerHTML = `
                        <div class="card mx-auto my-3 px-4 py-2" style="max-width: 80%">
                            <div class="card-body">
                                <h3 class="card-title">
                                    ${job.title}
                                    <span class="badge bg-primary badge-color">${job.status}</span>
                                </h3>
                                <p class="card-text">
                                    ${job.description} | ${job.city} | ${job.budget} | Due: ${job.deadline}
                                </p>
                                <div class="text-end">
                                    <span class="fs-3 fw-bold d-block lh-1">${offerCount}</span>
                                    <small class="text-muted">offers</small>
                                </div>
                            </div>
                            <div class="d-flex justify-content-between align-items-center pt-3">
                                <span class="text-muted small"> ${offerCount} new offers waiting for review </span>
                                <button class="btn btn-navy px-4 py-2 review-btn">
                                    Review Offers &rarr;
                                </button>
                            </div>
                        </div>
                    `;

                    // Safely attach event listener instead of using inline onclick
                    const reviewButton = jobCard.querySelector(".review-btn");
                    reviewButton.addEventListener("click", () => {
                        localStorage.setItem("selectedJob", JSON.stringify(job));
                        window.location.href = `../pages/View_Job.html`;
                    });

                    jobsContainer.appendChild(jobCard);
                });
        } else {
            console.error("Fetching jobs failed with status:", response.status);
            alert("Failed to fetch jobs.");
        }
    } catch (networkError) {
        console.error("Network error occurred:", networkError);
        alert("Unable to connect to the server. Please check your connection.");
    }
})();
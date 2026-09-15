const offers = JSON.parse(localStorage.getItem("offers"));
const selectedJob = JSON.parse(localStorage.getItem("selectedJob"));
const offersContainer = document.getElementById("offers-container");


(function displayOffers() {
    if (offers) {
                offers
                    .filter(offer => offer.jobId == selectedJob.jobId)
                    .forEach(offer => {
                        const offerCard = document.createElement("div");
                        offerCard.innerHTML = `
                                  <div class="card border-secondary mb-3">
                                    <div class="card-body">
                                    <div class="row align-items-center">
                                        <!-- Vendor -->
                                        <div class="col-md-8">
                                        <div class="d-flex align-items-center">
                                            <img
                                            src="../assets/Noor.jpg"
                                            class="rounded-circle me-3"
                                            width="45"
                                            height="45"
                                            alt="Vendor"
                                            />

                                            <div>
                                            <h6 class="mb-1">
                                                    Vendor Id: ${offer.vendorProfileId}

                                                <span class="badge text-bg-success"> ✓ Verified </span>
                                            </h6>

                                            <div>
                                                <span class="text-warning"> ★★★★★ </span>

                                                <small class="text-secondary"> 4.8 (142) </small>
                                            </div>
                                            </div>
                                        </div>

                                        <div class="mt-2">
                                            <span class="badge text-primary-emphasis bg-primary-subtle">
                                            🏆 Highest rated
                                            </span>

                                            <span class="badge text-bg-secondary"> 189 jobs done </span>
                                        </div>
                                        </div>

                                        <!-- Price -->
                                        <div class="col-md-4 text-md-end">
                                        <h5 class="mb-1">OMR 1,400</h5>

                                        <small class="text-secondary"> 7 days </small>
                                        </div>
                                    </div>
                                    </div>

                                    <!-- Vendor Message -->
                                    <div class="card-body border-top p-2">
                                    <small class="text-secondary fw-bold"> VENDOR'S MESSAGE </small>

                                    <p class="small mt-1 mb-2">
                                        ${offer.message}
                                    </p>

                                    <small class="text-secondary fw-bold"> ASK A QUESTION </small>

                                    <div class="input-group mt-1">
                                        <input
                                        type="text"
                                        class="form-control form-control-sm"
                                        placeholder="Type your question..."
                                        />

                                        <button class="btn btn-outline-secondary btn-sm">Send</button>
                                    </div>

                                    <!-- Buttons -->

                                    <div class="d-flex gap-2 mt-2">
                                        <a href="../pages/Accept_Offer.html?offerId=${offer.offerId}"
                                        ><button
                                            type="submit"
                                            class="btn text-white"
                                            style="background-color: var(--accent)"
                                        >
                                            ✓ Accept This Offer
                                        </button>
                                        </a>

                                        <button class="btn btn-outline-secondary btn-sm">
                                        View Profile
                                        </button>
                                    </div>
                                    </div>
                                </div>`;
                        offersContainer.appendChild(offerCard);
                    }); 
    }

    if (!offers){
        const noOffersMessage = document.createElement("p");
        noOffersMessage.textContent = "No offers available for this job.";
        noOffersMessage.classList.add("text-center", "mt-3");
        offersContainer.appendChild(noOffersMessage);
    }
})()


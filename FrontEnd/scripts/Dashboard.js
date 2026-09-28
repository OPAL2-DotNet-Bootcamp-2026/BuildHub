"use strict";
const base_url = "https://localhost:7101";
const token = localStorage.getItem("token");
const openJobsElement = document.querySelector("#openJobs");
const newOffersElement = document.querySelector("#newOffers");
const inProgressElement = document.querySelector("#inProgress");
const completedJobsElement = document.querySelector("#completedJobs");
const notificationsList = document.querySelector("#notificationsList");
//enums same as backend
var JobStatus;
(function (JobStatus) {
    JobStatus[JobStatus["Open"] = 1] = "Open";
    JobStatus[JobStatus["Hired"] = 2] = "Hired";
    JobStatus[JobStatus["Completed"] = 3] = "Completed";
    JobStatus[JobStatus["Cancelled"] = 4] = "Cancelled";
})(JobStatus || (JobStatus = {}));
var OfferStatus;
(function (OfferStatus) {
    OfferStatus[OfferStatus["Pending"] = 1] = "Pending";
    OfferStatus[OfferStatus["Accepted"] = 2] = "Accepted";
    OfferStatus[OfferStatus["NotSelected"] = 3] = "NotSelected";
})(OfferStatus || (OfferStatus = {}));
var NotificationType;
(function (NotificationType) {
    NotificationType[NotificationType["OfferReceived"] = 1] = "OfferReceived";
    NotificationType[NotificationType["OfferAccepted"] = 2] = "OfferAccepted";
    NotificationType[NotificationType["OfferNotSelected"] = 3] = "OfferNotSelected";
    NotificationType[NotificationType["AgreementStarted"] = 4] = "AgreementStarted";
    NotificationType[NotificationType["JobCompleted"] = 5] = "JobCompleted";
    NotificationType[NotificationType["PaymentReleased"] = 6] = "PaymentReleased";
    NotificationType[NotificationType["PaymentRefunded"] = 7] = "PaymentRefunded";
    NotificationType[NotificationType["ReviewReceived"] = 8] = "ReviewReceived";
})(NotificationType || (NotificationType = {}));
// GET JOBS
async function getJobs() {
    try {
        const response = await fetch(`${base_url}/api/Jobs`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });
        if (!response.ok) {
            throw new Error("Could not get jobs.");
        }
        const jobs = await response.json();
        console.log("Jobs:", jobs);
        // Count jobs by status
        const openJobs = jobs.filter((job) => job.status === JobStatus.Open).length;
        const inProgress = jobs.filter((job) => job.status === JobStatus.Hired).length;
        const completedJobs = jobs.filter((job) => job.status === JobStatus.Completed).length;
        // Show job numbers in HTML
        if (openJobsElement) {
            openJobsElement.textContent = openJobs.toString();
        }
        if (inProgressElement) {
            inProgressElement.textContent = inProgress.toString();
        }
        if (completedJobsElement) {
            completedJobsElement.textContent = completedJobs.toString();
        }
    }
    catch (error) {
        console.error("Jobs error:", error);
    }
}

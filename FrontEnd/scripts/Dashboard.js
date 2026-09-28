import { base_url } from "./base_url.js";
const token = localStorage.getItem("token");
if (!token) {
    console.error("Token is missing.");
}
const openJobsElement = document.querySelector("#openJobs");
const newOffersElement = document.querySelector("#newOffers");
const inProgressElement = document.querySelector("#inProgress");
const completedJobsElement = document.querySelector("#completedJobs");
const notificationsList = document.querySelector("#notificationsList");
const allNotifications = document.querySelector("#allNotifications");
// to click "All" and navigate to notification page
allNotifications?.addEventListener("click", (event) => {
    event.preventDefault();
    window.location.href = "notifications.html";
});
//for loggedUserName 
const loggedUserName = document.querySelector("#loggedUserName");
//for calculate newOffers from the database
const newOffersText = document.querySelector("#newOffersText");
function getUserIdFromToken() {
    if (!token) {
        return null;
    }
    const payload = JSON.parse(atob(token.split(".")[1]));
    console.log("Token payload:", payload);
    const userId = payload.nameid ??
        payload.sub ??
        payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    return userId ? Number(userId) : null;
}
async function getLoggedUser() {
    const userId = getUserIdFromToken();
    if (!userId) {
        console.error("User ID was not found in token.");
        return;
    }
    try {
        const response = await fetch(`${base_url}/api/Users/${userId}`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });
        if (!response.ok) {
            throw new Error("Could not get user.");
        }
        const user = await response.json();
        console.log("Logged user:", user);
        if (loggedUserName) {
            loggedUserName.textContent = user.fullName;
        }
    }
    catch (error) {
        console.error("User error:", error);
    }
}
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
// GET OFFERS
async function getOffers() {
    try {
        const response = await fetch(`${base_url}/api/Offers`, {
            method: "GET",
            headers: { "Authorization": `Bearer ${token}` }
        });
        if (!response.ok) {
            throw new Error("Could not get offers.");
        }
        const offers = await response.json();
        console.log("Offers:", offers);
        // Count only Pending offers
        const newOffers = offers.filter((offer) => offer.status === OfferStatus.Pending).length;
        if (newOffersText) {
            newOffersText.textContent = newOffers.toString();
        }
        // Show number in HTML
        if (newOffersElement) {
            newOffersElement.textContent = newOffers.toString();
        }
        // Number in welcome message
        if (newOffersText) {
            newOffersText.textContent =
                newOffers.toString();
        }
    }
    catch (error) {
        console.error("Offers error:", error);
    }
}
function getNotificationTitle(type) {
    switch (type) {
        case NotificationType.OfferReceived:
            return "New Offer Received";
        case NotificationType.OfferAccepted:
            return "Offer Accepted";
        case NotificationType.OfferNotSelected:
            return "Offer Not Selected";
        case NotificationType.AgreementStarted:
            return "Agreement Started";
        case NotificationType.JobCompleted:
            return "Job Completed";
        case NotificationType.PaymentReleased:
            return "Payment Released";
        case NotificationType.PaymentRefunded:
            return "Payment Refunded";
        case NotificationType.ReviewReceived:
            return "Review Received";
        default:
            return "Notification";
    }
}
// GET NOTIFICATIONS
async function getNotifications() {
    try {
        const response = await fetch(`${base_url}/api/Notifications`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });
        if (!response.ok) {
            throw new Error("Could not get notifications.");
        }
        const notifications = await response.json();
        console.log("Notifications:", notifications);
        if (!notificationsList) {
            return;
        }
        // Remove old HTML notifications
        notificationsList.innerHTML = "";
        notifications.forEach((notification) => {
            const card = document.createElement("article");
            card.className = "card border shadow-sm rounded-3";
            const title = getNotificationTitle(notification.type);
            card.innerHTML = `
        <div class="card-body d-flex">

            <span class="${notification.isRead
                ? "text-secondary"
                : "text-danger"} me-2">
              ●
            </span>

            <div>

              <h3 class="small fw-bold mb-1">
                ${title}
              </h3>

              <p class="small text-secondary mb-1">
                ${notification.message ??
                "You have a new notification."}
              </p>

              <small class="text-secondary">
                ${notification.createdAt
                ? new Date(notification.createdAt).toLocaleString()
                : ""}
              </small>

            </div>

          </div>
        `;
            notificationsList.appendChild(card);
        });
    }
    catch (error) {
        console.error("Notification error:", error);
    }
}
// RUN WHEN PAGE OPENS
getLoggedUser();
getJobs();
getOffers();
getNotifications();

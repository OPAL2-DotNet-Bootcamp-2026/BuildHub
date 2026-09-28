import { base_url } from "./base_url.js";

const token = localStorage.getItem("token");

if (!token) {
console.error("Token is missing.");
}

const openJobsElement = document.querySelector<HTMLElement>("#openJobs");

const newOffersElement = document.querySelector<HTMLElement>("#newOffers");

const inProgressElement = document.querySelector<HTMLElement>("#inProgress");

const completedJobsElement = document.querySelector<HTMLElement>("#completedJobs");

const notificationsList = document.querySelector<HTMLElement>("#notificationsList");

const allNotifications = document.querySelector<HTMLAnchorElement>("#allNotifications");

// to click "All" and navigate to notification page
allNotifications?.addEventListener("click", (event) => {event.preventDefault();
window.location.href = "notifications.html";
});

//for loggedUserName 
const loggedUserName = document.querySelector<HTMLElement>("#loggedUserName");

//for calculate newOffers from the database
const newOffersText =document.querySelector<HTMLElement>("#newOffersText");


function getUserIdFromToken(): number | null {

  if (!token) {
    return null;
  }

  const payload = JSON.parse(
    atob(token.split(".")[1])
  );

  console.log("Token payload:", payload);

  const userId =
    payload.nameid ??
    payload.sub ??
    payload[
      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
    ];

  return userId ? Number(userId) : null;
}

async function getLoggedUser(): Promise<void> {

  const userId = getUserIdFromToken();

  if (!userId) {
    console.error("User ID was not found in token.");
    return;
  }

  try {

    const response = await fetch(
      `${base_url}/api/Users/${userId}`,
      {
        method: "GET",
        headers: {
          "Authorization": `Bearer ${token}`
        }
      }
    );

    if (!response.ok) {
      throw new Error("Could not get user.");
    }

    const user = await response.json();

    console.log("Logged user:", user);

    if (loggedUserName) {loggedUserName.textContent =user.fullName;}

  } catch (error) {

    console.error("User error:", error);

  }
}


//enums same as backend

enum JobStatus {
    Open = 1,
    Hired = 2,
    Completed = 3,
    Cancelled = 4
}

enum OfferStatus {
    Pending = 1,
    Accepted = 2,
    NotSelected = 3
}

enum NotificationType {
    OfferReceived = 1,
    OfferAccepted = 2,
    OfferNotSelected = 3,
    AgreementStarted = 4,
    JobCompleted = 5,
    PaymentReleased = 6,
    PaymentRefunded = 7,
    ReviewReceived = 8
}

// GET JOBS

async function getJobs(): Promise<void> {

try {

    const response = await fetch(
    `${base_url}/api/Jobs`,
    {
        method: "GET",

        headers: {
        "Authorization": `Bearer ${token}`
        }
    }
);


if (!response.ok) {
    throw new Error("Could not get jobs.");}

    const jobs = await response.json();

    console.log("Jobs:", jobs);


    // Count jobs by status

    const openJobs = jobs.filter((job: any) =>job.status === JobStatus.Open).length;

    const inProgress =jobs.filter((job: any) => job.status === JobStatus.Hired).length;


    const completedJobs = jobs.filter((job: any) =>job.status === JobStatus.Completed).length;


    // Show job numbers in HTML

    if (openJobsElement) { openJobsElement.textContent =openJobs.toString();}


    if (inProgressElement) {inProgressElement.textContent =inProgress.toString(); }


    if (completedJobsElement) {completedJobsElement.textContent =completedJobs.toString();}
}

catch (error) {
    console.error("Jobs error:", error);
}

}


// GET OFFERS

async function getOffers(): Promise<void> {

try {

const response = await fetch(`${base_url}/api/Offers`,
    {
        method: "GET",

        headers: {"Authorization": `Bearer ${token}`}
    }
    );


    if (!response.ok) {throw new Error("Could not get offers.");}


    const offers = await response.json();

    console.log("Offers:", offers);


    // Count only Pending offers

    const newOffers =offers.filter((offer: any) =>offer.status === OfferStatus.Pending).length;

if (newOffersText) {newOffersText.textContent = newOffers.toString();}


    // Show number in HTML

    if (newOffersElement) {

    newOffersElement.textContent =newOffers.toString();

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

function getNotificationTitle(type: NotificationType): string {
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


async function getNotifications(): Promise<void> {

try {

    const response = await fetch(
    `${base_url}/api/Notifications`,
    {
        method: "GET",

        headers: {
        "Authorization": `Bearer ${token}`
        }
    }
    );


    if (!response.ok) {throw new Error("Could not get notifications.");
    }


    const notifications = await response.json();

    console.log("Notifications:",notifications);


    if (!notificationsList) {
    return;
    }


    // Remove old HTML notifications
    notificationsList.innerHTML = "";


    notifications.forEach((notification: any) => {

    const card =document.createElement("article");

    card.className ="card border shadow-sm rounded-3";

    const title =getNotificationTitle(notification.type);

    card.innerHTML = `
        <div class="card-body d-flex">

            <span class="${
              notification.isRead
                ? "text-secondary"
                : "text-danger"
            } me-2">
              ●
            </span>

            <div>

              <h3 class="small fw-bold mb-1">
                ${title}
              </h3>

              <p class="small text-secondary mb-1">
                ${
                  notification.message ??
                  "You have a new notification."
                }
              </p>

              <small class="text-secondary">
                ${
                  notification.createdAt
                    ? new Date(
                        notification.createdAt
                      ).toLocaleString()
                    : ""
                }
              </small>

            </div>

          </div>
        `;


        notificationsList.appendChild(card);

      }
    );

}

catch (error) {

    console.error("Notification error:",error);

}

}


// RUN WHEN PAGE OPENS

getLoggedUser();

getJobs();

getOffers();

getNotifications();
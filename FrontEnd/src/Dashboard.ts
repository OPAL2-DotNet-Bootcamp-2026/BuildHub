const base_url = "https://localhost:7101";

const token = localStorage.getItem("token");

const openJobsElement = document.querySelector<HTMLElement>("#openJobs");

const newOffersElement = document.querySelector<HTMLElement>("#newOffers");

const inProgressElement = document.querySelector<HTMLElement>("#inProgress");

const completedJobsElement = document.querySelector<HTMLElement>("#completedJobs");

const notificationsList = document.querySelector<HTMLElement>("#notificationsList");


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
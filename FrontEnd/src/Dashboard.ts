const base_url = "https://localhost:7101";

const token = localStorage.getItem("token");

const openJobsElement = document.querySelector<HTMLElement>("#openJobs");

const newOffersElement = document.querySelector<HTMLElement>("#newOffers");

const inProgressElement = document.querySelector<HTMLElement>("#inProgress");

const completedJobsElement = document.querySelector<HTMLElement>("#completedJobs");
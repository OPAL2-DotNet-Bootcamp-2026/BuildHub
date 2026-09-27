/* =========================================
   notifications.js
   Fetches notifications from the BuildHub API
   and renders them into the Unread / Earlier
   lists on notifications.html.

   Backend contract (from NotificationController):
     GET {API_BASE}/notifications/{userId}
     200 -> NotificationOutputDto[]
     204 -> no notifications (empty array, not an error)

   NotificationOutputDto currently has:
     notificationId, userId, title, type, isRead, createdAt

   NOTE: there is no message/body field yet. Until the backend
   adds one, buildMessage() below fills in a generic sentence
   based on `type` so the card isn't empty. Replace that function
   with `n.message` as soon as the DTO has real text.
========================================= */

const API_BASE = " https://localhost:7102/api"; // change to your deployed API origin in production
const CURRENT_USER_ID = 3;    // see note in that function below

document.addEventListener("DOMContentLoaded", loadNotifications);

async function loadNotifications() {
  const unreadList = document.getElementById("unread-list");
  const earlierList = document.getElementById("earlier-list");
  const unreadSection = document.getElementById("unread-section");
  const subtitle = document.querySelector(".notif-page .page-subtitle");

  try {
    const response = await fetch(`${API_BASE}/notifications/${CURRENT_USER_ID}`);

    if (response.status === 204) {
      renderEmptyState();
      return;
    }

    if (!response.ok) {
      throw new Error(`Request failed with status ${response.status}`);
    }

    const notifications = await response.json();

    const unread = notifications.filter((n) => !n.isRead);
    const earlier = notifications.filter((n) => n.isRead);

    // Sort newest first within each group
    unread.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
    earlier.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

    unreadList.innerHTML = unread.map((n) => notificationCardHTML(n, true)).join("");
    earlierList.innerHTML = earlier.map((n) => notificationCardHTML(n, false)).join("");

    // Hide the "Unread" section entirely if there's nothing unread
    unreadSection.style.display = unread.length ? "" : "none";

    // Update the "X unread" subtitle and the badge count in the header
    subtitle.textContent = `${unread.length} unread`;
    updateHeaderBadge(unread.length);

  } catch (err) {
    console.error("Failed to load notifications:", err);
    renderErrorState();
  }
}

/**
 * Builds one notification card's HTML.
 * @param {object} n - a NotificationOutputDto
 * @param {boolean} isUnread - whether to show the orange dot / unread background
 */
function notificationCardHTML(n, isUnread) {
  const icon = iconForType(n.type);
  const message = buildMessage(n); // TODO: replace with n.message once the backend adds it
  const time = formatRelativeTime(n.createdAt);

  return `
    <article class="notif-card ${isUnread ? "notif-card--unread" : ""}">
      <span class="notif-icon">${icon}</span>
      <div class="notif-body">
        <p class="notif-title">${escapeHTML(n.title)}</p>
        <p class="notif-text">${escapeHTML(message)}</p>
        <p class="notif-time">${time}</p>
      </div>
      ${isUnread ? '<span class="notif-dot"></span>' : ""}
    </article>
  `;
}

/**
 * Temporary generic message per notification type, until the DTO
 * carries real message text from the backend.
 */
function buildMessage(n) {
  const byType = {
    Offer: "You received a new offer. Open it to see the price and details.",
    Payment: "A payment update was made on one of your jobs.",
    Review: "A review is requested or has been left on a completed job.",
    Account: "There's an update to your account or verification status.",
  };
  return byType[n.type] || "You have a new notification.";
}

function iconForType(type) {
  const icons = {
    Offer: "📬",
    Payment: "🔒",
    Review: "⭐",
    Account: "🔔",
  };
  return icons[type] || "🔔";
}

/**
 * Converts an ISO date string (CreatedAt) into "2 hours ago" style text.
 */
function formatRelativeTime(isoString) {
  const date = new Date(isoString);
  const seconds = Math.floor((Date.now() - date.getTime()) / 1000);

  const units = [
    { label: "year", secs: 31536000 },
    { label: "month", secs: 2592000 },
    { label: "week", secs: 604800 },
    { label: "day", secs: 86400 },
    { label: "hour", secs: 3600 },
    { label: "minute", secs: 60 },
  ];

  for (const unit of units) {
    const value = Math.floor(seconds / unit.secs);
    if (value >= 1) {
      return `${value} ${unit.label}${value > 1 ? "s" : ""} ago`;
    }
  }
  return "just now";
}

/** Updates the "2" badge next to "Notifications" in the shared header. */
function updateHeaderBadge(count) {
  document.querySelectorAll(".badge-count").forEach((badge) => {
    if (count > 0) {
      badge.textContent = count;
      badge.style.display = "";
    } else {
      badge.style.display = "none";
    }
  });
}

function renderEmptyState() {
  const main = document.querySelector(".notif-page");
  document.getElementById("unread-section").style.display = "none";
  document.getElementById("earlier-list").innerHTML =
    `<p class="notif-empty">You have no notifications yet.</p>`;
  document.querySelector(".page-subtitle").textContent = "0 unread";
  updateHeaderBadge(0);
}

function renderErrorState() {
  document.getElementById("earlier-list").innerHTML =
    `<p class="notif-empty">Couldn't load notifications. Check your connection and try again.</p>`;
}

/** Basic HTML-escaping so notification content can never inject markup. */
function escapeHTML(str) {
  const div = document.createElement("div");
  div.textContent = str ?? "";
  return div.innerHTML;
}

/**
 * Placeholder for getting the logged-in user's id.
 * Replace this with however you store the session after login
 * (e.g. a JWT you decode, or a value saved in localStorage at login time).
 */
function getCurrentUserId() {
  const stored = localStorage.getItem("buildhub_user_id");
  if (stored) return Number(stored);

  console.warn("No logged-in user id found in localStorage — using placeholder userId=3.");
  return 3; // TODO: remove this fallback once real auth/session is wired up
}
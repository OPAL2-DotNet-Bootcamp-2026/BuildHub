document.addEventListener("DOMContentLoaded", () => {
  const proceedBtn = document.getElementById("proceedPaymentBtn") as HTMLAnchorElement | null;

  if (proceedBtn) {
    // Capture the current URL search parameters (e.g., ?jobId=123&vendorId=456)
    const currentParams = window.location.search;

    // Set the destination URL, appending the existing parameters if they exist
    proceedBtn.href = `payment.html${currentParams}`;
  }
});
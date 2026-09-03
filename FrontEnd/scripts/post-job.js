const API_BASE_URL = "https://localhost:7102"; 

let categoriesData = [];

document.addEventListener("DOMContentLoaded", () => {
  fetchCategories();
  setupNavigationHandlers();
  setupReviewTabListener();
  setupSubmitHandler();
});

// Fetch dynamic categories and populate dropdown select
async function fetchCategories() {
  const select = document.getElementById("category");
  try {
    const res = await fetch(`${API_BASE_URL}/category/GetAll`);
    categoriesData = await res.json();
    
    select.innerHTML = '<option value="">Select category</option>';
    categoriesData.forEach(cat => {
      const option = document.createElement("option");
      option.value = cat.categoryId;
      option.textContent = `${cat.nameEn} (${cat.nameAr})`;
      select.appendChild(option);
    });
  } catch (err) {
    console.error("Failed to fetch categories:", err);
    select.innerHTML = '<option value="">Error loading categories</option>';
  }
}

// Programmatically navigate to target tab and sync tab headers
function navigateToTab(targetTabId) {
  const targetTriggerEl = document.querySelector(`button[data-bs-target="${targetTabId}"]`);
  if (targetTriggerEl) {
    const tabInstance = bootstrap.Tab.getOrCreateInstance(targetTriggerEl);
    tabInstance.show();
  }
}

// Setup Next / Back button click handlers
function setupNavigationHandlers() {
  // Continue button on details step -> opens budget tab
  const btnContinueToBudget = document.querySelector('#details button.btn-primary');
  if (btnContinueToBudget) {
    btnContinueToBudget.addEventListener('click', () => {
      navigateToTab('#budget');
    });
  }

  // Continue button on budget step -> opens review tab
  const btnContinueToReview = document.querySelector('#budget button.btn-primary');
  if (btnContinueToReview) {
    btnContinueToReview.addEventListener('click', () => {
      navigateToTab('#review');
    });
  }

  // Back button on budget step -> opens details tab
  const btnBackToDetails = document.querySelector('#budget button.btn-outline-secondary');
  if (btnBackToDetails) {
    btnBackToDetails.addEventListener('click', () => {
      navigateToTab('#details');
    });
  }

  // Back button on review step -> opens budget tab
  const btnBackToBudget = document.querySelector('#review button.btn-outline-secondary');
  if (btnBackToBudget) {
    btnBackToBudget.addEventListener('click', () => {
      navigateToTab('#budget');
    });
  }
}

// Update the review pane dynamically when switching to Tab 3
function setupReviewTabListener() {
  const reviewTabBtn = document.getElementById("review-tab");
  if (!reviewTabBtn) return;
  
  reviewTabBtn.addEventListener("show.bs.tab", () => {
    const title = document.getElementById("title").value || "—";
    const categoryId = document.getElementById("category").value;
    const city = document.getElementById("city").value || "—";
    const minBudget = document.getElementById("budget-min").value;
    const maxBudget = document.getElementById("budget-max").value;
    const deadline = document.getElementById("deadline").value || "—";

    const selectedCategory = categoriesData.find(c => c.categoryId == categoryId);
    const categoryText = selectedCategory ? selectedCategory.nameEn : "—";

    document.getElementById("rev-title").textContent = title;
    document.getElementById("rev-category").textContent = categoryText;
    document.getElementById("rev-city").textContent = city;
    document.getElementById("rev-budget").textContent = (minBudget || maxBudget) 
      ? `OMR ${minBudget || 0} – ${maxBudget || 0}` 
      : "—";
    document.getElementById("rev-deadline").textContent = deadline;
  });
}

// Handle the QuoteRequest POST API submission
async function setupSubmitHandler() {
  const btnSubmit = document.getElementById("btnSubmitJob");
  if (!btnSubmit) return;

  btnSubmit.addEventListener("click", async () => {
    const categoryId = parseInt(document.getElementById("category").value);
    const description = document.getElementById("description").value;
    const deadlineVal = document.getElementById("deadline").value;

    if (!categoryId || !description || !deadlineVal) {
      alert("Please complete all required fields.");
      return;
    }

    // Convert deadline to ISO 8601 string
    const ISOStringDeadline = new Date(deadlineVal).toISOString();

    const payload = {
      projectId: 1, // Set dynamically as needed
      categoryId: categoryId,
      description: description,
      deadline: ISOStringDeadline,
      visibilityType: "public",
      vendorProfileId: 1 // Set dynamically as needed
    };

    try {
      btnSubmit.disabled = true;
      btnSubmit.textContent = "Posting...";

      const response = await fetch(`${API_BASE_URL}/api/quote-requests`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
      });

      if (response.ok) {
        const data = await response.json();
        alert(`Job posted successfully! Quote Request ID: ${data.quoteRequestId}`);
        window.location.href = "my-jobs.html";
      } else {
        alert("Failed to submit job. Please check your inputs.");
      }
    } catch (err) {
      console.error("Submission error:", err);
      alert("An error occurred during job submission.");
    } finally {
      btnSubmit.disabled = false;
      btnSubmit.textContent = "🚀 Post Job";
    }
  });
}
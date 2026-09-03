const fullName = document.getElementById("fullName");
const email = document.getElementById("email");
const phoneNumber = document.getElementById("phoneNumber");
const city = document.getElementById("city");
const password = document.getElementById("password");
const responseMessage = document.getElementById("responseMessage");

const homeownerBtn = document.getElementById("homeownerBtn");
const vendorBtn = document.getElementById("vendorBtn");
let selectedRole = "";

function selectRole(activeBtn, inactiveBtn, roleValue) {
  selectedRole = roleValue;
  activeBtn.classList.remove("btn-outline-primary");
  activeBtn.classList.add("btn-primary");
  inactiveBtn.classList.remove("btn-primary");
  inactiveBtn.classList.add("btn-outline-primary");
}

homeownerBtn.addEventListener("click", function () {
  selectRole(homeownerBtn, vendorBtn, "Homeowner");
});

vendorBtn.addEventListener("click", function () {
  selectRole(vendorBtn, homeownerBtn, "Vendor");
});

const createAccountBtn = document.getElementById("createAccountBtn");
createAccountBtn.addEventListener("click", async function () {
  // Reset previous messages
  responseMessage.innerHTML = "";

  if (!selectedRole) {
    responseMessage.innerHTML = `
      <div class="alert alert-warning py-2 mb-0" role="alert">
        Please select whether you are a Homeowner or a Vendor.
      </div>`;
    return;
  }

  const userData = {
    fullName: fullName.value,
    email: email.value,
    phoneNumber: phoneNumber.value,
    city: city.value,
    passwordHash: password.value,
    role: selectedRole,
    isVerified: false
  };

  try {
    // Show temporary loading state
    createAccountBtn.disabled = true;
    createAccountBtn.textContent = "Creating Account...";

    const response = await fetch("https://localhost:7102/user/AddUser", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(userData)
    });

    if (response.ok) {
      // Show success message
      responseMessage.innerHTML = `
        <div class="alert alert-success py-2 mb-0" role="alert">
          Account created successfully! Redirecting to your dashboard...
        </div>`;

      // Delay redirect by 1.5 seconds to allow user to read the message
      setTimeout(() => {
        if (selectedRole === "Homeowner") {
          window.location.href = "Dashboard.html";
        } else if (selectedRole === "Vendor") {
          window.location.href = "../vendor-dashboard/vendor-dashboard.html";
        }
      }, 1500);

    } else {
      createAccountBtn.disabled = false;
      createAccountBtn.textContent = "Create Account";

      responseMessage.innerHTML = `
        <div class="alert alert-danger py-2 mb-0" role="alert">
          Registration failed. Please check your details and try again.
        </div>`;
    }

  } catch (error) {
    console.error("Error:", error);
    createAccountBtn.disabled = false;
    createAccountBtn.textContent = "Create Account";

    responseMessage.innerHTML = `
      <div class="alert alert-danger py-2 mb-0" role="alert">
        Unable to connect to the server. Please try again later.
      </div>`;
  }
});
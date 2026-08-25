const fullName = document.getElementById("fullName");
const email = document.getElementById("email");
const phoneNumber = document.getElementById("phoneNumber");
const city = document.getElementById("city");
const password = document.getElementById("password");
let selectedRole = "";

// Select the user's role
document.getElementById("homeownerBtn").addEventListener("click", function () {
    selectedRole = "Homeowner";
});

document.getElementById("vendorBtn").addEventListener("click", function () {
    selectedRole = "Vendor";
});
const createAccountBtn = document.getElementById("createAccountBtn");
createAccountBtn.addEventListener("click", async function () {

// Values entered by the user
const userData = {
    fullName: fullName.value,
    email: email.value,
    phoneNumber: phoneNumber.value,
    city: city.value,
    passwordHash: password.value,
    role: selectedRole,
    isVerified: false
}; 
console.log(userData);
    // Send user data to the backend
     try {
    const response = await fetch("https://localhost:7102/user/AddUser", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(userData)
    });

        if (response.ok) {

            if (selectedRole === "Homeowner") {
                  window.location.href = "Dashboard.html";
            } 
            else if (selectedRole === "Vendor") {
                window.location.href = "../vendor-dashboard/vendor-dashboard.html";
            }

        } else {
            console.log("Registration failed");
        }

    } catch (error) {
        console.error("Error:", error);
    }
}); 

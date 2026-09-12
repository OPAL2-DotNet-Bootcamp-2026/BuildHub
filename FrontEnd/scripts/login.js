import { base_url } from "./base_url.js";
import {parseJwt} from "./parseJwt.js";

const emailInput = document.getElementById("inputEmail");
const passwordInput = document.getElementById("inputPassword");
const signInButton = document.getElementById("signIn");

async function login(event) {
    // Prevent default form reload if button is inside a <form>
    if (event) event.preventDefault();

    try {
        const response = await fetch(`${base_url}/api/Auth/login`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                email: emailInput.value,
                password: passwordInput.value
            })
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem("token", data.token);

            parseJwt(data.token);
            window.location.href = "../pages/dashboard.html";
        } else {
            // Handle server-side errors (e.g., 401 Unauthorized)
            console.error("Login failed with status:", response.status);
            alert("Invalid email or password.");
        }
    } catch (networkError) {
        // Handle network/connection failure
        console.error("Network error occurred:", networkError);
        alert("Unable to connect to the server. Please check your connection.");
    }
}

// Modern event listener attachment
signInButton.addEventListener("click", login);
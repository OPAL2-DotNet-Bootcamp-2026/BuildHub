import {base_url} from "./base_url.js";


const email = document.getElementById("inputEmail");
const password = document.getElementById("inputPassword");


async function login() {
    const response = await fetch(`${base_url}/api/Auth/login`,
    {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            email: email.value,
            password: password.value
        })
    });


    if (response.ok) {
        const data = await response.json();
        localStorage.setItem("token", data.token);
        window.location.href = "../pages/dashboard.html";
    }
}


document.getElementById("signIn").onclick = login;


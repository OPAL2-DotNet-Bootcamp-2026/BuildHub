"use strict";
// ===============================
// View Product Page
// ===============================
const base_url = "https://localhost:7101";
// Get product id from the page URL
function getProductIdFromUrl() {
    const params = new URLSearchParams(window.location.search);
    const id = params.get("id");
    return id ? parseInt(id, 10) : null;
}
// Get a single product from Backend
async function getProductById(id) {
    const response = await fetch(`${base_url}/api/Products/${id}`);
    if (response.status === 404) {
        return null;
    }
    if (!response.ok) {
        throw new Error(`Failed to load product. Status: ${response.status}`);
    }
    const product = await response.json();
    return product;
}
// Display product on the page
function displayProduct(product) {
    const titleEl = document.querySelector(".product-title");
    const priceEl = document.querySelector(".price-amount");
    const mainImgEl = document.querySelector(".gallery-container > img");
    if (titleEl)
        titleEl.textContent = product.name;
    if (priceEl)
        priceEl.textContent = `OMR ${product.price.toFixed(2)}`;
    if (mainImgEl && product.imageUrl)
        mainImgEl.src = product.imageUrl;
}
// Load product
async function loadProduct() {
    try {
        const id = getProductIdFromUrl();
        if (id === null) {
            console.error("No product id in URL");
            return;
        }
        const product = await getProductById(id);
        if (!product) {
            console.error("Product not found");
            return;
        }
        displayProduct(product);
    }
    catch (error) {
        console.error("Error loading product:", error);
    }
}
// Start application
loadProduct();

import { base_url } from "./base_url.js";
// Fetch products from Backend API
async function getProducts() {
    const response = await fetch(`${base_url}/api/Products`);
    if (!response.ok) {
        throw new Error(`Failed to load products. Status: ${response.status}`);
    }
    const products = await response.json();
    return products;
}
// Map unit IDs or values to readable labels if needed
function getUnitLabel(unit) {
    switch (unit) {
        case 1:
            return "per sqm";
        case 2:
            return "per piece";
        case 3:
            return "per set";
        default:
            return "per unit";
    }
}
// Create single product HTML template string
function createProductCard(product) {
    // Fallback image if imageUrl is null
    const imageSrc = product.imageUrl || "../assets/placeHolder.png";
    // Dynamic stock badge styling
    const stockBadge = product.isAvailable
        ? `<span class="badge rounded-pill bg-success-subtle text-success position-absolute top-0 start-0 m-3">In Stock</span>`
        : `<span class="badge rounded-pill bg-danger-subtle text-danger position-absolute top-0 start-0 m-3">Out of Stock</span>`;
    return `
        <div class="col-12 col-md-6 col-lg-4">
          <article class="card h-100 border shadow-sm rounded-3">
            <!-- Product image -->
            <div class="position-relative">
              <a href="../pages/view-product.html?id=${product.productId}">
                <img
                  src="${imageSrc}"
                  class="card-img-top object-fit-cover"
                  alt="${product.name}"
                  height="220"
                />
              </a>

              <!-- Stock status -->
              ${stockBadge}
            </div>

            <!-- Product information -->
            <div class="card-body d-flex flex-column justify-content-between">
              <div>
                <h2 class="fs-6 fw-bold text-truncate">${product.name}</h2>

                <p class="small text-secondary mb-3">
                  🏪 Vendor #${product.vendorProfileId}
                  <span class="mx-2">·</span>
                  📍 Location
                </p>
              </div>

              <!-- Price and compare button -->
              <div class="d-flex justify-content-between align-items-center mt-2">
                <p class="mb-0">
                  <strong class="fs-5"> OMR ${product.price.toFixed(2)} </strong>
                  <small class="text-secondary"> ${getUnitLabel(product.unit)} </small>
                </p>
                <button
                  type="button"
                  class="btn btn-outline-secondary btn-sm rounded-pill compare-btn"
                  data-product-id="${product.productId}"
                >
                  + Compare
                </button>
              </div>
            </div>
          </article>
        </div>
    `;
}
// Display products in DOM
function displayProducts(products) {
    const container = document.getElementById("products-container");
    if (!container) {
        console.error("Container element '#products-container' not found in DOM.");
        return;
    }
    if (products.length === 0) {
        container.innerHTML = `<p class="text-muted">No products available.</p>`;
        return;
    }
    // Map each product to HTML string and join them
    container.innerHTML = products.map((product) => createProductCard(product)).join("");
}
// Load products
async function loadProducts() {
    try {
        const products = await getProducts();
        displayProducts(products);
    }
    catch (error) {
        console.error("Error loading products:", error);
    }
}
// Start application
loadProducts();

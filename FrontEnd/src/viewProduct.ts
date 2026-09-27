
// Central place for all API calls

export const base_url = "https://localhost:7101";

export interface Product {
  productId: number;
  vendorProfileId: number;
  categoryId: number;
  name: string;
  unit: number;
  price: number;
  imageUrl: string;
  isAvailable: boolean;
}

/**
 * Fetches all products (GET /api/products)
 */
export async function getAllProducts(): Promise<Product[]> {
  const response = await fetch(`${base_url}/api/products`);
  if (!response.ok) {
    throw new Error(`failed to load products: ${response.status}`);
  }
  return response.json();
}

/**
 * Fetches a single product by id (GET /api/products/{id})
 * Returns null if the product doesn't exist (404)
 */
export async function getProductById(id: number): Promise<Product | null> {
  const response = await fetch(`${base_url}/api/products/${id}`);

  if (response.status === 404) {
    return null;
  }

  if (!response.ok) {
    throw new Error(`failed to load product: ${response.status}`);
  }

  return response.json();
}




import { getProductById, Product } from "./api";

/**
 * Reads the product id from the page URL
 * Example: view-product.html?id=5
 */
function getProductIdFromUrl(): number | null {
  const params = new URLSearchParams(window.location.search);
  const id = params.get("id");
  return id ? parseInt(id, 10) : null;
}

/**
 * Fills the page elements with the product data
 */
function renderProduct(product: Product): void {
  const titleEl = document.querySelector<HTMLElement>(".product-title");
  const priceEl = document.querySelector<HTMLElement>(".price-amount");
  const mainImgEl = document.querySelector<HTMLImageElement>(
    ".gallery-container > img"
  );
  const availabilityEl = document.querySelector<HTMLElement>(
    ".spec-row:last-child .spec-value"
  );

  if (titleEl) titleEl.textContent = product.name;
  if (priceEl) priceEl.textContent = `OMR ${product.price.toFixed(2)}`;
  if (mainImgEl) mainImgEl.src = product.imageUrl;
  if (availabilityEl) {
    availabilityEl.textContent = product.isAvailable
      ? "In Stock"
      : "Out of Stock";
  }
}

/**
 * Shows a simple error message inside the page
 */
function renderError(message: string): void {
  const container = document.querySelector<HTMLElement>(".details-container");
  if (container) {
    container.innerHTML = `<p class="text-danger">${message}</p>`;
  }
}

async function init(): Promise<void> {
  const productId = getProductIdFromUrl();

  if (productId === null) {
    renderError("Product ID not specified.");
    return;
  }

  try {
    const product = await getProductById(productId);

    if (!product) {
      renderError("Product not found.");
      return;
    }

    renderProduct(product);
  } catch (error) {
    console.error(error);
    renderError("Error while loading product data.");
  }
}

document.addEventListener("DOMContentLoaded", init);



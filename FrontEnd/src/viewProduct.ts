// ===============================
// View Product Page
// ===============================

 const base_url = "https://localhost:7101";

// Product interface

interface Product {
    productId: number;
    vendorProfileId: number;
    categoryId: number;
    name: string;
    unit: number;
    price: number;
    imageUrl: string | null;
    isAvailable: boolean;
}


// Get product id from the page URL

function getProductIdFromUrl(): number | null {
    const params = new URLSearchParams(window.location.search);
    const id = params.get("id");
    return id ? parseInt(id, 10) : null;
}


// Get a single product from Backend

async function getProductById(id: number): Promise<Product | null> {

    const response = await fetch(
        `${base_url}/api/Products/${id}`
    );

    if (response.status === 404) {
        return null;
    }

    if (!response.ok) {
        throw new Error(
            `Failed to load product. Status: ${response.status}`
        );
    }

    const product: Product = await response.json();

    return product;
}


// Display product on the page

function displayProduct(product: Product): void {

    const titleEl = document.querySelector<HTMLElement>(".product-title");
    const priceEl = document.querySelector<HTMLElement>(".price-amount");
    const mainImgEl = document.querySelector<HTMLImageElement>(".gallery-container > img");

    if (titleEl) titleEl.textContent = product.name;
    if (priceEl) priceEl.textContent = `OMR ${product.price.toFixed(2)}`;
    if (mainImgEl && product.imageUrl) mainImgEl.src = product.imageUrl;

}


// Load product

async function loadProduct(): Promise<void> {

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

    } catch (error) {

        console.error(
            "Error loading product:",
            error
        );

    }

}



// Start application

loadProduct();
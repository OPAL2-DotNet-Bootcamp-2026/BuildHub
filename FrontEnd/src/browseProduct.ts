// ===============================
// Backend API
// ===============================

const BASE_URL = "https://localhost:7101";


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


// Get products from Backend

async function getProducts(): Promise<Product[]> {

    const response = await fetch(
        `${BASE_URL}/api/Products`
    );

    if (!response.ok) {
        throw new Error(
            `Failed to load products. Status: ${response.status}`
        );
    }

    const products: Product[] = await response.json();

    return products;
}


// Display products

function displayProducts(products: Product[]): void {

    console.log("Products from Backend:");
    console.log(products);

}


// Load products

async function loadProducts(): Promise<void> {

    try {

        const products = await getProducts();

        displayProducts(products);

    } catch (error) {

        console.error(
            "Error loading products:",
            error
        );

    }

}



// Start application

loadProducts();
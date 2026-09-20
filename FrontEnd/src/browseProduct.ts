//search for products by name, store, price range, location, and category

interface Product {
    id: number;
    name: string;
    store: string;
    price: number;
    location: string;
    category: string;
}


//creat products array with 3 products
const products: Product[] = [
    {
        id: 1,
        name: "Beige Marble Floor Tile 60×60",
        store: "Salalah Tiles & Ceramics",
        price: 14.5,
        location: "Salalah",
        category: "Floor Tiles"
    },

    {
        id: 2,
        name: "White Marble Floor Tile 60×60",
        store: "Muscat Ceramics",
        price: 16,
        location: "Muscat",
        category: "Floor Tiles"
    },

    {
        id: 3,
        name: "Modern Wall Tile 30×60",
        store: "Oman Tiles",
        price: 12,
        location: "Muscat",
        category: "Wall Tiles"
    }
];


//save selected products in an array
const selectedProducts: Product[] = [];

// Get all compare buttons from the HTML page
const compareButtons =
    document.querySelectorAll<HTMLButtonElement>(".compare-btn");


// Add click event to every compare button
compareButtons.forEach((button) => {

    button.addEventListener("click", () => {

        // Get the product ID from the HTML button
        const productId = Number(button.dataset.productId);

        // Find the product using its ID
        const product = products.find(
            (item) => item.id === productId
        );

        // If the product does not exist, stop
        if (!product) {
            return;
        }

        // Check if the product is already selected
        const alreadySelected = selectedProducts.some(
            (item) => item.id === productId
        );

        if (alreadySelected) {
            alert("This product is already selected.");
            return;
        }

        // Add product to selected products
        selectedProducts.push(product);

        // Change button text
        button.textContent = "✓ Selected";

        // Show selected products in console
        console.log("Selected products:", selectedProducts);
    });

});



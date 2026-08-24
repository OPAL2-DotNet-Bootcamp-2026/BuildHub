// search functionality
const searchInput = document.getElementById('searchInput');
searchInput.addEventListener('input', function() {
  const searchTerm = this.value.toLowerCase();
  // Implementation for search logic
});

const searchInput = document.getElementById('searchInput');
const categorySelect = document.getElementById('categorySelect');
const citySelect = document.getElementById('citySelect');
const vendorsContainer = document.getElementById('vendorsContainer');


function filterVendors() {
    const query = searchInput.value.toLowerCase();
    const selectedCategory = categorySelect.value;
    const selectedCity = citySelect.value;



    const cards = vendorsContainer.getElementsByClassName('card');

    cards.forEach(card => {
        const cardText = card.textContent.toLowerCase();
        const matchesQuery = cardText.includes(query);
        const matchesCategory = selectedCategory === 'All Categories' || cardText.includes(selectedCategory.toLowerCase());
        const matchesCity = selectedCity === 'All Cities' || cardText.includes(selectedCity.toLowerCase());



const matchesSearch = cardText.includes(query);
    const matchesCategory =
      selectedCategory === "All Categories" ||
      cardText.includes(selectedCategory.toLowerCase());
    const matchesCity =
      selectedCity === "All Cities" ||
      cardText.includes(selectedCity.toLowerCase());


if (matchesSearch && matchesCategory && matchesCity) {
    card.classList.remove('d-none');
} else {
    card.classList.add('d-none');
}   
    });
}

searchInput.addEventListener('input', filterVendors);
categorySelect.addEventListener('change', filterVendors);
citySelect.addEventListener('change', filterVendors);



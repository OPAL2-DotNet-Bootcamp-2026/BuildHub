const API_BASE_URL: string = 'https://localhost:7102/VendorProfile/Vendors';

const vendorTypes: Record<number, string> = {
    0: 'General Contractor',
    1: 'Interior Designer',
    2: 'Tiles & Ceramics',
    3: 'Plumber',
    4: 'Electrician'
};

interface Review {
    id?: number;
    rating?: number;
    comment?: string;
}

interface Vendor {
    vendorProfileId: number;
    companyName: string;
    city: string;
    vendorType: number;
    isVerified: boolean;
    averageRating?: number;
    balance: number;
    reviews?: Review[];
}

document.addEventListener('DOMContentLoaded', (): void => {
    fetchVendors();
});

async function fetchVendors(): Promise<void> {
    try {
        const response: Response = await fetch(API_BASE_URL);

        if (response.status === 204) {
            renderNoData();
            return;
        }

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const vendors: Vendor[] = await response.json();
        renderVendors(vendors);
    } catch (error) {
        console.error('Error fetching vendors:', error);
        const container = document.getElementById('vendors-container');
        if (container) {
            container.innerHTML = `
            <div class="col-12 text-center py-5">
                <p class="text-danger">Failed to load vendors. Please check API connection.</p>
            </div>`;
        }
    }
}

function renderVendors(vendors: Vendor[]): void {
    const container = document.getElementById('vendors-container');
    const countElement = document.getElementById('vendors-count');

    if (!container || !countElement) return;

    container.innerHTML = '';
    countElement.textContent = `${vendors.length} vendors available`;

    vendors.forEach((vendor: Vendor) => {
        const rating: number = vendor.averageRating || 0;
        const totalReviews: number = vendor.reviews ? vendor.reviews.length : 0;
        const categoryName: string = vendorTypes[vendor.vendorType] || 'Vendor';

        const cardHtml: string = `
        <div class="col-xl-4 col-lg-6">
          <div class="card h-100 rounded-3 overflow-hidden shadow-sm">
            <div class="position-relative">
              <a href="../pages/view-vendor-profile.html?id=${vendor.vendorProfileId}">
                <img src="../assets/interior desing.jpg" class="card-img-top" alt="${vendor.companyName}" style="height: 180px; object-fit: cover;" />
              </a>

              ${vendor.isVerified ? `
                <span class="badge bg-success-subtle text-success position-absolute top-0 start-0 m-2 rounded-pill">
                  ✓ Verified
                </span>` : ''}

              <span class="badge bg-light text-dark position-absolute top-0 start-0 mt-2 ${vendor.isVerified ? 'ms-5 ps-4' : 'ms-2'} rounded-pill">
                ${categoryName}
              </span>
            </div>

            <div class="card-body">
              <div class="d-flex align-items-center mb-2">
                <img src="../assets/Noor.jpg" class="rounded-circle me-2" width="35" height="35" alt="${vendor.companyName}" />
                <div>
                  <h6 class="mb-0 fw-bold">${vendor.companyName}</h6>
                  <small class="text-muted">${vendor.city}</small>
                </div>
              </div>

              <div class="d-flex justify-content-between align-items-center mb-2">
                <div>
                  <span class="text-warning">★</span>
                  <small class="fw-bold">${rating.toFixed(1)}</small>
                  <small class="text-muted">(${totalReviews})</small>
                </div>
                <small class="text-muted">Balance: OMR ${vendor.balance}</small>
              </div>

              <p class="small text-muted mb-0">
                Professional services in ${vendor.city} specialized in ${categoryName}.
              </p>
            </div>
          </div>
        </div>`;

        container.insertAdjacentHTML('beforeend', cardHtml);
    });
}

function renderNoData(): void {
    const countElement = document.getElementById('vendors-count');
    const container = document.getElementById('vendors-container');

    if (countElement) countElement.textContent = '0 vendors available';
    if (container) {
        container.innerHTML = `
        <div class="col-12 text-center py-5">
            <p class="text-muted">No vendor profiles found.</p>
        </div>`;
    }
}
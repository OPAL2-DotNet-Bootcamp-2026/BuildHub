class SharedHeader extends HTMLElement {
  connectedCallback() {
    this.innerHTML = `
    <header class="site-header">
      <div class="wrap w-7xl bar">
        <a class="logo" href="index.html">
          <span class="mark">B</span>
          <span class="word">BuildHub</span>
        </a>
        <nav class="nav">
          <a class="nav-link active" href="#">Browse Vendors</a>
          <a class="nav-link" href="#">Post a Job</a>
          <a class="nav-link" href="#">Compare Products</a>
          <a class="nav-link" href="#">How It Works</a>
        </nav>
        <div class="header-actions">
          <button class="lang-btn" type="button">عربي</button>
          <a class="nav-link d-none d-md-block" href="#">Log in</a>
          <a class="cta-link" href="#">Get Started</a>
          <button
            class="hamburger"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#mobileNav"
            aria-expanded="false"
            aria-controls="mobileNav"
          >
            <svg
              data-nav-icon="open"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
            >
              <line x1="3" y1="6" x2="21" y2="6" />
              <line x1="3" y1="12" x2="21" y2="12" />
              <line x1="3" y1="18" x2="21" y2="18" />
            </svg>
            <svg
              data-nav-icon="close"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
            >
              <line x1="18" y1="6" x2="6" y2="18" />
              <line x1="6" y1="6" x2="18" y2="18" />
            </svg>
          </button>
        </div>
      </div>
      <div class="collapse mobile-nav" id="mobileNav">
        <a class="active" href="#">Browse Vendors</a>
        <a href="#">Post a Job</a>
        <a href="#">Compare Products</a>
        <a href="#">How It Works</a>
        <a href="#">Log in</a>
        <a class="btn btn-accent" href="#">Get Started</a>
      </div>
    </header>
    `;
  }
}

customElements.define('shared-header', SharedHeader);
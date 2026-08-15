class SharedHeader extends HTMLElement {
  connectedCallback() {
    this.innerHTML = `
    <style>
      @import url("https://fonts.googleapis.com/css2?family=Playfair+Display:ital,wght@0,400;0,600;0,700;1,400&family=Open+Sans:ital,wght@0,300;0,400;0,500;0,600;0,700;1,400&display=swap");

      /* --- Design tokens -------------------------------------------------- */
      :root {
        --primary: #1b3a5c;
        --accent: #e8622a;
        --accent-dark: #c94e1a;
        --fg: #1a1a1a;
        --card: #ffffff;
        --muted: #f0f3f6;
        --muted-fg: #555555;
        --border: #e2e8f0;

        --primary-08: #eef3f8;
        --primary-30: rgba(27, 58, 92, 0.3);

        --r-sm: 6px;
        --r-md: 10px;
        --r-lg: 12px;
        --r-full: 999px;

        --sans: "Open Sans", system-ui, -apple-system, Segoe UI, sans-serif;
        --serif: "Playfair Display", Georgia, serif;

        --header-h: 72px;
      }

      .site-header,
      .mobile-nav {
        font-family: var(--sans);
      }
      .site-header a,
      .mobile-nav a {
        color: inherit;
        text-decoration: none;
      }
      .site-header button {
        font: inherit;
        color: inherit;
        margin: 0;
        border: 0;
        background: none;
        cursor: pointer;
      }
      .site-header :focus-visible,
      .mobile-nav :focus-visible {
        outline: 2px solid var(--primary);
        outline-offset: 2px;
      }

      /* Layout */
      .site-header .wrap {
        width: 100%;
        margin-inline: auto;
        padding-inline: 24px;
      }
      .site-header .w-7xl {
        max-width: 1320px;
      }

      /* Header container */
      .site-header {
        position: sticky;
        top: 0;
        z-index: 50;
        background: #ffffff;
        border-bottom: 1px solid var(--border);
      }
      .site-header .bar {
        height: var(--header-h);
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 24px;
      }

      /* Logo with circular badge */
      .logo {
        display: flex;
        align-items: center;
        gap: 12px;
      }
      .logo .mark {
        width: 38px;
        height: 38px;
        border-radius: 50%;
        background: var(--primary);
        color: #fff;
        font-weight: 700;
        font-size: 16px;
        display: flex;
        align-items: center;
        justify-content: center;
      }
      .logo .word {
        font-family: var(--serif);
        font-weight: 700;
        font-size: 22px;
        color: var(--primary);
        letter-spacing: -0.01em;
      }

      /* Desktop navigation */
      .site-header .nav {
        display: none;
        align-items: center;
        gap: 8px;
      }
      @media (min-width: 992px) {
        .site-header .nav {
          display: flex;
        }
      }

      .site-header .nav-link {
        position: relative;
        display: inline-flex;
        align-items: center;
        border-radius: var(--r-lg);
        font-size: 15px;
        font-weight: 500;
        color: var(--muted-fg);
        padding: 10px 18px;
        transition: all 0.15s ease;
      }
      .site-header .nav-link:hover {
        color: var(--fg);
        background: var(--muted);
      }
      .site-header .nav-link.active {
        color: var(--primary);
        background: var(--primary-08);
        font-weight: 600;
      }

      /* Notification Counter Badge */
      .badge-count {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        width: 18px;
        height: 18px;
        background-color: var(--accent);
        color: #ffffff;
        border-radius: 50%;
        font-size: 11px;
        font-weight: 700;
        margin-left: 6px;
      }

      /* Right actions */
      .header-actions {
        display: flex;
        align-items: center;
        gap: 16px;
      }

      .lang-btn {
        padding: 6px 16px;
        border-radius: var(--r-full);
        border: 1px solid var(--border);
        font-size: 14px;
        font-weight: 600;
        transition: background-color 0.15s;
      }
      .lang-btn:hover {
        background: var(--muted);
      }

      /* User Profile Pill */
      .user-profile {
        display: flex;
        align-items: center;
        gap: 10px;
        font-size: 14px;
        font-weight: 600;
        color: var(--fg);
      }
      .user-avatar {
        width: 34px;
        height: 34px;
        border-radius: 50%;
        background-color: var(--primary-08);
        color: var(--primary);
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 13px;
        font-weight: 700;
      }

      /* Primary Action Button */
      .cta-link {
        display: none;
        align-items: center;
        justify-content: center;
        padding: 10px 22px;
        border-radius: var(--r-full);
        background: var(--accent);
        color: #fff;
        font-size: 14px;
        font-weight: 600;
        transition: background-color 0.15s;
      }
      .cta-link:hover {
        background: var(--accent-dark);
        color: #fff;
      }
      @media (min-width: 640px) {
        .cta-link {
          display: flex;
        }
      }

      /* Mobile Hamburger */
      .hamburger {
        display: flex;
        padding: 8px;
        border-radius: var(--r-md);
      }
      @media (min-width: 992px) {
        .hamburger {
          display: none;
        }
      }
      .hamburger svg {
        width: 24px;
        height: 24px;
      }
      .hamburger [data-nav-icon="close"] {
        display: none;
      }
      .hamburger[aria-expanded="true"] [data-nav-icon="open"] {
        display: none;
      }
      .hamburger[aria-expanded="true"] [data-nav-icon="close"] {
        display: block;
      }

      .mobile-nav {
        border-top: 1px solid var(--border);
        background: var(--card);
        padding: 16px;
      }
      @media (min-width: 992px) {
        .mobile-nav {
          display: none !important;
        }
      }
      .mobile-nav a {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 10px 14px;
        border-radius: var(--r-md);
        font-size: 14px;
        font-weight: 500;
      }
      .mobile-nav a + a {
        margin-top: 4px;
      }
      .mobile-nav a.active {
        color: var(--primary);
        background: var(--primary-08);
        font-weight: 600;
      }
      .mobile-nav .btn-accent {
        margin-top: 12px;
        width: 100%;
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 12px;
        background: var(--accent);
        color: #fff;
        border-radius: var(--r-full);
        font-weight: 600;
      }
    </style>

    <header class="site-header">
      <div class="wrap w-7xl bar">
        <a class="logo" href="../pages/index.html">
          <span class="mark">B</span>
          <span class="word">BuildHub</span>
        </a>

        <nav class="nav">
          <a class="nav-link" href="../pages/Dashboard.html">Dashboard</a>
          <a class="nav-link" href="../pages/Browse_Vendors.html">Browse Vendors</a>
          <a class="nav-link" href="../pages/my-jobs.html">My Jobs</a>
          <a class="nav-link" href="../pages/browse_products.html">Products</a>
          <a class="nav-link" href="../pages/notifications.html">
            Notifications <span class="badge-count">2</span>
          </a>
        </nav>

        <div class="header-actions">
          <button class="lang-btn" type="button">عربي</button>
          
          <a href="../pages/personalization.html"><div class="user-profile">
            <span class="user-avatar">SA</span>
            <span class="user-name d-none d-sm-inline">Salim Al-Balushi</span>
          </div>
          </a>

          <a class="cta-link" href="../pages/post-job.html">+ Post Job</a>

          <button
            class="hamburger"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#mobileNav"
            aria-expanded="false"
            aria-controls="mobileNav"
          >
            <svg data-nav-icon="open" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round">
              <line x1="3" y1="6" x2="21" y2="6" />
              <line x1="3" y1="12" x2="21" y2="12" />
              <line x1="3" y1="18" x2="21" y2="18" />
            </svg>
            <svg data-nav-icon="close" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round">
              <line x1="18" y1="6" x2="6" y2="18" />
              <line x1="6" y1="6" x2="18" y2="18" />
            </svg>
          </button>
        </div>
      </div>

      <div class="collapse mobile-nav" id="mobileNav">
        <a href="../pages/Dashboard.html">Dashboard</a>
        <a href="../pages/Browse_Vendors.html">Browse Vendors</a>
        <a class="active" href="../pages/my-jobs.html">My Jobs</a>
        <a href="../pages/browse_products.html">Products</a>
        <a href="../pages/notifications.html">
          Notifications <span class="badge-count">2</span>
        </a>
        <a class="btn-accent" href="../pages/post-job.html">+ Post Job</a>
      </div>
    </header>
    `;
  }
}

customElements.define('shared-header', SharedHeader);
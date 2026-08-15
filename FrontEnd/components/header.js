class SharedHeader extends HTMLElement {
  connectedCallback() {
    this.innerHTML = `
    <style>
      @import url("https://fonts.googleapis.com/css2?family=Playfair+Display:ital,wght@0,400;0,600;0,700;1,400&family=Open+Sans:ital,wght@0,300;0,400;0,500;0,600;0,700;1,400&display=swap");

      /* --- Design tokens used by the header ------------------------------ */
      :root {
        --primary: #1b3a5c;
        --accent: #e8622a;
        --accent-dark: #c94e1a;
        --fg: #1a1a1a;
        --card: #ffffff;
        --muted: #ebe6df;
        --muted-fg: #6b6258;
        --border: #ddd7ce;

        --primary-08: rgba(27, 58, 92, 0.08);
        --primary-30: rgba(27, 58, 92, 0.3);

        --r-sm: 6px;
        --r-md: 10px;
        --r-lg: 12px;
        --r-full: 999px;

        --sans: "Open Sans", system-ui, -apple-system, Segoe UI, sans-serif;
        --serif: "Playfair Display", Georgia, serif;

        --header-h: 64px;
      }

      /* --- Reboot rules that reach the header markup ---------------------- */
      .site-header,
      .mobile-nav {
        font-family: var(--sans);
      }
      .site-header a,
      .mobile-nav a {
        color: inherit;
        text-decoration: none;
      }
      .site-header a:hover,
      .mobile-nav a:hover {
        color: inherit;
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

      /* --- Layout utilities used by the header bar ------------------------ */
      .site-header .wrap {
        width: 100%;
        margin-inline: auto;
        padding-inline: 16px;
      }
      @media (min-width: 640px) {
        .site-header .wrap {
          padding-inline: 24px;
        }
      }
      .site-header .w-7xl {
        max-width: 1280px;
      }

      /* --- Header chrome -------------------------------------------------- */
      .site-header {
        position: sticky;
        top: 0;
        z-index: 50;
        background: rgba(255, 255, 255, 0.95);
        backdrop-filter: blur(8px);
        border-bottom: 1px solid var(--border);
      }
      .site-header .bar {
        height: var(--header-h);
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 16px;
      }

      .logo {
        display: flex;
        align-items: center;
        gap: 10px;
      }
      .logo .mark {
        width: 32px;
        height: 32px;
        border-radius: var(--r-md);
        background: var(--primary);
        color: #fff;
        font-weight: 700;
        font-size: 14px;
        display: flex;
        align-items: center;
        justify-content: center;
      }
      .logo .word {
        font-family: var(--serif);
        font-weight: 700;
        font-size: 20px;
        color: var(--primary);
        letter-spacing: -0.01em;
      }
      .logo.sm .mark {
        width: 28px;
        height: 28px;
        font-size: 12px;
        border-radius: var(--r-sm);
      }
      .logo.sm .word {
        font-size: 18px;
      }

      /* Desktop nav — hidden below 768px, where the hamburger takes over */
      .site-header .nav {
        display: none;
        align-items: center;
        gap: 4px;
        flex-wrap: nowrap;
      }
      @media (min-width: 768px) {
        .site-header .nav {
          display: flex;
        }
      }

      .site-header .nav-link,
      .header-actions .nav-link {
        position: relative;
        display: block;
        border-radius: var(--r-md);
        transition:
          background-color 0.15s,
          color 0.15s;
        font-size: 14px;
        font-weight: 500;
        color: var(--muted-fg);
        padding: 8px 14px;
      }
      .site-header .nav-link:hover,
      .header-actions .nav-link:hover {
        background: var(--muted);
        color: var(--fg);
      }
      .site-header .nav-link.active {
        color: var(--primary);
        background: var(--primary-08);
      }

      .header-actions {
        display: flex;
        align-items: center;
        gap: 8px;
      }

      .lang-btn {
        padding: 6px 12px;
        border-radius: var(--r-md);
        border: 1px solid var(--border);
        font-size: 12px;
        font-weight: 600;
        transition: background-color 0.15s;
      }
      .lang-btn:hover {
        background: var(--muted);
      }

      .cta-link {
        display: none;
        align-items: center;
        gap: 6px;
        padding: 8px 16px;
        border-radius: var(--r-md);
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

      /* Hamburger drives Bootstrap's collapse plugin; icon swap follows aria-expanded */
      .hamburger {
        display: flex;
        padding: 8px;
        border-radius: var(--r-md);
        transition: background-color 0.15s;
      }
      .hamburger:hover {
        background: var(--muted);
      }
      @media (min-width: 768px) {
        .hamburger {
          display: none;
        }
      }
      .hamburger svg {
        width: 20px;
        height: 20px;
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
        padding: 12px 16px;
      }
      @media (min-width: 768px) {
        .mobile-nav {
          display: none !important;
        }
      }
      .mobile-nav a {
        display: block;
        padding: 10px 12px;
        border-radius: var(--r-md);
        font-size: 14px;
        font-weight: 500;
      }
      .mobile-nav a + a {
        margin-top: 4px;
      }
      .mobile-nav a:hover {
        background: var(--muted);
      }
      .mobile-nav a.active {
        color: var(--primary);
        background: var(--primary-08);
      }
      .mobile-nav .btn {
        margin-top: 8px;
        width: 100%;
      }

      /* --- Button styles for the mobile "Get Started" CTA ------------------ */
      .mobile-nav .btn {
        --bs-btn-padding-y: 10px;
        --bs-btn-padding-x: 20px;
        --bs-btn-font-size: 14px;
        --bs-btn-font-weight: 600;
        --bs-btn-border-radius: var(--r-md);
        --bs-btn-border-width: 1px;
        --bs-btn-border-color: transparent;
        --bs-btn-hover-border-color: transparent;
        --bs-btn-active-border-color: transparent;
        --bs-btn-box-shadow: none;
        --bs-btn-focus-box-shadow: 0 0 0 3px var(--primary-30);
        display: inline-flex;
        align-items: center;
        justify-content: center;
        gap: 8px;
        white-space: nowrap;
        transition:
          background-color 0.15s,
          border-color 0.15s,
          color 0.15s,
          transform 0.1s;
      }
      .mobile-nav .btn:active {
        transform: scale(0.98);
      }
      .mobile-nav .btn-accent {
        --bs-btn-bg: var(--accent);
        --bs-btn-color: #fff;
        --bs-btn-hover-bg: var(--accent-dark);
        --bs-btn-hover-color: #fff;
        --bs-btn-active-bg: var(--accent-dark);
        --bs-btn-active-color: #fff;
      }
    </style>
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

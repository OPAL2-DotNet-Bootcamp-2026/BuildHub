class LandingHeader extends HTMLElement {
  connectedCallback() {
    this.innerHTML = `
    <style>
      /* Tokens (--primary, --border, --sans, --header-h, …) come from
         styles/styles.css, which the landing page loads before this
         component.
        */

      .landing-header {
        font-family: var(--sans);
      }
      .landing-header a {
        color: inherit;
        text-decoration: none;
      }
      .landing-header button {
        font: inherit;
        color: inherit;
        margin: 0;
        border: 0;
        background: none;
        cursor: pointer;
      }
      .landing-header :focus-visible {
        outline: 2px solid var(--primary);
        outline-offset: 2px;
      }

      /* Layout */
      .landing-header .wrap {
        width: 100%;
        margin-inline: auto;
        padding-inline: 16px;
      }
      @media (min-width: 640px) {
        .landing-header .wrap {
          padding-inline: 24px;
        }
      }
      .landing-header .w-7xl {
        max-width: 1280px;
      }

      /* Header container */
      landing-header {
        display: block;
      }
      .landing-header {
        position: sticky;
        top: 0;
        z-index: 50;
        background: #ffffff;
        border-bottom: 1px solid var(--border);
        display: block;
      }
      .landing-header .bar {
        height: var(--header-h);
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 24px;
      }

      /* Logo with circular badge */
      .landing-header .logo {
        display: flex;
        align-items: center;
        gap: 12px;
      }
      /* Bootstrap styles .mark as a text highlight (yellow background plus
         padding), so those two declarations have to be cleared here. */
      .landing-header .logo .mark {
        width: 36px;
        height: 36px;
        flex-shrink: 0;
        display: block;
        padding: 0;
        background: none;
      }
      .landing-header .logo .word {
        font-family: var(--serif);
        font-weight: 700;
        font-size: 22px;
        color: var(--primary);
        letter-spacing: -0.01em;
      }

      /* Right actions */
      .landing-header .header-actions {
        display: flex;
        align-items: center;
        gap: 16px;
      }

      .landing-header .lang-btn {
        padding: 6px 16px;
        border-radius: var(--r-full);
        border: 1px solid var(--border);
        font-size: 14px;
        font-weight: 600;
        transition: background-color 0.15s;
      }
      .landing-header .lang-btn:hover {
        background: var(--muted);
      }

      /* Log in link */
      .landing-header .nav-link {
        display: inline-flex;
        align-items: center;
        border-radius: var(--r-lg);
        font-size: 15px;
        font-weight: 500;
        color: var(--muted-fg);
        padding: 10px 18px;
        transition: all 0.15s ease;
      }
      .landing-header .nav-link:hover {
        color: var(--fg);
        background: var(--muted);
      }

      /* Primary Action Button */
      .landing-header .cta-link {
        display: flex;
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
      .landing-header .cta-link:hover {
        background: var(--accent-dark);
        color: #fff;
      }

      /* Small screens: trim the padding, keep every action reachable */
      @media (max-width: 575.98px) {
        .landing-header .wrap {
          padding-inline: 16px;
        }
        .landing-header .bar {
          gap: 8px;
        }
        .landing-header .header-actions {
          gap: 8px;
        }
        .landing-header .lang-btn {
          padding: 6px 12px;
        }
        .landing-header .nav-link {
          padding: 10px 12px;
        }
        .landing-header .cta-link {
          padding: 10px 16px;
        }
      }
    </style>

    <header class="landing-header">
      <div class="wrap w-7xl bar">
        <a class="logo" href="../pages/index.html">
          <img class="mark" src="../assets/buildhup-icon.svg" alt="" />
          <span class="word">BuildHub</span>
        </a>

        <div class="header-actions">
          <button class="lang-btn" type="button">عربي</button>
          <a class="nav-link" href="../pages/login.html">Log in</a>
          <a class="cta-link" href="../pages/registration.html">Get Started</a>
        </div>
      </div>
    </header>
    `;
  }
}

customElements.define('landing-header', LandingHeader);

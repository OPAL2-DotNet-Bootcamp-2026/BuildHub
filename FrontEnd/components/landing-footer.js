class LandingFooter extends HTMLElement {
  connectedCallback() {
    this.innerHTML = `
    <style>
      /* Tokens (--primary, --card, --border, --sans, …) come from
         styles/styles.css, which the landing page loads before this
         component. 
        */

      /* --- Reboot rules that reach the footer markup ---------------------- */
      .landing-footer {
        font-family: var(--sans);
      }
      .landing-footer a {
        color: inherit;
        text-decoration: none;
      }
      .landing-footer a:hover {
        color: inherit;
      }
      .landing-footer button {
        font: inherit;
        color: inherit;
        margin: 0;
        border: 0;
        background: none;
        cursor: pointer;
      }
      .landing-footer ul {
        list-style: none;
        margin: 0;
        padding: 0;
      }
      .landing-footer :focus-visible {
        outline: 2px solid var(--primary);
        outline-offset: 2px;
      }

      /* --- Layout utilities used by the footer ---------------------------- */
      .landing-footer .wrap {
        width: 100%;
        margin-inline: auto;
        padding-inline: 16px;
      }
      @media (min-width: 640px) {
        .landing-footer .wrap {
          padding-inline: 24px;
        }
      }
      .landing-footer .w-7xl {
        max-width: 1280px;
      }

      /* --- Footer chrome --------------------------------------------------- */
      landing-footer {
        display: block;
      }
      .landing-footer {
        border-top: 1px solid var(--border);
        background: var(--card);
        padding-block: 40px;
      }

      /* Logo with square badge, sized down for the footer */
      .landing-footer .logo {
        display: inline-flex;
        align-items: center;
        gap: 10px;
        margin-bottom: 12px;
      }
      /* Bootstrap styles .mark as a text highlight (yellow background plus
         padding), so those two declarations have to be cleared here. */
      .landing-footer .logo .mark {
        width: 30px;
        height: 30px;
        flex-shrink: 0;
        display: block;
        padding: 0;
        background: none;
      }
      .landing-footer .logo .word {
        font-family: var(--serif);
        font-weight: 700;
        font-size: 18px;
        color: var(--primary);
        letter-spacing: -0.01em;
      }
      .landing-footer .tagline {
        font-size: 12px;
        color: var(--muted-fg);
        margin: 0;
      }

      /* Link columns */
      .landing-footer .cols {
        display: grid;
        grid-template-columns: repeat(2, minmax(0, 1fr));
        gap: 32px;
        margin-bottom: 32px;
      }
      @media (min-width: 768px) {
        .landing-footer .cols {
          grid-template-columns: repeat(4, minmax(0, 1fr));
        }
      }
      .landing-footer .cols h4 {
        font-family: var(--sans);
        font-size: 12px;
        font-weight: 700;
        text-transform: uppercase;
        letter-spacing: 0.08em;
        margin: 0 0 12px;
      }
      .landing-footer .cols li + li {
        margin-top: 8px;
      }
      .landing-footer .cols a {
        font-size: 12px;
        color: var(--muted-fg);
        transition: color 0.15s;
      }
      .landing-footer .cols a:hover {
        color: var(--fg);
      }

      /* Bottom bar */
      .landing-footer .inner {
        border-top: 1px solid var(--border);
        padding-top: 24px;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: space-between;
        gap: 12px;
        font-size: 12px;
        color: var(--muted-fg);
      }
      @media (min-width: 640px) {
        .landing-footer .inner {
          flex-direction: row;
        }
      }
    </style>

    <footer class="landing-footer">
      <div class="wrap w-7xl">
        <div class="cols">
          <div>
            <a class="logo" href="../pages/index.html">
              <img class="mark" src="../assets/buildhup-icon.svg" alt="" />
              <span class="word">BuildHub</span>
            </a>
            <p class="tagline">
              Connecting Oman's homeowners with trusted builders, designers, and
              stores.
            </p>
          </div>
          <div>
            <h4>Platform</h4>
            <ul>
              <li><a href="../pages/Browse_Vendors.html">Browse Vendors</a></li>
              <li><a href="../pages/post-job.html">Post a Job</a></li>
              <li><a href="../pages/browse_products.html">Compare Products</a></li>
              <li><a href="#">How Escrow Works</a></li>
            </ul>
          </div>
          <div>
            <h4>Vendors</h4>
            <ul>
              <li><a href="../pages/registration.html">Register Business</a></li>
              <li><a href="#">How to Get Jobs</a></li>
              <li><a href="#">Pricing</a></li>
              <li><a href="#">Help Centre</a></li>
            </ul>
          </div>
          <div>
            <h4>Company</h4>
            <ul>
              <li><a href="#">About</a></li>
              <li><a href="#">Contact</a></li>
              <li><a href="#">Privacy Policy</a></li>
              <li><a href="#">Terms of Service</a></li>
            </ul>
          </div>
        </div>
        <div class="inner">
          <span>© 2026 BuildHub LLC. Muscat, Sultanate of Oman.</span>
          <button type="button">العربية</button>
        </div>
      </div>
    </footer>
    `;
  }
}

customElements.define('landing-footer', LandingFooter);

class SharedFooter extends HTMLElement {
  connectedCallback() {
    this.innerHTML = `
    <style>
      /* Tokens (--primary, --card, --border, --sans, …) come from
        styles/styles.css, which every page loads before this component. 
      */

      /* --- Reboot rules that reach the footer markup ---------------------- */
      .site-footer {
        font-family: var(--sans);
      }
      .site-footer a {
        color: inherit;
        text-decoration: none;
      }
      .site-footer a:hover {
        color: inherit;
      }
      .site-footer button {
        font: inherit;
        color: inherit;
        margin: 0;
        border: 0;
        background: none;
        cursor: pointer;
      }
      .site-footer :focus-visible {
        outline: 2px solid var(--primary);
        outline-offset: 2px;
      }

      /* --- Layout utilities used by the footer ---------------------------- */
      .site-footer .wrap {
        width: 100%;
        margin-inline: auto;
        padding-inline: 16px;
      }
      @media (min-width: 640px) {
        .site-footer .wrap {
          padding-inline: 24px;
        }
      }
      .site-footer .w-7xl {
        max-width: 1280px;
      }

      /* --- Footer chrome --------------------------------------------------- */
      .site-footer {
        border-top: 1px solid var(--border);
        background: var(--card);
        padding-block: 24px;
      }
      .site-footer .inner {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: space-between;
        gap: 12px;
        font-size: 12px;
        color: var(--muted-fg);
      }
      @media (min-width: 640px) {
        .site-footer .inner {
          flex-direction: row;
        }
      }
    </style>
    <footer class="site-footer">
      <div class="wrap w-7xl inner">
        <span>© 2026 BuildHub LLC. Muscat, Sultanate of Oman.</span>
        <button type="button">العربية</button>
      </div>
    </footer>
    `;
  }
}

customElements.define('shared-footer', SharedFooter);

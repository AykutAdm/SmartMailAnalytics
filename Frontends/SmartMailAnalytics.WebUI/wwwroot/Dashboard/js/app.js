/**
 * Mail Admin — minimal JS: tema tercihi + statik tarih etiketi
 */
(function () {
  var root = document.documentElement;
  var STORAGE_KEY = "mail-admin-theme";

  function getStoredTheme() {
    try {
      return localStorage.getItem(STORAGE_KEY);
    } catch (_) {
      return null;
    }
  }

  function applyTheme(theme) {
    if (theme !== "light" && theme !== "dark") return;
    root.setAttribute("data-theme", theme);
    try {
      localStorage.setItem(STORAGE_KEY, theme);
    } catch (_) {
      /* ignore */
    }
  }

  function initTheme() {
    var stored = getStoredTheme();
    if (stored === "light" || stored === "dark") {
      applyTheme(stored);
      return;
    }
    var prefersLight =
      window.matchMedia && window.matchMedia("(prefers-color-scheme: light)").matches;
    applyTheme(prefersLight ? "light" : "dark");
  }

  function toggleTheme() {
    var next = root.getAttribute("data-theme") === "light" ? "dark" : "light";
    applyTheme(next);
  }

  function stampStaticDate() {
    var nodes = document.querySelectorAll("[data-static-date]");
    var fmt = new Intl.DateTimeFormat("tr-TR", {
      dateStyle: "medium",
      timeStyle: "short",
    });
    var label = fmt.format(new Date());
    nodes.forEach(function (el) {
      el.textContent = label;
    });
  }

  document.addEventListener("DOMContentLoaded", function () {
    initTheme();
    stampStaticDate();
    var btn = document.getElementById("theme-toggle");
    if (btn) btn.addEventListener("click", toggleTheme);
  });
})();

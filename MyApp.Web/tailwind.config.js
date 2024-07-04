/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml',
        './wwwroot/**/*.html',
        './node_modules/flowbite/**/*.js'
    ],
  theme: {
      extend: {
          colors: {
              primary: "var(--color-primary)",
              secondary: "var(--color-secondary)",
              "text-primary": "var(--color-text-primary)",
              "text-secondary": "var(--color-text-secondary)",
              "text-third": "var(--color-text-third)",
              "text-fourth": "var(--color-text-fourth)",
              bg: "var(--color-bg)",
              "bg-primary": "var(--color-bg-primary)",
              "bg-secondary": "var(--color-bg-secondary)",
              "bg-icon": "var(--color-bg-icon)",
              "bg-hover": "var(--color-bg-hover)",
              error: "var(--color-error)",
              shadow: "var(--color-shadow)",
              line: "var(--color-line)",
              fur: "var(--color-fur)",
              "fur-dark": "var(--color-fur-dark)",
              spot: "var(--color-spot)",
              snout: "var(--color-snout)",
              collar: "var(--color-collar)",
          },

      },
  },
    plugins: [
        require('flowbite/plugin')
    ],
}

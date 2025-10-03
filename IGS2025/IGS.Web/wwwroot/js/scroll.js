;(function () {
  const btn = document.getElementById('scrollTopBtn')
  let lastY = window.scrollY
  let ticking = false

  function atTop() {
    return window.scrollY <= 4
  }

  function update() {
    const y = window.scrollY

    if (atTop()) {
      btn.classList.add('hidden')
      btn.classList.remove('dim', 'show')
    } else {
      // Determine scroll direction
      if (y > lastY) {
        // Scrolling down: make fully visible
        btn.classList.remove('hidden', 'dim')
        btn.classList.add('show')
      } else if (y < lastY) {
        // Scrolling up: slightly fade
        btn.classList.remove('hidden', 'show')
        btn.classList.add('dim')
      }
    }

    lastY = y
    ticking = false
  }

  // Scroll handler (rAF to avoid jank)
  window.addEventListener(
    'scroll',
    () => {
      if (!ticking) {
        window.requestAnimationFrame(update)
        ticking = true
      }
    },
    { passive: true }
  )

  // Initial state
  update()

  // Click -> scroll to top (respect reduced motion)
  btn.addEventListener('click', () => {
    const reduce = window.matchMedia('(prefers-reduced-motion: reduce)').matches
    window.scrollTo({
      top: 0,
      behavior: reduce ? 'auto' : 'smooth',
    })
  })
})()
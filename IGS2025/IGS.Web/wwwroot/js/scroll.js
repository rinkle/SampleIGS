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
          try {
              btn.classList.add('hidden')
          } catch (e) {

          }
      btn.classList.remove('dim', 'show')
    } else {
      // Determine scroll direction
      if (y > lastY) {
          // Scrolling down: make fully visible
          try {
              btn.classList.remove('hidden', 'dim')
              btn.classList.add('show')

          } catch (e) {

          }
        
      } else if (y < lastY) {
          // Scrolling up: slightly fade
          try {
              btn.classList.remove('hidden', 'show')
              btn.classList.add('dim')
          } catch (e) {

          }
        
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
    try {
        btn.addEventListener('click', () => {
            const reduce = window.matchMedia('(prefers-reduced-motion: reduce)').matches
            window.scrollTo({
                top: 0,
                behavior: reduce ? 'auto' : 'smooth',
            })
        })
    } catch (e) {

    }
  // Click -> scroll to top (respect reduced motion)

})()
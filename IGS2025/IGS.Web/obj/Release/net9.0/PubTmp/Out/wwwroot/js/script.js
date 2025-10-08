$(document).ready(function () {
  //Navbar minimize on scroll down
  if ($(window).scrollTop() > 1) {
    $('.navbar').addClass('navbar-min')
  }
  $(window).scroll(function () {
    var scroll = $(window).scrollTop()
    if (scroll >= 1) {
      $('.navbar').addClass('navbar-min')
    } else {
      $('.navbar').removeClass('navbar-min')
    }
  })

  //Navbar minimize on scroll down
  $('.navbar .nav-link').click(function () {
    $('.navbar .navbar-collapse').removeClass('show')
  })


  //Get active carousel item on team
  $('.team .person .img-container').click(function () {
    let id = $(this).attr('id')
    window.__igs_lastClickedImgId = id
    if (document.getElementById('teamCarousel')) {
      // Rebuild carousel to match current filter/order and set active slide
      if (typeof rebuildTeamCarousel === 'function') {
        rebuildTeamCarousel(id)
      } else {
        let carouselId = '#' + id + '-car'
        $(carouselId).addClass('active').siblings().removeClass('active')
      }
    }
  })


  //Get active carousel item on case
  $('.case-studies .slider-box').click(function () {
    let id = $(this).attr('id')
    let carouselId = '#' + id + '-car'
    $(carouselId).addClass('active').siblings().removeClass('active')
  })

  // Team modal URL handling (only on team page)
  if (document.body && document.body.id === 'team_page') {
    function slugifyName(name) {
      return name
        .toLowerCase()
        .replace(/[\u2019'’.]/g, '')
        .replace(/[^a-z0-9]+/g, '-')
        .replace(/^-+|-+$/g, '')
    }

    function getCleanNameFromPerson($person) {
      const $name = $person.find('.name').clone()
      $name.children().remove()
      return $name.text().trim()
    }

    function buildTeamIndex() {
      const index = {}
      $('.team .person').each(function () {
        const $person = $(this)
        const nameText = getCleanNameFromPerson($person)
        const slug = slugifyName(nameText)
        const imgId = $person.find('.img-container').attr('id')
        if (imgId) {
          index[slug] = imgId
        }
      })
      return index
    }

    const teamIndex = buildTeamIndex()
    const baseTeamPath = (function () {
      const p = window.location.pathname || ''
      return p.includes('team.html') ? '/team.html' : '/team'
    })()

    function getFilterFromQuery() {
      try {
        const params = new URLSearchParams(window.location.search)
        const v = (params.get('filter') || '').trim().toLowerCase()
        return v.replace(/\s+/g, '-') || 'all'
      } catch (e) {
        return 'all'
      }
    }

    function extractSlugFromPath(pathname) {
      const bases = [baseTeamPath.endsWith('/') ? baseTeamPath : baseTeamPath + '/', '/team.html/']
      const base = bases.find(b => pathname.startsWith(b))
      if (base) {
        const rest = pathname.slice(base.length)
        // If the first segment is a known filter, the slug would be after it
        const first = (rest.split('/')[0] || '').trim().toLowerCase()
        const knownFilters = ['executives', 'partners', 'consultants', 'operations']
        if (knownFilters.includes(first)) {
          return rest.split('/').slice(1).join('/').replace(/\/$/, '')
        }
        return rest.replace(/\/$/, '')
      }
      return ''
    }

    function setActiveCarouselByImgId(imgId) {
      const carouselId = '#' + imgId + '-car'
      $(carouselId).addClass('active').siblings().removeClass('active')
    }

    function getVisibleOrderedImgIds() {
      const container = Array.from(document.querySelectorAll('.team .row')).find(row => row.querySelector('.person'))
      if (!container) return []
      const cols = Array.from(container.children).filter(el => el.querySelector('.person'))
      const visibleCols = cols.filter(el => $(el).is(':visible'))
      return visibleCols.map(col => {
        const img = col.querySelector('.person .img-container')
        return img ? img.getAttribute('id') : null
      }).filter(Boolean)
    }

    function getActiveFilterKey() {
      const active = document.querySelector('.nav-filter .nav-item.active')
      if (!active) return 'all'
      const text = (active.textContent || '').trim().toLowerCase()
      return text.replace(/\s+/g, '-') || 'all'
    }

    function getLastName(fullName) {
      const parts = (fullName || '').trim().split(/\s+/)
      return parts.length ? parts[parts.length - 1] : ''
    }

    function getTitleRank(titleText) {
      const t = (titleText || '').trim().toLowerCase()
      if (t.includes('chief growth officer')) return -1
      if (t.includes('senior partner')) return 0
      if (t.includes('associate partner')) return 2
      if (t.includes('partner')) return 1
      return 99
    }

    function sortImgIdsAccordingToActiveFilter(imgIds) {
      const key = getActiveFilterKey()
      const byLastName = (aId, bId) => {
        const aPerson = $('.team .person .img-container#' + aId).closest('.person')
        const bPerson = $('.team .person .img-container#' + bId).closest('.person')
        const aName = getCleanNameFromPerson(aPerson)
        const bName = getCleanNameFromPerson(bPerson)
        const aLast = getLastName(aName)
        const bLast = getLastName(bName)
        return aLast.localeCompare(bLast)
      }
      if (key === 'partner-team') {
        return imgIds.slice().sort((aId, bId) => {
          const aPerson = $('.team .person .img-container#' + aId).closest('.person')
          const bPerson = $('.team .person .img-container#' + bId).closest('.person')
          const aTitle = (aPerson.find('.title').text() || '').trim()
          const bTitle = (bPerson.find('.title').text() || '').trim()
          const ra = getTitleRank(aTitle)
          const rb = getTitleRank(bTitle)
          if (ra !== rb) return ra - rb
          return byLastName(aId, bId)
        })
      }
      if (key === 'consultants') {
        return imgIds.slice()
      }
      // Default: All and other tabs sorted by last name
      return imgIds.slice().sort(byLastName)
    }

    function ensureCarouselStash() {
      let stash = document.getElementById('teamCarouselStash')
      if (!stash) {
        stash = document.createElement('div')
        stash.id = 'teamCarouselStash'
        stash.style.display = 'none'
        const carousel = document.getElementById('teamCarousel')
        if (carousel && carousel.parentNode) {
          carousel.parentNode.insertBefore(stash, carousel.nextSibling)
        }
      }
      return stash
    }

    function setModalArrowsVisibility(shouldShow) {
      const $buttons = $('#teamModal .nav [data-bs-slide]')
      if (!$buttons.length) return
      if (shouldShow) {
        $buttons.show()
      } else {
        $buttons.hide()
      }
    }

    function rebuildTeamCarousel(activeImgId) {
      const visibleIds = sortImgIdsAccordingToActiveFilter(getVisibleOrderedImgIds())
      // Hide left/right arrows when only one item is available
      setModalArrowsVisibility(visibleIds.length > 1)
      const inner = document.querySelector('#teamCarousel .carousel-inner')
      if (!inner) return
      const stash = ensureCarouselStash()

      // Move all items to stash first, so we can rebuild cleanly
      Array.from(inner.querySelectorAll('.carousel-item')).forEach(node => {
        stash.appendChild(node)
      })

      // Append only visible items back in the current page order
      visibleIds.forEach(id => {
        const item = stash.querySelector('#' + id + '-car') || document.getElementById(id + '-car')
        if (item) inner.appendChild(item)
      })

      // Activate the requested slide (or first if not found)
      $('#teamCarousel .carousel-item').removeClass('active')
      const targetId = activeImgId && document.getElementById(activeImgId + '-car') ? activeImgId : visibleIds[0]
      if (targetId) {
        setActiveCarouselByImgId(targetId)
      }
    }

    function openTeamModalForSlug(slug, push) {
      const imgId = teamIndex[slug]
      if (!imgId) return
      rebuildTeamCarousel(imgId)
      const modalEl = document.getElementById('teamModal')
      const modal = bootstrap.Modal.getOrCreateInstance(modalEl)
      modal.show()
      const search = window.location.search || ''
      const path = baseTeamPath + '/' + slug + search
      const method = push ? 'pushState' : 'replaceState'
      history[method]({ teamSlug: slug }, '', path)
    }

    // When clicking a team member, push the slug URL
    $('.team .person .img-container').on('click', function () {
      const $person = $(this).closest('.person')
      const nameText = getCleanNameFromPerson($person)
      const slug = slugifyName(nameText)
      const search = window.location.search || ''
      const path = baseTeamPath + '/' + slug + search
      history.pushState({ teamSlug: slug }, '', path)
    })

    // When the modal is closed, revert URL to /team
    $('#teamModal').on('hidden.bs.modal', function () {
      history.replaceState({}, '', baseTeamPath + window.location.search)
      window.__igs_lastClickedImgId = null
    })

    // Before showing modal, rebuild carousel to match the current filter/order
    $('#teamModal').on('show.bs.modal', function () {
      const path = window.location.pathname || ''
      let slug = ''
      if (path.startsWith(baseTeamPath + '/')) {
        slug = extractSlugFromPath(path)
      }
      const imgIdFromUrl = slug ? teamIndex[slug] : null
      const imgId = window.__igs_lastClickedImgId || imgIdFromUrl || getVisibleOrderedImgIds()[0]
      if (imgId) rebuildTeamCarousel(imgId)
    })

    // When the carousel slide changes, update the slug in the URL
    $('#teamCarousel').on('slid.bs.carousel', function () {
      const $active = $(this).find('.carousel-item.active')
      if (!$active.length) return
      const carId = $active.attr('id') || ''
      const imgId = carId.replace(/-car$/, '')
      if (!imgId) return
      const $person = $('.team .person .img-container#' + imgId).closest('.person')
      if (!$person.length) return
      const nameText = getCleanNameFromPerson($person)
      const slug = slugifyName(nameText)
      const search = window.location.search || ''
      history.replaceState({ teamSlug: slug }, '', baseTeamPath + '/' + slug + search)
    })

    // Handle back/forward navigation
    window.addEventListener('popstate', function () {
      const path = window.location.pathname || ''
      if (!path.startsWith(baseTeamPath)) return
      const slug = extractSlugFromPath(path)
      const modalEl = document.getElementById('teamModal')
      const modal = bootstrap.Modal.getOrCreateInstance(modalEl)
      if (slug) {
        openTeamModalForSlug(slug, false)
      } else {
        modal.hide()
      }
    })

    // On load, deep-link into a specific team member if present
    ;(function handleInitialDeepLink() {
      const path = window.location.pathname || ''
      if (path.startsWith(baseTeamPath + '/')) {
        const slug = extractSlugFromPath(path)
        if (slug) openTeamModalForSlug(slug, false)
      }
    })()
  }
})

$(document).ready(function() {
    //Counter
    $.fn.isInViewport = function () {
        try {
        var elementTop = $(this).offset().top
        var elementBottom = elementTop + $(this).outerHeight()

        var viewportTop = $(window).scrollTop()
        var viewportBottom = viewportTop + $(window).height()

            return elementBottom > viewportTop && elementTop < viewportBottom
        } catch (err) {
            return
        }
    }

    if ($('.count').isInViewport()) {
        $('.count').each(function () {
            $(this).prop('Counter', -1).animate({
                Counter: $(this).text()
            }, {
                duration: 2000,
                easing: 'swing',
                step: function (now) {
                    $(this).text(Math.ceil(now))
                }
            })
        })
        $('.count').removeClass("count")
    }

    $(window).on('resize scroll', function() {
        if ($('.count').isInViewport()) {
            $('.count').each(function () {
                $(this).prop('Counter', -1).animate({
                    Counter: $(this).text()
                }, {
                    duration: 2000,
                    easing: 'swing',
                    step: function (now) {
                        $(this).text(Math.ceil(now))
                    }
                })
            })
            $('.count').removeClass("count")
        }
    })
})
$(document).ready(function() {
    // Slick JS News Carousel
    $('.company-slider').slick({
        centerMode: false,
        centerPadding: '0px',
        slidesToShow: 3,
        infinite: true,
        dots: false,
        arrows: true,
        autoplay: false,
        autoplaySpeed: 4000,
        swipeToSlide: true,
        responsive: [
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    autoplaySpeed: 1250,
                    arrows: true
                }
            }
        ]
    })
})
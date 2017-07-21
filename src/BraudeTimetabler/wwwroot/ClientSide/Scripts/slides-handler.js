/******************slides handler************************/
var currentSlideIndex = 0;

$(document).ready(function() {
    setCurrentSlide(0);
});

function prevSlide() {

    setCurrentSlide(currentSlideIndex - 1);
}

function nextSlide() {

    setCurrentSlide(currentSlideIndex + 1);
}

function setCurrentSlide(newSlideIndex) {
    var slides = $(".mySlides");
    var slidesButtons = $("#slidesButtons a");
   
    newSlideIndex = (slides.length + newSlideIndex) % slides.length;

    if (newSlideIndex === currentSlideIndex)
        return;

    slidesButtons.eq(currentSlideIndex).removeClass("slide-highlighted");
    slidesButtons.eq(newSlideIndex).addClass("slide-highlighted");
    currentSlideIndex = newSlideIndex;

    if (currentSlideIndex === 2) {
        handleTimetablesSlide();
    }

    for (var i = 0; i < slides.length; i++) {
        if (i === currentSlideIndex) {
            slides.eq(i).show();
        } else {
            slides.eq(i).hide();
        }
    }

}


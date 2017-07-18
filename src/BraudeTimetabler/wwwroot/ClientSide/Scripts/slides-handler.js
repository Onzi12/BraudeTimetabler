/******************slides handler************************/
var currentSlideIndex = 0;
var calculatingLabelInterval = null;
var noResultsMessage = "No Results. try to relax your constraints.";
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
    var slides = document.getElementsByClassName("mySlides");
    var slidesButtons = $("#slidesButtons a");
   
    $(slidesButtons[currentSlideIndex]).removeClass("slide-highlighted");
    currentSlideIndex = (slides.length + newSlideIndex) % slides.length;
    $(slidesButtons[currentSlideIndex]).addClass("slide-highlighted");

    for (var i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }

    if (currentSlideIndex === 2) {
        handleTimetablesSlide();
    }


    slides[currentSlideIndex].style.display = "block";
}

function handleTimetablesSlide() {
    $("#timetablesTbl > tbody").empty();
    $("#myPager").empty();
    $("#errorMessageLbl").hide();
    $("#ratingLbl").text("");

    if (selectedCoursesList.length === 0) {
        setTimetablesErrorMessage("Please select at least one course");
        return;
    }

    var isClashes = $('#flatCheckBox').is(":checked");

    var freeD = $('#freeDays').slider("option", "value");
    var maxG = $('#maxGap').slider("option", "value");


    var calculatingObj = $('#calculatingLbl');
    calculatingObj.text("Calculating");

    if (calculatingLabelInterval) {
        clearInterval(calculatingLabelInterval);
    }

    calculatingLabelInterval = setInterval(function() {
            var prev = calculatingObj.text();
            var textToAdd = ".";

            calculatingObj.html(prev + textToAdd);
        },
        250);
    calculatingObj.show();

    var generateTimetablesInput = {
        ids: selectedCoursesList,
        clashes: isClashes,
        freeDays: freeD,
        maxGap: maxG
    };

    $.post("Home/GenerateTimetables", generateTimetablesInput, clientGenerateTimetablesResponseHandler);
}

function clientGenerateTimetablesResponseHandler(data, status) {

    clearInterval(calculatingLabelInterval);
    calculatingLabelInterval = null;
    $("#calculatingLbl").hide();

    if (!data || data.length === 0) {
        setTimetablesErrorMessage(noResultsMessage);
        return;
    }

    var timetables = [];
    for (var i = 0; i < data.length; ++i) {
        timetables[i] = JSON.parse(data[i]);
    }

    $.each(timetables,
        function(j, timetable) {
            var timeslots = timetable.timeslotsMatrix;


            $.each(timeslots,
                function(i, item) {
                    $c = "";
                    $c +=
                        "<tr><td style='direction: rtl;'>" +
                        item.startHour +
                        "</td>";
                    $c +=
                        "<td style=' direction: rtl;'>" +
                        item.sunday +
                        "</td>";
                    $c +=
                        "<td style='direction: rtl; '>" +
                        item.monday +
                        "</td>";
                    $c +=
                        "<td style=' direction: rtl; '>" +
                        item.tuesday +
                        "</td>";
                    $c +=
                        "<td style=' direction: rtl;'>" +
                        item.wednesday +
                        "</td>";
                    $c +=
                        "<td style='direction: rtl;'>" +
                        item.thursday +
                        "</td>";
                    $c +=
                        "<td style='direction: rtl;'>" +
                        item.friday +
                        "</td></tr>";

                    $('#timetablesTbl').find('tbody').append($c);

                }); //end of inner $each


        }); // end of outer $each 
    $('#timetablesTbl')
        .pageMe({ pagerSelector: '#myPager', showPrevNext: true, hidePageNumbers: false, perPage: 14 },
            timetables,
            $("#ratingLbl")
            );

}

function setTimetablesErrorMessage(msg) {
    $("#errorMessageLbl").show();
    $("#errorMessageLbl").text(msg);
}
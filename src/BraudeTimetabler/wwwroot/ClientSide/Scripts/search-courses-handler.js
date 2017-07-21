
var selectedCourse;
var selectedCoursesList = [];
var removeCourse;
var hiddenCourses = [];

$(document).ready(function () {


    $('#textInput').keyup(function () {
        //split the current value of searchInput
        var data = this.value.toLowerCase().split(" ");
        //create a jquery object of the rows
        var jo = $("#coursesDataBaseTbl tbody").find("tr");
        if (this.value === "") {
            jo.show();
            return;
        }
        //hide all the rows
        jo.hide();

        //Recusively filter the jquery object to get results.
        jo.filter(function (i, v) {
                var td = $(this).text().toLowerCase();
                for (var d = 0; d < data.length; ++d) {
                    if (td.search(data[d]) >= 0) {
                        return true;
                    }
                }
                return false;
            })
            //show the rows that match.
            .show();
    });

    /*-----------------add courses to list---------------------*/

    $('#coursesDataBaseTbl tbody').on('click', 'tr', function () {

        var highlightClass = "highlighted";
        var tableRow = $(this);

        if (tableRow.hasClass(highlightClass)) {
            tableRow.toggleClass(highlightClass);
            $('#selectCourseBtn').hide();
            return;
        }

        $('#coursesDataBaseTbl').find('tr.highlighted').removeClass(highlightClass);
        tableRow.toggleClass(highlightClass);

        $('#selectCourseBtn').show();
        selectedCourse = tableRow;
    });


    $('#selectCourseBtn').on('click', function () {

        $('#coursesDataBaseTbl').find('tr.highlighted').removeClass("highlighted");

        $('#slctdTbl').find('tbody').append('<tr class="tbl-row rtl">' + selectedCourse.html() + '</tr>');


        $('#selectCourseBtn').hide();

        var newCourseId = selectedCourse[0].cells.tableCourseId.innerText;

        selectedCoursesList.push(newCourseId);

        //remove from table
        selectedCourse.hide();
        hiddenCourses.push(selectedCourse);
    });


    /*-----------------remove courses from list---------------------*/
    $('#slctdTbl tbody').on('click', 'tr', function () {

        var highlightClass = "highlighted";
        var tableRow = $(this);

        if (tableRow.hasClass(highlightClass)) {
            tableRow.toggleClass(highlightClass);
            $('#removeCourseBtn').hide();
            return;
        }

        $('#slctdTbl').find('tr.highlighted').removeClass(highlightClass);
        tableRow.toggleClass(highlightClass);

        $('#removeCourseBtn').show();

        removeCourse = tableRow;
    });


    $('#removeCourseBtn').on('click', function () {

        //remove highlight
        $('#slctdTbl').find('tr.highlighted').removeClass("highlighted");

        //hide removeCourse button
        $(this).hide();

        //remove from selected table
        removeCourse.remove();

        //remove from selected courses list
        var removeThisCourseId = removeCourse[0].childNodes[1].innerText;
        var index = selectedCoursesList.indexOf(removeThisCourseId);
        if (index > -1) {
            selectedCoursesList.splice(index, 1);
        }

        //add back to database
        for (var i = 0; i < hiddenCourses.length; i++) {
            if (hiddenCourses[i][0].childNodes[1].innerText === removeThisCourseId) {
                hiddenCourses[i].show();
                hiddenCourses.splice(i, 1);
                break;
            }
        }
    });
});


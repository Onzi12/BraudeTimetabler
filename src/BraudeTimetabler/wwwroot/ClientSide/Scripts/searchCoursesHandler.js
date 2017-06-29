/**
 * Created by ASUS_TF on 17/6/2017.
 */

//var allCourses= {};
var selectedCourse;
var tableRow;
var selectedCoursesList = [];
var removeCrs;

$(document).ready(function() {

    // $.getJSON("http://localhost/databaseMock.php",
    //    function (data) {
    //        allCourses.data = data;
    //        var tbl_body = document.createElement("tbody");  // create a new table
    //        tbl_body.id = "rtbody";
    //        $.each(data, function () {
    //            var tbl_row = tbl_body.insertRow();
    //            tbl_row.className = "tbl-row";
    //            $.each(this, function (k, v) {
    //                var cell = tbl_row.insertCell();
    //                cell.appendChild(document.createTextNode(v.toString()));
    //            })

    //        })

    //        document.getElementById("coursesDataBaseTbl").appendChild(tbl_body);
    //    });


    $('#textInput').keyup(function () {
        //split the current value of searchInput
        var data = this.value.split(" ");
        //create a jquery object of the rows
        var jo = $("#coursesDataBaseTbl").find("tr");
        if (this.value == "") {
            jo.show();
            return;
        }
        //hide all the rows
        jo.hide();

        //Recusively filter the jquery object to get results.
        jo.filter(function (i, v) {
            var $t = $(this);
            for (var d = 0; d < data.length; ++d) {
                if ($t.is(":contains('" + data[d] + "')")) {
                    return true;
                }
            }
            return false;
        })
        //show the rows that match.
            .show();
    });


    /*-----------------add courses to list---------------------*/

    $('#coursesDataBaseTbl').on('click', 'td',function() {

        tableRow =  $(this).closest('tr');

        $('#coursesDataBaseTbl').find('tr.highlighted').removeClass("highlighted");
        $('#coursesDataBaseTbl').find('td.highlighted').removeClass("highlighted");
        tableRow.toggleClass("highlighted");

        document.getElementById("selectCourseBtn").style.display = document.getElementById("selectCourseBtn").style.display === "block" ? "none" : "block";
        selectedCourse = tableRow.html();

    });


    $('#selectCourseBtn').on('click',function() {

        $('#coursesDataBaseTbl').find('tr.highlighted').removeClass("highlighted");
        $('#coursesDataBaseTbl').find('td.highlighted').removeClass("highlighted");

        $('#slctdTbl').find('tbody').append('<tr>'+selectedCourse+'</tr>');


        document.getElementById("selectCourseBtn").style.display =   document.getElementById("selectCourseBtn").style.display === "block" ? "none" : "block";



        var newCourseId = tableRow.cells.namedItem("tableCourseId").innerHTML;
        selectedCoursesList.push(newCourseId);

        //remove from table
        tableRow.remove();

        //add id to list
       // var newCourseId = $(selectedCourse).find("td").html();





    });


    /*-----------------remove courses from list---------------------*/

    $('#slctdTbl').on('click', 'td',function() {

        tableRow = $(this).closest('tr');


        $('#slctdTbl').find('tr.highlighted').removeClass("highlighted");
        $('#slctdTbl').find('td.highlighted').removeClass("highlighted");
        tableRow.toggleClass("highlighted");

        document.getElementById("removeCourseBtn").style.display = document.getElementById("removeCourseBtn").style.display === "block" ? "none" : "block";


        removeCrs = tableRow.html();


    });


    $('#removeCourseBtn').on('click',function() {

        //remove highlight
        $('#slctdTbl').find('tr.highlighted').removeClass("highlighted");
        $('#slctdTbl').find('td.highlighted').removeClass("highlighted");


        //add back to database
        $('#coursesDataBaseTbl').find('tbody').append('<tr>'+removeCrs+'</tr>');

        //hide button
        document.getElementById("removeCourseBtn").style.display =   document.getElementById("removeCourseBtn").style.display === "block" ? "none" : "block";



        //remove from selected table
        tableRow.remove();



        //remove from selected courses list
        var removeThisCourseId = tableRow[0].childNodes[1].innerText;

        var index = selectedCoursesList.indexOf(removeThisCourseId);

        if (index > -1) {
            selectedCoursesList.splice(index, 1);
        }


    });







});


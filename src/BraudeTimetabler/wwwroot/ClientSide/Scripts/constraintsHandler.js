/**
 * Created by ASUS_TF on 12/6/2017.
 */


// With JQuery

$(document).ready(function(){

$(".slider")
    .slider({
        max: 5
    })
    .slider("pips", {
        rest: "label"
    });



  //  console.log( jQuery(".slider").slider("value"));
    /*
    $('#slider').slider({
        change: function(event, ui) {
            console.log(ui.value);
        }
    });
    */
});
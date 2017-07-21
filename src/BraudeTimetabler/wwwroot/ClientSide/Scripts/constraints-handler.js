
$(document).ready(function(){

$(".slider")
    .slider({
        max: 5
    })
    .slider("pips", {
        rest: "label"
    });

});
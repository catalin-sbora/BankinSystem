// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js
//<link href='https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css'>
//    <script src='https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js'></script>
 //   <script src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js'></script>
function filterTable(event) {

    var filter = event.target.value.toUpperCase();
    var rows = document.querySelector("#exportTable tbody").rows;

    for (var i = 0; i < rows.length; i++) {
        var firstCol = rows[i].cells[1].textContent.toUpperCase();
        var secondCol = rows[i].cells[2].textContent.toUpperCase();
        var thirdCol = rows[i].cells[3].textContent.toUpperCase();
        var fourthCol = rows[i].cells[4].textContent.toUpperCase();
        var fifthCol = rows[i].cells[5].textContent.toUpperCase();

        if (firstCol.indexOf(filter) > -1 || secondCol.indexOf(filter) > -1 || thirdCol.indexOf(filter) > -1 || fourthCol.indexOf(filter) > -1 || fifthCol.indexOf(filter) > -1) {
            rows[i].style.display = "";
        } else {
            rows[i].style.display = "none";
        }
    }
}

document.querySelector('#myInput').addEventListener('keyup', filterTable, false);




$('#filter').click(function () {
    var min = +$('#min').val(); // invalid input will use 0 as minimum
    // Replace invalid max input with very high value, so everything will be shown
    var max = +$('#max').val() || 1e20;
    updateFilter(min, max);
});


$(function () {
    $("#exporttable").click(function (e) {
        var table = $("#exportTable");
        if (table && table.length) {
            $(table).table2excel({
                exclude: ".noExl",
                name: "Excel Document Name",
                filename: "BBBootstrap" + new Date().toISOString().replace(/[\-\:\.]/g, "") + ".xls",
                fileext: ".xls",
                exclude_img: true,
                exclude_links: true,
                exclude_inputs: true,
                preserveColors: false
            });
        }
    });

});
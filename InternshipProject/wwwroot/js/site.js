// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js
//<link href='https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css'>
//    <script src='https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js'></script>
 //   <script src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js'></script>
function filterTable(event) {

    var filter = event.target.value.toLowerCase();
    var rows = document.querySelector("#exportTable tbody").rows;

    for (var i = 0; i < rows.length; i++) {
        for (var j = 0; j < rows[i].cells.length; j++) {
            if (rows[i].cells[j].textContent.toLowerCase().indexOf(filter) > -1) {
                displayRow = "";
            }
            else {
                displayRow = "none";
            }
        }
        rows[i].style.display = displayRow;
    }
}

function filterTable(rows, filter) {

    var filter = filter.toLowerCase();

    for (var i = 0; i < rows.length; i++) {
        for (var j = 0; j < rows[i].cells.length; j++) {
            if (rows[i].cells[j].textContent.toLowerCase().indexOf(filter) > -1) {
                displayRow = true;
                break;
            }
            else {
                displayRow = false;
            }
        }
        if (displayRow)
        {
            $(rows[i]).removeAttr("style");
        }
        else
        {
            $(rows[i]).attr("style", "display:none");
        }
    }
}

//document.querySelector('#myInput').addEventListener('keyup', filterTable, false);




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


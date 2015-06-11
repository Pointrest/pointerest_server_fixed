$(function () {

    var gestoreUsername = getQueryVariable("username");
    var tabella = $('#puntiInteresseGestore');

    $.get("/api/pi/username/" + gestoreUsername, function (data, textStatus, jqXHR) {

        var images = [];
        $.each(data, function (index, element) {

            var row = '<tr data-id="' + element.ID + '">'
                       + "<td>" + element.Nome + "</td>"
                       + "<td>" + element.Categoria + "</td>"
                       + "<td>" + element.Sottocategoria + "</td>"
                       + "<td>" + element.Descrizione + "</td>"
                       + "<td>" + element.Latitudine + "</td>"
                       + "<td>" + element.Longitudine + "</td>"
                       + '<td><span class="glyphicon glyphicon-picture showImages" aria-hidden="true"></span></td>'
                       + '<td><span class="glyphicon glyphicon-trash deletePI" aria-hidden="true"></span></td>'
                       + '<td><span class="glyphicon glyphicon-edit editPI" aria-hidden="true"></span></td>'
                       + "</tr>";

            images[index] = element.Images;
            tabella.append(row)
        });

        $('.showImages').on('click', function () {
            $('#imagesModal').modal();
            $('#imagesContainer').empty();
            var currentIndex = $('tr').index($(this).parent().parent('tr'));
            showImages(images, currentIndex - 1);
        });

        $('.deletePI').on('click', function () {

            var row = $(this).parent().parent('tr');
            var ID = row.attr('data-id');

            $.ajax({
                url: '/api/pi/' + ID,
                type: 'DELETE',
                success: function (result) {
                    console.log("Cancelled PI " + ID);
                    row.remove();
                }
            });
        });
    });
})

function getQueryVariable(variable) {

    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return (false);
}

function showImages(images, currentIndex) {

    $.each(images[currentIndex], function (eIndex, element) {
        var imageRow = '<div class="row>">'
                    + '<div class="col-md-12"><img src=data:image/png;base64,'
                    + element.Data
                    + ' /></div>'
                    + '</div>'
        $('#imagesContainer').append(imageRow);
    });
}
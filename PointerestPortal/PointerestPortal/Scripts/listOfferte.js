$(function () {

    $.get('api/offerte', function (data, textStatus, jqXHR) {

        $.each(data, function (index, element) {

            var tableRow = '<tr>';

            if ($(element).text() == "Immagine") {

                var column = '<td><img src="'
                            + $(element).text()
                            + '" /></td>'

                tablerow.append(column)
            }
            else
            {
                var column = '<td>'
                            + $(element).text()
                            + '</td>'

                tablerow.append(column)
            }
            $('#listaOffertePI').append(tableRow);
        });
    });
});
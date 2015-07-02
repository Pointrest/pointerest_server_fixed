var input;

$(function () {

    $.get('../api/offerta/' + getQueryVariable('PI'), function (data, textStatus, jqXHR) {

        $.each(data, function (index, element) {

            var row = '<tr data-offertaID="' + element.IDOfferta + '">'
                        + '<td>' + element.Nome + '</td>'
                        + '<td>' + element.Descrizione + '</td>'
                        + '<td>' + element.DataInizio + '</td>'
                        + '<td>' + element.DataFine + '</td>'
                        + '<td><img src="data:image/jpeg;base64,' + element.ImmagineOfferta + '" style="width:100px; height:auto;"/></td>'
                        + '<td><span title="Edit Data" class="clickable glyphicon glyphicon-edit editOffert clickable" aria-hidden="true"></span></td>'
                        + '<td><span class="glyphicon glyphicon-trash removeOffert clickable" aria-hidden="true"></span></td>'
                        + '</tr>'
            $('#listaOffertePI').append(row);
        });

        $('.editOffert').on('click', function () {

            $('#editOffertModal').modal();
            $.get("../api/offerta/" + $(this).parent().parent('tr').attr('data-offertaid')
                ,function (data, textStatus, jqXHR) {
                
                    populateModal(data[0]);
            });
        });

        $('.removeOffert').on('click', function () {

            var row = $(this).parent().parent('tr');

            $.ajax({
                url: '../api/offerta/' + row.attr('data-offertaid'),
                type: 'DELETE',
                headers: createHeaders(),
                success: function (result) {
                    row.remove();
                }
            });
        }); 
    });

    // Modal

    $("#dataInizioOfferta").datepicker({ format: 'yyyy-mm-dd' });
    $("#dataFineOfferta").datepicker({ format: 'yyyy-mm-dd' });
    input = $('#imageInput')[0];
    $('#updateOfferta').on('click', function () {

        readImage(input);
    });
});

function populateModal(element) {

    $('#nomeOfferta').val(element.Nome);
    $('#descrizioneOfferta').val(element.Descrizione);
    $('#dataInizioOfferta').val(element.DataInizio);
    $('#dataFineOfferta').val(element.DataFine);
    $('#offertaImg').attr('src', 'data:image/jpeg;base64,' + element.Immagine); 
}

function sendUpdatedData(image) {

    var updatedOffert = {
        'Nome': $('#nomeOfferta').val(),
        'Descrizione': $('#descrizioneOfferta').val(),
        'DataInizio': $('#dataInizioOfferta').val(),
        'DataFine': $('#dataFineOfferta').val(element.DataFine),
        'Immagine': image
    };

    sendUpdatedData(updatedOffert);

    $.ajax({
        url: "/api/pi/" + getQueryVariable('PI'),
        type: 'PUT',
        headers: createHeaders(),
        data: updateDataCommand,
        success: function (data) {
            $('#editOffertModa').modal('hide');
        }
    });
}

function readImage(input) {
    if (input.files && input.files[0]) {
        var FR = new FileReader();
        FR.onload = function (e) {
            sendUpdatedData(e.target.result);
        };
        FR.readAsDataURL(input.files[0]);
    }
}
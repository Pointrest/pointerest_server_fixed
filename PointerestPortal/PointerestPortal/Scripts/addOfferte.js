var input;

$(function () {

    $("#dataInizio").datepicker({ format: 'yyyy-mm-dd' });
    $("#dataFine").datepicker({ format: 'yyyy-mm-dd' });

    $('#inviaOfferta').on('click', function() {
        sendNewOfferta();
    });

    input = $('#imageInput')[0];
});

function sendNewOfferta() {

    readImage(input);
}

function readImage(input) {
    if (input.files && input.files[0]) {
        var FR = new FileReader();
        FR.onload = function (e) {
            sendData(e.target.result);
        };
        FR.readAsDataURL(input.files[0]);
    }
}

function sendData(image) {

    var offerta = {
        "IDPuntoInteresse": getQueryVariable('PI'),
        "Nome": $('#nomeOfferta').val(),
        "Descrizione": $('#descrizioneOfferta').val(),
        "DataInizio": $('#dataInizio').val(),
        "DataFine": $('#dataFine').val(),
        "Immagine": image.split(',')[1]
    };

    $.ajax({
        url: '../api/offerta',
        type: 'POST',
        headers: createHeaders,
        data: offerta,
        success: function (data, textStatus, jqXHR) {
            
            $('#feedback').text('Offerta inserita correttamente').fadeOut(5000);
        }
    });
}
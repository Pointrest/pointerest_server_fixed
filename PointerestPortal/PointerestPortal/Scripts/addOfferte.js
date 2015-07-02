var input;

$(function () {

    $("#dataInizio").datepicker();
    $("#dataFine").datepicker();

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
        "IDPuntoInterese": getQueryVariable('PI'),
        "Nome": $('#nomeOfferta').val(),
        "DescrizioneOfferta": $('#descrizioneOfferta').val(),
        "DataInizio": $('#dataInizio').val(),
        "DataFine": $('#dataFine').val(),
        "Immagine": image.split(',')[1]
    };

    $.ajax({
        url: 'api/offerte',
        type: 'POST',
        headers: createHeaders,
        data: offerta,
        success: function (data, textStatus, jqXHR) {
            
            $('#feedback').text('Offerta inserita correttamente');
        }
    });
    $('#feedback').text('Offerta inserita correttamente').fadeOut(5000);
}
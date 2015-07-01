var categorie;
var sottocategorie;
var tabella;
var gestoreUsername;

$(function () {

    var token = getToken();

    if (token != null) {

        $.get('/api/categorie', function (data, textStatus, jqXHR) {
            categorie = data;
        });

        $.get('/api/sottocategorie', function (data, textStatus, jqXHR) {
            sottocategorie = data;
        });

        gestoreUsername = sessionStorage.getItem("gestoreUsername")
        tabella = $('#puntiInteresseGestore');

        $('#addPIButton').on('click', function () {
            $('#dataModal').modal();
            addPI(gestoreUsername, $('.headField'));
            reInitializeMapContainer();
            
            
        });

        $('#dataModal').on('shown.bs.modal', function () {
            initializeMap(null);
        });

        getGestorePIAndAppendThem();
    }

    else {

        window.location = "../home"
    }
});

function reInitializeMapContainer() {
    var mapPieces = '<input id="pac-input" class="controls" type="text" placeholder="Search Box">'
                  + '<div id="map-canvas"></div>';
    $('#mapContainer').empty().append(mapPieces);
}

function addPI(gestoreUserName, headField) {

    createInputFields(headField);

    $('#updatePIData').off('click').on('click', function () {

        var addPIData = {
            'Nome': $('#nome').val(),
            'SottocategoriaID': $("#sottoCategorie option:selected").val(),
            'Descrizione': $('#descrizione').val(),
            'Indirizzo': $('#indirizzo').val(),
            'Latitudine': $('#latitudine').val(),
            'Longitudine': $('#longitudine').val()
        }

        var headers = createHeaders();

        $.ajax({
            url: 'api/pi/' + gestoreUsername + '/',
            type: 'POST',
            headers: headers,
            data: addPIData,
            success: function (data, textStatus, jqXHR) {
                $('#dataModal').modal('hide');
                getGestorePIAndAppendThem();
            }
        });
    });

    $('#cancelPIDataUpdate').off('click').click(function () { $('#dataModal').modal('hide'); })
    $('#updatePIData').text('Aggiungi Punto Interesse');
}

function createInputFields(headField) {

    $('#dataContainer').empty();

    $.each(headField, function (index, element) {
        if ($(element).text() == "Categoria") {
            div = '<div class="form-group">'
                    + '<label for="data">' + $(element).text() + '</label>'
                    + '<select id="categorie" class="form-control"></select>'
                    + '</div>';
        }
        else if ($(element).text() == "Sottocategoria") {
            div = '<div class="form-group">'
                    + '<label for="data">' + $(element).text() + '</label>'
                    + '<select id="sottoCategorie" class="form-control">'
                    + '</select>'
                    + '</div>';
        }
        else if ($(element).text() == "Latitudine" || $(element).text() == "Longitudine") {
            div = '<div class="form-group">'
                        + '<label for="data">' + $(element).text() + '</label>'
                        + '<input type="text" class="form-control" id="' + $(element).text().toLowerCase()
                        + '" placeholder="Select on the Map" '
                        + 'value="" readonly>'
                        + '</div>';
        }
        else {
            div = '<div class="form-group">'
                        + '<label for="data">' + $(element).text() + '</label>'
                        + '<input type="text" class="form-control" id="' + $(element).text().toLowerCase() + '" placeholder="Enter '
                        + $(element).text().toLowerCase() + '" '
                        + 'value="" >'
                        + '</div>';
        }
        $('#dataContainer').append(div);
    });

    $.each(categorie, function (index, element) {
        var option = '<option value="' + element.ID + '">' + element.CategoryName + '</option>';
        $('#categorie').append(option);
    });

    getSubCategorieOfCategoryAndAppendThem($($("#categorie").children()[0]).val(), null);

    $('#categorie').change(function () {

        //clean old subcategorie
        $('#sottoCategorie').empty();
        //get subcategories of category
        getSubCategorieOfCategoryAndAppendThem($("#categorie option:selected").val(), null);
    });

    $('#updatePIData').off('click').on('click', function () {

        var addPIData = {
            'Nome': $('#nome').val(),
            'SottocategoriaID': $("#sottoCategorie option:selected").val(),
            'Descrizione': $('#descrizione').val(),
            'Indirizzo': $('#indirizzo').val(),
            'Latitudine': $('#latitudine').val(),
            'Longitudine': $('#longitudine').val()
        }
    });
}

function getGestorePIAndAppendThem() {

    tabella.empty();

    $.get("/api/pi/username/" + gestoreUsername + "/", function (data, textStatus, jqXHR) {

        var images = [];
        $.each(data, function (index, element) {

            var row = '<tr data-id="' + element.ID + '">'
                       + '<td class="piField">' + element.Nome + "</td>"
                       + '<td class="piField piCategory" data-categoryID="' + element.CategoriaID + '">' + element.Categoria + "</td>"
                       + '<td class="piField piSubcategory" data-subcategoryID="' + element.SottocategoriaID + '">' + element.Sottocategoria + "</td>"
                       + '<td class="piField">' + element.Descrizione + "</td>"
                       + '<td class="piField">' + element.Indirizzo + "</td>"
                       + '<td class="piField">' + element.Latitudine + "</td>"
                       + '<td class="piField">' + element.Longitudine + "</td>"
                       + '<td><span title="Show Images"class="clickable glyphicon glyphicon-picture showImages" aria-hidden="true"></span></td>'
                       + '<td><span title="Edit Data" class="clickable glyphicon glyphicon-edit editPI" aria-hidden="true"></span></td>'
                        + '<td> <span title="Show Offerts" class="clickable glyphicon glyphicon-gift showOfferts" aria-hidden="true"></span></td>'
                       + '<td> <span title="Add Offert" class="clickable glyphicon glyphicon-plus addOffert" aria-hidden="true"></span></td>'
                       + '<td><span title="Delete" class="clickable glyphicon glyphicon-trash deletePI" aria-hidden="true"></span></td>'
                       + "</tr>";

            images[index] = element.Images;
            tabella.append(row)
        });

        // Show Images
        $('.showImages').on('click', function () {
            $('#imagesModal').modal();
            $('#imagesContainer').empty();
            var currentIndex = $('tr').index($(this).parent().parent('tr'));
            var piIndex = $(this).parent().parent('tr').attr('data-id');
            showImages(images, currentIndex - 1, piIndex);
        });

        //Edit PI Data
        $('.editPI').on('click', function () {
            var piID = $(this).parent().parent('tr').attr('data-id');
            var categoryID = $(this).parent().siblings('.piCategory').attr('data-categoryid');
            var subcategoryID = $(this).parent().siblings('.piSubcategory').attr('data-subcategoryid');
            var headCells = $('.headField');
            var row = $(this).parent().parent();
            var cells = row.children('.piField');
            $('#dataModal').modal();
            reInitializeMapContainer();
            editPI(piID, categoryID, subcategoryID, headCells, cells);
            $('#updatePIData').off('click').on('click', sendUpdatedData);
            $('#cancelPIDataUpdate').off('click').click(function () { $('#dataModal').modal('hide'); })
            $('#updatePIData').text('Aggiorna Punto Interesse');
        });

        // Add Offerts
        $('.addOffert').on('click', function () {
            window.location = "manage/addofferte?PI=" + $(this).parent().parent('tr').attr('data-id');
        });

        // Show Offerts
        $('.showOfferts').on('click', function () {

            window.location = "manage/listofferte?PI=" + $(this).parent().parent('tr').attr('data-id');
        });

        // Delete PI
        $('.deletePI').on('click', function () {
            var row = $(this).parent().parent('tr');
            var ID = row.attr('data-id');
            var headers = createHeaders();

            $.ajax({
                url: '/api/pi/' + ID,
                type: 'DELETE',
                headers: headers,
                success: function (result) {
                    console.log("Cancelled PI " + ID);
                    row.remove();
                }
            });
        });
    });
}

function showImages(imagesArray, currentIndex, piIndex) {

    $('#imagesContainer').attr('data-piid', piIndex)

    var tmpArray = [];
    for (var i = 0; i < imagesArray[currentIndex].length; ++i) {
        var tmpImage = {
            "ImageID": imagesArray[currentIndex][i].ImageID,
            "ImageData": ""
        };
        tmpArray[i] = tmpImage;
    }

    $.each(tmpArray, function (eIndex, element) {

        $('#imagesContainer').append(createImageRow(eIndex, imagesArray[currentIndex][eIndex].ImageData, "show"));
    });

    $('.deleteImg').on('click', function () {
        var index = $(this).parent().siblings('.imgContainer').children('img').attr('data-arrayindex');
        removeImageFromArray($(this), tmpArray, index);
    });

    var imgNew = '<form id="form1" runat="server">'
                + '<input type="file" id="inputFile" />'
                + '<img data-arrayID="" class="image_upload_preview" src="http://placehold.it/100x100" alt="your image" />'
                + '</form>';

    $("#inputFile").off('change')
                   .change(function () { readURL($(this)[0], tmpArray) })
                   .prop('files')[0] = null;

    $('#updatePIImages').off('click')
                        .on('click', function () {

                            var updateImageObj = {
                                "IDPuntoInteresse": $('#imagesContainer').attr('data-piid'),
                                "Images": tmpArray
                            };

                            var headers = createHeaders();

                            $.ajax({
                                url: "/api/pi/images/" + $('#imagesContainer').attr('data-piid'),
                                type: 'PUT',
                                headers: headers,
                                data: updateImageObj,
                                success: function (data) {
                                    $('#imagesModal').modal('hide');
                                    getGestorePIAndAppendThem();
                                }
                            });
                        });

    $('#cancelPIDataImages').off('click').on('click', function () { $('#imagesModal').modal('hide'); })

}

function editPI(piID, categoryID, subcategoryID, headCells, cells) {

    $('#dataContainer').empty();
    $('#dataContainer').attr('data-id', piID);
    var point = { "Lat": "", "Lon": "" };

    $.each(cells, function (index, element) {

        var div;

        if (headCells[index].textContent == "Categoria") {
            div = '<div class="form-group">'
                    + '<label for="data">' + $(headCells[index]).text() + '</label>'
                    + '<select id="categorie" class="form-control"></select>'
                    + '</div>';
        }
        else if (headCells[index].textContent == "Sottocategoria") {
            div = '<div class="form-group">'
                    + '<label for="data">' + $(headCells[index]).text() + '</label>'
                    + '<select id="sottoCategorie" class="form-control">'
                    + '</select>'
                    + '</div>';
        }
        else {

            if (headCells[index].textContent == "Latitudine") {
                point.Lat = $(cells[index]).text();
            }

            if (headCells[index].textContent == "Longitudine") {
                point.Lon = $(cells[index]).text();
            }

            div = '<div class="form-group">'
                        + '<label for="data">' + $(headCells[index]).text() + '</label>'
                        + '<input type="text" class="form-control" id="' + $(headCells[index]).text().toLowerCase() + '" placeholder="Enter email" '
                        + 'value="' + $(cells[index]).text() + '" >'
                        + '</div>';
        }
        $('#dataContainer').append(div);
    });

    initializeMap(point);
    $('#pac-input').css("visibility", "visible");

    $.each(categorie, function (index, element) {
        var option = '<option value="' + element.ID + '">' + element.CategoryName + '</option>';
        $('#categorie').append(option);
    });

    $('#categorie').val(categoryID);
    getSubCategorieOfCategoryAndAppendThem(categoryID, subcategoryID);

    $('#categorie').change(function () {

        //clean old subcategorie
        $('#sottoCategorie').empty();
        //get subcategories of category
        getSubCategorieOfCategoryAndAppendThem($("#categorie option:selected").val(), null);
    });
}

function getSubCategorieOfCategoryAndAppendThem(categoryID, subcategoryID) {
    $.get('/api/sottocategorie/' + categoryID, function (data, textStatus, jqXHR) {
        $.each(data, function (index, element) {
            var option = '<option value="' + element.ID + '">' + element.SubCategoryName + '</option>';
            $('#sottoCategorie').append(option);
        });

        if (subcategoryID != null)
            $('#sottoCategorie').val(subcategoryID);
        else
            $('#sottoCategorie').find('option:eq(0)').prop('selected', true);
    });
}

function sendUpdatedData() {

    var updateDataCommand = {
        'ID': $('#dataContainer').attr('data-id'),
        'Nome': $('#nome').val(),
        'SottocategoriaID': $("#sottoCategorie option:selected").val(),
        'Descrizione': $('#descrizione').val(),
        'Indirizzo': $('#indirizzo').val(),
        'Latitudine': $('#latitudine').val(),
        'Longitudine': $('#longitudine').val()
    };

    var headers = createHeaders();

    $.ajax({
        url: "/api/pi/" + $('#dataContainer').attr('data-id'),
        type: 'PUT',
        headers: headers,
        data: updateDataCommand,
        success: function (data) {
            $('#dataModal').modal('hide');
            getGestorePIAndAppendThem();
        }
    });
}

function readURL(input, imageArray) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {

            var undefinedID = -1;
            var tmiImageObj = {
                'ImageID': undefinedID,
                'ImageData': e.target.result
            };
            imageArray[imageArray.length] = tmiImageObj;

            $('#imagesContainer').append(createImageRow(imageArray.length, e.target.result, "new"));

            $('.deleteImg').off('click').on('click', function () {
                var obj = $(this);
                removeImageFromArray(obj, imageArray, obj.parent().siblings('.imgContainer').children('img').attr('data-arrayindex'));
            });
        }
        reader.readAsDataURL(input.files[0]);
    }
}

function removeImageFromArray(button, array, index) {

    array[index] = null;
    $(button).parent().parent('.row').remove();
}

function createImageRow(index, imageData, mode) {

    var src;

    if (mode == 'new') {

        src = '" src="'
    } else if (mode == 'show') {
        src = '" src="data:image/jpeg;base64,';
    }

    return '<div class="row imageRow">'
                   + '<div class="col-md-7 imgContainer">'
                   + '<img data-arrayindex="'
                   + index
                   + src
                   + imageData
                   + '" /></div>'
                   + '<div class="col-md-5">'
                   + '<button type="submit" class="btn btn-default deleteImg" style="width:100%;">Delete Image</button>'
                   + '</div>'
                   + '</div>';
}

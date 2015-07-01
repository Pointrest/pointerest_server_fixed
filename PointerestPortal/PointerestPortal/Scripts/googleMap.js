function initializeMap(point) {

    // Initialize Map
    var mapOptions;
    var myLatlng;
    if (point != null) {
        myLatlng = new google.maps.LatLng(point.Lat, point.Lon);
        mapOptions = {
            zoom: 4,
            center: myLatlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        }
    } else {
        myLatlng = new google.maps.LatLng(41.896017, 12.493426);
        mapOptions = {
            zoom: 4,
            center: myLatlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        }
    }

    var markers = [];
    var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    /*var defaultBounds = new google.maps.LatLngBounds(
        new google.maps.LatLng(-33.8902, 151.1759),
        new google.maps.LatLng(-33.8474, 151.2631));
    map.fitBounds(defaultBounds);*/

    if (point != null) {
        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map,
            title: 'Current Point',
            draggable : true
        });
        marker.setMap(map);

        /*google.maps.event.addListener(marker, 'dragend', function (evt) {
            stopDragMarker(evt);
        });*/
    }

    var input = (document.getElementById('pac-input'));
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    var searchBox = new google.maps.places.SearchBox((input));

    // Stop Drag Callback

    function stopDragMarker(evt) {
        setLatLon(evt.latLng.A, evt.latLng.F);
    }

    // Input Place Changes Callback

    google.maps.event.addListener(searchBox, 'places_changed', function () {
        var places = searchBox.getPlaces();

        if (places.length == 0) {
            return;
        }

        for (var i = 0, marker; marker = markers[i]; i++) {
            marker.setMap(null);
        }

        markers = [];
        var bounds = new google.maps.LatLngBounds();
        for (var i = 0, place; place = places[i]; i++) {
            var image = {
                url: place.icon,
                size: new google.maps.Size(71, 71),
                origin: new google.maps.Point(0, 0),
                anchor: new google.maps.Point(17, 34),
                scaledSize: new google.maps.Size(25, 25)
            };

            var marker = new google.maps.Marker({
                map: map,
                icon: image,
                title: place.name,
                position: place.geometry.location,
                draggable: true
            });

            setLatLon(marker.position.A, marker.position.F);

            google.maps.event.addListener(marker, 'dragend', function (evt) {
                stopDragMarker(evt);
            });

            markers.push(marker);

            bounds.extend(place.geometry.location);
        }

        map.fitBounds(bounds);
    });

    google.maps.event.addListener(map, 'bounds_changed', function () {
        var bounds = map.getBounds();
        searchBox.setBounds(bounds);
    });
}

function setLatLon(lat, lon) {
    $('#latitudine').val(lat);
    $('#longitudine').val(lon);
}

function resizeMap() {
    google.maps.event.trigger(this.map, 'resize');
}
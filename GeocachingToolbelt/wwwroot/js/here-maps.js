var platform = new H.service.Platform({
    apikey: '085vI9e-1LGilWPe4uURCrUuWpeLEW6V9GEO5whB95Q',
    app_id: 'QsrKH3biseeWWjfyTJgL',
    app_code: 'hyceuC1PnpUZBZLHaHzLfA'
});

var defaultLayers = platform.createDefaultLayers();

var map = new H.Map(
    document.getElementById('mapContainer'),
    defaultLayers.vector.normal.map,
);

function addCoordsToMap() {
    var group = new H.map.Group();
    var coords = document.querySelectorAll("[data-container='coordinates'] coord");
    var setZoomlevel = coords.length == 1;
    map.removeObjects(map.getObjects())

    coords.forEach(coord => {
        var latitude = parseFloat(coord.dataset.lat);
        var longitude = parseFloat(coord.dataset.long);
        var radius = parseFloat(coord.dataset.radius);
        var pngIcon = new H.map.Icon("/images/marker-" + (coord.dataset.color != undefined ? coord.dataset.color : "blue") + ".png");

        var marker = new H.map.Marker({
            lat: latitude,
            lng: longitude
        }, { icon: pngIcon });

        if (radius) {
            setZoomlevel = false;
            var radiusMarker = new H.map.Circle(
                { lat: latitude, lng: longitude },
                radius,
                {
                    style: {
                        strokeColor: 'rgba(55, 85, 170, 0.6)', // Color of the perimeter
                        lineWidth: 2,
                        fillColor: 'rgba(0, 128, 0, 0.3)'  // Color of the circle
                    }
                }
            );
            group.addObject(radiusMarker);
        }

        if (!radius || coord.dataset.forcemarker == "true") {
            group.addObject(marker);
        }

    });

    if (coords.length) {
        map.addObject(group);
        var lookdata = {
            bounds: group.getBoundingBox()
        }

        if (setZoomlevel) {
            lookdata["zoom"] = 17;
        }

        map.getViewModel().setLookAtData(lookdata);
    }
}

addCoordsToMap();

var ui = H.ui.UI.createDefault(map, defaultLayers);
var behavior = new H.mapevents.Behavior(new H.mapevents.MapEvents(map));

var streets = L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
	maxZoom: 18,
	id: 'mapbox/streets-v11',
	tileSize: 512,
	zoomOffset: -1,
	accessToken: 'pk.eyJ1Ijoibmlla3AiLCJhIjoiY2s4bXB4ZnZzMGhxaDNvbzJnaW1wNmFjYSJ9.avy05lhHCVWjRlKXbcwLyA'
});

var outdoor = L.tileLayer('https://tile.thunderforest.com/landscape/{z}/{x}/{y}.png?apikey={apikey}', {
    attribution: '&copy; <a href="http://www.thunderforest.com/">Thunderforest</a>, &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
	maxZoom: 22,
    tileSize: 512,
    zoomOffset: -1,
    apikey: 'aded984765ad45d692db2dc42dc4188e'
});

var satelite = L.tileLayer('https://geodata.nationaalgeoregister.nl/luchtfoto/rgb/wmts/2018_ortho25/EPSG:3857/{z}/{x}/{y}.png', {
    minZoom: 6,
    maxZoom: 19,
    bounds: [[50.5, 3.25], [54, 7.6]],
    attribution: 'Kaartgegevens &copy; <a href="kadaster.nl">Kadaster</a>',
    tileSize: 512,
    zoomOffset: -1,
});

var baseMaps = {
    "Sateliet": satelite,
    "Wandelen": outdoor,
	"Straten": streets,
};

var map = L.map('mapContainer', {
	layers: [streets, outdoor, satelite]
});

L.control.layers(baseMaps).addTo(map);

var layerGroup = L.layerGroup().addTo(map);

function addCoordsToMap() {
    var coords = document.querySelectorAll("[data-container='coordinates'] coord");
    layerGroup.clearLayers();

    var group = [];
    coords.forEach(coord => {
        var latitude = parseFloat(coord.dataset.lat);
        var longitude = parseFloat(coord.dataset.long);
        var circle_radius = parseFloat(coord.dataset.radius);
        
        if (circle_radius) {
            setZoomlevel = false;

            var circle = L.circle([latitude, longitude], {
                color: 'rgba(55, 85, 170)',
                fillColor: 'rgba(0, 128, 0, 0.3)',
                fillOpacity: 0.3,
                radius: circle_radius
            });

            group.push(circle);
        }

        if (!circle_radius || coord.dataset.forcemarker == "true") {
            myIcon = L.icon({
                iconUrl: "/images/marker-" + (coord.dataset.color != undefined ? coord.dataset.color : "blue") + ".png",
                iconSize: [40, 40], 
            });

            var marker = L.marker([latitude, longitude], { icon: myIcon, draggable: coords.length == 1, autoPan: true });

            if (coord.dataset.title) {
                if (coord.dataset.gccode) {
                    html = "<strong><a href='https://coord.info/" + coord.dataset.gccode + "' target='_blank'>" + coord.dataset.gccode + "</a></strong><br>" + coord.dataset.title;
                } else {
                    var html = "<strong>" + coord.dataset.title + "</strong>";
                }
                marker.bindPopup(html);
            }

            marker.on("dragend", function (e) {
                var marker = e.target;
                var position = marker.getLatLng();
                if ($("div[data-id='_CoordinatePartial']").length) {
                    $.get("/LeafletHelper/CoordinatePartial", { lat: position.lat, lng: position.lng }, function (data) {
                        $("div[data-id='_CoordinatePartial']").html(data);
                    });
                }
            });

            group.push(marker);
        }

        map.setView([latitude, longitude], 0);
    });

    var markerGroup = new L.featureGroup(group).addTo(layerGroup);
    map.fitBounds(markerGroup.getBounds());

    if (coords.length == 1 && !parseFloat(coords[0].dataset.radius)) {
        map.setZoom(17);
    }
}

addCoordsToMap();

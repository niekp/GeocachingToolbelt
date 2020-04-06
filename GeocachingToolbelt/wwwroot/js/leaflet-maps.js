var map = L.map('mapContainer');

L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
    attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
    maxZoom: 18,
    id: 'mapbox/streets-v11',
    tileSize: 512,
    zoomOffset: -1,
    accessToken: 'pk.eyJ1Ijoibmlla3AiLCJhIjoiY2s4bXB4ZnZzMGhxaDNvbzJnaW1wNmFjYSJ9.avy05lhHCVWjRlKXbcwLyA'
}).addTo(map);


var layerGroup = L.layerGroup().addTo(map);

function addCoordsToMap() {
    var coords = document.querySelectorAll("[data-container='coordinates'] coord");
    var setZoomlevel = coords.length == 1;
    layerGroup.clearLayers();

    var group = [];
    coords.forEach(coord => {
        var latitude = parseFloat(coord.dataset.lat);
        var longitude = parseFloat(coord.dataset.long);
        var radius = parseFloat(coord.dataset.radius);

        if (radius) {
            setZoomlevel = false;

            var circle = L.circle([latitude, longitude], {
                color: 'rgba(55, 85, 170)',
                fillColor: 'rgba(0, 128, 0, 0.3)',
                fillOpacity: 0.3,
                radius: radius
            });

            group.push(circle);
        }

        if (!radius || coord.dataset.forcemarker == "true") {
            myIcon = L.icon({
                iconUrl: "/images/marker-" + (coord.dataset.color != undefined ? coord.dataset.color : "blue") + ".png",
                iconSize: [40, 40], 
            });

            var marker = L.marker([latitude, longitude], { icon: myIcon });

            if (coord.dataset.title) {
                marker.bindPopup("<strong><a href='coord.info/" + coord.dataset.gccode + "' target='_blank'>" + coord.dataset.gccode + "</a></strong><br>" + coord.dataset.title);
            }

            group.push(marker);
        }

    });

    var markerGroup = new L.featureGroup(group).addTo(layerGroup);
    map.fitBounds(markerGroup.getBounds());
}

addCoordsToMap();

// This example requires the Places library. Include the libraries=places
// parameter when you first load the API. For example:
// 

//debugger;
$(document).ready(function () {
    alert('initMap');
    debugger;
    initAmenties(43.683334, -79.766670, 'school')
});

var latitude;
    var longitude;

    var Searchmap;
    var Searchinfowindow;
    var distance;
    var duration;
    var pyrmont;
    function initAmenties(lat, long, Type) {

        debugger;
        alert(Type);
        latitude = parseFloat(lat);
        longitude = parseFloat(long);
        pyrmont = { lat: latitude, lng: longitude };

        Searchmap = new google.maps.Map(document.getElementById('map'), {
            center: pyrmont,
            zoom: 13
        });

        Searchinfowindow = new google.maps.InfoWindow();
        var service = new google.maps.places.PlacesService(Searchmap);
        service.nearbySearch({
            location: pyrmont,
            radius: 3000,
            type: [Type],
            rankby: google.maps.places.RankBy.PROMINENCE
        }, callbackAmenities);
    }

    function callbackAmenities(results, status) {

        alert('callback');
        debugger;
        $("#tbodyid").empty();

        if (status === google.maps.places.PlacesServiceStatus.OK) {
            alert('ok');
            debugger;
            for (var i = 0; i < results.length; i++) {
                createMarker(results[i]);
            }
        }
    }

    function createMarker(place) {
        alert('create places');
        debugger;
        var placeLoc = place.geometry.location;
        var lati = place.geometry.location.lat();
        var longi = place.geometry.location.lng();

        var marker = new google.maps.Marker({
            map: Searchmap,
            position: place.geometry.location,
            id: lati + '_' + longi + '_' + place.name + '_' + place.icon + '_' + latitude + '_' + longitude
        });

        //add to table list of facilities.
        var pos1 = new google.maps.LatLng(lati, longi);
        var pos2 = new google.maps.LatLng(latitude, longitude);
        //var dstance = calcDistance(pos1, pos2);

        alert(place.icon);
        alert(place.name);
        alert(place.vicinity);
        alert(place.dstance);
        //$("<tr><td class='list_tb_details_img'><img src='" + place.icon + "' alt=''></td><td class='list_tb_details_name'>" + place.name + "</td><td class='list_tb_details_address'>" + place.vicinity + "</td><td class='list_tb_details_distance'>" + dstance + " km</td></tr>").appendTo("#myTable tbody");

        //end


        //google.maps.event.addListener(marker, 'mouseover', function () {


        //    var markid = this.id;
        //    var IconType = markid.split('_');
        //    var p1;
        //    if (IconType.length === 8) {
        //        p1 = new google.maps.LatLng(IconType[6], IconType[7]);
        //    }
        //    else {
        //        p1 = new google.maps.LatLng(IconType[5], IconType[6]);
        //    }
        //    var p2 = new google.maps.LatLng(IconType[0], IconType[1]);
        //    var distance = calcDistance(p1, p2);

        //    var markerHtml = "<div id='infoWindow' class='chats'>";
        //    markerHtml += "<div class='prprty_map_box2'>";
        //    markerHtml += "    <a class='map_prprty_img2' href='javascript:;'><img src='" + place.icon + "' alt=''></a>";

        //    markerHtml += "    <div id='BoxType' class='map_prprty_hdng2'>" + place.name + "";
        //    markerHtml += "    </div>";
        //    markerHtml += "    <span class='duration_sect'><b> Distance :</b> " + distance + "km</span>";
        //    //markerHtml += "    <span class='duration_sect'><b> Duration :</b> " + duration + "</span>";

        //    Searchinfowindow.setContent(markerHtml);
        //    Searchinfowindow.open(Searchmap, this);
        //});

        //calculates distance between two points in km's
        function calcDistance(p1, p2) {
            debugger;
            return (google.maps.geometry.spherical.computeDistanceBetween(p1, p2) / 1000).toFixed(2);
        }
    }
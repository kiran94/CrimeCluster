
/* Google Maps Variable*/
var map; 

/* Initialises the map instance */
function initMap()
{
	map = new google.maps.Map(document.getElementById('map'), 
	{
		center: {lat: 51.5331414, lng: -0.4773218},
	    scrollwheel: false,
	    zoom: 12
	});
}
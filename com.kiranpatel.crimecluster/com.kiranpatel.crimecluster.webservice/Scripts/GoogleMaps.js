
/* Google Maps Variable*/
var map; 

/* Markers to plot */
var markers = []; 

var locationsFound = $('#locationsfound'); 

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

/* Makes an AJAX call to retrieve all locations associated with the pased crime type  */
function filterByCrimeType(crimeType)
{
	var bound = new google.maps.LatLngBounds(); 

	$.post(
	{
		url : "/Cluster/Filter",
		data: { "crimeType" : crimeType },
		success: function(data, status, xhr)
		{
			var result = $.parseJSON(data); 
			var locations = 0; 

			for (var i = 0; i < result.length; i++)
			{
				var currentLoc = new google.maps.LatLng(result[i].Latitude, result[i].Longitude); 
				markers.push(new google.maps.Marker(
				{
					position : currentLoc,
					map : map
				})); 

				bound.extend(currentLoc); 
				locations++; 
			}

			map.setCenter(bound.getCenter()); 
			map.fitBounds(bound); 

			locationsFound.html(locations); 
		}
	});
}

/* Clears the markers currently on the map */
function clearMarkers()
{
	if (markers)
	{
		for (var i = 0; i < markers.length; i++)
		{
			markers[i].setMap(null); 
		}
	}

	markers = [];
}

/* On change of the drop down, get the selected item and plot the crimes.  */
$('#type').change(function()
{
 	$(this).prop("disabled", true);
	var selected = $(this).val(); 

	clearMarkers(); 
	filterByCrimeType(selected);  
	$(this).prop("disabled", false);
});
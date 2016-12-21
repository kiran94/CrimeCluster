
/* Google Maps Variable*/
var map; 

/* Markers to plot */
var markers = []; 

/* DOM element reference to the locations found label */
var locationsFound = $('#locationsfound'); 

/* DOM element reference to the loader */
var loader = $('#loader'); 

/* DOM element reference to crime label drop down */
var dropdown = $('#crimelabeldropdown'); 

/* DOM element reference to the cluster button */
var clusterButton = $("#clusterbutton");

/* Marker Colors */
var colours = ["0", "ff0000"];

/* Legend of the Clusters mapping cluster number to colour */
var clusterLegend = {}; 

/* DOM element reference to the legend */
var legend = $("#legend-content"); 

/* Initialises the map instance */
function initMap()
{
	map = new google.maps.Map(document.getElementById('map'), 
	{
		center: {lat: 51.513413, lng: -0.088961},
	    scrollwheel: false,
	    zoom: 12
	});
}

/* Generates the marker colours array */
function generateMarkerColours()
{
	for(var i=0; i < 100; i++)
	{
		colours.push(Math.floor(Math.random()*16777215).toString(16)); 
	}
}

/* Gets the cluster colour for the current cluster  */
function getClusterColour(clusterNo)
{
	return colours[clusterNo % colours.length]; 
}

/* Gets the Marker Icon for a given cluster */
function getMarkerIcon(pinColor)
{
    var pinImage = new google.maps.MarkerImage("http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|" + pinColor,
        new google.maps.Size(21, 34),
        new google.maps.Point(0,0),
        new google.maps.Point(10, 34));

    return pinImage; 
}

/* Adds a marker with the passed location to the map */
function addMarker(location, title, clusterNo)
{
	var pinColor = getClusterColour(clusterNo);
	var pinImage = getMarkerIcon(pinColor); 
	var infowindow = new google.maps.InfoWindow(title);
	var newMarker = new google.maps.Marker(
	{
		position : location,
		map : map,
		info : title,
		icon: pinImage
	});

	google.maps.event.addListener(newMarker, 'click', function() 
	{
		  infowindow.setContent(title);
		  infowindow.open(map, this);
		  infowindow.text(newMarker.info);
    });

    markers.push(newMarker); 
    addLegendItem(clusterNo, pinColor);
}

/* Makes an AJAX call to retrieve all locations associated with the pased crime type  */
function filterByCrimeType(crimeType, callback)
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
				addMarker(currentLoc, result[i].Latitude + " " + result[i].Longitude, 1); 

				bound.extend(currentLoc); 
				locations++; 
			}

			map.setCenter(bound.getCenter()); 
			map.fitBounds(bound); 

			locationsFound.html(locations); 
			plotLegend(); 
			callback(); 
		}
	});
}

/* Makes an AJAX call to retrieve the clusters in the current selected crime type */
function cluster(crimeType, callback)
{
	var bound = new google.maps.LatLngBounds(); 

	$.post(
	{
		url : "/Cluster/Cluster",
		data: { "crimeType" : crimeType },
		success: function(data, status, xhr)
		{
			var result = $.parseJSON(data); 
			var locations = 0; 

			for (var i = 0; i < result.length; i++)
			{
				var currentLoc = new google.maps.LatLng(result[i].Latitude, result[i].Longitude); 
				addMarker(currentLoc, result[i].ClusterNo, result[i].ClusterNo); 

				bound.extend(currentLoc); 
				locations++; 
			}

			map.setCenter(bound.getCenter()); 
			map.fitBounds(bound); 

			locationsFound.html(locations); 
			plotLegend(); 
			callback(); 
		}
	}); 
}

/* Clears the legend  */
function clearLegend()
{
	clusterLegend = {}; 
	legend.empty(); 
}

/* Plots the legend onto the page */
function plotLegend()
{
	for(var currentItem in clusterLegend)
	{
		var colorBlock = "<span style='background-color:#" + clusterLegend[currentItem] + ";width:20px;height:20px;color:white;'>"; 
		colorBlock += "Cluster " + currentItem; 
		colorBlock += "</span>"

		legend.append(colorBlock); 
		legend.append("<br/>"); 
	}
}

/* Adds the passed key entry with the value if the key does not already exist in the legend */
function addLegendItem(key, value)
{
	if (!clusterLegend[key])
	{
		clusterLegend[key] = value; 
	}
}

/* Disables all controls on the page */
function disableControls()
{
	loader.show(); 
	clusterButton.prop("disabled", true);
	dropdown.prop("disabled", true);
}

/* Enables all controls on the page */
function enableControls()
{
	dropdown.prop("disabled", false);		
	clusterButton.prop("disabled", false);
	loader.hide(); 
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
	clearLegend(); 
}

/* Checks whether the default has been submitted or not */
function checkDefault()
{
	var selected = dropdown.val(); 

	return (selected == 0)
}

/* Generates the marker colours for the clusters */
generateMarkerColours();

/* On change of the drop down, get the selected item and plot the crimes.  */
dropdown.change(function()
{
	disableControls(); 
	var selected = $(this).val(); 

	if (!checkDefault())
	{
		clearMarkers(); 
		filterByCrimeType(selected, enableControls);  
	}
	else
	{
		alert("Cannot choose default");
		enableControls(); 
	}
});

/* On clicking the cluster button, get the selected item, call cluster and plot the crimes. */
clusterButton.click(function()
{
	disableControls(); 
	var selected = dropdown.val(); 

	if (!checkDefault())
	{
		clearMarkers(); 
		cluster(selected, enableControls);  
	}	
	else
	{
		alert("Cannot choose default");
		enableControls(); 
	}
}); 
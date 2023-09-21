/*---- morrisBar1----*/	
$(function(e){
  'use strict';
	new Morris.Area({
	  element: 'morrisBar-chart',
	  behaveLikeLine: true,
	  data: [
		{x: '2018 Q1', y: 3, z: 0},
		{x: '2018 Q2', y: 2, z: 3},
		{x: '2018 Q3', y: 4, z: 2},
		{x: '2018 Q4', y: 2, z: 4},
		{x: '2018 Q5', y: 2, z: 1},
		{x: '2018 Q6', y: 2, z: 4},
		{x: '2018 Q7', y: 2, z: 0},
		{x: '2018 Q8', y: 3, z: 3}
	  ],
	  xkey: 'x',
	  ykeys: ['y', 'z'],
	  lineColors: ['#736cc7','#ff5c75'],
	  labels: ['Y', 'Z']
	});
});

$(function(e){
  'use strict';
	new Morris.Donut({
	  element: 'morrisBar-pie',
	  data: [
		{value: 35, label: 'data1'},
		{value: 25, label: 'data2'},
		{value: 15, label: 'data3'}
	  ],
	  backgroundColor: '#fff',
	  labelColor: '#5e7cac',
	  colors: [
		'#6574cd',
		'#2bcbba',
		'#f66d9b'
		
	  ],
	  formatter: function (x) { return x + "%"}
	});
});

$(function(e){
  'use strict';
	new Morris.Bar({
	  element: 'morrisBar-graph',
	  data: [
		{x: '2011 Q1', y: 0},
		{x: '2011 Q2', y: 1},
		{x: '2011 Q3', y: 2},
		{x: '2011 Q4', y: 3},
		{x: '2012 Q1', y: 4},
		{x: '2012 Q2', y: 5},
		{x: '2012 Q3', y: 6},
		{x: '2012 Q4', y: 7},
		{x: '2013 Q1', y: 8}
	  ],
	  xkey: 'x',
	  ykeys: ['y'],
	  labels: ['Y'],
	  barColors: function (row, series, type) {
		if (type === 'bar') {
		  var red = Math.ceil(0 * row.y / this.ymax);
		  return 'rgb( 115, 108, 199 )';
		}
		else {
		  return '#000';
		}
	  }
	});
});
	


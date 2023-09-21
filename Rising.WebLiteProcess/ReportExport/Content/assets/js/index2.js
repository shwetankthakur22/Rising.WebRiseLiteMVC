$(function(e){
  'use strict'	
  
	/*-----echartArea2-----*/
  var areaData2 = [
    {
      name: 'Sales',
      type: 'line',
      smooth: true,
      data: [20, 20, 36, 12, 15, 25],
      lineStyle: {
        normal: { width: 1 }
      },
      itemStyle: {
        normal: {
          areaStyle: { type: 'default' }
        }
      }
    },
    {
      name: 'Profit',
      type: 'line',
      smooth: true,
      data: [8, 5, 25, 10, 10, 20],
      lineStyle: {
        normal: { width: 1 }
      },
      itemStyle: {
        normal: {
          areaStyle: { type: 'default' }
        }
      }
    }
  ];

  var optionArea2 = {
    grid: {
      top: '6',
      right: '12',
      bottom: '17',
      left: '25',
    },
    xAxis: {
      data: [ '2013', '2014', '2015', '2016', '2017', '2018'],
      boundaryGap: false,
      axisLine: {
        lineStyle: { color: '#d7e7ff' }
      },
      axisLabel: {
        fontSize: 10,
        color: '#5e7cac',
		display:'false'
      }
    },
	tooltip: {
		show: true,
		showContent: true,
		alwaysShowContent: true,
		triggerOn: 'mousemove',
		trigger: 'axis',
		axisPointer:
			{
				label: {
					show: false,
				}
			}

	},
    yAxis: {
      splitLine: {
        lineStyle: { color: '#d7e7ff' },
		display:false
      },
      axisLine: {
        lineStyle: { color: '#d7e7ff' },
		display:false
      },
      axisLabel: {
        fontSize: 10,
        color: '#5e7cac'
      }
    },
    series: areaData2,
    color:[ '#736cc7','#ff5c75']
  };
	
  var chartArea2 = document.getElementById('echartArea2');
  var areaChart2 = echarts.init(chartArea2);
  areaChart2.setOption(optionArea2);
  
  
   /*-----echartpie-----*/
   
  var pieData = [{
    name: 'Designation',
    type: 'pie',
    radius: '60%',
    center: ['50%', '50%'],
    data: [
      {value: 525, name: 'sales', color:'#000' },
      {value: 310, name: 'profit', color:'#d7e7ff'},
      {value: 134, name: 'growth', color:'#d7e7ff'}
    ],
	color:[ '#817ad4','#fc7c8f','#48cadf'],
	responsive: true,
    label: {
      normal: {
        fontFamily: 'Roboto, sans-serif',
        fontSize: 11,
		responsive: true
      }
    },
	 options: {
          maintainAspectRatio: false,
          responsive: true,
		  
	 },
    labelLine: {
      normal: {
        show: false,
		responsive: true
      }
    },
    markLine: {
      lineStyle: {
        normal: {
          width: 'auto',
		  responsive: true
        }
      }
    }
  }];

  var pieOption = {
    tooltip: {
      trigger: 'item',
      formatter: '{a} <br/>{b}: {c} ({d}%)',
      textStyle: {
        fontSize: 11,
        fontFamily: 'Roboto, sans-serif'
      }
    },
    series: pieData
  };

  var pie = document.getElementById('echartPie');
  var pieChart = echarts.init(pie);
  pieChart.setOption(pieOption);


  
  /*-----echartdonut-----*/
  
  var donutData = [{
    name: 'Designation',
    type: 'pie',
    radius: ['30%','55%'],
    center: ['50%', '50%'],
    data: [
      {value: 635, name: 'WebDesigners'},
      {value: 450, name: 'Developers'},
      {value: 234, name: 'Accountants'},
      {value: 324, name: 'System Engineers'}
    ],
	color:[ '#817ad4','#fc7c8f','#48cadf','#ecb403'],
    label: {
      normal: {
        fontFamily: 'Roboto, sans-serif',
        fontSize: 11
      }
    },
    labelLine: {
      normal: {
        show: false
      }
    },
    markLine: {
      lineStyle: {
        normal: {
          width: 1
        }
      }
    }
  }];

  var donutOption = {
    tooltip: {
      trigger: 'item',
      formatter: '{a} <br/>{b}: {c} ({d}%)',
      textStyle: {
        fontSize: 11,
        fontFamily: 'Roboto, sans-serif'
      }
    },
    series: donutData
  };

  var donut = document.getElementById('echartDonut');
  var donutChart = echarts.init(donut);
  donutChart.setOption(donutOption);

  
});

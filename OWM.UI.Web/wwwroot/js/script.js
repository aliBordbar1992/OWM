
function RedAlert(ele, txt) {
    var element;
    if (typeof ele == "string") {element = $("#" + ele);} else {element = ele;}
    $(element).addClass('form-controlError');
    setTimeout(function () { $(element).removeClass("form-controlError"); }, 4000);
    $.notify(txt, { globalPosition: 'top left' });
}

function GreenAlert(ele, txt) {
    var element;
    if (typeof ele == "string") { element = $("#" + ele); } else { element = ele; }
    $(element).css('background-color', 'lightgreen');
    setTimeout(function () { $(element).removeAttr('style'); }, 4000);
    $.notify(txt, { className: 'success', clickToHide: false, autoHide: true, position: 'top left' });
}

function ExportToExcel(e) {
    $("#" + e).table2excel({
        filename: e + ".xls"
    });
}

function ClearFields(div) {
    $('#' + div).find('input:text').val('');
    $('#' + div).find('textarea').val('');
    //$('#' + div).find('input').val('');
    $('#' + div).find("select").prop('selectedIndex', 0);
}
function checkPastDate(ele) {
    var date = $('#' + ele).val();
    if (date === '') { return false; }
    var todayGregorain = new Date();
    var todayJalali = GtoJ(todayGregorain.getFullYear(), todayGregorain.getMonth() + 1, todayGregorain.getDate());
    var today = todayJalali[0] + '/' + todayJalali[1] + '/' + todayJalali[2];
    var todayArray = today.split('/');
    if (todayArray[1].length === 1) { todayArray[1] = '0' + todayArray[1]; }
    if (todayArray[2].length === 1) { todayArray[2] = '0' + todayArray[2]; }
    var selectedDayArr = date.split('/');
    if (selectedDayArr[1].length === 1) { selectedDayArr[1] = '0' + selectedDayArr[1]; }
    if (selectedDayArr[2].length === 1) { selectedDayArr[2] = '0' + selectedDayArr[2]; }
    today = todayArray[0] + '/' + todayArray[1] + '/' + todayArray[2];
    var selectedDate = selectedDayArr[0] + '/' + selectedDayArr[1] + '/' + selectedDayArr[2];
    return selectedDate > today;
}

function JalaliDateTime() {
    var date = new Date();
    var todayJalali = GtoJ(date.getFullYear(), date.getMonth() + 1, date.getDate());
    todayJalali[1] = todayJalali[1] < 10 ? '0' + todayJalali[1] : todayJalali[1]; 
    todayJalali[2] = todayJalali[2] < 10 ? '0' + todayJalali[2] : todayJalali[2]; 
    var tarikh = todayJalali[0] + '/' + todayJalali[1] + '/' + todayJalali[2];
    return tarikh;
}

function CheckPastTime(sDate, sTime, eDate, eTime) {
    var sDateArr = sDate.split('/');
    var eDateArr = eDate.split('/');
    var validStartTimeDate = new Date(JtoG(sDateArr[0], sDateArr[1], sDateArr[2], true) + ' ' + sTime);
    var validSEndTimeDate = new Date(JtoG(eDateArr[0], eDateArr[1], eDateArr[2], true) + ' ' + eTime);
    return validStartTimeDate < validSEndTimeDate;
}

function DaysBetween2Date(s ,e) {
    var sDateArr = s.split('/');
    var eDateArr = e.split('/');
    var validStartTimeDate = new Date(JtoG(sDateArr[0], sDateArr[1], sDateArr[2], true) + ' ' + '12:00');
    var validSEndTimeDate = new Date(JtoG(eDateArr[0], eDateArr[1], eDateArr[2], true) + ' ' + '12:00');
    var startt = Math.floor(validStartTimeDate.getTime() / (3600 * 24 * 1000)); //days as integer from..
    var endd = Math.floor(validSEndTimeDate.getTime() / (3600 * 24 * 1000)); //days as integer from..
    return endd - startt;
}

function DatesBetween2Date(s, e) {
    var between = [];
    var sDateArr = s.split('/');
    var eDateArr = e.split('/');
    var sTime = new Date(JtoG(sDateArr[0], sDateArr[1], sDateArr[2], true) + ' ' + '12:00');
    var eTime = new Date(JtoG(eDateArr[0], eDateArr[1], eDateArr[2], true) + ' ' + '12:00');
    while (sTime <= eTime) {
        between.push(new Date(sTime));
        sTime.setDate(sTime.getDate() + 1);
    }
    for (var i = 0; i < between.length; i++) {
        between[i] = GtoJ(between[i].getFullYear(), between[i].getMonth() + 1, between[i].getDate());
        between[i][1] = between[i][1] < 10 ? '0' + between[i][1] : between[i][1];
        between[i][2] = between[i][2] < 10 ? '0' + between[i][2] : between[i][2]; 
        between[i] = between[i][0] + '/' + between[i][1] + '/' + between[i][2];
    }
    return between;
}
/* jalali to gregorian  */
function JtoG(year, month, day, f) {
    function div(x, y) { return Math.floor(x / y); }
    $g_days_in_month = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
    $j_days_in_month = new Array(31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29);
    $jy = year - 979; $jm = month - 1; $jd = day - 1;
    $j_day_no = 365 * $jy + div($jy, 33) * 8 + div($jy % 33 + 3, 4);
    for ($i = 0; $i < $jm; ++$i)$j_day_no += $j_days_in_month[$i];
    $j_day_no += $jd; $g_day_no = $j_day_no + 79; $gy = 1600 + 400 * div($g_day_no, 146097);
    $g_day_no = $g_day_no % 146097; $leap = true; if ($g_day_no >= 36525) {
        $g_day_no--; $gy += 100 * div($g_day_no, 36524); $g_day_no = $g_day_no % 36524;
        if ($g_day_no >= 365) $g_day_no++; else $leap = false;
    } $gy += 4 * div($g_day_no, 1461); $g_day_no %= 1461; if ($g_day_no >= 366) {
        $leap = false;
        $g_day_no--; $gy += div($g_day_no, 365); $g_day_no = $g_day_no % 365;
    } for ($i = 0; $g_day_no >= $g_days_in_month[$i] + ($i == 1 && $leap); $i++)
        $g_day_no -= $g_days_in_month[$i] + ($i == 1 && $leap); $gm = $i + 1; $gd = $g_day_no + 1; if (!f || f == undefined) return { y: $gy, m: $gm, d: $gd }
    else return $gy + '/' + $gm + '/' + $gd;
}

//gregorian to jalali
function GtoJ(a, r, s) {
    a = parseInt(a), r = parseInt(r), s = parseInt(s);
    for (var n = a - 1600, e = r - 1, t = s - 1, p = 365 * n + parseInt((n + 3) / 4) - parseInt((n + 99) / 100) + parseInt((n + 399) / 400), I = 0; e > I; ++I) p += gorgian_days[I];
    e > 1 && (n % 4 == 0 && n % 100 != 0 || n % 400 == 0) && ++p, p += t; var v = p - 79, d = parseInt(v / 12053);
    v %= 12053; var o = 979 + 33 * d + 4 * parseInt(v / 1461); v %= 1461, v >= 366 && (o += parseInt((v - 1) / 365), v = (v - 1) % 365);
    for (var I = 0; 11 > I && v >= jalali_days[I]; ++I) v -= jalali_days[I]; var y = I + 1, _ = v + 1; return [o, y, _]
} var gorgian_days = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31], jalali_days = [31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29];

function GetChartData(obj) {
    $.ajax({
        type: "POST",
        url: "Reports.asmx/" + obj.url,
        data: JSON.stringify(obj.data[0]),
        contentType: 'application/json;',
        dataType: 'json',
        success: function (e) {
            var data = JSON.parse(e.d);
            if (obj.chartype == 'pie') {
                CreatePieChart(obj.header, data, obj.element, obj.chartype);   
            }
            if (obj.chartype == 'column') {
                CreateColumnChart(obj.lblkind,obj.header, data, obj.element, obj.chartype);
            }
            if (obj.chartype == 'categorycolumn') {
                CreateMultipleColumnChart(obj.kind, obj.comment1, obj.comment2,obj.label,obj.header, data, obj.element, obj.chartype);
            }
            CreateTableForChart(data);
        },
        error: function () {
            RedAlert('no', '!!خطا در دریافت اطلاعات');
        }
    });
}

function CreatePieChart(reportName, reportData, chartElement, chartype) {
    var dataa = [];
    for (var i = 0; i < reportData.length; i++) {
        dataa.push([reportData[i][0],parseInt(reportData[i][1])]);
    }
    Highcharts.chart(chartElement, {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: chartype,
            style: {
                fontFamily: 'myfont'
            }
        },
        title: {
            text: reportName,
            style: {
                fontFamily: 'myfont'
            }
        },
        tooltip: {
            style: {
                fontFamily: 'myfont',
                fontSize: '12px'
            },
            formatter: function () {
                return this.y + ' : ' + this.point.name;
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>% {point.name} </b>: {point.percentage:.1f}',
                    style: {
                        fontSize: "14px",
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor)
                    },
                    useHTML: true
                }
            }
        },
        series: [{
            colorByPoint: true,
            data: dataa
        }]
    });
}

function CreateColumnChart(lblkind,reportName, reportData, chartElement, chartype) {
    Highcharts.chart(chartElement, {
        title: {
            text: reportName,
            style: {
                fontFamily: 'myfont'
            }
        },
        tooltip: {
            style: {
                fontFamily: 'myfont',
                fontSize: '12px'
            },
            formatter: function () {
                return lblkind+ ' : ' + this.y ;
            }
        },
        xAxis: {
            categories: reportData.Strings,
            labels: {
                style: {
                    fontFamily:'myfont',
                    fontSize: '12px'
                }
            }
        },
        yAxis: {
            title: {
                text: lblkind,
                style: {
                    fontFamily: 'myfont',
                    fontSize: '16px'
                }
            },
            labels: {
                style: {
                    fontFamily:'myfont'
                }
            }
        },
        series: [{
            type: chartype,
            colorByPoint: true,
            data: reportData.Integers,
            showInLegend: false
        }]
    });
}
function CreateMultipleColumnChart(kind,c1,c2,lbl,reportName, reportData, chartElement, cat) {

    Highcharts.chart(chartElement, {
        chart: {
            type: 'column'
        },
        title: {
            text: reportName,
            style: {
                fontFamily: 'myfont'
            }
        },
        xAxis: {
            categories: reportData.Machine,
            crosshair: true,
            labels: {
                style: {
                    fontFamily: 'myfont',
                    fontSize: '12px'
                }
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: kind,
                style: {
                    fontFamily: 'myfont',
                    fontSize: '16px'
                }
            },
            labels: {
                style: {
                    fontFamily: 'myfont',
                    fontSize: '12px'
                }
            }
        },
        tooltip: {
            style: {
                fontFamily: 'myfont',
                fontSize: '12px'
            },
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f} '+lbl+'</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
           
            column: {
                style: {
                    fontFamily: 'myfont',
                    fontSize: '12px'
                },
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [{
            name: c1,
            data: reportData.Mtt
            
        }, {
            name: c2,
            data: reportData.MttH
           
            }
        ]
       
    });
}

function AjaxCall(obj) {
    $.ajax({
        type: 'POST',
        url: obj[0].url,
        data: JSON.stringify(obj[0].parameters[0]),
        contentType: 'application/json;',
        dataType: 'json',
        success: obj[0].func,
        error: function () {
            console.log('error');
        }
    });
}

function AjaxData(obj) {
    $.ajax({
        type: 'POST',
        url: obj.url,
        data: JSON.stringify(obj.param),
        contentType: 'application/json;',
        dataType: 'json',
        success: obj.func,
        error: function () {
            console.log('error');
        }
    });
}

function FilterMachineByUnit(unit , machine) {
    var location = $("#" + unit + " :selected").val();
    AjaxData({
        url: 'WebService.asmx/FilterMachineOrderByLocation',
        param: { loc: location },
        func: fillelement
    });
    function fillelement(e) {
        var data = JSON.parse(e.d);
        $('#' + machine).empty();
        var options = [];
        $('#' + machine).append($("<option></option>").attr("value", -1).text('انتخاب کنید'));
        for (var i = 0; i < data.length; i++) {
          options.push('<option value="' + data[i].MachineId + '">' + data[i].MachineName+'</option>');
        }
        $('#' + machine).append(options);
    }
}
let holidays = [];
let leaves = [];
let weekends = [];
let dates = [];

var d = new Date();
var year = d.getFullYear();

$.ajax({
    url: 'https://kalenderindonesia.com/api/API5han87EGKz/libur/masehi/' + year
}).done((data) => {
    var text = ""
    $.each(data.data.holiday, function (key, month) {
        $.each(month.data, function (key, i) {
            var newDate = formatDate(i.date)
            text += `<tr>
                    <td>${i.name}</td>
                    <td>${newDate}</td>
                 </tr>`
            holidays.push(i.date);
        })
    })
    $("#holidayTable").html(text)
}).fail((error) => {
    console.log(error)
})

$.ajax({
    url: 'https://kalenderindonesia.com/api/API5han87EGKz/libur/masehi/' + year
}).done((data) => {
    var text = "";
    var len = 0;
    $.each(data.data.leave, function (key, month) {
        $.each(month.data, function (key, i) {
            len += 1;
            var newDate = formatDate(i.date)
            text += `<tr>
                    <td>${i.name}</td>
                    <td>${newDate}</td>
                 </tr>`
            leaves.push(i.date);
        })
    })
    if (window.location.href.indexOf("quota") > -1) {
        $("#massleave").val(len);
        $(".year").val(year);
    }
    $("#leaveCalendarTable").html(text)
}).fail((error) => {
    console.log(error)
})

$.ajax({
    url: 'https://kalenderindonesia.com/api/API5han87EGKz/kalender/masehi/' + year
}).done((data) => {
    var text = ""
    $.each(data.data.monthly, function (key, month) {
        $.each(month.daily, function (key, day) {
            var newDate = formatDate(day.date.M)
            //console.log("Hari: " + day.text.W + ", Tanggal: " + day.date.M);
            text += `<tr>
                    <td>${day.text.W}</td>
                    <td>${newDate}</td>
                 </tr>`
            if (day.text.W == "Sabtu" || day.text.W == "Ahad") {
                weekends.push(day.date.M);
            }
        })
    })
    $("#calendarTable").html(text)
}).fail((error) => {
    console.log(error)
})

function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [day, month, year].join('-');
}

function addQuota() {
    q1 = parseInt($("#leavequota").val());
    len = parseInt($("#massleave").val());
    var q2 = q1 - len;
    var obj = new Object();
    obj.Quota = q2;

    console.log(obj);

    $.ajax({
        url: '/Leaves/Quota',
        type: "PUT",
        data: obj
    }).done((result) => {
        console.log(result)
        Swal.fire({
            title: 'New Quota Added',
            /*    text: 'Input Success!',*/
            icon: 'success'
        })
    }).fail((error) => {
        console.log(error)
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
            footer: `<a href=${'https://httpstatuses.com/' + error.status} target="_blank"/>Why do I have this issue?</a>`
        })
    })
}

function totalDays() {
    var sdate = document.getElementById("startDate"),
        edate = document.getElementById("endDate"),
        startDate = new Date(sdate.value),
        endDate = new Date(edate.value);
    let countholiday = 0;
    let countweekend = 0;
    var leaveDate = startDate;
    const diffInMs = new Date(endDate) - new Date(startDate)
    console.log(diffInMs);
    const diffInDays = (diffInMs + 86400000) / (1000 * 60 * 60 * 24);
    console.log("total Leave before cut: " + diffInDays);

    const addDays = function (days) {
        const date = new Date(this.valueOf())
        date.setDate(date.getDate() + days)
        return date
    }

    for (var i = 0; i < diffInDays; i++) {
        dates.push(leaveDate);
        leaveDate = addDays.call(leaveDate, 1);
    }

    $.each(dates, function (key, index) {
        $.each(holidays, function (key, day) {
            var date = new Date(day);
            if (index.toDateString() === date.toDateString()) {
                countholiday++
            }
        })
        $.each(weekends, function (key, day) {
            var date = new Date(day);
            if (index.toDateString() === date.toDateString()) {
                countweekend++
            }
        })
    })
    dates = [];
    if (diffInMs == 0) {
        var leaveTotal = 1;
    }
    else if (countweekend == 0) {
        var leaveTotal = diffInDays;
    }
    else if (countweekend % 2 != 0) {
        var leaveTotal = diffInDays - (countholiday + countweekend - 1);
    }
    else {
        var leaveTotal = diffInDays - (countholiday + countweekend);
    }
    console.log("holiday + weekend count: " + (countholiday + countweekend));
    console.log("total Leave after cut: " + leaveTotal);
    return leaveTotal;
}

function FormatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('/');
}

function totalDay(startDate, endDate) {
    var sDate = new Date(FormatDate(startDate));
    var eDate = new Date(FormatDate(endDate));
    let countholiday = 0;
    let countweekend = 0;
    var leaveDate = sDate;
    const diffInMs = eDate - sDate;
    console.log(diffInMs);
    const diffInDays = (diffInMs + 86400000) / (1000 * 60 * 60 * 24);
    console.log("total Leave before cut: " + diffInDays);

    const addDays = function (days) {
        const date = new Date(this.valueOf())
        date.setDate(date.getDate() + days)
        return date
    }

    for (var i = 0; i < diffInDays; i++) {
        dates.push(leaveDate);
        leaveDate = addDays.call(leaveDate, 1);
    }

    $.each(dates, function (key, index) {
        $.each(holidays, function (key, day) {
            var date = new Date(day);
            if (index.toDateString() === date.toDateString()) {
                countholiday++
            }
        })
        $.each(weekends, function (key, day) {
            var date = new Date(day);
            if (index.toDateString() === date.toDateString()) {
                countweekend++
            }
        })
    })
    dates = [];
    if (diffInMs == 0) {
        var leaveTotal = 1;
    }
    else if (countweekend == 0) {
        var leaveTotal = diffInDays;
    }
    else if (countweekend % 2 != 0) {
        var leaveTotal = diffInDays - (countholiday + countweekend - 1);
    }
    else {
        var leaveTotal = diffInDays - (countholiday + countweekend);
    }
    console.log("holiday + weekend count: " + (countholiday + countweekend));
    console.log("total Leave after cut: " + leaveTotal);
    return leaveTotal;
}


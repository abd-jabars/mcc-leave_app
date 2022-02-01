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
    $("#leaveTable").html(text)
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

    console.log(JSON.stringify(obj));

    $.ajax({
        url: '/Leaves/Quota',
        type: "PUT",
        contentType: "application/json;charset=utf-8",
        traditional: true,
        data: JSON.stringify(obj)
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
    var count = -1;
    var startDate = new Date("2022-02-01");
    var currentDate = startDate;
    var endDate = new Date("2022-02-10");
    const diffInMs = new Date(endDate) - new Date(startDate)
    const diffInDays = diffInMs / (1000 * 60 * 60 * 24);
    console.log("start: " + startDate + ", End: " + endDate);
    console.log("total Leave before cut: " + diffInDays);

    console.log(dates);

    const addDays = function (days) {
        const date = new Date(this.valueOf())
        date.setDate(date.getDate() + days)
        return date
    }

    for (var i = 0; i <= diffInDays; i++) {
        dates.push(currentDate);
        currentDate = addDays.call(currentDate, 1);
    }

    $.each(dates, function (key, index) {
        $.each(holidays, function (key, day) {
            var date = new Date(day);
            if (index.toDateString() === date.toDateString()) {
                count++
            }
        })
        $.each(weekends, function (key, day) {
            var date = new Date(day);
            if (index.toDateString() === date.toDateString()) {
                count++
            }
        })
    })
    console.log("holiday + weekend count: " + count);
    console.log("total Leave after cut: " + (diffInDays - count));
}




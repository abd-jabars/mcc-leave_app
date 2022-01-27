let holidays = [];
let leaves = [];
let weekends = [];
let dates = [];


$.ajax({
    url: 'https://kalenderindonesia.com/api/API5han87EGKz/libur/masehi/2022'
}).done((data) => {
    var text = ""
    $.each(data.data.holiday, function (key, month) {
        $.each(month.data, function (key, i) {
            text += `<tr>
                    <td>${i.name}</td>
                    <td>${i.date}</td>
                 </tr>`
            holidays.push(i.date);
        })
    })
    $("#holidayTable").html(text)
    console.log(holidays);
}).fail((error) => {
    console.log(error)
})

$.ajax({
    url: 'https://kalenderindonesia.com/api/API5han87EGKz/libur/masehi/2022'
}).done((data) => {
    var text = ""
    $.each(data.data.leave, function (key, month) {
        $.each(month.data, function (key, i) {
            text += `<tr>
                    <td>${i.name}</td>
                    <td>${i.date}</td>
                 </tr>`
            leaves.push(i.date);
        })
    })
    $("#leaveTable").html(text)
    console.log(leaves);
}).fail((error) => {
    console.log(error)
})

$.ajax({
    url: 'https://kalenderindonesia.com/api/API5han87EGKz/kalender/masehi/2022'
}).done((data) => {
    var text = ""
    $.each(data.data.monthly, function (key, month) {
        $.each(month.daily, function (key, day) {
            //console.log("Hari: " + day.text.W + ", Tanggal: " + day.date.M);
            text += `<tr>
                    <td>${day.text.W}</td>
                    <td>${day.date.M}</td>
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




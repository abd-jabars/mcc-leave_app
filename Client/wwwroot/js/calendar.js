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
        })
    })
    $("#holidayTable").html(text)
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
        })
    })
    $("#leaveTable").html(text)
}).fail((error) => {
    console.log(error)
})




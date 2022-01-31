$.ajax({
    url: '/Leaves/Normal'
}).done((data) => {
    var text = ""
    $.each(data, function (key, i) {
        $.each(i, function (key, j) {
            text += `<tr>
                    <td>${j.name}</td>
                    <td>12 Hari dalam setahun dikurangi cuti bersama</td>
                 </tr>`
        })
    })
    $("#normalTable").html(text)
    console.log(data);
}).fail((error) => {
    console.log(error)
})

$.ajax({
    url: '/Leaves/Special'
}).done((data) => {
    var text = ""
    $.each(data, function (key, i) {
        $.each(i, function (key, j) {
            text += `<tr>
                    <td>${j.name}</td>
                    <td>${j.period} hari</td>
                 </tr>`
        })
    })
    $("#specialTable").html(text)
    console.log(data);
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
    $("#massTable").html(text);
}).fail((error) => {
    console.log(error)
})



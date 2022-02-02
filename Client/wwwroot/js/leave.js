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

function addLeave() {
    var obj = new Object();
    obj.name = $("#leaveName").val();
    obj.period = $("#leavePeriod").val();
    obj.type = 1;

    console.log(obj)

    $.ajax({
        url: '/Leaves/POST/',
        type: "POST",
        contentType: "application/json;charset=utf-8",
        traditional: true,
        data: JSON.stringify(obj)
        //data: obj
    }).done((result) => {
        console.log(result)
        if (result.status == 200) {
            swalIcon = 'success';
            swalTitle = 'Input Success';
            swalFooter = '';
        } else {
            swalIcon = 'error';
            swalTitle = 'Oops...';
            swalFooter = `<a href=${'/leaves'}/>Ketentuan Cuti</a>`;
        }
        Swal.fire({
            title: swalTitle,
            icon: swalIcon,
            footer: swalFooter,
            text: result.message
        })
        $('#insertModal').modal('hide');
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


var nik = localStorage.getItem("nik");

var table = $('#leaveTable').DataTable({
    dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>><"row"<"col-sm-12"t>><"row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
    'ajax': {
        'url': '/leaveemployees/approval/' + nik,
        'dataType': 'json',
        'dataSrc': ''
    },
    'columns': [
        {
            'data': null,
            'render': function (data, type, row, meta) {
                return (meta.row + meta.settings._iDisplayStart + 1)
            }
        },
        {
            'data': 'nik'
        },
        {
            'data': 'fullName'
        },
        {
            'bSortable': false,
            'data': 'startDate'
        },
        {
            'bSortable': false,
            'data': 'endDate'
        },
        {
            "data": null,
            'bSortable': false,
            "defaultContent": `
                                <button class="btn btn-sm btn-outline-primary" id="btn-details"><i class="fas fa-info-circle"></i></button>
                               `
        }
    ]
});
$(document).ready(function () {
    table;
    $('#declineModal').on('click', '#btn-decline', function () {
        $('#leaveDetailModal').modal('hide');
        declineLeave();
        tableReload();
    });
    $('#leaveDetailModal').on('click', '#btn-approve', function () {
        var data = table.row($(this).closest('tr')).data();
        approveLeave(data);
    });
    $('#leaveTable').on('click', '#btn-details', function () {
        var data = table.row($(this).closest('tr')).data();
        detailLeave(data);
    });
});

function tableReload() {
    var myTable = $('#leaveTable').DataTable();
    myTable.ajax.reload();
};

function detailLeave(data) {
    sessionStorage.setItem("leaveID", data.id);
    $.ajax({
        url: '/leaveemployees/show/' + data.id,
        dataSrc: ''
    }).done((leaveDetails) => {
        console.log(leaveDetails);
        
        for (var i = 0; i < leaveDetails.length; i++) {
            sessionStorage.setItem("detailNIK", leaveDetails[i].nik);
            var types = leaveDetails[i].type;
            if (types == 0) {
                types = "Cuti Normal";
            }
            else {
                types = "Cuti Spesial";
            }
            var text = `
                    <tr>
                        <td>NIK: </td>
                        <td>${leaveDetails[i].nik}</td>
                   </tr>
                    <tr>
                        <td>Name : </td>
                        <td>${leaveDetails[i].fullName}</td>
                   </tr>
                    <tr>
                        <td>Department : </td>
                        <td>${leaveDetails[i].id}</td>
                   </tr>
                    <tr>
                        <td>Leave Type : </td>
                        <td>${types}</td>
                   </tr>
                    <tr>
                        <td>Date : </td>
                        <td>${leaveDetails[i].startDate} - ${leaveDetails[i].endDate}</td>
                   </tr>
                    <tr>
                        <td>Total Leave : </td>
                        <td>${leaveDetails[i].totalLeave} Days</td>
                   </tr>
                    <tr>
                        <td>Notes : </td>
                        <td>${leaveDetails[i].attachment}</td>
                   </tr>`
        }
        $("#infoTable").html(text)
        $('#leaveDetailModal').modal('show');
    }).fail((error) => {
        console.log(error)
    })
}

function approveLeave(data) {
    var obj = new Object();
    obj.nik = sessionStorage.getItem("detailNIK");
    obj.leaveId = sessionStorage.getItem("leaveID");
    obj.leaveStatus = 1

    console.log(JSON.stringify(obj));

    $.ajax({
        url: '/Leaves/Approval',
        type: "PUT",
        // contentType: "application/json;charset=utf-8",
        traditional: true,
        //data: JSON.stringify(obj)
        data: obj
    }).done((result) => {
        console.log(result)
        Swal.fire({
            title: 'Leave Approved',
            /*    text: 'Input Success!',*/
            icon: 'success'
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
    tableReload();
}

function declineLeave() {
    var obj = new Object();
    obj.nik = sessionStorage.getItem("detailNIK");
    obj.leaveId = sessionStorage.getItem("leaveID");
    obj.managerNote = $("#declineNotes").val();
    obj.leaveStatus = 2

    $('#declineModal').modal('hide');

    console.log(JSON.stringify(obj));

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Decline it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Leaves/Approval',
                type: "PUT",
                // contentType: "application/json;charset=utf-8",
                traditional: true,
                // data: JSON.stringify(obj)
                data: obj
            }).done((result) => {
                console.log(result);
                Swal.fire({
                    title: 'Leave Declined',
                    //text: 'Input Success!',
                    icon: 'success'
                })
                $('#insertModal').modal('hide');
            }).fail((error) => {
                console.log(error);
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Something went wrong!',
                    footer: `<a href=${'https://httpstatuses.com/' + error.status} target="_blank"/>Why do I have this issue?</a>`
                })
            })
        }
    })
    tableReload();
}
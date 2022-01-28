$(document).ready(function () {
    var table = $('#leaveTable').DataTable({
        dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>><"row"<"col-sm-12"t>><"row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
        buttons: [
            {
                extend: 'copy',
                text: '<i class="fa fa-files-o"></i>',
                exportOptions: {
                    columns: [0, 1]
                }
            },
            {
                extend: 'csv',
                text: '<i class="fa fa-file-text-o"></i>',
                exportOptions: {
                    columns: [0, 1]
                }
            },
            {
                extend: 'excel',
                text: '<i class="fa fa-file-excel-o"></i>',
                exportOptions: {
                    columns: [0, 1]
                }
            },
            {
                extend: 'pdf',
                text: '<i class="fa fa-file-pdf-o"></i>',
                orientation: 'portrait',
                title: 'Registered Data',
                pageSize: 'LEGAL',
                exportOptions: {
                    columns: [0, 1]
                }
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i>',
                title: 'Registered Data',
                exportOptions: {
                    columns: [0, 1]
                }
            }
        ],
        'ajax': {
            'url': 'https://localhost:44316/api/leaveemployees/approval',
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
                               <button class="btn btn-sm btn-outline-success"id="btn-approve"><i class="fas fa-check"></i></button>
                               <button class="btn btn-sm btn-outline-danger" id="btn-decline"><i class="fas fa-ban"></i></button>
                               `
            }
        ]
    });
    $('#leaveTable').on('click', '#btn-decline', function () {
        var data = table.row($(this).closest('tr')).data();
        declineLeave(data);
        tableReload();
    });
    $('#leaveTable').on('click', '#btn-approve', function () {
        var data = table.row($(this).closest('tr')).data();
        approveLeave(data);
        btnDisable(data);
        tableReload();
    });
    $('#leaveTable').on('click', '#btn-details', function () {
        var data = table.row($(this).closest('tr')).data();
        detailLeave(data);
    });
    function tableReload() {
        console.log("page load");
        table.ajax.reload();
    };
});

function btnDisable(data) {
    if (data.status == 1) {
        document.getElementById("btn-decline").disabled = true;
    }
    else if (data.status == 2) {
        document.getElementById("btn-approve").disabled = true;
    }
}

function detailLeave(data) {
    $.ajax({
        url: 'https://localhost:44316/api/leaveemployees/show/' + data.id,
        dataSrc: ''
    }).done((leaveDetails) => {
        console.log(leaveDetails);
        for (var i = 0; i < leaveDetails.length; i++) {
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
                        <td>${leaveDetails[i].type}</td>
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
                        <td>Attachment : </td>
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
    obj.nik = data.nik
    obj.leaveId = data.id
    obj.leaveStatus = 1

    console.log(JSON.stringify(obj));

    $.ajax({
        url: 'https://localhost:44316/api/Leaves/Approval',
        type: "PUT",
        contentType: "application/json;charset=utf-8",
        traditional: true,
        data: JSON.stringify(obj)
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
}

function declineLeave(data) {
    var obj = new Object();
    obj.nik = data.nik
    obj.leaveId = data.id
    obj.leaveStatus = 2

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
                url: 'https://localhost:44316/api/Leaves/Approval',
                type: "PUT",
                contentType: "application/json;charset=utf-8",
                traditional: true,
                data: JSON.stringify(obj)
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
}
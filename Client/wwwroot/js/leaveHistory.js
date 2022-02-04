var nik = localStorage.getItem("nik");

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
            'url': '/leaveemployees/history/' + nik,
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
                'data': 'fullName'
            },
            {
                'data': 'startDate'
            },
            {
                'data': 'endDate'
            },
            {
                'data': null,
                'render': function (data, type, row) {
                    if (row['status'] == 1) {
                        return row['status'] = "Disetujui"
                    }
                    else if (row['status'] == 2) {
                        return row['status'] = "Ditolak"
                    }
                    else {
                        return row['status'] = "Diproses"
                    }
                }
            },
            {
                "data": null,
                'bSortable': false,
                "defaultContent": `<button class="btn btn-sm btn-outline-primary" id="btn-details"><i class="fas fa-info-circle"></i></button>
                               `
            }
        ]
    });
    $('#leaveTable').on('click', '#btn-details', function () {
        var data = table.row($(this).closest('tr')).data();
        detailLeave(data);
    });
});

function detailLeave(data) {
    $.ajax({
        url: '/leaveemployees/show/' + data.id,
        dataSrc: ''
    }).done((leaveDetails) => {
        for (var i = 0; i < leaveDetails.length; i++) {
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
                        <td>Phone : </td>
                        <td>${leaveDetails[i].phone}</td>
                   </tr>
                    <tr>
                        <td>Email : </td>
                        <td>${leaveDetails[i].email}</td>
                   </tr>
                    <tr>
                        <td>Leave Type : </td>
                        <td>${types}</td>
                   </tr>
                    <tr>
                        <td>Total Leave : </td>
                        <td>${totalDay(leaveDetails[i].startDate, leaveDetails[i].endDate)} Days</td>
                   </tr>
                    <tr>
                        <td>Date : </td>
                        <td>${formatDate(leaveDetails[i].startDate)} to ${formatDate(leaveDetails[i].endDate)}</td>
                   </tr>
                    <tr>
                        <td>Notes : </td>
                        <td>${leaveDetails[i].attachment}</td>
                   </tr>
                    <tr>
                        <td>Manager Notes : </td>
                        <td>${leaveDetails[i].managerNote}</td>
                   </tr>`
        }
        $("#infoTable").html(text)
        $('#leaveDetailModal').modal('show');
    }).fail((error) => {
        console.log(error)
    })
}
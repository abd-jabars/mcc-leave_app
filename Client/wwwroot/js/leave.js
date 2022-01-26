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
            'url': 'https://localhost:44316/api/leaveemployees',
            'dataType': 'json',
            'dataSrc': ''
        },
        'columns': [
            {
                'data': 'id'
            },
            {
                'data': 'nik'
            },
            {
                'data': 'startDate'
            },
            {
                'data': 'endDate'
            },
            {
                'data': 'status'
            },
            {
                "data": null,
                'bSortable': false,
                "defaultContent": `<button class="btn btn-sm btn-outline-secondary" data-toggle="modal" data-target="#insertModal" id="btn-edit"><i class="fas fa-edit"></i></button>
                                   <button class="btn btn-sm btn-outline-danger" id="btn-delete"><i class="fas fa-trash"></i></button>`
            }
        ]
    });
    $('#leaveTable').on('click', '#btn-delete', function () {
        var data = table.row($(this).closest('tr')).data();
        deleteEmployee(data.nik);
    });
    $('#leaveTable').on('click', '#btn-edit', function () {
        var data = table.row($(this).closest('tr')).data();
        document.getElementById('btn-update').style.visibility = 'visible';
        document.getElementById('btn-insert').style.visibility = 'hidden';
        document.getElementById('btn-insert').style.display = 'none';
        document.getElementById('btn-update').style.display = 'inline';
        document.getElementById('leaveRequestForm').classList.remove('was-validated');
        editEmployee(data);
    });
    $('#leaveTable').on('click', '#btn-details', function () {
        var data = table.row($(this).closest('tr')).data();
        detailEmployee(data);
    });
});

$('.btn-add').on('click', function () {
    document.getElementById('btn-insert').style.visibility = 'visible';
    document.getElementById('btn-update').style.visibility = 'hidden';
    document.getElementById('btn-update').style.display = 'none';
    document.getElementById('btn-insert').style.display = 'inline';
    document.getElementById('leaveRequestForm').classList.remove('was-validated');
}
);

$('#insertModal').on('hidden.bs.modal', function (e) {
    document.getElementById('leaveRequestForm').classList.remove('was-validated');
    $('#leaveRequestForm')
        .find("input[type=text]")
        .val("");
})
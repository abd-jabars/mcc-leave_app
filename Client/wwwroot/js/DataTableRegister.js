$(document).ready(function () {
    $('#tes').DataTable({
        'ajax': {
            'url': "https://localhost:44316/API/Employees/RegisteredData",
            'dataSrc': 'result'
        },
        'columns': [
            {
                'data': null,
                'bSortable': false,
                'render': (data, type, row, meta) => {
                    return (meta.row + meta.settings._iDisplayStart + 1);
                }
            },
            {
                'data': 'nik'
            },
            {
                'data': null,
                'width': '100px',
                'render': function (data, type, row) {
                    return row['fullName']
                }
            },
            {
                'data': 'birthDate',
                'width': '100px'
            },
            {
                'data': null,
                'render': function (data, type, row) {
                    if (row['gender'] == 0) {
                        return row['gender'] = "Pria"
                    }
                    else {
                        return row['gender'] = "Wanita"
                    }
                }
            },
            {
                'data': 'email'
            },
            {
                'data': 'phone'
            },
            {
                'data': 'roleName'
            },
            {
                'data': null,
                'width': '150px',
                'render': function (data, type, row) {
                    return `<button data-toggle="modal" data-target="#registerNewEmployee" class="btn btn-warning fa fa-pencil" onclick="Update(${row["nik"]})"></button>
                            <button data-toggle="modal" data-target="#getEmployeeDetail" class="btn btn-danger fa fa-trash" onclick="Delete(${row["nik"]})"></button>`;
                }
            }
        ]
    });
});
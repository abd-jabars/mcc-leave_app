$(document).ready(function () {
    $('#dataTableRegister').DataTable({
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

function GetDepartmentManager() {
    $.ajax({
        url: "https://localhost:44316/API/Departments"

    }).done((result) => {
        var departmentOptions = "";

        $.each(result.result, function (key, val) {
            departmentOptions += `<option value="${val.id}">${val.name}</option>`
        });
        $("#department").html(departmentOptions);

        var managerOptions = "";

        $.each(result.result, function (key, val) {
            managerOptions += `<option value="${val.managerId}">${val.managerId}</option>`
        });
        $("#manager").html(managerOptions);

    }).fail((error) => {
        console.log(error);
    });
}

function Insert() {
    GetDepartmentManager();
    console.log("tes");

    $('#password').attr("readonly", false);

    $('#employeeForm').trigger("reset");
        
    var insertTitle = "";
    insertTitle += `<h3 class="mx-auto my-1"> Register an Employee </h3>`;
    $("#employeeModal .modal-header").html(insertTitle);

    $("#submitButton").html("Insert");

}

function RegisterEmployee() {
    var firstName = $('#firstName').val();
    var lastName = $('#lastName').val();
    var phone = $('#phone').val();
    var email = $('#email').val();
    var password = $('#password').val();
    var birthDate = $('#birthDate').val();
    var gender = $('#gender').val();
    var department = $('#department').val();
    var manager = $('#manager').val();

    var register = Object();
    register.FirstName = firstName;
    register.LastName = lastName;
    register.Phone = phone;
    register.Email = email;
    register.Password = password;
    register.BirthDate = birthDate;
    register.Gender = gender;
    register.Department = department;
    register.Manager = manager;

    console.log(register);

    var myTable = $('#dataTableRegister').DataTable();
    $.ajax({
        url: "https://localhost:44316/API/Employees/Register",
        contentType: "application/json;charset=utf-8",
        type: "POST",
        data: JSON.stringify(register)
        //data: register
    }).done((result) => {
        console.log(result);
        console.log(result.result);
        console.log(result.status);
        console.log(result.message);

        myTable.ajax.reload();
        var swalIcon;
        if (result.status == 200) {
            swalIcon = 'success';
            swalTitle = 'Success';
        } else {
            swalIcon = 'error'
            swalTitle = 'Oops!'
        }
        Swal.fire({
            icon: swalIcon,
            title: swalTitle,
            text: result.message,
        });
    }).fail((error) => {
        console.log(error);
        Swal.fire({
            icon: 'error',
            title: 'Something went wrong',
            text: "Hmmm....",
        });
    });

}

$('#employeeForm').submit(function (e) {
    e.preventDefault();
    if ($("#submitButton").html() == "Insert") {
        RegisterEmployee();
        $('#employeeForm').trigger("reset");
        $('#employeeModal').modal('hide');
    } else {
        UpdateData();
        $('#employeeForm').trigger("reset");
        $('#employeeModal').modal('hide');
    }
    //RegisterEmployee();
    //$('#employeeForm').trigger("reset");
    //$('#employeeModal').modal('hide');
});
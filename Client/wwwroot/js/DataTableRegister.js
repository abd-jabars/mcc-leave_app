$(document).ready(function () {
    $('#dataTableRegister').DataTable({
        'ajax': {
            'url': "https://localhost:44367/Employees/RegisteredData",
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
                    return `<button data-toggle="modal" data-target="#employeeModal" class="btn btn-warning fa fa-pencil" onclick="UpdateModal(${row["nik"]})"></button>
                            <button data-toggle="modal" class="btn btn-danger fa fa-trash" onclick="Delete(${row["nik"]})"></button>`;
                }
            }
        ]
    });
});

function GetDepartmentManager() {
    $.ajax({
        'url': "https://localhost:44367/Department/GetAll",
        'dataSrc': ''
    }).done((result) => {
        var departmentOptions = "";

        $.each(result, function (key, val) {
            departmentOptions += `<option value="${val.id}">${val.name}</option>`
        });
        $("#department").html(departmentOptions);

        var managerOptions = "";

        $.each(result, function (key, val) {
            managerOptions += `<option value="${val.managerId}">${val.managerId}</option>`
        });
        $("#manager").html(managerOptions);

    }).fail((error) => {
        console.log(error);
    });
}

function InsertModal() {
    GetDepartmentManager();
    
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
        url: "https://localhost:44367/Employees/Register",
        type: "POST",
        data: register
    }).done((result) => {
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

function SetFormValue(result) {
    var updateTitle = "";
    updateTitle += `<h3 class="mx-auto my-1"> Update data: ${result.nik} - ${result.firstName} ${result.lastName} </h3>`;
    $("#employeeModal .modal-header").html(updateTitle);

    const splitBirthDate = result.birthDate.split("T");

    let nik = result.nik;
    let firstName = result.firstName;
    let lastName = result.lastName;
    let birthDate = splitBirthDate[0];
    let email = result.email;
    let phone = result.phone;
    let gender = result.gender;
    let departmentId = result.departmentId;
    let managerId = result.managerId;
    
    $("#nik").val(nik);
    $("#firstName").val(firstName);
    $("#lastName").val(lastName);
    $("#birthDate").val(birthDate);
    $("#email").val(email);
    $('#password').attr("readonly", true);
    $("#phone").val(phone);
    $("#gender").val(gender);
    $("#department").val(departmentId);
    $("#manager").val(managerId);
    $("#submitButton").html("Update");
}

function UpdateModal(nik) {
    GetDepartmentManager();
    $.ajax({
        'url': "https://localhost:44367/Employees/RegisteredData/" + nik,
        'dataSrc': ''
    }).done((result) => {

        console.log(result);

        SetFormValue(result);

    }).fail((error) => {
        console.log(error);
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
});
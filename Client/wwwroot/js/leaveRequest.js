﻿var nik = localStorage.getItem("nik");

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
            'url': '/LeaveEmployees/GetByNik/' + nik,
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
                               <button class="btn btn-sm btn-outline-secondary" data-toggle="modal" data-target="#insertModal" id="btn-edit"><i class="fas fa-edit"></i></button>
                               <button class="btn btn-sm btn-outline-danger" id="btn-delete"><i class="fas fa-trash"></i></button>
                               `
            }
        ]
    });
    $('#leaveTable').on('click', '#btn-delete', function () {
        var data = table.row($(this).closest('tr')).data();
        deleteRequest(data);
    });
    $('#leaveTable').on('click', '#btn-edit', function () {
        var data = table.row($(this).closest('tr')).data();
        $("#leaveNIK").val(nik);
        document.getElementById('btn-update').style.visibility = 'visible';
        document.getElementById('btn-insert').style.visibility = 'hidden';
        document.getElementById('btn-insert').style.display = 'none';
        document.getElementById('btn-update').style.display = 'inline';
        document.getElementById('leaveRequestForm').classList.remove('was-validated');
        SetFormValue(data);
    });
    $('#leaveTable').on('click', '#btn-details', function () {
        var data = table.row($(this).closest('tr')).data();
        detailLeave(data);
    });
    $('#leaveRequestForm').on('click', '#btn-insert', function () {
        requestLeave();
    });
    $('#leaveRequestForm').on('click', '#btn-update', function () {
        updateLeave();
    });
    //setInterval(function () {
    //    table.ajax.reload();
    //}, 3000);
    getLeave();
});

function isFutureDate() {
    var today = new Date(),
        idate = document.getElementById("startDate"),
        date = new Date(idate.value);
    if (date > today) {
        $("#endDate").val("");
        $("#endDate").prop('disabled', false);
        console.log("Entered date is a future date");
    } else {
        $("#endDate").val("You entered an invalid date")
        $("#endDate").prop('disabled', true);
        console.log("Entered date is a past date");
    }
}

function SetFormValue(data) {
    let Id = data.id;
    let leaveId = data.leaveId;
    let startDate = data.startDate;
    let endDate = data.endDate;
    let attachment = data.attachment;

    $("#formId").val(Id);
    $("#leaveSelect").val(leaveId);
    $("#startDate").val(startDate);
    $("#endDate").val(endDate);
    $("#attachment").val(attachment);
}

$('.btn-add').on('click', function () {
    $("#leaveNIK").val(nik);
    document.getElementById('btn-insert').style.visibility = 'visible';
    document.getElementById('btn-update').style.visibility = 'hidden';
    document.getElementById('btn-update').style.display = 'none';
    document.getElementById('btn-insert').style.display = 'inline';
    document.getElementById('leaveRequestForm').classList.remove('was-validated');
}
);

function detailLeave(data) {
    $.ajax({
        url: '/leaveemployees/show/' + data.id,
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
                        <td>Phone : </td>
                        <td>${leaveDetails[i].phone}</td>
                   </tr>
                    <tr>
                        <td>Email : </td>
                        <td>${leaveDetails[i].email}</td>
                   </tr>
                    <tr>
                        <td>Leave Type : </td>
                        <td>${leaveDetails[i].type}</td>
                   </tr>
                    <tr>
                        <td>Total Leave : </td>
                        <td>${leaveDetails[i].totalLeave} Days</td>
                   </tr>
                    <tr>
                        <td>Date : </td>
                        <td>${leaveDetails[i].startDate} to ${leaveDetails[i].endDate}</td>
                   </tr>
                    <tr>
                        <td>Notes : </td>
                        <td>${leaveDetails[i].attachment}</td>
                   </tr>
                    <tr>
                        <td>Status : </td>
                        <td>${leaveDetails[i].status}</td>
                   </tr>`
        }
        $("#infoTable").html(text)
        $('#leaveDetailModal').modal('show');
    }).fail((error) => {
        console.log(error)
    })
}

function getLeave() {
    $.ajax({
        url: '/Leaves/GetAll'
    }).done((data) => {
        var leaveSelect = `<option value="" >Select Leave type</option>`;
        $.each(data, function (key, val) {
            leaveSelect += `<option value='${val.id}'>${val.name}</option>`
        });
        $("#leaveSelect").html(leaveSelect);
        console.log(leaveSelect)
    }).fail((error) => {
        console.log(error)
    })
}

function requestLeave() {
    var obj = new Object();
    obj.nik = $("#leaveNIK").val();
    obj.leaveId = $("#leaveSelect").val();
    obj.startDate = $("#startDate").val();
    obj.endDate = $("#endDate").val();
    obj.attachment = $("#attachment").val();

    const diffInMs = new Date(obj.endDate) - new Date(obj.startDate)
    const diffInDays = diffInMs / (1000 * 60 * 60 * 24);
    console.log("total Leave: " + diffInDays);

    obj.totalLeave = diffInDays;

    console.log(JSON.stringify(obj))

    var myTable = $('#leaveTable').DataTable();

    $.ajax({
        url: '/Leaves/LeaveRequest',
        type: "POST",
        // contentType: "application/json;charset=utf-8",
        traditional: true,
        // data: JSON.stringify(obj)
        data: obj
    }).done((result) => {
        console.log(result)
        myTable.ajax.reload();
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

function updateLeave() {
    var obj = new Object();
    obj.id = $("#formId").val();
    obj.nik = $("#leaveNIK").val();
    obj.leaveId = $("#leaveSelect").val();
    obj.startDate = $("#startDate").val();
    obj.endDate = $("#endDate").val();
    obj.attachment = $("#attachment").val();

    console.log(JSON.stringify(obj));

    $.ajax({
        url: 'https://localhost:44316/api/Leaveemployees/',
        type: "PUT",
        contentType: "application/json;charset=utf-8",
        traditional: true,
        data: JSON.stringify(obj)
    }).done((result) => {
        console.log(result)
        Swal.fire({
            title: 'Leave Request Updated',
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

function deleteRequest(data) {
    console.log(data.id);
    var obj = new Object();
    obj.id = data.id;
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            var myTable = $('#leaveTable').DataTable();
            $.ajax({
                url: '/leaveemployees/delete',
                type: "DELETE",
                traditional: true,
                data: obj
            }).done((result) => {
                console.log(result);
                Swal.fire({
                    title: 'Delete Success',
                    //text: 'Input Success!',
                    icon: 'success'
                })
                $('#insertModal').modal('hide');
                myTable.ajax.reload();
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

$('#insertModal').on('hidden.bs.modal', function (e) {
    document.getElementById('leaveRequestForm').classList.remove('was-validated');
    $('#leaveRequestForm')
        .find("input[type=text]")
        .val("");
})

$(function () {
    var dateFormat = "mm/dd/yy",
        from = $("#startDate")
            .datepicker({
                defaultDate: "+1w",
                changeYear: true,
                changeMonth: true,
                minDate: 0,
                numberOfMonths: 1,
                yearRange: "-100:+20",
                beforeShowDay: $.datepicker.noWeekends
            })
            .on("change", function () {
                to.datepicker("option", "minDate", getDate(this));
            }),
        to = $("#endDate").datepicker({
            defaultDate: "+1w",
            changeYear: true,
            changeMonth: true,
            yearRange: "-100:+20",
            numberOfMonths: 1,
            beforeShowDay: $.datepicker.noWeekends
        })
            .on("change", function () {
                from.datepicker("option", "maxDate", getDate(this));
            });

    function getDate(element) {
        var date;
        try {
            date = $.datepicker.parseDate(dateFormat, element.value);
        } catch (error) {
            date = null;
        }
        return date;
    }
});
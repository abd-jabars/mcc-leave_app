$.ajax({
    url: "/Employees/GetUserData",
}).done((result) => {
    var userName = `${result.firstName} ${result.lastName}`;
    $("#userName").html(userName);
    $("#userName1").html(userName);

    var userEmail = `${result.email}`;
    $("#userEmail").html(userEmail);

    if (result.roleName != null) {
        var countRoles = result.roleName.length;
        var userRole = result.roleName[countRoles - 1];
        $("#userRole").html(userRole);
    }
}).fail((error) => {
    console.log(error)
})

$.ajax({
    url: "/LeaveEmployees/GetAll",
}).done((result) => {
    console.log(result);
    let countRequest = 0;
    $.each(result, function (key, val) {
        if (result[key].status == 0) {
            countRequest += 1;
        }
    });
    $("#emailNotification").html(countRequest);
    $("#notifTitle").html("Ada " + countRequest + " pengajuan cuti yang belum diproses");

    var notifBody = "";
    $.each(result, function (key, val) {
        if (result[key].status == 0) {
            notifBody += `<div class="notif-center">
                        <a href="#">
                            <div class="notif-icon notif-info"> <i class="fa fa-envelope"></i> </div>
                            <div class="notif-content">
                                <span class="block">
                                    ${result[key].nik} - ${result[key].employee.firstName} ${result[key].employee.lastName} mengirim pengajuan cuti
                                </span>
                            </div>
                        </a>
                    </div>`;
        }
    });
    $("#notifBody").html(notifBody);


}).fail((error) => {
    console.log(error)
})
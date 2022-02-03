$.ajax({
    url: "/Employees/GetUserData",
}).done((result) => {
    var userNik = result.nik;

    var userName = `${result.firstName} ${result.lastName}`;
    $("#userName").html(userName);
    $("#userName1").html(userName);
    localStorage.setItem("name", userName);

    var userEmail = `${result.email}`;
    $("#userEmail").html(userEmail);

    var tempNIK = result.nik;
    localStorage.setItem("nik", tempNIK);
    $("#userNIK").html(tempNIK);

    if (result.roleName != null) {
        var sorting = result.roleName.sort();
        var reverse = result.roleName.reverse();
        var role;
        if (reverse.at(-1) == "Admin") {
            role = reverse.at(-1)
        }
        else if (reverse[0] == "Manager") {
            role = reverse[0]
        }
        else {
            role = reverse[0]
        }
        $("#userRole").html(role);
    }

    if (result.managerId == null || result.managerId == userNik) {
        document.getElementById("reqLeave").style.display = "none";
        document.getElementById("bellIcon").style.display = "none";
        document.getElementById("historyCuti").style.display = "none";
    }

    setInterval(function () {
        RequestNotif(userNik);
        ApprovalNotif(userNik);
    }, 3000);

}).fail((error) => {
    console.log(error)
})

function RequestNotif(managerId) {
    $.ajax({
        url: "/LeaveEmployees/GetAll",
    }).done((result) => {
        // console.log(result);
        //console.log(managerId);

        let countRequest = 0;
        var notifBody = "";
        $.each(result, function (key, val) {
            //console.log(result[key].employee.managerId);
            if (result[key].status == 0 && result[key].employee.managerId == managerId) {
                countRequest += 1;
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
        $("#notifBodyManager").html(notifBody);
        $("#emailNotificationManager").html(countRequest);
        $("#notifTitleManager").html("Ada " + countRequest + " pengajuan cuti yang belum diproses");
    }).fail((error) => {
        console.log(error)
    })
}

function ApprovalNotif(employeeId) {
    $.ajax({
        url: "/LeaveEmployees/GetAll",
    }).done((result) => {
        //console.log(result);
        //console.log(employeeId);

        let countRequest = 0;
        var notifBody = "";
        $.each(result, function (key, val) {
            //console.log(result[key].employee.managerId);
            if (result[key].status != 0 && result[key].employee.nik == employeeId) {
                var startDateTime = result[key].startDate.split('T')[0];
                var endDateTime = result[key].endDate.split('T')[0];
                startDate = FormatDate(startDateTime);
                endDate = FormatDate(endDateTime);
                //console.log("start date: " + startDate);
                //console.log("end date: " + endDate);
                countRequest += 1;
                notifBody += `<div class="notif-center">
                        <a href="#">
                            <div class="notif-icon notif-info"> <i class="fa fa-envelope"></i> </div>
                            <div class="notif-content">
                                <span class="block">
                                    Cuti ${result[key].leave.name} - ${startDate} - ${endDate} telah diproses
                                </span>
                            </div>
                        </a>
                    </div>`;
            }
        });
        $("#notifBodyEmployee").html(notifBody);
        $("#emailNotificationEmployee").html(countRequest);
        $("#notifTitleEmployee").html("Ada " + countRequest + " pengajuan cuti yang telah diproses");

    }).fail((error) => {
        console.log(error)
    })
}

function FormatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [day, month, year].join('/');
}

$('.dropdown-item').on('click', '#logout-btn', function () {
    window.localStorage.clear();
});

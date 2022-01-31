$.ajax({
    url: "/Employees/GetUserData",
}).done((result) => {
    var userNik = result.nik;

    var userName = `${result.firstName} ${result.lastName}`;
    $("#userName").html(userName);
    $("#userName1").html(userName);

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

    $.ajax({
        url: "/LeaveEmployees/GetAll",
    }).done((result) => {
        console.log(result);
        console.log(userNik);

        let countRequest = 0;
        var notifBody = "";
        $.each(result, function (key, val) {
            //console.log(result[key].employee.managerId);
            if (result[key].status == 0 && result[key].employee.managerId == userNik) {
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

}).fail((error) => {
    console.log(error)
})

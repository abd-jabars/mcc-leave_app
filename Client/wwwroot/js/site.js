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
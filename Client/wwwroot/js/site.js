$.ajax({
    url: "/Employees/GetUserData",
}).done((result) => {
    var userName = `${result.firstName} ${result.lastName}`;
    $("#userName").html(userName);
    $("#userName1").html(userName);

    var userEmail = `${result.email}`;
    $("#userEmail").html(userEmail);

}).fail((error) => {
    console.log(error)
})
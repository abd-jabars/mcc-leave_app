$('#forgotPasswordForm').submit(function (e) {
    e.preventDefault();
    ForgotPassword()
    $('#forgotPasswordForm').trigger("reset");
});

function ForgotPassword() {
    var email = $('#inputEmail').val();

    var forgotPassword = Object();
    forgotPassword.email = email;

    $.ajax({
        url: "/Login/ForgotPassword",
        type: "PUT",
        data: forgotPassword
    }).done((result) => {
        console.log(result);

        var swalIcon;
        if (result.status == 200) {
            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: result.message,
            }).then(function () {
                location.href = "/Login/ChangePassword";
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Oops!',
                text: result.message,
            })
        }
    }).fail((error) => {
        console.log(error);
        Swal.fire({
            icon: 'error',
            title: 'Something went wrong',
            text: "Hmmm....",
        });
    });
}

$('#changePasswordForm').submit(function (e) {
    e.preventDefault();
    ChangePassword();
    $('#changePasswordForm').trigger("reset");
});

function ChangePassword() {
    var email = $('#inputEmail').val();
    var otp = $('#inputOTP').val();
    var password = $('#inputPassword').val();

    var forgotPassword = Object();
    forgotPassword.email = email;
    forgotPassword.otp = otp;
    forgotPassword.password = password;

    $.ajax({
        url: "/Login/ChangePassword",
        type: "PUT",
        data: forgotPassword
    }).done((result) => {
        console.log(result);

        var swalIcon;
        if (result.status == 200 || result.caseNumber == 1) {
            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: result.message,
            }).then(function () {
                location.href = "/Login";
            });
        } else if (result.caseNumber == 2) {
            Swal.fire({
                icon: 'error',
                title: 'Oops!',
                text: result.message,
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Oops!',
                text: result.message,
            }).then(function () {
                location.href = "/Login";
            });
        }
    }).fail((error) => {
        console.log(error);
        Swal.fire({
            icon: 'error',
            title: 'Something went wrong',
            text: "Hmmm....",
        });
    });
}
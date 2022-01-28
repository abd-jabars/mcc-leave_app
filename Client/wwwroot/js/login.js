const { fn } = require("jquery");

const signUpButton = document.getElementById('signUp');
const signInButton = document.getElementById('signIn');
const container = document.getElementById('container');

signUpButton.addEventListener('click', () => {
	container.classList.add("right-panel-active");
});

signInButton.addEventListener('click', () => {
	container.classList.remove("right-panel-active");
});

//function failedNotification() {
//    if ('@(TempData["code"])' == null) {
//        Swal.fire({
//            title: 'Update Success!',
//            /*    text: 'Input Success!',*/
//            icon: 'success'
//        })
//    }
//    else {
//        Swal.fire({
//            icon: 'error',
//            title: 'Oops...',
//            text: '@(TempData["message"])'
//        })
//    }
//}
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let name = document.getElementById("user-name").innerText;
let nik = document.getElementById("user-nik").innerText;

let name1 = '@Session["name"]';
let nik1 = '@Session["nik"]';

console.log("name: " + name1);
console.log("nik: " + nik);

$("#user-name").val(name);
$("#user-nik").val(nik);

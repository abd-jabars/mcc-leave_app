var nik = localStorage.getItem("nik");
var name = localStorage.getItem("name");


var connection = new signalR.HubConnectionBuilder()
    .withUrl("/notification")
    .build();

connection.on("ReceiveMessage", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var div = document.createElement("div");
    div.innerHTML = msg + "<hr/>";
    document.getElementById("messages").appendChild(div);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("btn-insert").addEventListener("click", function (event) {
    var message = name + " - " + nik + " mengirim pengajuan cuti";
    connection.invoke("SendMessageToAll", message).catch(function (err) {
        return console.error(err.toString());
    });

    event.preventDefault();
});
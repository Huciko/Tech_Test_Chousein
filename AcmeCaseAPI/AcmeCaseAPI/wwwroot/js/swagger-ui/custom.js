
// This file is used to manipulate the loaded swagger documentation UI

window.addEventListener("load", function () {
    var logoImg = document.querySelectorAll('.topbar .link img')[0];
    logoImg.alt = "Acme Case API";
    logoImg.src = "../images/logo.png";
    //logoImg.height = 80;
    //logoImg.width = 496;
});
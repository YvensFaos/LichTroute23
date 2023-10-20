document.addEventListener('DOMContentLoaded', function () {
    const previousPageButton = document.getElementById("previousPageButton");
    const noButton = document.getElementById("noResponse");
    const yesButton = document.getElementById("yesResponse");

    noButton.addEventListener('click', function(event) {
        window.location.href=`../`;
    });

    yesButton.addEventListener('click', function(event) {
        window.location.href='../wait.html';
    });
});
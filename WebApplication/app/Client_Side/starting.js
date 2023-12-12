document.addEventListener('DOMContentLoaded', function () {
    const startButton = document.getElementById("startButton");
    startButton.addEventListener('click', function (event) {
        window.location.href='../index.html';
    });


    function incrementVisitCount() {
        let visitCount = localStorage.getItem('visitCount') || 0;
        visitCount++;
        localStorage.setItem('visitCount', visitCount);
        return visitCount;
    }
});

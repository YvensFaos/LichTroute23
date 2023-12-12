document.addEventListener('DOMContentLoaded', function () {
    const startButton = document.getElementById("startButton");
    startButton.addEventListener('click', function (event) {
        window.location.href='/app/customize.html';
    });


    function incrementVisitCount() {
        let visitCount = localStorage.getItem('visitCount') || 0;
        visitCount++;
        localStorage.setItem('visitCount', visitCount);
        return visitCount;
    }
});

document.addEventListener('DOMContentLoaded', function () {
    const backButton = document.getElementById('back-button');
    const submitButton = document.getElementById('submit-button');
    
    backButton.addEventListener('click', function () {
        window.location.href = "/app/Character.html";
    });

    const selectedCharacterHead = localStorage.getItem('selectedCharacterHead') || '';
    const selectedCharacterBody = localStorage.getItem('selectedCharacterBody') || '';

    document.getElementById('selectedCharacterHead').value = selectedCharacterHead;
    document.getElementById('selectedCharacterBody').value = selectedCharacterBody;

    
});

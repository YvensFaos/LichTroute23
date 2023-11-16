document.addEventListener('DOMContentLoaded', function () {
    const characterHeadPreview = document.getElementById('characterHeadPreview');
    const characterBodyPreview = document.getElementById('characterBodyPreview');

    const characterHead = {
        Head1: '/public/thalia_head.png',
        Head2: '/public/OrangeHead.png',
        Head3: '/public/GreenHead.png',
        Head4: '/public/RedHead.png',
        Head5: '/public/BlueBody.png',
    };
    

    const characterBody = {
        Body1: '/public/body_thalia.png',
        Body2: 'public/OrangeBody.png',
        Body3: 'public/GreenBody.png',
        Body4: 'public/RedBody.png',
        Body5: 'public/BlueBody.png',
    };

    let selectedCharacterHead = localStorage.getItem('selectedCharacterHead');
    let selectedCharacterBody = localStorage.getItem('selectedCharacterBody');

    if (!selectedCharacterHead || !characterHead[selectedCharacterHead]) {
        selectedCharacterHead = 'Head1';
        localStorage.setItem('selectedCharacterHead', selectedCharacterHead);
    }

    if (!selectedCharacterBody || !characterBody[selectedCharacterBody]) {
        selectedCharacterBody = 'Body1';
        localStorage.setItem('selectedCharacterBody', selectedCharacterBody);
    }

    characterHeadPreview.src = characterHead[selectedCharacterHead];
    characterBodyPreview.src = characterBody[selectedCharacterBody];

    const previousPageButton = document.getElementById("previousPageButton");
    const noButton = document.getElementById("noResponse");
    const yesButton = document.getElementById("yesResponse");

    previousPageButton.addEventListener('click', function (event) {
        window.location.href = '../index.html';
    });

    noButton.addEventListener('click', function (event) {
        window.location.href = '../index.html';
    });

    yesButton.addEventListener('click', function (event) {
        window.location.href = '../wait.html';
    });
});

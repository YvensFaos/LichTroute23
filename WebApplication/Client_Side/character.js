document.addEventListener('DOMContentLoaded', function () {
    const characterHeadPreview = document.getElementById('characterHeadPreview');
    const characterBodyPreview = document.getElementById('characterBodyPreview');

    const characterHead = {
        Head1: '/public/CustomizationPage/head1.png',
        Head2: '/public/CustomizationPage/head2.png',
        Head3: '/public/CustomizationPage/head3.png',
        Head4: '/public/CustomizationPage/head4.png',
    };
    

    const characterBody = {
        Body1: '/public/CustomizationPage/body1.png',
        Body2: '/public/CustomizationPage/body2.png',
        Body3: '/public/CustomizationPage/body3.png',
        Body4: '/public/CustomizationPage/body4.png',
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

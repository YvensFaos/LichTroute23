document.addEventListener('DOMContentLoaded', function () {
    const characterHeadPreview = document.getElementById('characterHeadPreview');
    const characterBodyPreview = document.getElementById('characterBodyPreview');
    const dataHead = document.getElementById('head')
    const dataBody = document.getElementById('body')
    const dataInstrument = document.getElementById('instrument')
    const characterInfo = document.getElementById('character-info');


    const selectedCharacterHead = localStorage.getItem('selectedCharacterHead') || '';
    const selectedCharacterBody = localStorage.getItem('selectedCharacterBody') || '';
    const selectedInstrument = localStorage.getItem('selectedInstrument') || '';
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


    document.getElementById('yesResponse').addEventListener('click', function () {
        fetch('http://localhost:8000/queueMusicalCharacter', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: '{"character":"bob", "parameter":"Aulos", "value":1.0}'

        })
            .then(response => response.json())
            .then(data => {
                document.getElementById('myForm').reset();
            })
            .catch(error => {
                console.error('Error:', error);
            });
    });

    const selectedData = {
        character: 'bob',
        parameter: 'Aulos',
        value: 1.0,
    };

    document.getElementById('myForm').addEventListener('submit', function (event) {
        event.preventDefault();

        
    });
});
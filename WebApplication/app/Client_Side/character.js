document.addEventListener('DOMContentLoaded', function () {
    const characterHeadPreview = document.getElementById('characterHeadPreview');
    const characterBodyPreview = document.getElementById('characterBodyPreview');
    const dataHead = localStorage.getItem('selectedCharacterHead');
    const dataBody = localStorage.getItem('selectedCharacterBody');
    const dataInstrument = localStorage.getItem('selectedInstrument');

    let selectedCharacterHead = localStorage.getItem('selectedCharacterHead') || '';
    let selectedCharacterBody = localStorage.getItem('selectedCharacterBody') || '';
    const selectedInstrument = localStorage.getItem('selectedInstrument') || '';
    const characterHead = {
        1: '/app/public/CustomizationPage/head1.png',
        2: '/app/public/CustomizationPage/head2.png',
        3: '/app/public/CustomizationPage/head3.png',
        4: '/app/public/CustomizationPage/head4.png',
    };

    const characterBody = {
        1: '/app/public/CustomizationPage/body1.png',
        2: '/app/public/CustomizationPage/body2.png',
        3: '/app/public/CustomizationPage/body3.png',
        4: '/app/public/CustomizationPage/body4.png',
    };

    if (!selectedCharacterHead || !characterHead[selectedCharacterHead]) {
        selectedCharacterHead = '1';
        localStorage.setItem('selectedCharacterHead', selectedCharacterHead);
    }

    if (!selectedCharacterBody || !characterBody[selectedCharacterBody]) {
        selectedCharacterBody = '1';
        localStorage.setItem('selectedCharacterBody', selectedCharacterBody);
    }

    characterHeadPreview.src = characterHead[selectedCharacterHead];
    characterBodyPreview.src = characterBody[selectedCharacterBody];

    const noButton = document.getElementById("noResponse");
    const yesButton = document.getElementById("yesResponse");

    noButton.addEventListener('click', function (event) {
        window.location.href = '../index.html';
    });
    const characterInfo = {
        head: dataHead,
        body: dataBody,
        parameter: dataInstrument,
    };

    yesButton.addEventListener('click', function (event) {
        
        window.location.href = '../wait.html';
        console.log('Yes button clicked!');
        fetch('https://panfun.ngrok.io/queueMusicalCharacter', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(characterInfo)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            if (data && data.UID) {
                const uid = data.UID;
                console.log('UID:', uid);

                localStorage.setItem('UID', uid);
            } else {
                console.error('Error: UID not found in the response');
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
    });

    document.getElementById('myForm').addEventListener('submit', function (event) {
        event.preventDefault();
    });
});

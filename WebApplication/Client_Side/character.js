document.addEventListener('DOMContentLoaded', function () {
    const characterHeadPreview = document.getElementById('characterHeadPreview');
    const characterBodyPreview = document.getElementById('characterBodyPreview');
    const dataHead = localStorage.getItem('selectedCharacterHead');
    const dataBody = localStorage.getItem('selectedCharacterBody');
    const dataInstrument = localStorage.getItem('selectedInstrument');
    

    const selectedCharacterHead = localStorage.getItem('selectedCharacterHead') || '';
    const selectedCharacterBody = localStorage.getItem('selectedCharacterBody') || '';
    const selectedInstrument = localStorage.getItem('selectedInstrument') || '';
    const characterHead = {
        1: '/public/CustomizationPage/head1.png',
        2: '/public/CustomizationPage/head2.png',
        3: '/public/CustomizationPage/head3.png',
        4: '/public/CustomizationPage/head4.png',
    };
    

    const characterBody = {
        1: '/public/CustomizationPage/body1.png',
        2: '/public/CustomizationPage/body2.png',
        3: '/public/CustomizationPage/body3.png',
        4: '/public/CustomizationPage/body4.png',
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
        //window.location.href = '../wait.html';
    });
    /*
    yesButton.addEventListener('click', function (event) {
        console.log('Yes button clicked!');
        fetch('https://panfun.ngrok.io/queueRandomCharacter', {
            method: 'GET', 
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json(); 
        })
        .then(data => {

            console.log('Response:', data);
        })
        .catch(error => {
            console.error('Error:', error);
        });
    });
    */
    const characterInfo = {
        head: dataHead,
        body: dataBody,
        parameter: dataInstrument,
    };

    
    yesButton.addEventListener('click', function (event) {
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
            if (data) {
                document.getElementById('myForm').reset();
            } else {
                console.error('Error: Response is empty');
            }
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
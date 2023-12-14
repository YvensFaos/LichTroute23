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
        window.location.href = '/app/index.html';
    });

    const characterInfo = {
        head: dataHead,
        body: dataBody,
        parameter: dataInstrument,
    };

    yesButton.addEventListener('click', async function (event) {
        console.log('Yes button clicked!');
        
        const preflightResponse = await fetch('https://panfun.ngrok.io/queueMusicalCharacter', {
            method: 'OPTIONS',
            headers: {
                'Access-Control-Request-Method': 'POST',
                'Access-Control-Request-Headers': 'Content-Type',
            },
        });

        if (preflightResponse.ok) {
            
            const mainResponse = await fetch('https://panfun.ngrok.io/queueMusicalCharacter', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(characterInfo),
            });

            if (mainResponse.ok) 
            {
                const jsonResponse = await mainResponse.json();
                const UID = jsonResponse.UID;
                localStorage.setItem('UID', UID);
                window.location.href = '/app/wait.html';
            } else {
                console.error('Server returned an error:', mainResponse.status);
                window.location.href = '/app/wait.html';
            }
            
        } else {
            console.error('Preflight request failed:', preflightResponse.status);
        }
    });

    document.getElementById('myForm').addEventListener('submit', function (event) {
        event.preventDefault();
    });
});

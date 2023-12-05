document.addEventListener('DOMContentLoaded', function () {
    const continueLink = document.getElementById('continueLink');
    const selectedInstrumentInput = document.getElementById('selectedInstrument');
    const characterHeadPreview = document.getElementById('characterHeadPreview');
    const characterBodyPreview = document.getElementById('characterBodyPreview');
    const characterButtons = document.querySelectorAll('.character-button');
    const prevCharacterButton = document.getElementById('prevCharacterButton');
    const nextCharacterButton = document.getElementById('nextCharacterButton');
    const prevCharacterBodyButton = document.getElementById('prevCharacterBodyButton');
    const nextCharacterBodyButton = document.getElementById('nextCharacterBodyButton');
    let currentCharacterHeadIndex = 0;
    let currentCharacterBodyIndex = 0;
    
    const selectedData = {
        character: localStorage.getItem('selectedCharacterBody') || '',
        parameter: localStorage.getItem('selectedInstrument') || '',
        modifier: 1.0 || '',
    };
    const characterHead = {
        Head1:  '/public/CustomizationPage/head1.png',
        Head2:  '/public/CustomizationPage/head2.png',
        Head3:  '/public/CustomizationPage/head3.png',
        Head4:  '/public/CustomizationPage/head4.png',
    };
    const characterBody = {
        Body1: '/public/CustomizationPage/body1.png',
        Body2: '/public/CustomizationPage/body2.png',
        Body3: '/public/CustomizationPage/body3.png',
        Body4: '/public/CustomizationPage/body4.png',
    };

    const characterNames = {
        Character1: 'Mickey',
        Character2: 'Donald',
        Character3: 'Jerry',
    };

    function updateCharacterPreview() {
        const characterHeadArray = Object.keys(characterHead);
        const characterBodyArray = Object.keys(characterBody);
        const characterHeads = characterHeadArray[currentCharacterHeadIndex];
        const characterBodys = characterBodyArray[currentCharacterBodyIndex];
        
        characterHeadPreview.src = characterHead[characterHeads];
        characterBodyPreview.src = characterBody[characterBodys];
        console.log(characterHeads);
        console.log(characterBodys);
        
        localStorage.setItem('selectedCharacterHead', characterHeads)
        localStorage.setItem('selectedCharacterBody', characterBodys)
    }

    function storeData(field,value) {
        let characterHead = value;
        let characterBody = value;
        if(field == "characterHead"){
            localStorage.setItem('selectedCharacterHead', characterHead)
        } else if (field == "characterBody"){
            localStorage.setItem('selectedCharacterBody', characterBody)
        };
    }


    characterButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            const field = button.getAttribute('data-field');
            const value = button.getAttribute('data-value');
            document.getElementById(`selected${field.charAt(0).toUpperCase() + field.slice(1)}`).value = value;

            if (field === 'character') {
                currentCharacterHeadIndex = [...characterButtons].findIndex(b => b === button);
                updateCharacterPreview();
            }
        });
    });

    prevCharacterButton.addEventListener('click', function () {
        currentCharacterHeadIndex = (currentCharacterHeadIndex - 1 + Object.keys(characterHead).length) % Object.keys(characterHead).length;
        updateCharacterPreview();
    });

    nextCharacterButton.addEventListener('click', function () {
        currentCharacterHeadIndex = (currentCharacterHeadIndex + 1) % Object.keys(characterHead).length;
        updateCharacterPreview();
    });

    prevCharacterBodyButton.addEventListener('click', function () {
        currentCharacterBodyIndex = (currentCharacterBodyIndex - 1 + Object.keys(characterBody).length) % Object.keys(characterBody).length;
        updateCharacterPreview();
    });

    nextCharacterBodyButton.addEventListener('click', function () {
        currentCharacterBodyIndex = (currentCharacterBodyIndex + 1) % Object.keys(characterBody).length;
        updateCharacterPreview();
    });




    document.querySelectorAll('.btn-primary[data-field="instrument"]').forEach(button => {
        button.addEventListener('click', function () {
            const value = button.getAttribute('data-value');
            selectedInstrumentInput.value = value;
        });
    });


    continueLink.addEventListener('click', function (event) {
        event.preventDefault();

        const selectedInstrument = selectedInstrumentInput.value;

        window.location.href = `../instrument.html?instrument=${selectedInstrument}`;
    });

    updateCharacterPreview();





    
});

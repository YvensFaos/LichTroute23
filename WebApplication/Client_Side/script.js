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
        instrument: localStorage.getItem('selectedInstrument') || '',
        characterHead: localStorage.getItem('selectedCharacterHead') || '',
        characterBody: localStorage.getItem('selectedCharacterHead') || '',
    };
    // Character Images and Names
    const characterHead = {
        Head1: 'public/BlueHead.png',
        Head2: 'public/OrangeHead.png',
        Head3: 'public/GreenHead.png',
        Head4: 'public/RedHead.png',
    };
    const characterBody = {
        Body1: 'public/BlueBody.png',
        Body2: 'public/OrangeBody.png',
        Body3: 'public/GreenBody.png',
        Body4: 'public/RedBody.png',
    };

    const characterNames = {
        Character1: 'Mickey',
        Character2: 'Donald',
        Character3: 'Jerry',
    };

    // Function to update the character preview
    function updateCharacterPreview() {
        const characterHeadArray = Object.keys(characterHead);
        const characterBodyArray = Object.keys(characterBody);
        const characterHeads = characterHeadArray[currentCharacterHeadIndex];
        const characterBodys = characterBodyArray[currentCharacterBodyIndex];
        
        characterHeadPreview.src = characterHead[characterHeads];
        characterBodyPreview.src = characterBody[characterBodys];
        

        
        //document.getElementById('selectedCharacterHead').value = characterNames[character];
        //document.getElementById('selectedCharacterBody').value = characterNames[character];
    }

    function storeData(field,value) {
        let characterHead = value;
        let characterBody = value;
        if(field == "characterHead"){
            localStorage.setItem('selectedCharacterHead', characterHead)
            console.log("character HEad " + characterHead);
        } else if (field == "characterBody"){
            localStorage.setItem('selectedCharacterBody', characterBody)
            console.log("character Body " +characterBody);
        };
    }


    // Add click event listeners to character buttons
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


    // Add click event listeners to previous and next character buttons
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




    
    // Add click event listeners to instrument buttons
    document.querySelectorAll('.btn-primary[data-field="instrument"]').forEach(button => {
        button.addEventListener('click', function () {
            console.log("aaaaaaaaaaaaa");
            const value = button.getAttribute('data-value');
            selectedInstrumentInput.value = value;
        });
    });


    // Continue link click event listener
    continueLink.addEventListener('click', function (event) {
        console.log("11111111111111");
        event.preventDefault();

        const selectedInstrument = selectedInstrumentInput.value;
        console.log('Selected Instrument:', selectedInstrument); // Add this line for debugging

        // Ensure the URL being generated is correct
        window.location.href = `../instrument.html?instrument=${selectedInstrument}`;
        console.log('Generated URL:', continueLink.href); // Add this line for debugging
    });

    // Initial character preview update
    updateCharacterPreview();





    
});

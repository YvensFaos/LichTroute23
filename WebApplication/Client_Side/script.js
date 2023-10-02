document.addEventListener('DOMContentLoaded', function () {
    // Add a click event listener to the "Continue" button
    document.getElementById('continueLink').addEventListener('click', function (event) {
        // Prevent the link from navigating
        event.preventDefault();

        // Get the selected instrument from the hidden input field
        const selectedInstrument = document.getElementById('selectedInstrument').value;

        // Redirect to instrument.html with the selected instrument as a query parameter
        window.location.href = `instrument.html?instrument=${selectedInstrument}`;
    });

    // Get the data from local storage
    const selectedData = {
        instrument: localStorage.getItem('selectedInstrument') || '',
        character: localStorage.getItem('selectedCharacter') || '',
    };

    // Update preview image
    function updateCharacterPreview(character) {
        const characterPreview = document.getElementById('characterPreview');

        const characterImages = {
            Character1: 'public/Mickey.png',
            Character2: 'public/Donald.png', 
            Character3: 'public/Jerry.png',
            
        };

        
        const defaultCharacterImage = 'public/Mickey.png';

        characterPreview.src = characterImages[character] || defaultCharacterImage;
    }

    function handleButtonClick(event) {
        const button = event.target;
        const field = button.getAttribute('data-field');
        const value = button.getAttribute('data-value');

        console.log(`Clicked button value: ${value}`);

        document.getElementById(`selected${field.charAt(0).toUpperCase() + field.slice(1)}`).value = value;

        const buttonName = button.getAttribute('name');

        // Update instrument or characer
        if (field === 'instrument') {
            selectedData.instrument = value;
        } else if (field === 'character') {
            selectedData.character = value;

            
            updateCharacterPreview(value);
        }

        localStorage.setItem('selectedInstrument', selectedData.instrument);
        console.log("The character is " + selectedData.character);
        console.log("The instrument is " + selectedData.instrument);
        localStorage.setItem('selectedCharacter', selectedData.character);

    }

    const buttons = document.querySelectorAll('button[data-field]');
    buttons.forEach(button => {
        button.addEventListener('click', handleButtonClick);
    });

    document.getElementById('myForm').addEventListener('submit', function (event) {
        event.preventDefault(); 

        // Send the form data to the server
        fetch('/submit', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(selectedData)
        })
        .then(response => response.json())
        .then(data => {
            console.log(data.message);

            document.getElementById('myForm').reset();
        })
        .catch(error => {
            console.error('Error:', error);
        });
    });

});

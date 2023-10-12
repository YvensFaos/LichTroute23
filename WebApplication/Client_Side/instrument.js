document.addEventListener('DOMContentLoaded', function () {
    const urlParams = new URLSearchParams(window.location.search);
    const selectedInstrument = urlParams.get('instrument');
    const selectedCharacterHead = localStorage.getItem('selectedCharacterHead');
    const selectedCharacterBody = localStorage.getItem('selectedCharacterBody');
    const selectedInstrumentText = document.getElementById('selectedInstrumentText');
    const selectedCharacterText = document.getElementById('selectedCharacterText');
    console.log("selected instrument is " + selectedInstrument); 
    console.log("selected character head is " + selectedCharacterHead); 
    console.log("selected character body is " + selectedCharacterBody); 
    selectedInstrumentText.textContent = selectedInstrument || 'No instrument selected';
    selectedCharacterText.textContent = selectedCharacter || 'No character selected';
    
    let lastClickedModifier = '';
    
    



    
    const selectedData = {
        instrument: localStorage.getItem('selectedInstrument') || '',
        characterHead: localStorage.getItem('selectedCharacterHead') || '',
        characterBody: localStorage.getItem('selectedCharacterBody') || '',
        modifier : '',
    };

    const instrumentButtonsContainer = document.getElementById('instrumentButtons');
    function createButtonsForInstrument(instrument) {
        const buttonsHTML = [];
        const buttonCount = 3;

        const instrumentValues = [
            'High Pitch',   
            'Medium Pitch', 
            'Low Pitch'  
        ];

        for (let i = 1; i <= buttonCount; i++) {
            buttonsHTML.push(`
            <button type="button" class="btn btn-primary" data-value=${instrumentValues[i-1]}>${instrument} ${instrumentValues[i - 1]} Button ${i}</button>
            `);
        }

        return buttonsHTML.join('');
    }
    let buttonsHTML = '';

    if (selectedInstrument === 'Guitar') {
        buttonsHTML = createButtonsForInstrument('Guitar');
    } else if (selectedInstrument === 'Piano') {
        buttonsHTML = createButtonsForInstrument('Piano');
    } else if (selectedInstrument === 'Drums') {
        buttonsHTML = createButtonsForInstrument('Drums');
    }

    instrumentButtonsContainer.innerHTML = buttonsHTML;


    const instrumentButtons = document.querySelectorAll('.btn-primary');
    instrumentButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            console.log("pressed0");
            const value = button.getAttribute('data-value');
            lastClickedModifier = value; 
        });
    });

// Send the form data to the server
    document.getElementById('myForm').addEventListener('submit', function (event) {
        event.preventDefault(); 
        console.log(lastClickedModifier);
        selectedData.modifier = lastClickedModifier;
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
document.addEventListener('DOMContentLoaded', function () {
    const selectedData = {
        character: '',
        parameter: '',
        modifier: 1.0,
    };
    let lastClickedModifier = '';

    function createButtonsForInstrument(instrument) {
        const buttonsHTML = [];
        const buttonCount = 3;

        const instrumentValues = ['High Pitch', 'Medium Pitch', 'Low Pitch'];

        for (let i = 1; i <= buttonCount; i++) {
            buttonsHTML.push(`
                <button type="button" class="btn btn-primary" data-value="${instrumentValues[i - 1]}">
                    ${instrument} ${instrumentValues[i - 1]} Button ${i}
                </button>
            `);
        }

        return buttonsHTML.join('');
    }

    const urlParams = new URLSearchParams(window.location.search);

    let buttonsHTML = '';




    const instrumentButtons = document.querySelectorAll('.btn-primary');
    instrumentButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            const value = button.getAttribute('data-value');
            lastClickedModifier = value;
        });
    });

    document.querySelectorAll('.instrument').forEach(button => {
        button.addEventListener('click', function (event) {
            const value = button.getAttribute('data-field');
            selectedData.parameter = value;
            window.location.href='../Character.html';
        });
    });

    document.getElementById('myForm').addEventListener('submit', function (event) {
        event.preventDefault();
        selectedData.modifier = lastClickedModifier;

        fetch('http://localhost:8000/queueMusicalCharacter', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(selectedData)
        })
            .then(response => response.json())
            .then(data => {
                localStorage.setItem('UID', data.UID);
                localStorage.setItem('queueSize', data.queueSize);
                document.getElementById('myForm').reset();
            })
            .catch(error => {
                console.error('Error:', error);
            });
    });
});

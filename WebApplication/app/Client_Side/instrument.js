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
            localStorage.setItem('selectedInstrument', value);
            window.location.href='/app/Character.html';
        });
    });

    
});

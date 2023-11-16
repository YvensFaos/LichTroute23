document.addEventListener('DOMContentLoaded', function () {
    const horizontalImages = document.querySelectorAll('.horizontal-image');

    horizontalImages.forEach(function (image) {
        image.addEventListener('click', function () {
            const dataField = this.getAttribute('data-field');
            const selectedData = { 'horizontal-image': dataField };
            console.log("form submitted");
            fetch('http://localhost:8000/visualEffects', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(selectedData)
            })
            
                .then(response => response.json())
                .then(data => {
                })
                .catch(error => {
                    console.error('Error:', error);
                });
        });
    });
});

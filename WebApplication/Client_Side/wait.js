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

    document.querySelectorAll('.share-btn').forEach(function (shareBtn) {
        shareBtn.addEventListener('click', function() {
            const platform = this.getAttribute('data-platform');
            const urlToShare = 'https://lu23.saxionxrlab.com/';
            shareOnPlatform(platform, urlToShare);
        });
    });

    function shareOnPlatform(platform, url) {
        let shareUrl;

        switch (platform) {
            case 'facebook':
                shareUrl = 'https://www.facebook.com/sharer/sharer.php?u=' + encodeURIComponent(url);
                break;
            case 'twitter':
                shareUrl = 'https://twitter.com/intent/tweet?url=' + encodeURIComponent(url);
                break;
            case 'linkedin':
                shareUrl = 'https://www.linkedin.com/shareArticle?url=' + encodeURIComponent(url);
                break;
            default:
                alert('Unsupported platform');
                return;
        }

        window.open(shareUrl, '_blank', 'width=600,height=400');
    }
});

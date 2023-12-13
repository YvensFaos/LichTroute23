document.addEventListener('DOMContentLoaded', function () {
    const horizontalImages = document.querySelectorAll('.horizontal-image');

    horizontalImages.forEach(function (image, index) {
        image.addEventListener('click', function () {
            const dataField = this.getAttribute('data-field');
            const storedUID = localStorage.getItem('UID');
            if (!storedUID) {
                console.error('Error: UID not found in localStorage');
                return;
            }

            const action =
                dataField === 'cheering' ? 0 :
                dataField === 'clapping' ? 1 :
                dataField === 'dancing' ? 2 : 0;

            const selectedData = {
                UID: storedUID,
                action: action
            };
            const headers = new Headers();
            headers.append('Access-Control-Request-Method', 'POST');
            headers.append('Access-Control-Request-Headers', 'Content-Type');

            fetch('https://panfun.ngrok.io/animateCharacter', {
                method: 'POST',
                hedears: headers,
                body: JSON.stringify(selectedData)
            })
                .then(response => response.json())
                .then(data => {
                })
                .catch(error => {
                    
                });
        });
    });

    document.querySelectorAll('.share-btn').forEach(function (shareBtn) {
        shareBtn.addEventListener('click', function () {
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

const express = require('express');
const app = express();
const path = require('path');

// Serve static files from public
app.use(express.static(path.join(__dirname, 'public')));

// Serve JavaScript files with the correct MIME type
app.use(express.static('Client_Side', {
    setHeaders: (res, path, stat) => {
        if (path.endsWith('.js')) {
            res.setHeader('Content-Type', 'application/javascript');
        }
    },
}));

// Start the server
const port = 3000;
app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});

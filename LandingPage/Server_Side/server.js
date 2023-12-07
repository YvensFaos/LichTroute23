const express = require('express');
const bodyParser = require('body-parser');
const path = require('path');
const app = express();
const port = 3000;

app.use(bodyParser.json());

app.use(express.static(path.join(__dirname, 'public')));

app.get('/', (req, res) => {
    const indexPath = path.join(__dirname, 'LandingPage.html');
    res.sendFile(indexPath);
});

app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});

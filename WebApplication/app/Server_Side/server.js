const express = require('express');

const bodyParser = require('body-parser');
const path = require('path');
const app = express();
const port = 3000;

app.use(bodyParser.json());

// Serve static files from the 'Client_Side' directory
app.use('/Client_Side', express.static(path.join(__dirname, '../Client_Side')));

// Serve your HTML files
app.get('/', (req, res) => {
    const indexPath = path.join(__dirname, '../app/index.html');
    res.sendFile(indexPath);
});

app.get('/index.html', (req, res) => {
    const characterPath = path.join(__dirname, '../app/customize.html.html');
    res.sendFile(characterPath);
});

app.get('/characterSelect.html', (req, res) => {
    const characterPath = path.join(__dirname, '../app/characterSelect.html');
    res.sendFile(characterPath);
});

app.get('/Character.html', (req, res) => {
    const characterPath = path.join(__dirname, '../app/Character.html');
    res.sendFile(characterPath);
});

app.get('/name.html', (req, res) => {
    const namePath = path.join(__dirname, '../app/name.html');
    res.sendFile(namePath);
});

app.get('/instrument.html', (req, res) => {
    const namePath = path.join(__dirname, '../app/instrument.html');
    res.sendFile(namePath);
});

app.get('/wait.html', (req, res) => {
    const namePath = path.join(__dirname, '../app/wait.html');
    res.sendFile(namePath);
});

app.get('/confirmation.html', (req, res) => {
    const namePath = path.join(__dirname, '../app/confirmation.html');
    res.sendFile(namePath);
});

app.use('/app/public', express.static(path.join(__dirname, '../app/public')));

app.post('/submit', (req, res) => {
    const formData = req.body;
    console.log(formData);
    const instrument = formData.instrument;
    const character = formData.character;
    res.json({ message: 'Form data received successfully' });
});

app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});

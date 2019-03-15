let reseau = require('./reseau-ferre.json');

// couleurs à appliquer
const COLORSMAP = {
    'M1': '#FFCD00',
    'M2': '#003CA6',
    'M3': '#837902',
    'M3bis': '#6EC4E8',
    'M4': '#CF009E',
    'M5': '#FF7E2E',
    'M6': '#6ECA97',
    'M7': '#FA9ABA',
    'M7bis': '#6ECA97',
    'M8': '#E19BDF',
    'M9': '#B6BD00',
    'M10': '#C9910D',
    'M11': '#704B1C',
    'M12': '#007852',
    'M13': '#6EC4E8',
    'M14': '#62259D',

    'FUNICULAIRE MONTMARTRE' : '#000A82',
    'ORLYVAL' : '#0055C8',
    'CDGVAL': '#B6077E',

    'TER': '#034C9D',

    'T1': '#003CA6',
    'T2': '#CF009E',
    'T3A': '#FF7E2E',
    'T3B': '#00AE41',
    'T4': '#FED100',
    'T5': '#62259D',
    'T6': '#E2231A',
    'T7': '#704B1C',
    'T8': '#837902',
    'T11': '#FB4F14',

    'RER A': '#F7403A',
    'RER B': '#4B92DB',
    'RER C': '#F3D311',
    'RER D': '#3F9C35',
    'RER E': '#DE81D3',

    'LIGNE J': '#B6BF00',
    'LIGNE L': '#7577C0',
    'LIGNE H': '#844C54',
    'LIGNE K': '#AE9A00',
    'LIGNE R': '#E59FDB',
    'LIGNE P': '#EAAB00',
    'LIGNE N': '#00B092',
    'LIGNE U': '#C90062'
}

// ajout de la couleur
reseau.features = reseau.features.map(f => {
    f.properties.couleur = COLORSMAP[f.properties.res_com];
    return f;
});

const json = JSON.stringify(reseau);
const fs = require('fs');
fs.writeFile('reseau.json', json, 'utf8', error => console.log);

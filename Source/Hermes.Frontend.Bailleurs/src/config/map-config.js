import L from 'leaflet';

export const LOGEMENT_ICONS = {
  1: L.icon({
    iconUrl: 'images/marker-icon.png',
    shadowUrl: 'images/marker-shadow.png',
    iconSize: [24, 42],
    iconAnchor: [12, 42],
    popupAnchor: [0, -42]
  }),
  2: L.icon({
    iconUrl: 'images/marker-icon2.png',
    shadowUrl: 'images/marker-shadow.png',
    iconSize: [24, 42],
    iconAnchor: [12, 42],
    popupAnchor: [0, -42]
  })
};

const TYPE_RESEAU_STYLES = {
  'TER': { opacity: 0, weight: 3, dashArray: '' },
  'Train': { opacity: 0.7, weight: 7, dashArray: '4 2' },
  'Metro': { opacity: 1, weight: 4, dashArray: '' },
  'RER': { opacity: 0.9, weight: 7, dashArray: '' },
  'Navette': { opacity: 1, weight: 3, dashArray: '' },
  'Tramway': { opacity: 1, weight: 3, dashArray: '6 10' }
};

const TYPE_RESEAU_DEFAULT_STYLE = {
  opacity: 0.9,
  weight: 3,
  dashArray: ''
};

export const applyLigneStyle = feature => ({ color: feature.properties.couleur, ...(TYPE_RESEAU_STYLES[feature.properties.mode] || TYPE_RESEAU_DEFAULT_STYLE) });

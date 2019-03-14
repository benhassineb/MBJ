import L from 'leaflet';

const LOGEMENTICONS = {
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

const LIGNESTYLES = {
  RER: {
    opacity: 1.0,
    weight: 8,
    dashArray: ''
  },
  Train: {
    opacity: 1.0,
    weight: 5,
    dashArray: '10 10'
  },
  Tramway: {
    opacity: 1.0,
    weight: 3,
    dashArray: '8 6'
  },
  Metro: {
    opacity: 1.0,
    weight: 5,
    dashArray: ''
  }
};

const DEFAULTLIGNESTYLE = {
  opacity: 1.0,
  weight: 3,
  dashArray: ''
};

export { LOGEMENTICONS, LIGNESTYLES, DEFAULTLIGNESTYLE };

import { inject, DOM, bindable, bindingMode, BindingEngine } from 'aurelia-framework';
import { HttpClient } from 'aurelia-fetch-client';
import L from 'leaflet';

import { LOGEMENT_ICONS, applyLigneStyle } from 'config/map-config';
@inject(DOM.Element, HttpClient, BindingEngine)
export class MapCustomElement {

  @bindable({ defaultBindingMode: bindingMode.oneWay })
  logements = [];

  @bindable({ defaultBindingMode: bindingMode.oneWay })
  communes = [];

  @bindable({ defaultBindingMode: bindingMode.oneWay })
  reseauferre = [];

  constructor(element, httpClient, bindingEngine) {
    this._element = element;
    this._bindingEngine = bindingEngine;
    this._logementsSubscription = this._bindingEngine.propertyObserver(this, 'logements').subscribe(this.logementsChanged.bind(this));
    this._communesSubscription = this._bindingEngine.propertyObserver(this, 'communes').subscribe(this.communesChanged.bind(this));
    this._reseauferreSubscription = this._bindingEngine.propertyObserver(this, 'reseauferre').subscribe(this.reseauferreChanged.bind(this));
    this._logementsLayer = null;
    this._communesLayer = null;
    this._reseauferreLayer = null;
  }

  attached() {
    this._map = L.map('map', {
      center: [48.912863, 2.329724],
      zoom: 15,
      maxZoom: 18,
      minZoom: 10,
      zoomSnap: 0,
      zoomDelta: 0.2,
      wheelPxPerZoomLevel: 250
    });
    L.tileLayer('https://{s}.tile.openstreetmap.fr/osmfr/{z}/{x}/{y}.png', {
      attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(this._map);
    if (this._logementsLayer) this._logementsLayer.addTo(this._map);
    if (this._communesLayer) this._communesLayer.addTo(this._map);
    if (this._reseauferreLayer) this._reseauferreLayer.addTo(this._map);
  }

  logementsChanged(newValue, oldValue) {
    this._logementsLayer = L.geoJson(newValue, {
      pointToLayer: (feature, latlng) => {
        let logementProperties = feature.properties;
        let logementType = logementProperties.type;
        let logementIcon = LOGEMENT_ICONS[logementType];
        let rank = logementProperties.rank || '';
        return L.marker(latlng, { icon: logementIcon })
          .bindPopup(logementProperties.name)
          .bindTooltip(`${rank}`, {
            className: 'marker-number',
            direction: 'left',
            permanent: true
          });
      }
    });
  }

  communesChanged(newValue, oldValue) {
    this._communesLayer = L.geoJson(newValue);
  }

  reseauferreChanged(newValue, oldValue) {
    this._reseauferreLayer = L.geoJson(newValue, {
      style: (feature) => applyLigneStyle(feature)
    });
  }

  detached() {
    this._logementsSubscription.dispose();
    this._communesSubscription.dispose();
    this._reseauferreSubscription.dispose();
  }

}

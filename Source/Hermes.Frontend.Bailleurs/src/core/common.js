import { OpenIdConnectRoles } from 'aurelia-open-id-connect';

/**
 * Common functions module.
 * @module
 */

export const isNumeric = n => !isNaN(parseFloat(n)) && isFinite(n);

export const wait = ms => new Promise(callback => setTimeout(callback, ms));

export const delay = callback => setTimeout(callback, 10);

export const nop = () => {};

export const identity = (x) => x;

export const falsy = () => false;

export const truthy = () => true;

export const isNullOrEmpty = (value) => !value || value === null || value === '';

export const handleApiError = (error) => {
  // if (error.status !== 404 && error.status !== 403) {
  //   showError(error);
  // }
  return Promise.reject(new Error('HTTP Error :' + error.status));
};

const f = (k, p) => (p !== undefined) ? `${k}/:${p}` : `${k}`;

/**
 * Construire les routes Aurelia
 * @param {Object} sitemap -La liste des routes à construire
 * @param {Object} parentView -Le parent des routes à construire
 * @return {Object} la liste des routes Aurelia
 */
export const toRoutes = (sitemap) => Object.keys(sitemap).map(k => ({
  route: (sitemap[k].isDefault) ? ['', f(k, sitemap[k].parameter)] : f(k, sitemap[k].parameter),
  moduleId: `views/${k}`,
  nav: (sitemap[k].parameter === undefined),
  name: k,
  title: sitemap[k].title,
  //activationStrategy: 'invoke-lifecycle',
  settings: {
    roles: (sitemap[k].isAnonymous) ? [OpenIdConnectRoles.Anonymous] : [OpenIdConnectRoles.Authenticated]
  }
}));

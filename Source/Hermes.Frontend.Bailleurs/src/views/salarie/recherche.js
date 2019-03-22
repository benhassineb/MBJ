import { inject } from 'aurelia-framework';
import { Auth } from './auth';

@inject(Auth)
export class Rechercher {
  logements = '';
  accommodations;
  constructor(auth) {
    this.logements = this.getLogements();
    this.accommodations = this.getAccommodations();
    this.auth = auth;
    //alert(this.auth.isLoggedIn);
  }

  getLogements() {
    return '9 logements';
  }

  getAccommodations() {
    return [
      { 'commune': 'Versailles', 'nbrPiece': '1', 'superficie': '21', 'price': '750', 'image': 'images/apt1.jpg', 'points': '98' },
      { 'commune': 'Versailles', 'nbrPiece': '2', 'superficie': '31', 'price': '750', 'image': 'images/apt2.jpg', 'points': '90' },
      { 'commune': 'Versailles', 'nbrPiece': '2', 'superficie': '37', 'price': '750', 'image': 'images/apt4.jpg', 'points': '80' },
      { 'commune': 'Versailles', 'nbrPiece': '3', 'superficie': '45', 'price': '750', 'image': 'images/apt5.jpg', 'points': '60' },
      { 'commune': 'Versailles', 'nbrPiece': '1', 'superficie': '21', 'price': '750', 'image': 'images/apt2.jpg', 'points': '55' },
      { 'commune': 'Versailles', 'nbrPiece': '2', 'superficie': '31', 'price': '750', 'image': 'images/apt1.jpg', 'points': '52' },
      { 'commune': 'Versailles', 'nbrPiece': '1', 'superficie': '17', 'price': '750', 'image': 'images/apt2.jpg', 'points': '42' },
      { 'commune': 'Versailles', 'nbrPiece': '2', 'superficie': '36', 'price': '750', 'image': 'images/apt3.jpg', 'points': '35' },
      { 'commune': 'Versailles', 'nbrPiece': '1', 'superficie': '19', 'price': '750', 'image': 'images/apt5.jpg', 'points': '20' }
    ];
  }

  getStyleColor(points) {
    if (points < 50) {
      return 'border red-border';
    } else if (points < 70) {
      return 'border orange-border';
    }
    return 'border green-border';
  }

}

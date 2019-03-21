export class SaisieDomicileActuel {
  label = 'Domicile Actuel';
  constructor() {
    this.entreprise = 'Orange';
  }
  getModeLogement() {
    return [{key: '1', value: 'Française'}, {key: '2', value: 'Union Européenne'}, {key: '3', value: 'Hors Union Européenne'}];
  }
  getTypeLogement() {
    return [
      {key: '1', value: 'Célibataire'}, {key: '2', value: 'Concubin'}, {key: '3', value: 'PACS'},
      {key: '4', value: 'Marié(e)'}, {key: '5', value: 'Divorcé(e)'}, {key: '6', value: 'Séparé(e)'},
      {key: '7', value: 'Veuf(ve)'}
    ];
  }
  getCategorie() {
    return [
      {key: '1', value: 'Conjoint'}, {key: '2', value: 'Pacsé(e)'}, {key: '3', value: 'Concubin(e)'},{key: '4', value: 'co-locataire'}
    ]
  }
  getAttributes() {
    return [
      {
        bloc: 'Adresse du logement',
        content: [
          {label: 'Adresse à l\'étranger', type: 'checkbox', name: 'adresseEtranger', value: '', tailleBloc: '12'},
          {label: 'Numéro', type: 'text', name: 'adresseNum', value: '', tailleBloc: '4'},
          {label: 'Voie', type: 'text', name: 'adresseVoie', value: '', tailleBloc: '4'},
          {label: 'Cplmt. Adr.', type: 'text', name: 'adresseCplmt', values: '', tailleBloc: '4'},
          {label: 'Code Postal', type: 'text', name: 'adresseCP', value: '', tailleBloc: '4'},
          {label: 'Localité', type: 'text', name: 'adresseLocalite', value: '', tailleBloc: '4'},
          {label: 'Lieu-dit', type: 'text', name: 'adresseLieu', value: '', tailleBloc: '4'},
          {label: 'Batiment', type: 'text', name: 'adresseBat', value: '', tailleBloc: '4'},
          {label: 'Escalier', type: 'text', name: 'adresseEscl', value: '', tailleBloc: '4'},
          {label: 'Etage', type: 'text', name: 'adresseEtage', value: '', tailleBloc: '2'},
          {label: 'Appartement', type: 'text', name: 'adresseApt', value: '', tailleBloc: '2'},
          {label: 'Nom personne ou structure hébergeante', type: 'text', name: 'adresseHeberg', value: '', tailleBloc: '8'}
        ]
      },
      {
        bloc: 'Adresse du courrier (si différente du logement actuel)',
        content: [
          {label: 'Adresse à l\'étranger', type: 'checkbox', name: 'adresseCEtranger', value: '', tailleBloc: '12'},
          {label: 'Numéro', type: 'text', name: 'adresseCNum', value: '', tailleBloc: '4'},
          {label: 'Voie', type: 'text', name: 'adresseCVoie', value: '', tailleBloc: '4'},
          {label: 'Cplmt. Adr.', type: 'text', name: 'adresseCCplmt', values: '', tailleBloc: '4'},
          {label: 'Code Postal', type: 'text', name: 'adresseCCP', value: '', tailleBloc: '4'},
          {label: 'Localité', type: 'text', name: 'adresseCLocalite', value: '', tailleBloc: '4'},
          {label: 'Lieu-dit', type: 'text', name: 'adresseCLieu', value: '', tailleBloc: '4'},
          {label: 'Batiment', type: 'text', name: 'adresseCBat', value: '', tailleBloc: '4'},
          {label: 'Escalier', type: 'text', name: 'adresseCEscl', value: '', tailleBloc: '4'},
          {label: 'Etage', type: 'text', name: 'adresseCEtage', value: '', tailleBloc: '2'},
          {label: 'Appartement', type: 'text', name: 'adresseCApt', value: '', tailleBloc: '2'},
          {label: 'Nom personne ou structure hébergeante', type: 'text', name: 'adresseCHeberg', value: '', tailleBloc: '8'}
        ]
      }, {
        bloc: 'Adresse du co-titulaire',
        content: [
          {label: 'Adresse à l\'étranger', type: 'checkbox', name: 'adresseTEtranger', value: '', tailleBloc: '12'},
          {label: 'Numéro', type: 'text', name: 'adresseTNum', value: '', tailleBloc: '4'},
          {label: 'Voie', type: 'text', name: 'adresseTVoie', value: '', tailleBloc: '4'},
          {label: 'Cplmt. Adr.', type: 'text', name: 'adresseTCplmt', values: '', tailleBloc: '4'},
          {label: 'Code Postal', type: 'text', name: 'adresseTCP', value: '', tailleBloc: '4'},
          {label: 'Localité', type: 'text', name: 'adresseTLocalite', value: '', tailleBloc: '4'},
          {label: 'Lieu-dit', type: 'text', name: 'adresseTLieu', value: '', tailleBloc: '4'},
          {label: 'Batiment', type: 'text', name: 'adresseTBat', value: '', tailleBloc: '4'},
          {label: 'Escalier', type: 'text', name: 'adresseTEscl', value: '', tailleBloc: '4'},
          {label: 'Etage', type: 'text', name: 'adresseTEtage', value: '', tailleBloc: '2'},
          {label: 'Appartement', type: 'text', name: 'adresseTApt', value: '', tailleBloc: '2'},
          {label: 'Nom personne ou structure hébergeante', type: 'text', name: 'adresseTHeberg', value: '', tailleBloc: '8'}
        ]
      }, {
        bloc: 'Logement actuel',
        content: [
          {label: 'Mode de logement', type: 'select', name: 'modeLogement', values: this.getModeLogement(), tailleBloc: '9'},
          {label: 'Type de logement', type: 'select', name: 'typeLogement', values: this.getTypeLogement(), tailleBloc: '4'},
          {label: 'Catégorie', type: 'select', name: 'categorie', values: this.getCategorie(), tailleBloc: '4'},
          {label: 'Surface', type: 'text', name: 'surface', value: '', tailleBloc: '4'},
          {label: 'Loyer mensuel avec charges', type: 'text', name: 'loyer', value: '', tailleBloc: '4'},
          {label: 'Date d\'entrée dans les lieux', type: 'text', name: 'dateEntre', value: '', tailleBloc: '4'},
          {label: 'Temps de transport', type: 'text', name: 'temps', value: '', tailleBloc: '4'},
          {label: 'Distance du lieu de travail', type: 'text', name: 'distance', value: '', tailleBloc: '4'},
          {label: 'Nom du bailleur', type: 'text', name: 'nomBailleur', value: '', tailleBloc: '4'},
          {label: 'Code postal bailleur', type: 'text', name: 'cpBailleur', value: '', tailleBloc: '4'},
          {label: 'Localité bailleur', type: 'text', name: 'localiteBailleur', value: '', tailleBloc: '4'},
          {label: 'N de SIREN de l\'organisme bailleur', type: 'text', name: 'siren', value: '', tailleBloc: '12'},
          {label: 'Combien de personnes habitent dans le logement actuel', type: 'text', name: 'nbrPers', value: '', tailleBloc: '12'},
          {label: 'Si vous percevez l\'AL ou l\'APL, montant actuel', type: 'text', name: 'alApl', value: '', tailleBloc: '12'},
          {label: 'Etes-vous propriétaire d\'un logement autre que celui que vous habitez', type: 'checkbox', name: 'proprio', value: '', tailleBloc: '12'}
        ]
      }
    ];
  }
}

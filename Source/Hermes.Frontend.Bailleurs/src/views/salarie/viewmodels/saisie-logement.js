export class SaisieLogement {
  label = 'Logement recherché';
  constructor() {
    this.entreprise = 'Orange';
  }
  getCP(entreprise) {
    return [{key: '1', value: 'Mr Monsieur'}, {key: '2', value: 'Mme Madame'}];
  }
  getCiv() {
    return [{key: '1', value: 'Mr'}, {key: '2', value: 'Mme'}];
  }
  getNationalite() {
    return [{key: '1', value: 'Française'}, {key: '2', value: 'Union Européenne'}, {key: '3', value: 'Hors Union Européenne'}];
  }
  getSituation() {
    return [
      {key: '1', value: 'Célibataire'}, {key: '2', value: 'Concubin'}, {key: '3', value: 'PACS'},
      {key: '4', value: 'Marié(e)'}, {key: '5', value: 'Divorcé(e)'}, {key: '6', value: 'Séparé(e)'},
      {key: '7', value: 'Veuf(ve)'}
    ];
  }
  getLienCotitulaire() {
    return [
      {key: '1', value: 'Conjoint'}, {key: '2', value: 'Pacsé(e)'}, {key: '3', value: 'Concubin(e)'},{key: '4', value: 'co-locataire'}
    ]
  }
  getAttributes() {
    return [
      {
        bloc: 'Motif de votre demande',
        content: [
          {label: 'Nom de l\'entreprise', type: 'text', name: 'entreprise', value: '', tailleBloc: '4'},
          {label: 'Priorite entreprise', type: 'checkbox', name: 'priorite', value: '', tailleBloc: '4'},
          {label: 'N unique dept', type: 'text', name: 'nud', value: '', tailleBloc: '4'},
          {label: 'Correspondant Action Logement', type: 'select', name: 'cp', values: this.getCP(this.entreprise), tailleBloc: '4'}
        ]
      }, {
        bloc: 'Logement recherché',
        content: [
          {label: 'Nom de l\'entreprise', type: 'text', name: 'entreprise', value: '', tailleBloc: '4'},
          {label: 'Priorite entreprise', type: 'checkbox', name: 'priorite', value: '', tailleBloc: '4'},
          {label: 'N unique dept', type: 'text', name: 'nud', value: '', tailleBloc: '4'},
          {label: 'Correspondant Action Logement', type: 'select', name: 'cp', values: this.getCP(this.entreprise), tailleBloc: '4'}
        ]
      },{
        bloc: 'localisations souhaitées actives',
        content: [
          {label: 'Nom de l\'entreprise', type: 'text', name: 'entreprise', value: '', tailleBloc: '4'},
          {label: 'Priorite entreprise', type: 'checkbox', name: 'priorite', value: '', tailleBloc: '4'},
          {label: 'N unique dept', type: 'text', name: 'nud', value: '', tailleBloc: '4'},
          {label: 'Correspondant Action Logement', type: 'select', name: 'cp', values: this.getCP(this.entreprise), tailleBloc: '4'}
        ]
      },{
        bloc: 'Localisations souhaitées inactives',
        content: [
          {label: 'Civilité', type: 'select', name: 'civilite', values: this.getCiv(), tailleBloc: '2'},
          {label: 'Nom', type: 'text', name: 'nom', value: '', tailleBloc: '4'},
          {label: 'Prénom', type: 'text', name: 'prenom', value: '', tailleBloc: '4'},
          {label: 'Date de naissance', type: 'text', name: 'dateNaissance', value: '', tailleBloc: '4'},
          {label: 'Nationalité', type: 'select', name: 'nationalite', values: this.getNationalite(), tailleBloc: '4'},
          {label: 'Téléphone portable', type: 'text', name: 'telPort', value: '', tailleBloc: '4'},
          {label: 'Téléphone domicile', type: 'text', name: 'telDom', value: '', tailleBloc: '4'},
          {label: 'Téléphone professionnel', type: 'text', name: 'telPro', value: '', tailleBloc: '4'},
          {label: 'Fax', type: 'text', name: 'fax', value: '', tailleBloc: '4'},
          {label: 'Courriel professionnel', type: 'text', name: 'courrielPro', value: '', tailleBloc: '4'},
          {label: 'Courriel personnel', type: 'text', name: 'courrielPerso', value: '', tailleBloc: '4'},
          {label: 'Autre courriel', type: 'text', name: 'courrielAutre', value: '', tailleBloc: '4'},
          {label: 'Situation familiale', type: 'select', name: 'situation', values: this.getSituation(), tailleBloc: '4'},
          {label: 'Depuis le', type: 'text', name: 'situationDepuis', value: '', tailleBloc: '4'},
          {label: 'Renouvellement numéro unique (SNE) par voir émectronique', type: 'text', name: 'renouvellement', value: '', tailleBloc: '12'},
          {label: 'Co-titulaire', type: 'checkbox', name: 'cotitulaire', value: '', tailleBloc: '4'},
          {label: 'Lien avec le Co-titulaire', type: 'select', name: 'lienCotitulaire', values: this.getLienCotitulaire(), tailleBloc: '4'},
          {label: 'Courriel d\'une personne ou structure vous aidant dans les démarches', type: 'text', name: 'courrielDemarche', value: '', tailleBloc: '4'},
          {label: 'Notification (SNE) par SMS', type: 'text', name: 'notif', value: '', tailleBloc: '4'}
        ]
      }
    ];
  }
}
  
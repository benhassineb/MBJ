export class SaisieSituationFamiliale {
  label = 'Situation Familiale';
  constructor() {
    this.entreprise = 'Orange';
  }
  getSexe() {
    return [{key: '1', value: 'Masculin'}, {key: '2', value: 'Mme Madame'}];
  }
  getLien() {
    return [{key: '1', value: 'Parent'}, {key: '2', value: 'Enfant'}, {key: '3', value: 'Autre'}];
  }
  getAttributes() {
    return [
      {
        bloc: 'Composition Familiale',
        content: [
          {label: 'Nom', type: 'text', name: 'nom', value: '', tailleBloc: '4'},
          {label: 'Prénom', type: 'text', name: 'prenom', value: '', tailleBloc: '4'},
          {label: 'Date de naissance', type: 'text', name: 'dateNaiss', value: '', tailleBloc: '4'},
          {label: 'Sexe', type: 'select', name: 'sexe', values: this.getSexe(), tailleBloc: '4'},
          {label: 'Lien de parenté', type: 'select', name: 'parente', values: this.getLien(), tailleBloc: '4'},
          {label: 'Garde Alternée', type: 'checkbox', name: 'garde', values: '', tailleBloc: '2'},
          {label: 'Droit de visite', type: 'select', name: 'droit', values: '', tailleBloc: '2'},
          {label: 'Si une naissance est attendue, nombre d\'enfants à naitre', type: 'text', name: 'nbrEnfant', values: '', tailleBloc: '8'},
          {label: 'Date de naissance prévue', type: 'text', name: 'dateNaissance', values: '', tailleBloc: '4'},
          {label: 'Nombre occupants', type: 'text', name: 'nbrocc', values: '', tailleBloc: '4'},
          {label: 'Nombre personnes à charge', type: 'text', name: 'nbrCharge', values: '', tailleBloc: '4'},
          {label: 'Nombre personnes actives', type: 'text', name: 'nbrActive', values: '', tailleBloc: '4'}
        ]
      }
    ];
  }
}


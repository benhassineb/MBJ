CREATE PROCEDURE  [HABILITATION].[GetUtilisateurDroits](
   @Email varchar(100),
   @c_fonc VARCHAR (15))
AS 
BEGIN
    
SELECT 
    droits.[I_PROFIL],
    droits.[I_FONC],
    droits.[B_CONSULT],
    droits.[B_CREATE],
    droits.[B_UPDATE],
    droits.[B_DELETE],
    droits.[B_EXEC],
    droits.[C_SPEC]
    FROM [HABILITATION].[HAB_DROITS] droits
    INNER JOIN 	[HABILITATION].[HAB_PROFIL] profil ON profil.[I_PROFIL] = droits.[I_PROFIL]
    INNER JOIN 	[HABILITATION].[HAB_HABILITATION] hab ON hab.[I_PROFIL] = profil.[I_PROFIL]
    INNER JOIN 	[HABILITATION].[HAB_USER] u ON u.[C_USER] = hab.[C_USER]
    INNER JOIN 	[HABILITATION].[HAB_FONCTIONNALITE] fonc ON fonc.[I_FONC] = droits.[I_FONC]
    LEFT JOIN HABILITATION.HAB_USER_MEMBER UM ON UM.I_TIEPHY = U.I_TIEPHY
    WHERE (u.[L_EMAIL] = @Email or UM.L_EMAIL = @Email)
    AND fonc.[C_FONC] = @c_fonc
END
GO;

GRANT EXECUTE ON OBJECT::[HABILITATION].[GetUtilisateurDroits] TO [AuthorizationReader];
GO;
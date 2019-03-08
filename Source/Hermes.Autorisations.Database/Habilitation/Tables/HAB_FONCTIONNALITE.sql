﻿CREATE TABLE [HABILITATION].[HAB_FONCTIONNALITE] (
    [I_FONC] NUMERIC (8)   NOT NULL,
    [L_FONC] VARCHAR (100) NOT NULL,
    [C_ETAT] CHAR (1)      NOT NULL,
    [D_DEB]  DATETIME      NULL,
    [D_FIN]  DATETIME      NULL,
    [C_FONC] VARCHAR (15)  NULL,
    CONSTRAINT [PK_T_FONCTIONNALITE] PRIMARY KEY CLUSTERED ([I_FONC] ASC)
)
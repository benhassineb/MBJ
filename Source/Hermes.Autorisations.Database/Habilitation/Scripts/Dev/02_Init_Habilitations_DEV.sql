﻿INSERT [HABILITATION].[HAB_USER] ([C_USER], [C_PWD], [I_TIEPHY], [L_NOM], [L_PRENOM], [L_EMAIL], [C_ETAT], [D_DEB], [D_FIN], [N_ETP]) 
    VALUES (N'1', N'', 1, N'CARITEY', N'Vincent', N'vincent.caritey@outlook.fr', N'A',N'2019-01-01T00:00:00.000', NULL, 1)

INSERT [HABILITATION].[HAB_HABILITATION] ([C_USER], [I_PROFIL]) 
    VALUES (N'1', 1)
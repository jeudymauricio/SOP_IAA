SET IDENTITY_INSERT persona ON   -- Apaga el auto incremento Identity
GO
insert into persona (id, nombre, apellido1, apellido2, cedula) values 
	(1, 'Nikzon', 'Espinoza', 'Matamonos', '203510255'),
	(2, 'Javier', 'G�mez', 'Jara', '203140255'),
	(3, 'Esteban', 'Coto', 'Corrales', '203510255'),
	(4, 'Jonathan', 'Granados', '-', '208510255'),
	(5, 'Jos� Antonio', 'Araya', '-', '201310255'),
	(6, 'Edgar', 'May', 'Cantillano', '203020255'),
	(7 ,'Ileana', 'Aguilar', 'Aguilar', '103510255'),
	(8, 'Cristian', 'Vargas', 'Calvo', '603510255')
SET IDENTITY_INSERT persona OFF		-- Enciende el auto incremento Identity
GO

-------------------------------------------------------------------------------
insert into ingeniero(idPersona,rol,descripcion,departamento) values
	('2','Unidad Supervisora de proyecto','Organismo de Inspecci�n Zona 6-1, San Carlos Este.','Ileana Aguilar. Ingenier�a y Administraci�n S.A'),
	('3','Ingeniero Responsable CONAVI','Ingeniero a cargo de la Zona 6-1, San Carlos Este.','Gerencia de Conservaci�n de V�as y Puentes'),
	('4','Ingeniero responsable de la empresa','Ingeniero Residente Zona 6-1, San Carlos Este.','Constructora MECO S.A.'),
	('5','Director Regional Region Huetar Norte','Director Regional, Regi�n Huetar Norte','Gerencia de Conservaci�n de V�as y Puentes'),
	('6','Gerente de Conservaci�n de V�as y Puentes','Gerente a.i. Conservaci�n de V�as y Puentes','Gerencia de Conservaci�n de V�as y Puentes'),
	('7','Unidad Supervisora de proyecto','Organismo de Inspecci�n Zona 6-1, San Carlos Este.','Ileana Aguilar. Ingenier�a y Administraci�n S.A'),
	('8','Gerente de Conservaci�n de V�as y Puentes','Gerente a.i. Conservaci�n de V�as y Puentes','Gerencia de Conservaci�n de V�as y Puentes')
go

-------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].laboratorioCalidad ON   -- Apaga el auto incremento Identity
GO
insert into laboratorioCalidad(id, nombre, tipo) values
	(1, 'CACISA','Vericicaci�n'),
	(2, 'ITP','Autocontrol')
go
SET IDENTITY_INSERT [dbo].laboratorioCalidad OFF   -- Enciende el auto incremento Identity
GO

------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].zona ON   -- Apaga el auto incremento Identity
GO
insert into zona(id, nombre) Values
	(1, '6-1'),
	(2, '6-2'),
	(3, '6-3'),
	(4, '6-4')
go
SET IDENTITY_INSERT [dbo].zona OFF   -- Enciende el auto incremento Identity
GO

-------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].fondo ON   -- Apaga el auto incremento Identity
GO
insert into fondo(id, nombre) values
	(1, 'Vial')
go
SET IDENTITY_INSERT [dbo].fondo OFF   -- Enciende el auto incremento Identity
GO

-------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].contratista ON   -- Apaga el auto incremento Identity
GO
insert into contratista(id, nombre, descripcion) values
	(1, 'Constructora MECO S.A.','San Carlos'),
	(2, 'Constructora Herrera','San Carlos')
go
SET IDENTITY_INSERT [dbo].contratista OFF   -- Enciende el auto incremento Identity
GO

-------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].Contrato ON   -- Apaga el auto incremento Identity
GO
insert into Contrato(id, licitacion, idContratista,lugar,lineaContrato,idZona,fechaInicio, plazo, idFondo) values
	(1, 'LP N�2009-000003-CV','1','CONSERVACI�N VIAL DE LA RED VIAL NACIONAL PAVIMENTADA DE LA ZONA 6-1, SAN CARLOS ESTE.','11','1','01/09/2011','1095','1'),
	(2, 'LP N�2010-000001-CV','1','CONSERVACI�N VIAL DE LA RED VIAL NACIONAL PAVIMENTADA DE LA ZONA 6-2, SAN CARLOS ESTE.','11','1','01/09/2012','1095','1')
go
SET IDENTITY_INSERT [dbo].Contrato OFF   -- Enciende el auto incremento Identity
GO

-------------------------------------------------------------------------------
insert into ingenieroContrato(idIngeniero, idContrato, fechaInicio, fechaFin) values
	('3','1','2011-09-01','2014-08-30'),
	('2','1','2011-09-01','2014-08-30'),
	('4','1','2011-09-01','2014-08-30'),
	('5','1','2011-09-01','2014-08-30'),
	('6','1','2011-09-01','2014-08-30'),
	('5','2','2011-09-01','2015-08-30'),
	('6','2','2011-09-01','2015-08-30')
go

-------------------------------------------------------------------------------
insert into labCalidadContrato(idLabCalidad, idContrato) values
	('1','1'),
	('2','1'),
	('1','2'),
	('2','2')
go

SET IDENTITY_INSERT [dbo].ruta ON   -- Apaga el auto incremento Identity
GO
insert into ruta(id, nombre, descripcion) values 
	(1,'Ruta 1','-'),
	(2,'Ruta 2', '-'),
	(3,'Ruta 3','-'),
	(4,'Ruta 4','-'),
	(5,'Ruta 5','-'),
	(6,'Ruta 126','San Carlos'),
	(7,'Ruta 140','San Carlos'),
	(8,'Ruta 141','San Carlos'),	
	(9,'Ruta 708','San Carlos')
go
SET IDENTITY_INSERT [dbo].Contrato OFF   -- Enciende el auto incremento Identity
GO

insert into zonaRuta (idZona, idRuta) values
	(2,1),
	(2,2),
	(2,3),
	(2,4),
	(2,5),
	(1,6),
	(1,7),
	(1,8),
	(1,9)
go

--select * from item 
--insert para items
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M21(F)', 'Limpieza de tomas, cabezales y alcantarillas', 'u');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M21(E)', 'Limpieza de cunetas revestidas de manera manual', 'm3');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M22(A)', 'Remoci�n de derrumbes', 'm3');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M20(A)', 'Chapea derecho de v�a', 'm2');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M20(E)', 'Recolecci�n de basura', 'h');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M20(D)', 'Descuaje de �rboles por hora', 'h');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M21(G)', 'Conformaci�n de cunetas y espaldones', 'm2');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M41(A)', 'Bacheo con mezcla asf�ltica en caliente', 't');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M41(D)', 'Bacheo de urgencia', 't');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M42(B)', 'Perfilado de pavimentos', 'm2');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M45(A)','Pavimento bituminosos en caliente',' t ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M45(E)','Pavimento bituminosos en caliente con pol�meros',' t ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M40(A)','Levantamiento de tapas de pozos',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M46(B)','Demolici�n de losas',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M46(A)','Suministro y colocaci�n de concreto de MR 45 kg/cm2',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M46 (C )','Suministro y colocaci�n de acero para dovelas y barras de sujeci�n',' kg ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M43(D)','Sellado de juntas para losas recontruidas',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M43(C )','Ruteo y sellado de grietas',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M47(B)','Tratamiento bituminoso de preservaci�n tipo S-2',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('410(6)A','Lechada asf�ltica tipo slurry seal, graduaci�n A',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('410(6)B','Lechada asf�ltica tipo slurry seal, graduaci�n B',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('MP-50(A)','Brigada de limpieza de puentes',' h ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M30(A)','Reacondicionamiento de la calzada',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('308(1)','Cemento P�rtland ',' t ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('408(3)','Emulsi�n asf�ltica para imprimaci�n',' l ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('408(5)','Material de secado ',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('203(2)','Excavaci�n com�n',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('206(1)','Excavaci�n para estructuras',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('206(3)','Relleno para fundaci�n',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M-304(4)','Suministro, colocaci�n y compactaci�n de base de agregado triturado, Graduaci�n B ',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('602A(3)','Hormig�n ciclopeo ',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('602A(1)','Hormig�n estructural clase A de 225 kg/cm2',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('602A(5)','Hormig�n estructural clase X de 180 kg/cm2',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('603(21)3B','Tuber�a de Hormig�n clase III C-76 de 0,76 m para carreteras',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('603(21)3A','Tuber�a de Hormig�n clase III C-76 de 0,81 m para carreteras',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('603(21)3C','Tuber�a de Hormig�n clase III C-76 de 0,90 m para carreteras',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('603(21)3D','Tuber�a de Hormig�n clase III C-76 de 1,00 m para carreteras',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('603(21)3E','Tuber�a de Hormig�n clase III C-76 de 1,22 m para carreteras',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('603(21)3F','Tuber�a de Hormig�n clase III C-76 de 1,50 m para carreteras',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('603(21)3H','Tuber�a de Hormig�n clase III C-76 de 2,13 m para carreteras',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('707 (2)','Tuber�a corrugada de acero de 3,00 m para carreteras',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('605(20)','Relleno granular filtrante para subdrenaje franc�s',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('605(22)','Tela fibra sint�tica para subdrenaje franc�s',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M-609(2A)','Cuneta de hormig�n de cemento Portland',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('619C(3)B','Colchoneta de gaviones con revestimiento de PVC',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('619C(1)','Construcci�n de gavi�n convencional con revestimiento de PVC',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('619C(1)A1','Construcci�n de gavi�n tipo terramesh, 4m de cola',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('619C(1)A2','Construcci�n de gavi�n tipo terramesh, 5m de cola',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('619C(1)A3','Construcci�n de gavi�n tipo terramesh, 6m de cola',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('MP51(A)','Reparaci�n de baranda de concreto',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('606(5)B1','Suministro e instalaci�n de viga galvanizada para guardacamino',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('606(5)A1','Suministro e instalaci�n de postes para guardacamino',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('606(5)B2','Sustituci�n de viga galvanizada para guardacamino',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('606(5)A2','Sustituci�n de postes para guardacamino',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('622A(5)','Cauce revestido con toba cemento pl�stico',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('M-204(1)','Suministro, colocaci�n y compactaci�n de sub-base granular, Graduaci�n B',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('203(8)','Material de pr�stamo',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('608(1)','Aceras de Hormig�n',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('612(2)','Construcci�n de barandas de acero para puentes',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('609(01)','Construcci�n de cord�n de hormig�n',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('611(1)A','Construcci�n de pasarelas peatonales',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('613(1)A','Reparaci�n de adoquines',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('717(1)C','Acero estructural grado 40',' kg ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('609(4)','Bordillo de  hormig�n asf�ltico de 0,15m de altura',' m ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('609(8)','Bolardos de hormig�n reforzado',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('201(6)','Remoci�n selectiva de �rboles',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('202(1)A','Remoci�n de estructuras tipo cabezal o similares',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('R-1-2(A)','Limpieza y reparaci�n de se�alamiento vertical',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('726(1)','Suministro de se�ales para emergencias chevron',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('726(2)','Suministro de se�ales para emergencias Ceda',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('726(3)','Suministro de se�ales para emergencias Despacio',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('726(4)','Suministro de se�ales para emergencias V�a cerrada adelante',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('726(5)','Suministro de se�ales para emergencias Peligro',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('MP-620(3)','Suministro de roca de r�o',' m3 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('704(2)','Tela geotextil para repavimentaci�n',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)1','Dise�o de muros de retenci�n, tipo I',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)2','Dise�o de muros de retenci�n, tipo II',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)3','Dise�o de muros de retenci�n, tipo III',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)4','Dise�o de muros de retenci�n, tipo IV',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)5','Dise�o de muros de retenci�n, tipo V',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)6','Dise�o de muros de retenci�n, tipo VI',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)7','Dise�o de muros de retenci�n, tipo VII',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)8','Dise�o de muros de retenci�n, tipo VIII',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)9','Dise�o de muros de retenci�n, tipo IX',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)10','Dise�o de muros de retenci�n, tipo X',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)11','Dise�o de muros de retenci�n, tipo XI',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)12','Dise�o de muros de retenci�n, tipo XII',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)13','Dise�o de muros de retenci�n, tipo XIII',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)14','Dise�o de muros de retenci�n, tipo XIV',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)15','Dise�o de muros de retenci�n, tipo XV',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)16','Dise�o de muros de retenci�n, tipo XVI',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)17','Dise�o de muros de retenci�n, tipo XVII',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)18','Dise�o de muros de retenci�n, tipo XVIII',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)19','Dise�o de muros de retenci�n, tipo XIX',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)20','Dise�o de muros de retenci�n, tipo XX',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)21','Dise�o de muros de retenci�n, tipo XXI',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)22','Dise�o de muros de retenci�n, tipo XXII',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)23','Dise�o de muros de retenci�n, tipo XXIII',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('634(1)24','Dise�o de muros de retenci�n, tipo XXIV',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('403 (1)A','Dise�o de Rehabilitaciones y Sobrecapas Asf�lticas',' km ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)E-LI','L�nea borde izquierda (continua)',' km ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)B-LI','L�nea de carril izquierda (blanca discontinua) ',' km ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)E-LS','L�nea simple continua',' km ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)B-LS','L�nea simple discontinua',' km ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)A2','L�nea doble continua discontinua',' km ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)D2','L�nea doble continua ',' km ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)B-LD','L�nea de carril derecha (blanca discontinua) ',' km ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)E-LD','L�nea borde derecha (continua)',' km ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)G','Letreros de Alto',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)H','Letreros de Ceda',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)J','Letreros de Velocidad de KPH',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)I','Letreros de Escuela',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)�','Letreros de Solo',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(10)','Sendas peatonales',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)F','Flechas',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)M','Isla de Canalizaci�n Amarilla',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(3)N','Isla de Canalizaci�n Blanca',' m2 ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(11)R','Captaluces 2 Cara Roja',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('107(11)A','Captaluces 2 Caras Amarillas',' u ');
INSERT INTO item (codigoItem,descripcion,unidadMedida) VALUES ('109.04','Trabajo a costo m�s porcentaje',' glob ');
go

insert into contratoItem (idContrato, idItem, precioUnitario) VALUES
	('1','1','500'),
	('1','2','1500'),
	('1','3','2000'),
	('2','4','1000')
go
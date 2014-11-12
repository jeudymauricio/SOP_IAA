use Proyecto_IAA

SET IDENTITY_INSERT persona ON   -- Apaga el auto incremento Identity
GO
insert into persona (id, nombre, apellido1, apellido2, cedula) values 
	(1, 'Nixon', 'Jiménez', 'Espinoza', '203510255'),
	(2, 'Javier', 'Gómez', 'Jara', '203140255'),
	(3, 'Esteban', 'Coto', 'Corrales', '203510255'),
	(4, 'Jonathan', 'Granados', 'Granados', '208510255'),
	(5, 'José Antonio', 'Araya', 'Granados', '201310255'),
	(6, 'Edgar', 'May', 'Cantillano', '203020255'),
	(7 ,'Ileana', 'Aguilar', 'Aguilar', '103510255'),
	(8, 'Cristian', 'Vargas', 'Calvo', '603510255'),
	(9, 'Victor Hugo', 'Pérez', 'Salas', '603510256'),
	(10, 'José Angel', 'Castro', 'Murillo', '603510257'),
	(11, 'Raimond', 'Sánchez', 'López', '603510257')
go
SET IDENTITY_INSERT persona OFF		-- Enciende el auto incremento Identity
GO

-------------------------------------------------------------------------------
insert into ingeniero(idPersona) values
	('2'),
	('3'),
	('4'),
	('5'),
	('6'),
	('7'),
	('8')
go

-------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].laboratorioCalidad ON   -- Apaga el auto incremento Identity
GO
insert into laboratorioCalidad(id, nombre, tipo) values
	(1, 'CACISA','Verificación'),
	(2, 'ITP','Autocontrol')
go
SET IDENTITY_INSERT [dbo].laboratorioCalidad OFF   -- Enciende el auto incremento Identity
GO

------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].zona ON   -- Apaga el auto incremento Identity
GO
insert into zona(id, nombre) Values
	(1, '6-1 San Carlos'),
	(2, '6-2 Los Chiles'),
	(3, '1-1 San José '),
	(4, '1-4 Alajuela Sur')
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
	(1, 'LP N°2009-000003-CV','1','CONSERVACIÓN VIAL DE LA RED VIAL NACIONAL PAVIMENTADA DE LA ZONA 6-1, SAN CARLOS ESTE.','11','1','01/09/2011','1095','1'),
	(2, 'LP N°2010-000001-CV','1','CONSERVACIÓN VIAL DE LA RED VIAL NACIONAL PAVIMENTADA DE LA ZONA 6-2, SAN CARLOS ESTE.','11','2','01/09/2012','1095','1')
go
SET IDENTITY_INSERT [dbo].Contrato OFF   -- Enciende el auto incremento Identity
GO

-------------------------------------------------------------------------------
insert into ingenieroContrato(idIngeniero, idContrato, rol, descripcion, departamento, fechaInicio, fechaFin) values
	('3','1', 'Ingeniero Responsable CONAVI','Ingeniero a cargo de la Zona 6-1, San Carlos Este.','Gerencia de Conservación de Vías y Puentes','2011-09-01','2014-08-30'),
	('2','1', 'Unidad Supervisora de proyecto','Organismo de Inspección Zona 6-1, San Carlos Este.','Ileana Aguilar. Ingeniería y Administración S.A', '2011-09-01','2014-08-30'),
	('4','1', 'Ingeniero responsable de la empresa','Ingeniero Residente Zona 6-1, San Carlos Este.','Constructora MECO S.A.','2011-09-01','2014-08-30'),
	('5','1', 'Director Regional Region Huetar Norte','Director Regional, Región Huetar Norte','Gerencia de Conservación de Vías y Puentes','2011-09-01','2014-08-30'),
	('6','1', 'Gerente de Conservación de Vías y Puentes','Gerente a.i. Conservación de Vías y Puentes','Gerencia de Conservación de Vías y Puentes','2011-09-01','2014-08-30'),
	('5','2', 'Director Regional Region Huetar Norte','Director Regional, Región Huetar Norte','Gerencia de Conservación de Vías y Puentes','2011-09-01','2014-08-30'),
	('6','2', 'Gerente de Conservación de Vías y Puentes','Gerente a.i. Conservación de Vías y Puentes','Gerencia de Conservación de Vías y Puentes','2011-09-01','2014-08-30')
go

-------------------------------------------------------------------------------
insert into labCalidadContrato(idLabCalidad, idContrato) values
	('1','1'),
	('2','1'),
	('1','2'),
	('2','2')
go

-------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].ruta ON   -- Apaga el auto incremento Identity
GO
insert into ruta(id, nombre, descripcion) values 
	(1,'1','Sn José, Heredia, Belén, Alaj, Grecia, Narnjo, Palmares, Sn Ramón, Esparza, Puntarenas, Abangares...'),
	(2,'2','SAN JOSE, MONTES DE OCA, CURRIDABAT, LA UNION, CARTAGO, DESAMPARADOS, EL GUARCO, DOTA, PARAISO...'),
	(3,'3','San José, Heredia, Flores, Alajuela, Atenas, San Mateo, Orotina'),
	(4,'4','Pococí, Sarapiquí, San Carlos, Guatuso, Upala, La Cruz'),
	(5,'5','Tibás, Santo Domingo, San Pablo, Heredia'),
	(6,'126','Heredia, Barva, Santa Bárbara, Alajuela, Sarapiquí'),
	(7,'140','San Carlos, Grecia, Alajuela'),
	(8,'141','Naranjo, Alfaro Ruiz, San Carlos, '),	
	(9,'708','Valverde Vega, Grecia')
go
SET IDENTITY_INSERT [dbo].Contrato OFF   -- Enciende el auto incremento Identity
GO

-------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[proyecto_estructura] ON   -- Apaga el auto incremento Identity
GO
insert into proyecto_estructura(id, idRuta, descripcion) values
	(1,'6','Sector-Las Torres del ICE'),
	(2,'7','Sector Puente los Negritos'),
	(3,'6','Mantenimiento y limpieza'),
	(4,'7','Mantenimiento y limpieza'),
	(5,'6','Rehabilitación'),
	(6,'7','Rehabilitación'),
	(7,'6','Mantenimiento'),
	(8,'7','Mantenimiento'),
	(9,'7','Proyecto-Bomba Delta'),
	(10,'6','Mejoramiento del sistema de drenajes de Cinchona'),
	(11,'8','Colocación de Tubería-Sector Grupo Q'),
	(12,'8','Puente Casa'),
	(13,'8','Cuneta revestida-Sector de Florencia'),
	(14,'8','Mejoramiento y manejo de aguas-Sectot Grupo Q'),
	(15,'8','Cuneta revestida-Puente Casa')
go
SET IDENTITY_INSERT [dbo].[proyecto_estructura] OFF   -- Eciende el auto incremento Identity
GO

-------------------------------------------------------------------------------
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

-------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[item] ON   -- Apaga el auto incremento Identity
GO
INSERT INTO item (id, codigoItem,descripcion,unidadMedida) VALUES 
	(1,'M21(F)', 'Limpieza de tomas, cabezales y alcantarillas', 'u'),
	(2,'M21(E)', 'Limpieza de cunetas revestidas de manera manual', 'm3'),
	(3,'M22(A)', 'Remoción de derrumbes', 'm3'),
	(4,'M20(A)', 'Chapea derecho de vía', 'm2'),
	(5,'M20(E)', 'Recolección de basura', 'h'),
	(6,'M20(D)', 'Descuaje de árboles por hora', 'h'),
	(7,'M21(G)', 'Conformación de cunetas y espaldones', 'm2'),
	(8,'M41(A)', 'Bacheo con mezcla asfáltica en caliente', 't'),
	(9,'M41(D)', 'Bacheo de urgencia', 't'),
	(10,'M42(B)', 'Perfilado de pavimentos', 'm2'),
	(11,'M45(A)','Pavimento bituminosos en caliente',' t '),
	(12,'M45(E)','Pavimento bituminosos en caliente con polímeros',' t '),
	(13,'M40(A)','Levantamiento de tapas de pozos',' u '),
	(14,'M46(B)','Demolición de losas',' m2 '),
	(15,'M46(A)','Suministro y colocación de concreto de MR 45 kg/cm2',' m3 '),
	(16,'M46 (C )','Suministro y colocación de acero para dovelas y barras de sujeción',' kg '),
	(17,'M43(D)','Sellado de juntas para losas recontruidas',' m '),
	(18,'M43(C )','Ruteo y sellado de grietas',' m '),
	(19,'M47(B)','Tratamiento bituminoso de preservación tipo S-2',' m2 '),
	(20,'410(6)A','Lechada asfáltica tipo slurry seal, graduación A',' m2 '),
	(21,'410(6)B','Lechada asfáltica tipo slurry seal, graduación B',' m2 '),
	(22,'MP-50(A)','Brigada de limpieza de puentes',' h '),
	(23,'M30(A)','Reacondicionamiento de la calzada',' m2 '),
	(24,'308(1)','Cemento Pórtland ',' t '),
	(25,'408(3)','Emulsión asfáltica para imprimación',' l '),
	(26,'408(5)','Material de secado ',' m3 '),
	(27,'203(2)','Excavación común',' m3 '),
	(28,'206(1)','Excavación para estructuras',' m3 '),
	(29,'206(3)','Relleno para fundación',' m3 '),
	(30,'M-304(4)','Suministro, colocación y compactación de base de agregado triturado, Graduación B ',' m3 '),
	(31,'602A(3)','Hormigón ciclopeo ',' m3 '),
	(32,'602A(1)','Hormigón estructural clase A de 225 kg/cm2',' m3 '),
	(33,'602A(5)','Hormigón estructural clase X de 180 kg/cm2',' m3 '),
	(34,'603(21)3B','Tubería de Hormigón clase III C-76 de 0,76 m para carreteras',' m '),
	(35,'603(21)3A','Tubería de Hormigón clase III C-76 de 0,81 m para carreteras',' m '),
	(36,'603(21)3C','Tubería de Hormigón clase III C-76 de 0,90 m para carreteras',' m '),
	(37,'603(21)3D','Tubería de Hormigón clase III C-76 de 1,00 m para carreteras',' m '),
	(38,'603(21)3E','Tubería de Hormigón clase III C-76 de 1,22 m para carreteras',' m '),
	(39,'603(21)3F','Tubería de Hormigón clase III C-76 de 1,50 m para carreteras',' m '),
	(40,'603(21)3H','Tubería de Hormigón clase III C-76 de 2,13 m para carreteras',' m '),
	(41,'707 (2)','Tubería corrugada de acero de 3,00 m para carreteras',' m '),
	(42,'605(20)','Relleno granular filtrante para subdrenaje francés',' m3 '),
	(43,'605(22)','Tela fibra sintética para subdrenaje francés',' m2 '),
	(44,'M-609(2A)','Cuneta de hormigón de cemento Portland',' m2 '),
	(45,'619C(3)B','Colchoneta de gaviones con revestimiento de PVC',' m3 '),
	(46,'619C(1)','Construcción de gavión convencional con revestimiento de PVC',' m3 '),
	(47,'619C(1)A1','Construcción de gavión tipo terramesh, 4m de cola',' m3 '),
	(48,'619C(1)A2','Construcción de gavión tipo terramesh, 5m de cola',' m3 '),
	(49,'619C(1)A3','Construcción de gavión tipo terramesh, 6m de cola',' m3 '),
	(50,'MP51(A)','Reparación de baranda de concreto',' m3 '),
	(51,'606(5)B1','Suministro e instalación de viga galvanizada para guardacamino',' m '),
	(52,'606(5)A1','Suministro e instalación de postes para guardacamino',' u '),
	(53,'606(5)B2','Sustitución de viga galvanizada para guardacamino',' m '),
	(54,'606(5)A2','Sustitución de postes para guardacamino',' u '),
	(55,'622A(5)','Cauce revestido con toba cemento plástico',' m2 '),
	(56,'M-204(1)','Suministro, colocación y compactación de sub-base granular, Graduación B',' m3 '),
	(57,'203(8)','Material de préstamo',' m3 '),
	(58,'608(1)','Aceras de Hormigón',' m2 '),
	(59,'612(2)','Construcción de barandas de acero para puentes',' m '),
	(60,'609(01)','Construcción de cordón de hormigón',' m '),
	(61,'611(1)A','Construcción de pasarelas peatonales',' m '),
	(62,'613(1)A','Reparación de adoquines',' m2 '),
	(63,'717(1)C','Acero estructural grado 40',' kg '),
	(64,'609(4)','Bordillo de  hormigón asfáltico de 0,15m de altura',' m '),
	(65,'609(8)','Bolardos de hormigón reforzado',' u '),
	(66,'201(6)','Remoción selectiva de árboles',' u '),
	(67,'202(1)A','Remoción de estructuras tipo cabezal o similares',' u '),
	(68,'R-1-2(A)','Limpieza y reparación de señalamiento vertical',' u '),
	(69,'726(1)','Suministro de señales para emergencias chevron',' u '),
	(70,'726(2)','Suministro de señales para emergencias Ceda',' u '),
	(71,'726(3)','Suministro de señales para emergencias Despacio',' u '),
	(72,'726(4)','Suministro de señales para emergencias Vía cerrada adelante',' u '),
	(73,'726(5)','Suministro de señales para emergencias Peligro',' u '),
	(74,'MP-620(3)','Suministro de roca de río',' m3 '),
	(75,'704(2)','Tela geotextil para repavimentación',' m2 '),
	(76,'634(1)1','Diseño de muros de retención, tipo I',' u '),
	(77,'634(1)2','Diseño de muros de retención, tipo II',' u '),
	(78,'634(1)3','Diseño de muros de retención, tipo III',' u '),
	(79,'634(1)4','Diseño de muros de retención, tipo IV',' u '),
	(80,'634(1)5','Diseño de muros de retención, tipo V',' u '),
	(81,'634(1)6','Diseño de muros de retención, tipo VI',' u '),
	(82,'634(1)7','Diseño de muros de retención, tipo VII',' u '),
	(83,'634(1)8','Diseño de muros de retención, tipo VIII',' u '),
	(84,'634(1)9','Diseño de muros de retención, tipo IX',' u '),
	(85,'634(1)10','Diseño de muros de retención, tipo X',' u '),
	(86,'634(1)11','Diseño de muros de retención, tipo XI',' u '),
	(87,'634(1)12','Diseño de muros de retención, tipo XII',' u '),
	(88,'634(1)13','Diseño de muros de retención, tipo XIII',' u '),
	(89,'634(1)14','Diseño de muros de retención, tipo XIV',' u '),
	(90,'634(1)15','Diseño de muros de retención, tipo XV',' u '),
	(91,'634(1)16','Diseño de muros de retención, tipo XVI',' u '),
	(92,'634(1)17','Diseño de muros de retención, tipo XVII',' u '),
	(93,'634(1)18','Diseño de muros de retención, tipo XVIII',' u '),
	(94,'634(1)19','Diseño de muros de retención, tipo XIX',' u '),
	(95,'634(1)20','Diseño de muros de retención, tipo XX',' u '),
	(96,'634(1)21','Diseño de muros de retención, tipo XXI',' u '),
	(97,'634(1)22','Diseño de muros de retención, tipo XXII',' u '),
	(98,'634(1)23','Diseño de muros de retención, tipo XXIII',' u '),
	(99,'634(1)24','Diseño de muros de retención, tipo XXIV',' u '),
	(100,'403 (1)A','Diseño de Rehabilitaciones y Sobrecapas Asfálticas',' km '),
	(101,'107(3)E-LI','Línea borde izquierda (continua)',' km '),
	(102,'107(3)B-LI','Línea de carril izquierda (blanca discontinua) ',' km '),
	(103,'107(3)E-LS','Línea simple continua',' km '),
	(104,'107(3)B-LS','Línea simple discontinua',' km '),
	(105,'107(3)A2','Línea doble continua discontinua',' km '),
	(106,'107(3)D2','Línea doble continua ',' km '),
	(107,'107(3)B-LD','Línea de carril derecha (blanca discontinua) ',' km '),
	(108,'107(3)E-LD','Línea borde derecha (continua)',' km '),
	(109,'107(3)G','Letreros de Alto',' u '),
	(110,'107(3)H','Letreros de Ceda',' u '),
	(111,'107(3)J','Letreros de Velocidad de KPH',' u '),
	(112,'107(3)I','Letreros de Escuela',' u '),
	(113,'107(3)Ñ','Letreros de Solo',' u '),
	(114,'107(10)','Sendas peatonales',' m2 '),
	(115,'107(3)F','Flechas',' u '),
	(116,'107(3)M','Isla de Canalización Amarilla',' m2 '),
	(117,'107(3)N','Isla de Canalización Blanca',' m2 '),
	(118,'107(11)R','Captaluces 2 Cara Roja',' u '),
	(119,'107(11)A','Captaluces 2 Caras Amarillas',' u '),
	(120,'109.04','Trabajo a costo más porcentaje',' glob ')
go
SET IDENTITY_INSERT [dbo].[item] OFF   -- Enciende el auto incremento Identity
GO

-------------------------------------------------------------------------------
insert into inspector (idPersona) values
	('1'),
	('2'),
	('3'),
	('4'),
	('5'),
	('9'),
	('10'),
	('11')
go

-------------------------------------------------------------------------------
insert into tipoProyecto (nombre) values 
	('Mejoramiento derecho via'),
	('Mejoramiento sistema drenaje'),
	('Mejoramiento de Puentes'),
	('Bacheos'),
	('Chapea'),
	('Mejoramiento de sistema cunetas y sistema drenajes'),
	('Ampliación'),
	('Construcción'),
	('Mejoramiento'),
	('Mejoramiento Entroque'),
	('Muros'),
	('Mantenimiento')
go

-------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[contratoItem] ON   -- Apaga el auto incremento Identity
GO
insert into contratoItem (id, idContrato, idItem, precioUnitario) VALUES
	(1,'1','1','28501.910'),
	(2,'1','2','5338.590'),
	(3,'1','3','1627.560'),
	(4,'1','4','39.220'),
	(5,'1','5','17550.200'),
	(6,'1','6','18305.150'),
	(7,'1','7','137.260'),
	(8,'1','8','56000.880'),
	(9,'1','9','61816.550'),
	(10,'1','10','713.490'),
	(11,'1','11','47362.060'),
	(12,'1','12','73897.700'),
	(13,'1','13','33752.270'),
	(14,'1','14','4696.390'),
	(15,'1','15','193086.780'),
	(16,'1','16','2848.230'),
	(17,'1','17','2196.690'),
	(18,'1','18','3008.510'),
	(19,'1','19','2041.660'),
	(20,'1','20','1966.680'),
	(21,'1','21','1972.450'),
	(22,'1','22','33374.790'),
	(23,'1','23','1294.670'),
	(24,'1','24','165201.410'),
	(25,'1','25','374.880'),
	(26,'1','26','19349.630'),
	(27,'1','27','6208.030'),
	(28,'1','28','6471.020'),
	(29,'1','29','11035.050'),
	(30,'1','30','17674.310'),
	(31,'1','31','83192.900'),
	(32,'1','32','133803.100'),
	(33,'1','33','150673.330'),
	(34,'1','34','122211.210'),
	(35,'1','35','120844.330'),
	(36,'1','36','131886.130'),
	(37,'1','37','157620.270'),
	(38,'1','38','240876.850'),
	(39,'1','39','344003.490'),
	(40,'1','40','613531.400'),
	(41,'1','41','999160.240'),
	(42,'1','42','20799.550'),
	(43,'1','43','875.490'),
	(44,'1','44','16565.360'),
	(45,'1','45','83012.500'),
	(46,'1','46','55160.570'),
	(47,'1','47','75431.540'),
	(48,'1','48','79657.310'),
	(49,'1','49','85152.780'),
	(50,'1','50','202400.850'),
	(51,'1','51','32771.810'),
	(52,'1','52','69220.330'),
	(53,'1','53','34379.760'),
	(54,'1','54','74480.490'),
	(55,'1','55','16506.300'),
	(56,'1','56','14447.340'),
	(57,'1','57','11402.150'),
	(58,'1','58','17371.870'),
	(59,'1','59','127783.090'),
	(60,'1','60','14501.320'),
	(61,'1','61','261752.870'),
	(62,'1','62','17409.010'),
	(63,'1','63','3085.560'),
	(64,'1','64','14416.420'),
	(65,'1','65','84140.600'),
	(66,'1','66','44825.160'),
	(67,'1','67','66155.310'),
	(68,'1','68','28090.010'),
	(69,'1','69','60489.360'),
	(70,'1','70','92251.290'),
	(71,'1','71','92251.290'),
	(72,'1','72','92251.290'),
	(73,'1','73','92251.290'),
	(74,'1','74','22730.940'),
	(75,'1','75','875.490'),
	(76,'1','76','3405233.580'),
	(77,'1','77','3405233.580'),
	(78,'1','78','4723685.450'),
	(79,'1','79','4723685.450'),
	(80,'1','80','3621019.700'),
	(81,'1','81','4775889.640'),
	(82,'1','82','4862896.640'),
	(83,'1','83','4862896.640'),
	(84,'1','84','5068239.730'),
	(85,'1','85','5068239.730'),
	(86,'1','86','5148288.350'),
	(87,'1','87','5148288.350'),
	(88,'1','88','13894080.410'),
	(89,'1','89','6768766.270'),
	(90,'1','90','14990390.480'),
	(91,'1','91','7865081.820'),
	(92,'1','92','15554204.580'),
	(93,'1','93','8428884.960'),
	(94,'1','94','15971235.470'),
	(95,'1','95','10066161.580'),
	(96,'1','96','22759567.360'),
	(97,'1','97','10884038.300'),
	(98,'1','98','11748300.500'),
	(99,'1','99','11748300.500'),
	(100,'1','100','474593.580'),
	(101,'1','101','538900.090'),
	(102,'1','102','344550.240'),
	(103,'1','103','552176.640'),
	(104,'1','104','347803.060'),
	(105,'1','105','922610.980'),
	(106,'1','106','1137060.210'),
	(107,'1','107','368311.930'),
	(108,'1','108','571185.990'),
	(109,'1','109','40862.030'),
	(110,'1','110','40862.030'),
	(111,'1','111','43861.080'),
	(112,'1','112','47004.310'),
	(113,'1','113','27850.770'),
	(114,'1','114','9758.440'),
	(115,'1','115','27383.620'),
	(116,'1','116','8749.150'),
	(117,'1','117','8749.150'),
	(118,'1','118','4446.670'),
	(119,'1','119','4446.670'),
	(120,'1','120','264248692.190')
go
SET IDENTITY_INSERT [dbo].[contratoItem] OFF   -- Eciende el auto incremento Identity
GO

--- Reajustes de 7 meses de los primeros 10 ítems
-------------------------------------------------------------------------------
INSERT INTO itemReajuste (idContratoItem,fecha,reajuste,precioReajustado) VALUES 
	('1','2011-09-01','0.08794',' 31,008.37 '),
	('2','2011-09-01','0.09226',' 5,831.13 '),
	('3','2011-09-01','0.08433',' 1,764.81 '),
	('4','2011-09-01','0.09612',' 42.99 '),
	('5','2011-09-01','0.0876',' 19,087.60 '),
	('6','2011-09-01','0.0899',' 19,950.78 '),
	('7','2011-09-01','0.08201',' 148.52 '),
	('8','2011-09-01','0.10737',' 62,013.69 '),
	('9','2011-09-01','0.10561',' 68,345.00 '),
	('10','2011-09-01','0.08165',' 771.75 '),
	('1','2011-10-01','0.08132',' 33,529.97 '),
	('2','2011-10-01','0.08781',' 6,343.16 '),
	('3','2011-10-01','0.07768',' 1,901.90 '),
	('4','2011-10-01','0.09335',' 47.00 '),
	('5','2011-10-01','0.08128',' 20,639.04 '),
	('6','2011-10-01','0.08408',' 21,628.24 '),
	('7','2011-10-01','0.07322',' 159.39 '),
	('8','2011-10-01','0.06751',' 66,200.24 '),
	('9','2011-10-01','0.06886',' 73,051.23 '),
	('10','2011-10-01','0.07178',' 827.14 '),
	('1','2011-11-01','0.07848',' 36,161.40 '),
	('2','2011-11-01','0.08599',' 6,888.61 '),
	('3','2011-11-01','0.07513',' 2,044.79 '),
	('4','2011-11-01','0.0923',' 51.34 '),
	('5','2011-11-01','0.07866',' 22,262.50 '),
	('6','2011-11-01','0.08159',' 23,392.89 '),
	('7','2011-11-01','0.0695',' 170.47 '),
	('8','2011-11-01','0.06626',' 70,586.67 '),
	('9','2011-11-01','0.06768',' 77,995.34 '),
	('10','2011-11-01','0.06741',' 882.90 '),
	('1','2011-12-01','0.08196',' 39,125.19 '),
	('2','2011-12-01','0.08881',' 7,500.39 '),
	('3','2011-12-01','0.08015',' 2,208.68 '),
	('4','2011-12-01','0.0944',' 56.19 '),
	('5','2011-12-01','0.08245',' 24,098.05 '),
	('6','2011-12-01','0.08467',' 25,373.57 '),
	('7','2011-12-01','0.07433',' 183.14 '),
	('8','2011-12-01','0.06931',' 75,479.03 '),
	('9','2011-12-01','0.0706',' 83,501.81 '),
	('10','2011-12-01','0.07177',' 946.27 '),
	('1','2012-01-01','0.09862',' 42,983.71 '),
	('2','2012-01-01','0.10543',' 8,291.15 '),
	('3','2012-01-01','0.09392',' 2,416.12 '),
	('4','2012-01-01','0.11137',' 62.45 '),
	('5','2012-01-01','0.09835',' 26,468.09 '),
	('6','2012-01-01','0.1016',' 27,951.52 '),
	('7','2012-01-01','0.08972',' 199.57 '),
	('8','2012-01-01','0.0798',' 81,502.26 '),
	('9','2012-01-01','0.08137',' 90,296.35 '),
	('10','2012-01-01','0.08865',' 1,030.15 '),
	('1','2012-02-01','0.08524',' 46,647.65 '),
	('2','2012-02-01','0.09634',' 9,089.92 '),
	('3','2012-02-01','0.08022',' 2,609.94 '),
	('4','2012-02-01','0.10567',' 69.04 '),
	('5','2012-02-01','0.08549',' 28,730.85 '),
	('6','2012-02-01','0.08984',' 30,462.69 '),
	('7','2012-02-01','0.07193',' 213.93 '),
	('8','2012-02-01','0.09942',' 89,605.21 '),
	('9','2012-02-01','0.09964',' 99,293.48 '),
	('10','2012-02-01','0.06887',' 1,101.10 '),
	('1','2012-03-01','0.03',' 48,047.08 '),
	('2','2012-03-01','0.03',' 9,362.62 '),
	('3','2012-03-01','0.03',' 2,688.24 '),
	('4','2012-03-01','0.03',' 71.12 '),
	('5','2012-03-01','0.03',' 29,592.77 '),
	('6','2012-03-01','0.03',' 31,376.57 '),
	('7','2012-03-01','0.03',' 220.34 '),
	('8','2012-03-01','0.03',' 92,293.37 '),
	('9','2012-03-01','0.03',' 102,272.29 '),
	('10','2012-03-01','0.03',' 1,134.13 ')
go

-------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[boleta] ON   -- Apaga el auto incremento Identity
GO
INSERT INTO boleta (id, idContrato,numeroBoleta,fecha,idRuta,idFondo,seccionControl,estacionamientoInicial,estacionamientoFinal,periodo,idInspector,idProyecto_Estructura,observaciones) VALUES 
	(1,'1','84366','2014-06-02','7','1','20931','17050','17050','6','9','2','Atención rutas alternas. Puente los Negrito OS 12'),
	(2,'1','84368','2014-06-03','7','1','20931','17050','17050','6','9','2','Atención rutas alternas. Puente los Negrito OS 12'),
	(3,'1','84369','2014-06-04','7','1','20931','17050','17050','6','9','2','Atención rutas alternas. Puente los Negrito OS 12'),
	(4,'1','84370','2014-06-04','7','1','20931','17050','17050','6','9','2','Atención rutas alternas. Puente los Negrito OS 12'),
	(5,'1','84371','2014-06-06','7','1','20931','17050','17050','6','9','2','Atención rutas alternas. Puente los Negrito OS 12'),
	(6,'1','84372','2014-06-07','7','1','20931','17050','17050','6','9','2','Atención rutas alternas. Puente los Negrito OS 12'),
	(7,'1','84360','2014-06-09','7','1','20931','17050','17050','6','10','2','Atención rutas alternas. Puente los Negrito OS 12'),
	(8,'1','1248','2014-06-09','7','1','20931','17008','17137','6','11','2','Atención a la estructura del puente'),
	(9,'1','1902','2014-06-10','7','1','20931','17009','17138','6','11','2','Carpeta asfáltica'),
	(10,'1','84375','2014-06-11','7','1','20931','17050','17050','6','9','2','Atención rutas alternas. Puente los Negrito OS 12'),
	(11,'1','84376','2014-06-12','7','1','20931','17050','17050','6','9','2','Atención rutas alternas. Puente los Negrito OS 12'),
	(12,'1','1249','2014-06-12','7','1','20931','17050','17050','6','11','2','Atención a la estructura del puente'),
	(13,'1','84377','2014-06-14','7','1','20931','17050','17050','6','9','2','Atención rutas alternas. Puente los Negrito OS 12'),
	(14,'1','1250','2014-06-16','7','1','20931','17090','17100','6','11','2','Atención a la estructura del puente'),
	(15,'1','85001','2014-06-18','7','1','20931','17050','17050','6','11','2','Atención a la estructura del puente')
go
SET IDENTITY_INSERT [dbo].[boleta] OFF   -- Eciende el auto incremento Identity
GO

-------------------------------------------------------------------------------
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('1','0','88.96','1','4149774.652','46647.64672');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('2','0','160.6','1','48047.07612','46647.64672');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('3','0','222.8','1','48047.07612','46647.64672');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('4','0','44','1','48047.07612','46647.64672');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('5','0','401.8','1','48047.07612','46647.64672');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('6','0','153','1','48047.07612','46647.64672');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('7','0','13','1','48047.07612','46647.64672');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('8','3.672868217','473.8','2','9362.617974','9089.920363');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('9','1.102170543','142.18','3','2688.241962','2609.943653');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('10','0','60','1','48047.07612','46647.64672');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('11','0','86','1','48047.07612','46647.64672');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('12','0','106','1','48047.07612','46647.64672');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('13','0','86','1','48047.07612','46647.64672');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('14','1.781','17.81','4','71.11551163','69.04418605');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('14','0.271','2.71','5','29592.77307','28730.84764');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('14','1.129','11.29','1','48047.07612','46647.64672');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('14','1','6','6','31376.56988','30462.6892');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('15','0','29.61','7','220.3439298','213.9261455');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('15','0','1123.92','8','92293.3657','89605.20942');
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem,costototal,precioUnitarioFecha) VALUES ('15','0','37.11','9','102272.286','99293.48158');
go

/*
select * from (select ci.idItem, ci.precioUnitario, ir.fecha, ir.reajuste, ir.precioReajustado from itemReajuste ir inner join contratoItem ci on ir.idContratoItem = ci.id )as a inner join item i on a.idItem = i.id order by i.codigoItem
select * from itemReajuste where fecha = '2012-03-01'

select * from boletaItem
select * from contratoItem

-- Muestra el indentity sin reiniciarlo
DBCC CHECKIDENT ('boleta', NORESEED);

-- Reinicia el identity
DBCC CHECKIDENT ('boleta', reseed, 1);
*/
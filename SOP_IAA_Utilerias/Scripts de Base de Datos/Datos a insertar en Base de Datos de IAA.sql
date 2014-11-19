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
	(1, 'LP N°2009-000003-CV','1','CONSERVACIÓN VIAL DE LA RED VIAL NACIONAL PAVIMENTADA DE LA ZONA 6-1, SAN CARLOS ESTE.','11','1','09/01/2011','1095','1'),
	(2, 'LP N°2010-000001-CV','1','CONSERVACIÓN VIAL DE LA RED VIAL NACIONAL PAVIMENTADA DE LA ZONA 6-2, SAN CARLOS ESTE.','11','2','09/01/2012','1095','1')
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
	(6,'6','Cañas, Bagaces, Upala'),
	(7,'126','Heredia, Barva, Santa Bárbara, Alajuela, Sarapiquí'),
	(8,'140','San Carlos, Grecia, Alajuela'),
	(9,'141','Naranjo, Alfaro Ruiz, San Carlos, '),	
	(10,'708','Valverde Vega, Grecia')
go
SET IDENTITY_INSERT [dbo].Contrato OFF   -- Enciende el auto incremento Identity
GO

-------------------------------------------------------------------------------
INSERT INTO seccionControl (idRuta,seccion,descripcion) VALUES 
	('1','19002','SABANA ESTE(R.2)(R.27)(C.42)-LA URUCA(R.3)(HOTEL IRAZU)'),
	('1','19003','LA URUCA(R.3)(HOTEL IRAZU)-LTE PROV.SAN JOSE/HEREDIA(R.VIRILLA)'),
	('1','20000','LTE PROV.HEREDIA/ALAJUELA(R.SEGUNDO)-RADIAL ALAJUELA(R.153)'),
	('1','20010','RADIAL ALAJUELA(R.153)-LTE CANT.ALAJUELA/GRECIA(R.POAS)'),
	('1','20020','LTE CANT.ALAJUELA/GRECIA(R.POAS)-LTE CANT.GRECIA/NARANJO(R.COLORADO)'),
	('1','20031','LTE CANT.GRECIA/NARANJO(R.COLORADO)-RADIAL NARANJO(R.141)'),
	('1','20032','RADIAL NARANJO(R.141)-LTE CANT.NARANJO/PALMARES(R.GRANDE)'),
	('1','20040','LTE NARANJO/PALMARES(R.GRANDE)-LTE PALMARES/SAN RAMON(1+250 DESP R.135)'),
	('1','20050','LTE CANT.PALMARES/SAN RAMON(1+250 KM DESPUES R.135)-MONSERRAT(R.135)'),
	('1','20060','MONSERRAT(R.135)-LTE PROV.ALAJUELA/PUNTARENAS(CRUCE CERRO ESQUIVEL)'),
	('1','40040','LTE PROV.SAN JOSE/HEREDIA(R.VIRILLA)-LTE CANT.HEREDIA/BELEN(R.111)'),
	('1','40710','LTE CANT.HEREDIA/BELEN(R.111)-LTE PROV.HEREDIA/ALAJUELA(R.SEGUNDO)'),
	('1','50000','LTE PROV.PUNTARENAS/GUANACASTE(R.LAGARTO)-LA IRMA(R.145)'),
	('1','50010','LA IRMA(R.145)-LTE CANT.ABANGARES/CAÑAS(R.LAJAS)'),
	('1','50020','CAÑAS(R.142)-LTE CANT.CAÑAS/BAGACES(R.TENORIO)'),
	('1','50030','BAGACES(R.164)-LTE CANT.BAGACES/LIBERIA(R.SALTO)'),
	('1','50040','LIBERIA(R.21)(R.918)-LTE CANT.LIBERIA/LA CRUZ(QUEB.PUERCOS)'),
	('1','50050','LTE CANT.LIBERIA/LA CRUZ(QUEB.PUERCOS)-LA CRUZ(R.935)(CLINICA C.C.S.S.)'),
	('1','50060','LA CRUZ(R.935)(CLINICA C.C.S.S.)-PEÑAS BLANCAS(FRONTERA NORTE)'),
	('1','50110','LTE CANT.ABANGARES/CAÑAS(R.LAJAS)-CAÑAS(R.142)'),
	('1','51120','LTE CANT.CAÑAS/BAGACES(R.TENORIO)-BAGACES(R.164)'),
	('1','51130','LTE CANT.BAGACES/LIBERIA(R.SALTO)-LIBERIA(R.21)(R.918)'),
	('1','60200','LTE PROV.ALAJUELA/PUNTARENAS(CRUCE CERRO ESQUIVEL)-ESPARZA(R.131)'),
	('1','60210','ESPARZA(R.131)-LTE CANT.ESPARZA/PUNTARENAS(R.BARRANCA)'),
	('1','60220','LTE ESPARZA/PUNTARENAS(R.BARRANCA)-LTE PUNT/MONTES DE ORO(R.SAN MIGUEL)'),
	('1','60230','LTE PUNTARENAS/MONTES DE ORO(R.SAN MIGUEL)-LTE MONTES DE ORO/PUNT(Q.PALO)'),
	('1','60240','LTE CANT.MONTES DE ORO/PUNTARENAS(Q.PALO)-LTE PROV.PUNT/GTE(R.LAGARTO)'),
	('2','10001','JUNTAS DE PACUAR(R.244)-LTE PROV.SAN JOSE/PUNTARENAS(R.CONVENTO)'),
	('2','10002','PALMARES(FINAL CINCO CARRILES)-JUNTAS DE PACUAR(R.244)'),
	('2','10003','SAN ISIDRO DE EL GENERAL(R.243)-PALMARES(FINAL CINCO CARRILES)'),
	('2','10010','LA ESE(ESCUELA)-SAN ISIDRO DE EL GENERAL(R.243)'),
	('2','10020','LTE PROV.CARTAGO/SAN JOSE(CRUCE PIEDRA ALTA)-LA ESE(ESCUELA)'),
	('2','10030','OJO AGUA(CRUCE PROVIDENCIA)-LTE PROV.SAN JOSE/CARTAGO(CERRO ASUNCION)'),
	('2','10041','LTE PROV.CARTAGO/SAN JOSE(R.406)-LA SIERRA(R.222)'),
	('2','10042','LA SIERRA(R.222)-LA GUARIA DE EL EMPALME(R.226)'),
	('2','10250','CURRIDABAT(R.251)-LTE PROV.SAN JOSE/CARTAGO(0+100 MTS ANTES R.CHAGUITE)'),
	('2','19001','SAN JOSE(SABANA ESTE)(R.1)(R.27)(C.42)-SAN JOSE(Av.2/C.0)'),
	('2','19004','SAN JOSE(Av.2/C.0)-LTE CANT.SAN JOSE/MONTES DE OCA(CALLE LOS NEGRITOS)'),
	('2','19005','LTE SAN JOSE/MTES DE OCA(CALLE NEGRITOS)-LTE MTES DE OCA/CURRI(R.OCLORO)'),
	('2','19006','LTE CANT.MONTES DE OCA/CURRIDABAT(R.OCLORO)-CURRIDABAT(R.251)'),
	('2','30090','LTE CANT.CARTAGO/EL GUARCO(CRUCE PURIRES)-LTE PROV.CARTAGO/S.J.(R.406)'),
	('2','30101','TARAS(R.219)-LA LIMA(R.10)'),
	('2','30102','LA LIMA(R.10)-LTE CANT.CARTAGO/EL GUARCO(CRUCE PURIRES)'),
	('2','30110','SAN RAFAEL(PASO SUPERIOR R.251)-LTE CANT.LA UNION/CARTAGO(QUEB.QUIRAZU)'),
	('2','30600','LTE PROV.SAN JOSE/CART(0+100 ANTES R.CHAGUITE)-SAN RAFAEL(PASO SUP R.251)'),
	('2','30680','LTE SAN JOSE/CARTAGO(CERRO ASUNCION)-LTE CARTAGO/S.J.(CRUCE PIEDRA ALTA)'),
	('2','30690','LA GUARIA DE EL EMPALME(R.226)-OJO AGUA(CRUCE PROVIDENCIA)'),
	('2','30730','LTE CANT.LA UNION/CARTAGO(QUEB.QUIRAZU)-TARAS(R.219)'),
	('2','30740','TARAS(R.2)-LTE CANT.CARTAGO/LA UNION(QUEB.QUIRAZU)'),
	('2','30750','LTE CANT.CARTAGO/LA UNION(QUEB.QUIRAZU)-SAN RAFAEL(PASO SUPERIOR R.251)'),
	('2','60001','CIUDAD NEILY(R.237)(R.608)-PASO CANOAS(R.238)(FRONTERA SUR)'),
	('2','60002','LTE CANT.GOLFITO/CORREDORES(R.CARACOL)-CIUDAD NEILY(R.237)(R.608)'),
	('2','60011','RIO CLARO(R.14)-LTE CANT.GOLFITO/CORREDORES(R.CARACOL)'),
	('2','60012','LTE CANT.OSA/GOLFITO(R.ESQUINAS)-RIO CLARO(R.14)'),
	('2','60020','CHACARITA(R.245)-LTE CANT.OSA/GOLFITO(R.ESQUINAS)'),
	('2','60030','PALMAR NORTE(R.34)-CHACARITA(R.245)'),
	('2','60040','LTE CANT.BUENOS AIRES/OSA(QUEB.IGUANA)-PALMAR NORTE(R.34)'),
	('2','60051','PASO REAL(R.237)-LTE CANT.BUENOS AIRES/OSA(QUEB.IGUANA)'),
	('2','60052','BUENOS AIRES(R.246)-PASO REAL(R.237)'),
	('2','60060','LTE PROV.SAN JOSE/PUNTARENAS(R.CONVENTO)-BUENOS AIRES(R.246)'),
	('3','19007','LA URUCA(R.1)(PUENTE JUAN PABLO II)-LTE PROV.SAN JOSE/HEREDIA(R.VIRILLA)'),
	('3','20070','BARRIO SAN JOSE(R.118)-MANOLOS(PASO SUPERIOR R.1)'),
	('3','20081','MANOLOS(PASO SUPERIOR R.1)-LA GARITA(R.136)'),
	('3','20082','LA GARITA(R.136)-LTE CANT.ALAJUELA/ATENAS(R.GRANDE)'),
	('3','20090','LTE CANT.ALAJUELA/ATENAS(R.GRANDE)-ATENAS(R.135)'),
	('3','20100','ATENAS(R.135)-LTE CANT.ATENAS/SAN MATEO(QUEB.S N)(PATO DE AGUA)'),
	('3','20111','LTE CANT.ATENAS/SAN MATEO(QUEB.S N)(PATO AGUA)-DESMONTE(ESCUELA)'),
	('3','20112','DESMONTE(ESCUELA)-LTE CANT.SAN MATEO/OROTINA(R.MACHUCA)'),
	('3','20131','LTE PROV.HEREDIA/ALAJUELA(R.SEGUNDO)-INTERSECCION AEROPUERTO(R.111)'),
	('3','20132','INTERSECCION AEROPUERTO(R.111)-ALAJUELA(R.130)(R.153)(Av.Central/C.2)'),
	('3','20140','ALAJUELA(R.130)(R.153)(Av.Central/C.2)-BARRIO SAN JOSE(R.118)'),
	('3','20700','LTE CANT.SAN MATEO/OROTINA(R.MACHUCA)-OROTINA(PASO INFERIOR R.27)(R.137)'),
	('3','40000','LTE PROV.SAN JOSE/HEREDIA(R.VIRILLA)-HEREDIA(R.126)(Av.4/C.2)'),
	('3','40010','HEREDIA(R.126)(Av.4/C.2)-LTE CANT.HEREDIA/FLORES(QUEB.SECA)'),
	('3','40750','LTE CANT.HEREDIA/FLORES(QUEB.SECA)-LTE PROV.HEREDIA/ALAJUELA(R.SEGUNDO)'),
	('4','20721','LTE CANT.SAN CARLOS/GUATUSO(EL EDEN)-SAN RAFAEL,GUATUSO(R.143)'),
	('4','20722','SAN RAFAEL,GUATUSO(R.143)-LTE CANT.GUATUSO/UPALA(R.RITO)'),
	('4','21011','SAN JOSE DE AGUAS ZARCAS(R.751)-MUELLE,SAN CARLOS(R.35)'),
	('4','21012','MUELLE,SAN CARLOS(R.35)-EL TANQUE(R.142)'),
	('4','21021','LTE CANT.GUATUSO/UPALA(R.RITO)-COLONIA PUNTARENAS(R.138)'),
	('4','21022','COLONIA PUNTARENAS(R.138)-UPALA(R.6)'),
	('4','21023','UPALA(R.6)-SAN JOSE,UPALA(R.735)'),
	('4','21024','SAN JOSE,UPALA(R.735)-BIRMANIA(CRUCE CENTRO POBLACION)'),
	('4','21025','BIRMANIA(CRUCE CENTRO POBLAC)-LTE PROV.ALAJUELA/GTE(R.HACIENDAS O COLON)'),
	('4','21351','EL TANQUE(R.142)-MONTERREY(R.752)'),
	('4','21352','MONTERREY(R.752)-LTE CANT.SAN CARLOS/GUATUSO(EL EDEN)'),
	('4','40460','PUERTO VIEJO,SARAPIQUI(R.505)-BAJOS DE CHILAMATE(R.126)(R.506)'),
	('4','40521','LTE PROV.LIMON/HEREDIA(R.CHIRRIPO)-LAS VUELTAS,HORQUETAS(R.229)'),
	('4','40522','LAS VUELTAS,HORQUETAS(R.229)-PUERTO VIEJO,SARAPIQUI(R.505)'),
	('4','50280','LOS INOCENTES(CRUCE ESCUELA)-LA CRUZ(R.1)'),
	('4','50290','SANTA CECILIA(R.170)-LOS INOCENTES(CRUCE ESCUELA)'),
	('4','50300','LTE PROV.ALAJUELA/GUANACASTE(R.HACIENDAS O COLON)-SANTA CECILIA(R.170)'),
	('4','70110','LA Y GRIEGA,POCOCI(R.32)-LTE PROV.LIMON/HEREDIA(R.CHIRRIPO)'),
	('5','19008','TOURNON(R.108)-SAN JUAN,TIBAS(R.102)'),
	('5','19009','SAN JUAN,TIBAS(R.102)-LTE PROV.SAN JOSE/HEREDIA(R.VIRILLA)'),
	('5','40050','LTE PROV.SAN JOSE/HEREDIA(R.VIRILLA)-SANTO DOMINGO(R.103)(Av.2/C.3)'),
	('5','40060','SANTO DOMINGO(R.103)(Av.2/C.3)-LTE SANTO DOMINGO/SAN PABLO(R.BERMUDEZ)'),
	('5','40730','LTE SANTO D./S.P.(R.BERMUDEZ)-LTE S.P./HERED(0+100 ANTES ASILO ANCIANOS)'),
	('5','40740','LTE CANT.SAN PABLO/HEREDIA(0+100 ANTES ASILO ANCIANOS)-EL PIRRO(R.3)'),
	('6','21221','LTE PROV.GUANACASTE/ALAJUELA(QUEB.PICHARDO)-LLANO AZUL(R.730)'),
	('6','21222','LLANO AZUL(R.730)-UPALA(R.4)'),
	('6','50900','COROBICI(R.1)-LTE CANT.CAÑAS/BAGACES(R.TENORIO)'),
	('6','51020','LTE CANT.CAÑAS/BAGACES(R.TENORIO)-LTE PROV.GUANACASTE/ALAJ(QUEB.PICHARDO)'),
	('7','20340','LTE HERED/ALAJ(0+285 DESP R.AHOGADOS)-LTE ALAJ/HERED(1+270 DESP R.TAMBOR)'),
	('7','20580','LTE PROV.HEREDIA/ALAJUELA(0+425 MTS ANTES R.LA PAZ)-CARIBLANCO(ESCUELA)'),
	('7','20590','CARIBLANCO(ESCUELA)-SAN MIGUEL(R.140)'),
	('7','21800','SAN MIGUEL(R.140)-LTE PROV.ALAJUELA/HEREDIA(0+760 MTS DESP QUEB.GRANDE)'),
	('7','40070','HEREDIA(R.3)(Av.4/C.2)-LTE CANT.HEREDIA/BARVA(QUEB.SECA)'),
	('7','40080','LTE CANT.HEREDIA/BARVA(Q.SECA)-LTE CANT.BARVA/SANTA BARBARA(R.PORROSATI)'),
	('7','40090','LTE BARVA/S.B.(R.PORROSATI)-LTE HEREDIA/ALAJ(0+285 DESP R.LOS AHOGADOS)'),
	('7','40100','LTE STA BARBARA/HERED(R.DESENGAÑO)-LTE HERED/ALAJ(0+425 ANTES R.LA PAZ)'),
	('7','40481','LTE PROV.ALAJUELA/HEREDIA(0+760 MTS DESP QUEB.GRANDE)-LA VIRGEN(IGLESIA)'),
	('7','40482','LA VIRGEN(IGLESIA)-BAJOS DE CHILAMATE(R.4)(R.506)'),
	('7','40600','LTE ALAJ/HEREDIA(1+270 DESP R.TAMBOR)-LTE STA BARBARA/HERED(R.DESENGAÑO)'),
	('8','20661','CIUDAD QUESADA(R.141)-LA MARINA(R.748)'),
	('8','20662','LA MARINA(R.748)-AGUAS ZARCAS(R.250)'),
	('8','20931','AGUAS ZARCAS(R.250)-VENECIA(CUADRANTE IGLESIA)'),
	('8','20932','VENECIA(CUADRANTE IGLESIA)-LTE CANT.SAN CARLOS/GRECIA(R.TORO)(MARSELLA)'),
	('8','21560','LTE CANT.SAN CARLOS/GRECIA(R.TORO)-LTE CANT.GRECIA/ALAJUELA(R.SARDINAL)'),
	('8','21570','LTE CANT.GRECIA/ALAJUELA(R.SARDINAL)-SAN MIGUEL,SARAPIQUI(R.126)'),
	('9','20190','NARANJO(R.118)-EL MURO(R.148)'),
	('9','20440','SAN MIGUEL(R.1)(RADIAL NARANJO)-NARANJO(R.118)'),
	('9','20600','EL MURO(R.148)-LTE CANT.NARANJO/ALFARO RUIZ(R.ESPINO)'),
	('9','20610','LTE CANT.NARANJO/ALFARO RUIZ(R.ESPINO)-ZAPOTE(IGLESIA)'),
	('9','20620','ZAPOTE(IGLESIA)-LTE CANT.ALFARO RUIZ/SAN CARLOS(R.LA VIEJA)'),
	('9','20630','CIUDAD QUESADA(R.140)-FLORENCIA(R.35)'),
	('9','20681','FLORENCIA(R.35)-SANTA CLARA(IGLESIA)'),
	('9','20682','SANTA CLARA(IGLESIA)-JABILLOS(R.738)'),
	('9','20980','JABILLOS(R.738)-EL TANQUE(R.142)'),
	('9','21550','LTE CANT.ALFARO RUIZ/SAN CARLOS(R.LA VIEJA)(LAJAS)-CIUDAD QUESADA(R.140)'),
	('10','20921','SARCHI SUR(R.118)-PUENTE RIO TROJAS'),
	('10','20922','PUENTE RIO TROJAS-LOS ANGELES(ESCUELA)'),
	('10','20923','LOS ANGELES(ESCUELA)-BAJOS DEL TORO(IGLESIA)'),
	('10','20924','BAJOS DEL TORO(IGLESIA)-LTE CANT.VALVERDE VEGA/GRECIA(QUEB.GATA)'),
	('10','21541','LTE CANT.VALVERDE VEGA/GRECIA(QUEB.GATA)-COLONIA DEL TORO(IGLESIA)'),
	('10','21542','COLONIA DEL TORO(IGLESIA)-RIO CUARTO(R.140)')
go

-------------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[proyecto_estructura] ON   -- Apaga el auto incremento Identity
GO
insert into proyecto_estructura(id, idRuta, descripcion) values
	(1,'7','Sector-Las Torres del ICE'),
	(2,'8','Sector Puente los Negritos'),
	(3,'7','Mantenimiento y limpieza'),
	(4,'8','Mantenimiento y limpieza'),
	(5,'7','Rehabilitación'),
	(6,'8','Rehabilitación'),
	(7,'7','Mantenimiento'),
	(8,'8','Mantenimiento'),
	(9,'8','Proyecto-Bomba Delta'),
	(10,'7','Mejoramiento del sistema de drenajes de Cinchona'),
	(11,'9','Colocación de Tubería-Sector Grupo Q'),
	(12,'9','Puente Casa'),
	(13,'9','Cuneta revestida-Sector de Florencia'),
	(14,'9','Mejoramiento y manejo de aguas-Sectot Grupo Q'),
	(15,'9','Cuneta revestida-Puente Casa')
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
SET IDENTITY_INSERT [dbo].[contratoItem] ON   -- Apaga el auto incremento Identity
GO
insert into contratoItem (id, idContrato, idItem, precioUnitario, cantidadAprobada) VALUES
	(1,'1','1','28501.910','0'),
	(2,'1','2','5338.590','0'),
	(3,'1','3','1627.560','0'),
	(4,'1','4','39.220','0'),
	(5,'1','5','17550.200','0'),
	(6,'1','6','18305.150','0'),
	(7,'1','7','137.260','0'),
	(8,'1','8','56000.880','0'),
	(9,'1','9','61816.550','0'),
	(10,'1','10','713.490','0'),
	(11,'1','11','47362.060','0'),
	(12,'1','12','73897.700','0'),
	(13,'1','13','33752.270','0'),
	(14,'1','14','4696.390','0'),
	(15,'1','15','193086.780','0'),
	(16,'1','16','2848.230','0'),
	(17,'1','17','2196.690','0'),
	(18,'1','18','3008.510','0'),
	(19,'1','19','2041.660','0'),
	(20,'1','20','1966.680','0'),
	(21,'1','21','1972.450','0'),
	(22,'1','22','33374.790','0'),
	(23,'1','23','1294.670','0'),
	(24,'1','24','165201.410','0'),
	(25,'1','25','374.880','0'),
	(26,'1','26','19349.630','0'),
	(27,'1','27','6208.030','0'),
	(28,'1','28','6471.020','0'),
	(29,'1','29','11035.050','0'),
	(30,'1','30','17674.310','0'),
	(31,'1','31','83192.900','0'),
	(32,'1','32','133803.100','0'),
	(33,'1','33','150673.330','0'),
	(34,'1','34','122211.210','0'),
	(35,'1','35','120844.330','0'),
	(36,'1','36','131886.130','0'),
	(37,'1','37','157620.270','0'),
	(38,'1','38','240876.850','0'),
	(39,'1','39','344003.490','0'),
	(40,'1','40','613531.400','0'),
	(41,'1','41','999160.240','0'),
	(42,'1','42','20799.550','0'),
	(43,'1','43','875.490','0'),
	(44,'1','44','16565.360','0'),
	(45,'1','45','83012.500','0'),
	(46,'1','46','55160.570','0'),
	(47,'1','47','75431.540','0'),
	(48,'1','48','79657.310','0'),
	(49,'1','49','85152.780','0'),
	(50,'1','50','202400.850','0'),
	(51,'1','51','32771.810','0'),
	(52,'1','52','69220.330','0'),
	(53,'1','53','34379.760','0'),
	(54,'1','54','74480.490','0'),
	(55,'1','55','16506.300','0'),
	(56,'1','56','14447.340','0'),
	(57,'1','57','11402.150','0'),
	(58,'1','58','17371.870','0'),
	(59,'1','59','127783.090','0'),
	(60,'1','60','14501.320','0'),
	(61,'1','61','261752.870','0'),
	(62,'1','62','17409.010','0'),
	(63,'1','63','3085.560','0'),
	(64,'1','64','14416.420','0'),
	(65,'1','65','84140.600','0'),
	(66,'1','66','44825.160','0'),
	(67,'1','67','66155.310','0'),
	(68,'1','68','28090.010','0'),
	(69,'1','69','60489.360','0'),
	(70,'1','70','92251.290','0'),
	(71,'1','71','92251.290','0'),
	(72,'1','72','92251.290','0'),
	(73,'1','73','92251.290','0'),
	(74,'1','74','22730.940','0'),
	(75,'1','75','875.490','0'),
	(76,'1','76','3405233.580','0'),
	(77,'1','77','3405233.580','0'),
	(78,'1','78','4723685.450','0'),
	(79,'1','79','4723685.450','0'),
	(80,'1','80','3621019.700','0'),
	(81,'1','81','4775889.640','0'),
	(82,'1','82','4862896.640','0'),
	(83,'1','83','4862896.640','0'),
	(84,'1','84','5068239.730','0'),
	(85,'1','85','5068239.730','0'),
	(86,'1','86','5148288.350','0'),
	(87,'1','87','5148288.350','0'),
	(88,'1','88','13894080.410','0'),
	(89,'1','89','6768766.270','0'),
	(90,'1','90','14990390.480','0'),
	(91,'1','91','7865081.820','0'),
	(92,'1','92','15554204.580','0'),
	(93,'1','93','8428884.960','0'),
	(94,'1','94','15971235.470','0'),
	(95,'1','95','10066161.580','0'),
	(96,'1','96','22759567.360','0'),
	(97,'1','97','10884038.300','0'),
	(98,'1','98','11748300.500','0'),
	(99,'1','99','11748300.500','0'),
	(100,'1','100','474593.580','0'),
	(101,'1','101','538900.090','0'),
	(102,'1','102','344550.240','0'),
	(103,'1','103','552176.640','0'),
	(104,'1','104','347803.060','0'),
	(105,'1','105','922610.980','0'),
	(106,'1','106','1137060.210','0'),
	(107,'1','107','368311.930','0'),
	(108,'1','108','571185.990','0'),
	(109,'1','109','40862.030','0'),
	(110,'1','110','40862.030','0'),
	(111,'1','111','43861.080','0'),
	(112,'1','112','47004.310','0'),
	(113,'1','113','27850.770','0'),
	(114,'1','114','9758.440','0'),
	(115,'1','115','27383.620','0'),
	(116,'1','116','8749.150','0'),
	(117,'1','117','8749.150','0'),
	(118,'1','118','4446.670','0'),
	(119,'1','119','4446.670','0'),
	(120,'1','120','264248692.190','0')
go
SET IDENTITY_INSERT [dbo].[contratoItem] OFF   -- Eciende el auto incremento Identity
GO

--- Reajustes de 7 meses de los primeros 10 ítems
-------------------------------------------------------------------------------
INSERT INTO itemReajuste (idContratoItem,fecha,reajuste) VALUES 
	('1','2011-09-01','0.08794'),
	('2','2011-09-01','0.09226'),
	('3','2011-09-01','0.08433'),
	('4','2011-09-01','0.09612'),
	('5','2011-09-01','0.0876'),
	('6','2011-09-01','0.0899'),
	('7','2011-09-01','0.08201'),
	('8','2011-09-01','0.10737'),
	('9','2011-09-01','0.10561'),
	('10','2011-09-01','0.08165'),
	('1','2011-10-01','0.08132'),
	('2','2011-10-01','0.08781'),
	('3','2011-10-01','0.07768'),
	('4','2011-10-01','0.09335'),
	('5','2011-10-01','0.08128'),
	('6','2011-10-01','0.08408'),
	('7','2011-10-01','0.07322'),
	('8','2011-10-01','0.06751'),
	('9','2011-10-01','0.06886'),
	('10','2011-10-01','0.07178'),
	('1','2011-11-01','0.07848'),
	('2','2011-11-01','0.08599'),
	('3','2011-11-01','0.07513'),
	('4','2011-11-01','0.0923'),
	('5','2011-11-01','0.07866'),
	('6','2011-11-01','0.08159'),
	('7','2011-11-01','0.0695'),
	('8','2011-11-01','0.06626'),
	('9','2011-11-01','0.06768'),
	('10','2011-11-01','0.06741'),
	('1','2011-12-01','0.08196'),
	('2','2011-12-01','0.08881'),
	('3','2011-12-01','0.08015'),
	('4','2011-12-01','0.0944'),
	('5','2011-12-01','0.08245'),
	('6','2011-12-01','0.08467'),
	('7','2011-12-01','0.07433'),
	('8','2011-12-01','0.06931'),
	('9','2011-12-01','0.0706'),
	('10','2011-12-01','0.07177'),
	('1','2012-01-01','0.09862'),
	('2','2012-01-01','0.10543'),
	('3','2012-01-01','0.09392'),
	('4','2012-01-01','0.11137'),
	('5','2012-01-01','0.09835'),
	('6','2012-01-01','0.1016'),
	('7','2012-01-01','0.08972'),
	('8','2012-01-01','0.0798'),
	('9','2012-01-01','0.08137'),
	('10','2012-01-01','0.08865'),
	('1','2012-02-01','0.08524'),
	('2','2012-02-01','0.09634'),
	('3','2012-02-01','0.08022'),
	('4','2012-02-01','0.10567'),
	('5','2012-02-01','0.08549'),
	('6','2012-02-01','0.08984'),
	('7','2012-02-01','0.07193'),
	('8','2012-02-01','0.09942'),
	('9','2012-02-01','0.09964'),
	('10','2012-02-01','0.06887'),
	('1','2012-03-01','0.03'),
	('2','2012-03-01','0.03'),
	('3','2012-03-01','0.03'),
	('4','2012-03-01','0.03'),
	('5','2012-03-01','0.03'),
	('6','2012-03-01','0.03'),
	('7','2012-03-01','0.03'),
	('8','2012-03-01','0.03'),
	('9','2012-03-01','0.03'),
	('10','2012-03-01','0.03')
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
INSERT INTO boletaItem (idBoleta,redimientos,cantidad,idContratoItem) VALUES 
	('1','0','88.96','1'),
	('2','0','160.6','1'),
	('3','0','222.8','1'),
	('4','0','44','1'),
	('5','0','401.8','1'),
	('6','0','153','1'),
	('7','0','13','1'),
	('8','3.672868217','473.8','2'),
	('9','1.102170543','142.18','3'),
	('10','0','60','1'),
	('11','0','86','1'),
	('12','0','106','1'),
	('13','0','86','1'),
	('14','1.781','17.81','4'),
	('14','0.271','2.71','5'),
	('14','1.129','11.29','1'),
	('14','1','6','6'),
	('15','0','29.61','7'),
	('15','0','1123.92','8'),
	('15','0','37.11','9')
go

/*
select * from (select ci.idItem, ci.precioUnitario, ir.fecha, ir.reajuste, ir.precioReajustado from itemReajuste ir inner join contratoItem ci on ir.idContratoItem = ci.id )as a inner join item i on a.idItem = i.id order by i.codigoItem
select * from itemReajuste where fecha = '2012-03-01'

select * from boletaItem
select * from contratoItem

-- Muestra el indentity sin reiniciarlo
DBCC CHECKIDENT ('boleta', NORESEED),

-- Reinicia el identity
DBCC CHECKIDENT ('boleta', reseed, 1),
*/
/*
	Cantidades con 3 decimales
	Precios con 4 decimales
*/

create database Proyecto_IAA
go

 use Proyecto_IAA
 go

 -- Regla de Formato de Teléfono
Create rule Rtelefono as (@telefono like '[1-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]')
go

-- Tipo de datos teléfono
EXEC sp_addtype 'TTelefono', 'varchar (14)', 'not null'
go
EXEC sp_bindrule 'RTelefono','TTelefono'
go

create table persona(
	id int unique not null identity (1,1),
	nombre varchar(50) not null,
	apellido1 varchar(50) not null,
	apellido2 varchar(50) not null,
	cedula varchar(50) not null,
	constraint pk_id_persona
        primary key (id)
)
go

create table telefono(
	idPersona int not null,
	telefono TTelefono,
	constraint pk_idPersonaTelefono_telefono
        primary key (idPersona,telefono),
	constraint fk_id_telefono
        foreign key (idPersona) references persona
)
go

create table usuario(
	idPersona int not null,
	nombreUsuario varchar(30) unique not null,
	contrasena varchar(30) not null,
	tipo tinyint,
	constraint pk_idPersona_usuario
        primary key (idPersona),
	constraint fk_idPersona_usuario
        foreign key (idPersona) references persona
)
go

create table ingeniero(
	idPersona int not null,
	constraint pk_idPersona_ingeniero
        primary key (idPersona),
	constraint fk_idPersona_ingeniero
        foreign key (idPersona) references persona
)
go

create table contratista(
	id int unique not null identity (1,1),
	nombre varchar(50) not null,
	descripcion varchar(60),
	constraint pk_id_contratista
        primary key (id)
)
go

create table inspector(
	idPersona int not null,
	constraint pk_idPersona_inspector
        primary key (idPersona),
	constraint fk_idPersona_inspector
        foreign key (idPersona) references persona
)
go

create table fondo(
	id smallint unique not null identity (1,1),
	nombre varchar(20) not null,
	constraint pk_id_fondo
        primary key (id)
)
go

create table zona(
	id smallint unique not null identity (1,1),
	nombre varchar(20) not null,
	constraint pk_id_zona
        primary key (id)
)
go

create table Contrato
(
	id int unique not null identity (1,1),
	idContratista int not null,
	licitacion varchar(50) not null,
	lineaContrato smallint not null,
	idZona smallint not null,
	fechaInicio date not null,
	plazo smallint not null,
	lugar varchar(100) not null,
	idFondo smallint not null,
	constraint pk_id_Contrato
        primary key (id),
	constraint fk_idContratista_Contrato
        foreign key (idContratista) references contratista,
	constraint fk_idZona_Contrato
        foreign key (idZona) references zona,
	constraint fk_idFondo_Contrato
        foreign key (idFondo) references fondo
)
go

--alter table ingenieroContrato add activo bit default 1
create table ingenieroContrato(
	idContrato int not null,
	idIngeniero int not null,
	descripcion varchar(150) not null,
	departamento varchar(150) not null,
	rol varchar(150) not null,
	fechaInicio date,
	fechaFin date,
	activo bit default 1,
	constraint pk_idContrato_idIngeniero_ingenieroContrato
        primary key (idContrato,idIngeniero),
    constraint fk_idContrato_ingenieroContrato
        foreign key (idContrato) references Contrato,
    constraint fk_idIngeniero_ingenieroContrato
        foreign key (idIngeniero) references ingeniero
)
go

create table ruta(
	id int unique not null identity (1,1),
	nombre varchar(20),
	descripcion varchar(100),
	constraint pk_id_ruta
        primary key (id)
)
go

create table zonaRuta(
	idZona smallint not null,
	idRuta int not null,
	constraint pk_idZona_idRuta_zonaRuta
		primary key(idZona, idRuta),
	constraint fk_idZona_zonaRuta
        foreign key (idZona) references zona,
	constraint fk_idRuta_zonaRuta
        foreign key (idRuta) references ruta
)
go

create table proyecto_estructura(
	id int not null identity(1,1),
	idRuta int not null,
	descripcion varchar(150) not null,
	constraint pk_id_proyecto_estructura
		primary key(id),
	constraint fk_idRuta_proyecto_estructura
        foreign key (idRuta) references ruta
)
go

create table laboratorioCalidad(
	id smallint unique not null identity (1,1),
	nombre varchar(30) not null,
	tipo varchar(30) not null,
	constraint pk_id_laboratorioCliente
        primary key (id)
)
go

create table labCalidadContrato(
	idLabCalidad smallint not null,
	idContrato int not null,
	constraint pk_idLabControl_idContrato_labCalidadContrato
        primary key (idLabCalidad,idContrato),
    constraint fk_idLabControl_labCalidadContrato
        foreign key (idLabCalidad) references laboratorioCalidad,
    constraint fk_idContrato_labCalidadContrato
        foreign key (idContrato) references Contrato
)
go

create table programa(
	id int identity(1,1) not null,
	idContrato int not null,
	ano smallint not null,
	trimestre tinyint not null,
	fechaInicio	date not null,
	fechaFin date not null,
	monto money not null,
	constraint pk_id_programa
		primary key (id),
	constraint uq_idContrato_ano_trimestre_programa
        unique (idContrato, ano, trimestre),
	constraint fk_idContrato_programa
        foreign key (idContrato) references Contrato
)
go

create table tipoProyecto(
	id int unique not null identity (1,1),
	nombre varchar(50) not null,
	constraint pk_id_TipoProyecto
        primary key (id)
)
go

create table proyecto
(
	id int unique not null identity (1,1),
	idPrograma int not null,
	idTipoProyecto int not null,
	idRuta int not null,
	nombre varchar(50),
	constraint pk_id_subProyecto
        primary key (id),
	constraint fk_idPrograma_proyecto
        foreign key (idPrograma) references programa,
	constraint fk_idTipoProyecto_proyecto
        foreign key (idTipoProyecto) references tipoProyecto,
	constraint fk_idRuta_ruta
        foreign key (idRuta) references ruta
)
go

---alter table item add unique (codigoItem)
create table item(
	id int unique not null identity (1,1),
	codigoItem varchar(25) unique not null,
	descripcion varchar(100) not null,
	unidadMedida varchar(10) not null,
	constraint pk_id_item
        primary key (id)
)
go

---
create table contratoItem(
	id int not null identity(1,1),
	idContrato int not null,
	idItem int not null,
	precioUnitario money not null,
	CONSTRAINT uq_idContrato_idItem UNIQUE(idContrato, idItem),
	constraint pk_id_contratoItem
		primary key(id),
	constraint fk_id_contratoItem_idContrato
		foreign key (idContrato) references contrato,
	constraint fk_id_contratoItem_idItem
		foreign key (idItem) references item
)
go

-- Recordar, los porcentajes se convierten a decimal para almacenarlos en la BD  Ej: 8.7941% -> 0.087941; el 100% equivale a 1
create table itemReajuste(
	id int not null identity(1,1),
	idContratoItem int not null,
	fecha date not null,
	mes AS MONTH(fecha),
	ano AS YEAR(fecha),
	reajuste decimal(7,6) not null,
	precioReajustado money not null,
	constraint pk_id_itemReajuste
		primary key (id),
	constraint fk_idContratoItem_itemReajuste
		foreign key (idContratoItem) references contratoItem,
	constraint uq_idContratoItem_ano_mes_itemReajuste
		unique (idContratoItem, mes, ano)
)
go

---
create table proyectoItemReajuste(	
	id int unique not null identity (1,1),
	idProyecto int not null,
	idContratoItem int not null,
	fechaInicio date not null,
	fechaFin date not null,
	costoEstimado money not null,
	CONSTRAINT uq_idProyecto_IdContratoItem UNIQUE (idProyecto, idContratoItem),
	constraint pk_id_ProyectoItem
        primary key (id),
    constraint fk_idProyecto_proyectoItem
        foreign key (idProyecto) references proyecto,
    constraint fk_idContratoItem_proyectoItem
        foreign key (idContratoItem) references contratoItem
)
go

--ALTER TABLE [dbo].[boleta] ALTER COLUMN seccionControl int not null;
create table boleta(
	id int unique not null identity (1,1),
	idContrato int not null,
	numeroBoleta int unique not null,
	idFondo smallint not null,
	idRuta int not null,
	idInspector int not null,
	fecha date not null,
	seccionControl int not null,
	estacionamientoInicial varchar(10) not null,
	estacionamientoFinal varchar(10) not null,
	periodo tinyint not null,
	idProyecto_Estructura int not null,
	observaciones varchar(150)
	constraint pk_id_boleta
        primary key (id),
    constraint fk_idFondo_boleta
        foreign key (idFondo) references fondo,
    constraint fk_idRuta_boleta
        foreign key (idRuta) references ruta,
	 constraint fk_idInspector_boleta
        foreign key (idInspector) references inspector,
	constraint fk_idProyecto_Estructura_proyecto_estructura
        foreign key (idProyecto_Estructura) references proyecto_estructura,
	constraint fk_idContrato_boleta
        foreign key (idContrato) references Contrato
)
go

---
create table boletaItem(
	idContratoItem int not null,
	idBoleta int not null,
	cantidad decimal(10,3) not null,
	costoTotal money not null,
	redimientos decimal(10,3) not null,
	precioUnitarioFecha money not null,
	constraint pk_idItemReajuste_idBoleta_boletaItem
        primary key (idContratoItem,idBoleta),
    constraint fk_idContratoItem_boletaItem
        foreign key (idContratoItem) references contratoItem,
    constraint fk_idBoleta_boletaItem
        foreign key (idBoleta) references boleta
)
go

/*Hasta aquí*/
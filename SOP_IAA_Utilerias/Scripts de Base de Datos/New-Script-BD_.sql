create database Proyecto_IAA
go

 use Proyecto_IAA
 go

--tipo de datos teléfono
Create rule Rtelefono as (@telefono like '[1-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]')
go
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
	descripcion varchar(100) not null,
	departamento varchar(100) not null,
	rol varchar(100) not null,
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

create table ingenieroContrato(
	idContrato int not null,
	idIngeniero int not null,
	fechaInicio date,
	fechaFin date,
	constraint pk_idContrato_idIngeniero_ingenieroContrato
        primary key (idContrato,idIngeniero),
    constraint fk_idContrato_ingenieroContrato
        foreign key (idContrato) references Contrato,
    constraint fk_idIngeniero_ingenieroContrato
        foreign key (idIngeniero) references ingeniero
)
go

-----
create table ruta(
	id int unique not null identity (1,1),
	nombre varchar(20),
	descripcion varchar(100),
	constraint pk_id_ruta
        primary key (id)
)
go

-----
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

-----
create table proyecto_estructura(
	id int not null,
	idRuta int not null,
	descripcion varchar(100) not null,
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

create table progProy(
	id int unique not null identity (1,1),
	fechaInicio date,
	fechaFin date,
	monto int not null,
	constraint pk_idProgProy
        primary key (id),
)
go

create table programa(
	idContrato int not null,
	ano smallint not null,
	trimestre tinyint not null,
	idProgProy int not null,
	constraint pk_idContrato_ano_trimestre_programa
        primary key (idContrato, ano, trimestre),
	constraint fk_idContrato_programa
        foreign key (idContrato) references Contrato,
	constraint fk_idProgProy_progProy
        foreign key (idProgProy) references progProy
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
	idProgProy int not null,
	idTipoProyecto int not null,
	idRuta int not null,
	nombre varchar(50),
	constraint pk_id_subProyecto
        primary key (id),
	constraint fk_idProgProy_proyecto
        foreign key (idProgProy) references progproy,
	constraint fk_idTipoProyecto_proyecto
        foreign key (idTipoProyecto) references tipoProyecto,
	constraint fk_idRuta_ruta
        foreign key (idRuta) references ruta
)
go

---------
---alter table item add unique (codigoItem)
create table item(
	id int unique not null identity (1,1),
	codigoItem varchar(25) unique not null,
	descripcion varchar(100) not null,
	unidadMedida varchar(10) not null,
	--precioUnitario int not null,
	constraint pk_id_item
        primary key (id)
)
go

---- <<
create table contratoItem(
	idContrato int not null,
	idItem int not null,
	precioUnitario int not null,
	constraint pk_id_contratoItem_idContrato_idItem
		primary key(idContrato, idItem),
	constraint fk_id_contratoItem_idContrato
		foreign key (idContrato) references contrato,
	constraint fk_id_contratoItem_idItem
		foreign key (idItem) references item
)
go

---------
create table proyectoItem(	
	id int unique not null identity (1,1),
	idProyecto int not null,
	idItem int not null,
	fechaInicio date not null,
	fechaFin date not null,
	costoEstimado int not null,
	constraint pk_id_ProyectoItem
        primary key (id),
    constraint fk_idProyecto_proyectoItem
        foreign key (idProyecto) references proyecto,
    constraint fk_idItem_proyectoItem
        foreign key (idItem) references item
)
go


create table boleta(
	id int unique not null identity (1,1),
	numeroBoleta int unique not null,
	idFondo smallint not null,
	idRuta int not null,
	idInspector int not null,
	fecha date not null,
	seccionControl smallint not null,
	estacionamientoInicial varchar(10) not null,
	estacionamientoFinal varchar(10) not null,
	periodo tinyint not null,
	idProyecto_Estructura int not null,
	observaciones varchar(100)
	constraint pk_id_boleta
        primary key (id),
    constraint fk_idFondo_boleta
        foreign key (idFondo) references fondo,
    constraint fk_idRuta_boleta
        foreign key (idRuta) references ruta,
	 constraint fk_idInspector_boleta
        foreign key (idInspector) references inspector,
	constraint fk_idProyecto_Estructura_proyecto_estructura
        foreign key (idProyecto_Estructura) references proyecto_estructura
)
go

---------
create table boletaItem(
	idProyectoItem int not null,
	idBoleta int not null,
	cantidad int not null,
	costoActual int not null,
	constraint pk_idItem_idBoleta_boletaItem
        primary key (idProyectoItem,idBoleta),
    constraint fk_idProyectoItem_boletaItem
        foreign key (idProyectoItem) references proyectoItem,
    constraint fk_idBoleta_boletaItem
        foreign key (idBoleta) references boleta
)
go

/*Hasta aquí*/


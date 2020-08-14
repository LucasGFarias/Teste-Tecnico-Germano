create database api

use api

create table marcas
(
	id int(11) AUTO_INCREMENT not null,
    nome varchar(50) not null,
    constraint id_pk primary key (id)
);

create unique index	indiceNomeMarca on marcas(nome);

create table patrimonios
(
	numeroTombo int,
    nome nvarchar(50) not null,
    idMarca int not null,
    descricao nvarchar(150),
    constraint nTombo_pk primary key(numeroTombo),
    constraint FK_marcas_marcaId foreign key (idMarca) references marcas(id)    
);

select * from patrimonios

select * from marcas

insert into marcas (nome) values ('Fiat') 

insert into patrimonios (numeroTombo,nome,idMarca,descricao) values (1,'Ka',1,'Carro utilizado para entregas') 
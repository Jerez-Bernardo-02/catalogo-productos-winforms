-- ========================================
-- CREAR BASE DE DATOS
-- ========================================
USE master;
GO

CREATE DATABASE CATALOGO_PRODUCTOS;
GO

USE CATALOGO_PRODUCTOS;
GO

-- ========================================
-- CONFIGURACIÓN
-- ========================================
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
GO

-- ========================================
-- TABLAS
-- ========================================

CREATE TABLE MARCAS (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion VARCHAR(50)
);

CREATE TABLE CATEGORIAS (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion VARCHAR(50)
);

CREATE TABLE ARTICULOS (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Codigo VARCHAR(50),
    Nombre VARCHAR(50),
    Descripcion VARCHAR(150),
    IdMarca INT,
    IdCategoria INT,
    Precio MONEY,

    CONSTRAINT FK_Articulos_Marcas 
        FOREIGN KEY (IdMarca) REFERENCES MARCAS(Id),

    CONSTRAINT FK_Articulos_Categorias 
        FOREIGN KEY (IdCategoria) REFERENCES CATEGORIAS(Id)
);

CREATE TABLE IMAGENES (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdArticulo INT,
    ImagenUrl VARCHAR(1000),

    CONSTRAINT FK_Imagenes_Articulos 
        FOREIGN KEY (IdArticulo) REFERENCES ARTICULOS(Id)
);

-- ========================================
-- DATOS
-- ========================================

INSERT INTO MARCAS VALUES 
('Samsung'),
('Apple'),
('Sony'),
('Huawei'),
('Motorola');

INSERT INTO CATEGORIAS VALUES 
('Celulares'),
('Televisores'),
('Media'),
('Audio');

INSERT INTO ARTICULOS VALUES
('S01', 'Galaxy S10', 'Una canoa cara', 1, 1, 69999),
('M03', 'Moto G Play 7ma Gen', 'Ya siete de estos?', 5, 1, 15699),
('S99', 'Play 4', 'Ya no se cuantas versiones hay', 3, 3, 35000),
('S56', 'Bravia 55', 'Alta tele', 3, 2, 49500),
('A23', 'Apple TV', 'lindo loro', 2, 3, 7850);

INSERT INTO IMAGENES VALUES
(1,'https://images.samsung.com/is/image/samsung/co-galaxy-s10-sm-g970-sm-g970fzyjcoo-frontcanaryyellow-thumb-149016542'),
(2,'https://www.motorola.cl/arquivos/moto-g7-play-img-product.png?v=636862863804700000'),
(2,'https://i.blogs.es/9da288/moto-g7-/1366_2000.jpg'),
(3,'https://www.euronics.cz/image/product/800x800/532620.jpg'),
(4,'https://intercompras.com/product_thumb_keepratio_2.php?img=images/product/SONY_KDL-55W950A.jpg&w=650&h=450'),
(5,'https://cnnespanol2.files.wordpress.com/2015/12/gadgets-mc3a1s-populares-apple-tv-2015-18.jpg');
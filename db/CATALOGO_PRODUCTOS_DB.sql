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
(2,'https://fdn2.gsmarena.com/vv/pics/motorola/motorola-moto-g7-1.jpg'),
(2,'https://i.blogs.es/9da288/moto-g7-/1366_2000.jpg'),
(3,'https://i5.walmartimages.com/asr/230f7166-6198-47e2-9d7c-6765dd55eaa0_1.1814fe6b8a1755f07a4d7d4aaf92c4b7.jpeg'),
(4,'https://i5.walmartimages.com/seo/Sony-55-class-BRAVIA-7-Mini-LED-QLED-4K-HDR-Smart-Google-TV-K55XR70-2024-Model_726d0d4b-a029-48c4-883a-9cd10ce2ca05.d8ed3dbe4169d0e8846a4f403f4ae02d.jpeg'),
(5,'https://images.macrumors.com/article-new/2022/10/apple-tv-4k-yellow-bg-feature.jpg');
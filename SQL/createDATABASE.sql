-- ============================================================
-- XÓA DATABASE CŨ NẾU CÓ (tùy chọn)
-- ============================================================
-- USE master;
-- GO
-- ALTER DATABASE STUDY_TRACKER SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
-- GO
-- DROP DATABASE STUDY_TRACKER;
-- GO

-- ============================================================
-- STUDY TRACKER - DATABASE SCHEMA (SQL SERVER - FIXED)
-- ============================================================

-- Tạo database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'STUDY_TRACKER')
BEGIN
    CREATE DATABASE STUDY_TRACKER;
END
GO

USE STUDY_TRACKER;
GO

-- ============================================================
-- BẢNG 1: NGUOI_DUNG
-- ============================================================
IF OBJECT_ID('dbo.GHI_CHU', 'U') IS NOT NULL DROP TABLE dbo.GHI_CHU;
IF OBJECT_ID('dbo.PHIEN_HOC', 'U') IS NOT NULL DROP TABLE dbo.PHIEN_HOC;
IF OBJECT_ID('dbo.NHIEM_VU', 'U') IS NOT NULL DROP TABLE dbo.NHIEM_VU;
IF OBJECT_ID('dbo.DIEM_SO', 'U') IS NOT NULL DROP TABLE dbo.DIEM_SO;
IF OBJECT_ID('dbo.COT_DIEM', 'U') IS NOT NULL DROP TABLE dbo.COT_DIEM;
IF OBJECT_ID('dbo.MON_HOC', 'U') IS NOT NULL DROP TABLE dbo.MON_HOC;
IF OBJECT_ID('dbo.NGUOI_DUNG', 'U') IS NOT NULL DROP TABLE dbo.NGUOI_DUNG;
GO

CREATE TABLE NGUOI_DUNG (
    MA_NGUOI_DUNG          INT IDENTITY(1,1),
    HO_TEN                 NVARCHAR(100) NOT NULL,
    EMAIL                  NVARCHAR(150) NOT NULL,
    MAT_KHAU_MA_HOA        NVARCHAR(255) NOT NULL,
    ANH_DAI_DIEN           NVARCHAR(255) NULL,
    GPA_MUC_TIEU           DECIMAL(3,2) DEFAULT 3.60,
    NGAY_TAO               DATETIME DEFAULT GETDATE(),
    LAN_CAP_NHAT_CUOI      DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT PK_NGUOI_DUNG PRIMARY KEY (MA_NGUOI_DUNG),
    CONSTRAINT UQ_NGUOI_DUNG_EMAIL UNIQUE (EMAIL)
);
GO

-- ============================================================
-- BẢNG 2: MON_HOC
-- ============================================================
CREATE TABLE MON_HOC (
    MA_MON_HOC             NVARCHAR(20),
    MA_NGUOI_DUNG          INT NOT NULL,
    TEN_MON_HOC            NVARCHAR(200) NOT NULL,
    SO_TIN_CHI             INT NOT NULL,
    TEN_GIANG_VIEN         NVARCHAR(100) NOT NULL,
    HINH_THUC_THI          NVARCHAR(20) NOT NULL,
    HOC_KY                 NVARCHAR(20) NULL,
    NGAY_TAO               DATETIME DEFAULT GETDATE(),
    NGAY_CAP_NHAT          DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT PK_MON_HOC PRIMARY KEY (MA_MON_HOC),
    CONSTRAINT FK_MON_HOC_NGUOI_DUNG FOREIGN KEY (MA_NGUOI_DUNG) 
        REFERENCES NGUOI_DUNG(MA_NGUOI_DUNG) ON DELETE CASCADE,
    CONSTRAINT CHK_MON_HOC_SO_TIN_CHI CHECK (SO_TIN_CHI > 0),
    CONSTRAINT CHK_MON_HOC_HINH_THUC CHECK (
        HINH_THUC_THI IN (N'TỰ_LUẬN', N'TRẮC_NGHIỆM', N'BÀI_TẬP_LỚN', N'KHÁC')
    )
);
GO

-- ============================================================
-- BẢNG 3: COT_DIEM
-- ============================================================
CREATE TABLE COT_DIEM (
    MA_COT_DIEM            INT IDENTITY(1,1),
    MA_MON_HOC             NVARCHAR(20) NOT NULL,
    TEN_COT_DIEM           NVARCHAR(100) NOT NULL,
    TRONG_SO               DECIMAL(5,2) NOT NULL,
    THU_TU                 INT DEFAULT 0,
    NGAY_TAO               DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT PK_COT_DIEM PRIMARY KEY (MA_COT_DIEM),
    CONSTRAINT FK_COT_DIEM_MON_HOC FOREIGN KEY (MA_MON_HOC) 
        REFERENCES MON_HOC(MA_MON_HOC) ON DELETE CASCADE,
    CONSTRAINT CHK_COT_DIEM_TRONG_SO CHECK (TRONG_SO > 0 AND TRONG_SO <= 100)
);
GO

-- ============================================================
-- BẢNG 4: DIEM_SO
-- ============================================================
CREATE TABLE DIEM_SO (
    MA_DIEM                INT IDENTITY(1,1),
    MA_COT_DIEM            INT NOT NULL,
    GIA_TRI                DECIMAL(5,2) NULL,
    NGAY_NHAP              DATETIME DEFAULT GETDATE(),
    GHI_CHU                NVARCHAR(255) NULL,
    
    CONSTRAINT PK_DIEM_SO PRIMARY KEY (MA_DIEM),
    CONSTRAINT FK_DIEM_SO_COT_DIEM FOREIGN KEY (MA_COT_DIEM) 
        REFERENCES COT_DIEM(MA_COT_DIEM) ON DELETE CASCADE,
    CONSTRAINT CHK_DIEM_SO_GIA_TRI CHECK (
        GIA_TRI IS NULL OR (GIA_TRI >= 0 AND GIA_TRI <= 10)
    )
);
GO

-- ============================================================
-- BẢNG 5: NHIEM_VU
-- ĐÃ SỬA: ON DELETE NO ACTION thay vì CASCADE
-- ============================================================
CREATE TABLE NHIEM_VU (
    MA_NHIEM_VU            INT IDENTITY(1,1),
    MA_NGUOI_DUNG          INT NOT NULL,
    MA_MON_HOC             NVARCHAR(20) NULL,
    TIEU_DE                NVARCHAR(200) NOT NULL,
    MO_TA                  NVARCHAR(MAX) NULL,
    THOI_HAN               DATETIME NOT NULL,
    TRANG_THAI             NVARCHAR(20) DEFAULT N'CHƯA_HOAN_THANH',
    NGAY_TAO               DATETIME DEFAULT GETDATE(),
    NGAY_HOAN_THANH        DATETIME NULL,
    
    CONSTRAINT PK_NHIEM_VU PRIMARY KEY (MA_NHIEM_VU),
    CONSTRAINT FK_NHIEM_VU_NGUOI_DUNG FOREIGN KEY (MA_NGUOI_DUNG) 
        REFERENCES NGUOI_DUNG(MA_NGUOI_DUNG) ON DELETE NO ACTION,
    CONSTRAINT FK_NHIEM_VU_MON_HOC FOREIGN KEY (MA_MON_HOC) 
        REFERENCES MON_HOC(MA_MON_HOC) ON DELETE SET NULL,
    CONSTRAINT CHK_NHIEM_VU_TRANG_THAI CHECK (
        TRANG_THAI IN (N'CHƯA_HOAN_THANH', N'DA_HOAN_THANH')
    )
);
GO

-- ============================================================
-- BẢNG 6: PHIEN_HOC
-- ĐÃ SỬA: ON DELETE NO ACTION thay vì CASCADE
-- ============================================================
CREATE TABLE PHIEN_HOC (
    MA_PHIEN               INT IDENTITY(1,1),
    MA_NGUOI_DUNG          INT NOT NULL,
    MA_MON_HOC             NVARCHAR(20) NULL,
    THOI_GIAN_BAT_DAU      DATETIME NOT NULL,
    THOI_GIAN_KET_THUC     DATETIME NULL,
    THOI_LUONG_PHUT        INT NULL,
    NGAY_TAO               DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT PK_PHIEN_HOC PRIMARY KEY (MA_PHIEN),
    CONSTRAINT FK_PHIEN_HOC_NGUOI_DUNG FOREIGN KEY (MA_NGUOI_DUNG) 
        REFERENCES NGUOI_DUNG(MA_NGUOI_DUNG) ON DELETE NO ACTION,
    CONSTRAINT FK_PHIEN_HOC_MON_HOC FOREIGN KEY (MA_MON_HOC) 
        REFERENCES MON_HOC(MA_MON_HOC) ON DELETE SET NULL
);
GO

-- ============================================================
-- BẢNG 7: GHI_CHU
-- ĐÃ SỬA: ON DELETE NO ACTION thay vì CASCADE
-- ============================================================
CREATE TABLE GHI_CHU (
    MA_GHI_CHU             INT IDENTITY(1,1),
    MA_NGUOI_DUNG          INT NOT NULL,
    MA_MON_HOC             NVARCHAR(20) NULL,
    TIEU_DE                NVARCHAR(200) NOT NULL,
    NOI_DUNG               NVARCHAR(MAX) NULL,
    TU_KHOA                NVARCHAR(255) NULL,
    LIEN_KET_TAI_LIEU      NVARCHAR(MAX) NULL,
    NGAY_TAO               DATETIME DEFAULT GETDATE(),
    NGAY_CAP_NHAT          DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT PK_GHI_CHU PRIMARY KEY (MA_GHI_CHU),
    CONSTRAINT FK_GHI_CHU_NGUOI_DUNG FOREIGN KEY (MA_NGUOI_DUNG) 
        REFERENCES NGUOI_DUNG(MA_NGUOI_DUNG) ON DELETE NO ACTION,
    CONSTRAINT FK_GHI_CHU_MON_HOC FOREIGN KEY (MA_MON_HOC) 
        REFERENCES MON_HOC(MA_MON_HOC) ON DELETE SET NULL
);
GO


PRINT N'Tạo database STUDY_TRACKER thành công!';
GO
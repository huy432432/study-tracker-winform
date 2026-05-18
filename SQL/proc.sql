USE STUDY_TRACKER;
GO

-- ==============================================================================
-- 1. DỌN DẸP DỮ LIỆU
-- ==============================================================================
DELETE FROM GHI_CHU WHERE MA_NGUOI_DUNG = 1;
DELETE FROM PHIEN_HOC WHERE MA_NGUOI_DUNG = 1;
DELETE FROM NHIEM_VU WHERE MA_NGUOI_DUNG = 1;
DELETE FROM DIEM_SO WHERE MA_COT_DIEM IN (SELECT MA_COT_DIEM FROM COT_DIEM WHERE MA_MON_HOC IN (SELECT MA_MON_HOC FROM MON_HOC WHERE MA_NGUOI_DUNG = 1));
DELETE FROM COT_DIEM WHERE MA_MON_HOC IN (SELECT MA_MON_HOC FROM MON_HOC WHERE MA_NGUOI_DUNG = 1);
DELETE FROM MON_HOC WHERE MA_NGUOI_DUNG = 1;

-- ==============================================================================
-- 2. THÊM MÔN HỌC CHO 3 HỌC KỲ (Mỗi kỳ 3-4 môn)
-- ==============================================================================

-- HỌC KỲ 2024.2 (Kỳ trước nữa - ĐÃ HOÀN THÀNH)
INSERT INTO MON_HOC (MA_MON_HOC, TEN_MON_HOC, SO_TIN_CHI, HOC_KY, MA_NGUOI_DUNG, TEN_GIANG_VIEN, HINH_THUC_THI) VALUES
('SQL101', N'Cơ sở dữ liệu 1', 3, N'2024.2', 1, N'Thầy Minh', N'TRẮC_NGHIỆM'),
('NET101', N'Mạng máy tính cơ bản', 3, N'2024.2', 1, N'Cô Thủy', N'TỰ_LUẬN'),
('PRO101', N'Lập trình C cơ bản', 4, N'2024.2', 1, N'Thầy Hoàng', N'BÀI_TẬP_LỚN');

-- HỌC KỲ 2025.1 (Kỳ vừa xong - ĐÃ HOÀN THÀNH)
INSERT INTO MON_HOC (MA_MON_HOC, TEN_MON_HOC, SO_TIN_CHI, HOC_KY, MA_NGUOI_DUNG, TEN_GIANG_VIEN, HINH_THUC_THI) VALUES
('WIN201', N'Lập trình WinForms', 4, N'2025.1', 1, N'Thầy Hải Dương', N'BÀI_TẬP_LỚN'),
('DBI202', N'Hệ quản trị CSDL', 3, N'2025.1', 1, N'Cô Lan Anh', N'TRẮC_NGHIỆM'),
('OSG202', N'Hệ điều hành', 2, N'2025.1', 1, N'Thầy Nam', N'TỰ_LUẬN');

-- HỌC KỲ 2025.2 (Kỳ hiện tại - ĐANG HỌC)
INSERT INTO MON_HOC (MA_MON_HOC, TEN_MON_HOC, SO_TIN_CHI, HOC_KY, MA_NGUOI_DUNG, TEN_GIANG_VIEN, HINH_THUC_THI) VALUES
('MLG301', N'Machine Learning', 3, N'2025.2', 1, N'Thầy Tuấn Huy', N'TỰ_LUẬN'),
('DSV301', N'Data Science với Python', 3, N'2025.2', 1, N'Cô Linh', N'BÀI_TẬP_LỚN'),
('SWE301', N'Kỹ thuật phần mềm', 3, N'2025.2', 1, N'Thầy Bình', N'BÀI_TẬP_LỚN');

-- ==============================================================================
-- 3. THÊM CỘT ĐIỂM & ĐIỂM SỐ CHO TẤT CẢ CÁC MÔN
-- ==============================================================================

-- ========== KỲ 2024.2 (Đã xong - Có điểm đầy đủ) ==========

-- SQL101: Điểm cao (Xuất sắc)
INSERT INTO COT_DIEM (MA_MON_HOC, TEN_COT_DIEM, TRONG_SO, THU_TU) VALUES 
('SQL101', N'Giữa kỳ', 40, 1),
('SQL101', N'Cuối kỳ', 60, 2);
DECLARE @CotDiemSQL101_GK INT = SCOPE_IDENTITY() - 1;  -- Lấy MA_COT_DIEM của Giữa kỳ
DECLARE @CotDiemSQL101_CK INT = SCOPE_IDENTITY();      -- Lấy MA_COT_DIEM của Cuối kỳ
INSERT INTO DIEM_SO (MA_COT_DIEM, GIA_TRI) VALUES 
(@CotDiemSQL101_GK, 9.0),
(@CotDiemSQL101_CK, 10.0);

-- NET101: Điểm khá
INSERT INTO COT_DIEM (MA_MON_HOC, TEN_COT_DIEM, TRONG_SO, THU_TU) VALUES 
('NET101', N'Giữa kỳ', 30, 1),
('NET101', N'Cuối kỳ', 70, 2);
DECLARE @CotDiemNET101_GK INT = SCOPE_IDENTITY() - 1;
DECLARE @CotDiemNET101_CK INT = SCOPE_IDENTITY();
INSERT INTO DIEM_SO (MA_COT_DIEM, GIA_TRI) VALUES 
(@CotDiemNET101_GK, 7.5),
(@CotDiemNET101_CK, 8.0);

-- PRO101: Điểm trung bình
INSERT INTO COT_DIEM (MA_MON_HOC, TEN_COT_DIEM, TRONG_SO, THU_TU) VALUES 
('PRO101', N'Bài tập lớn', 100, 1);
DECLARE @CotDiemPRO101 INT = SCOPE_IDENTITY();
INSERT INTO DIEM_SO (MA_COT_DIEM, GIA_TRI) VALUES (@CotDiemPRO101, 6.5);

-- ========== KỲ 2025.1 (Đã xong) ==========

-- WIN201: Điểm khá
INSERT INTO COT_DIEM (MA_MON_HOC, TEN_COT_DIEM, TRONG_SO, THU_TU) VALUES 
('WIN201', N'Đồ án giữa kỳ', 50, 1),
('WIN201', N'Đồ án cuối kỳ', 50, 2);
DECLARE @CotDiemWIN201_GK INT = SCOPE_IDENTITY() - 1;
DECLARE @CotDiemWIN201_CK INT = SCOPE_IDENTITY();
INSERT INTO DIEM_SO (MA_COT_DIEM, GIA_TRI) VALUES 
(@CotDiemWIN201_GK, 8.0),
(@CotDiemWIN201_CK, 8.5);

-- DBI202: Điểm giỏi
INSERT INTO COT_DIEM (MA_MON_HOC, TEN_COT_DIEM, TRONG_SO, THU_TU) VALUES 
('DBI202', N'Trắc nghiệm GK', 40, 1),
('DBI202', N'Trắc nghiệm CK', 60, 2);
DECLARE @CotDiemDBI202_GK INT = SCOPE_IDENTITY() - 1;
DECLARE @CotDiemDBI202_CK INT = SCOPE_IDENTITY();
INSERT INTO DIEM_SO (MA_COT_DIEM, GIA_TRI) VALUES 
(@CotDiemDBI202_GK, 8.5),
(@CotDiemDBI202_CK, 9.0);

-- OSG202: Điểm liệt (Cảnh báo)
INSERT INTO COT_DIEM (MA_MON_HOC, TEN_COT_DIEM, TRONG_SO, THU_TU) VALUES 
('OSG202', N'Giữa kỳ', 40, 1),
('OSG202', N'Cuối kỳ', 60, 2);
DECLARE @CotDiemOSG202_GK INT = SCOPE_IDENTITY() - 1;
DECLARE @CotDiemOSG202_CK INT = SCOPE_IDENTITY();
INSERT INTO DIEM_SO (MA_COT_DIEM, GIA_TRI) VALUES 
(@CotDiemOSG202_GK, 4.0),
(@CotDiemOSG202_CK, 3.0);  -- Tổng kết: 3.4 → Liệt

-- ========== KỲ 2025.2 (Đang học - Mới có điểm quá trình) ==========

-- MLG301: Mới có giữa kỳ
INSERT INTO COT_DIEM (MA_MON_HOC, TEN_COT_DIEM, TRONG_SO, THU_TU) VALUES 
('MLG301', N'Assignment 1', 15, 1),
('MLG301', N'Giữa kỳ', 25, 2),
('MLG301', N'Cuối kỳ', 60, 3);
DECLARE @CotDiemMLG301_A1 INT = SCOPE_IDENTITY() - 2;
DECLARE @CotDiemMLG301_GK INT = SCOPE_IDENTITY() - 1;
INSERT INTO DIEM_SO (MA_COT_DIEM, GIA_TRI) VALUES 
(@CotDiemMLG301_A1, 8.5),
(@CotDiemMLG301_GK, 7.0);  -- Chưa có điểm cuối kỳ

-- DSV301: Mới có 1 bài tập nhỏ
INSERT INTO COT_DIEM (MA_MON_HOC, TEN_COT_DIEM, TRONG_SO, THU_TU) VALUES 
('DSV301', N'Lab 1: Pandas', 10, 1),
('DSV301', N'Lab 2: Matplotlib', 10, 2),
('DSV301', N'Đồ án giữa kỳ', 30, 3),
('DSV301', N'Đồ án cuối kỳ', 50, 4);
DECLARE @CotDiemDSV301_L1 INT = SCOPE_IDENTITY() - 3;
INSERT INTO DIEM_SO (MA_COT_DIEM, GIA_TRI) VALUES (@CotDiemDSV301_L1, 9.0);

-- SWE301: Chưa có điểm nào (mới bắt đầu)
INSERT INTO COT_DIEM (MA_MON_HOC, TEN_COT_DIEM, TRONG_SO, THU_TU) VALUES 
('SWE301', N'Bài tập nhóm', 40, 1),
('SWE301', N'Đồ án cuối kỳ', 60, 2);
-- Chưa có DIEM_SO → Test case: môn chưa có điểm

-- ==============================================================================
-- 4. RẢI PHIÊN HỌC (30 ngày qua - Thực tế hơn)
-- ==============================================================================
DECLARE @i INT = 0;
WHILE @i < 30
BEGIN
    DECLARE @NgayHoc DATETIME = DATEADD(DAY, -@i, GETDATE());
    DECLARE @ThuTrongTuan INT = DATEPART(WEEKDAY, @NgayHoc);
    
    -- Cuối tuần ít học hơn
    IF @ThuTrongTuan IN (1, 7)  -- Chủ nhật, Thứ 7
    BEGIN
        IF RAND() > 0.6  -- 40% xác suất có học cuối tuần
        BEGIN
            INSERT INTO PHIEN_HOC (MA_NGUOI_DUNG, MA_MON_HOC, THOI_GIAN_BAT_DAU, THOI_LUONG_PHUT)
            VALUES (1, 'MLG301', DATEADD(HOUR, 14, @NgayHoc), 45 + RAND() * 60);
        END
    END
    ELSE  -- Ngày thường
    BEGIN
        -- 90% xác suất có học
        IF RAND() > 0.1
        BEGIN
            -- Phiên 1: Sáng (60-120 phút)
            INSERT INTO PHIEN_HOC (MA_NGUOI_DUNG, MA_MON_HOC, THOI_GIAN_BAT_DAU, THOI_LUONG_PHUT)
            VALUES (1, 
                CASE RAND()
                    WHEN 0 THEN 'MLG301'
                    WHEN 1 THEN 'DSV301'
                    ELSE 'SWE301'
                END,
                DATEADD(HOUR, 8 + RAND() * 2, @NgayHoc),  -- 8:00-10:00 AM
                60 + RAND() * 60);
            
            -- 60% xác suất có phiên 2: Chiều
            IF RAND() > 0.4
            BEGIN
                INSERT INTO PHIEN_HOC (MA_NGUOI_DUNG, MA_MON_HOC, THOI_GIAN_BAT_DAU, THOI_LUONG_PHUT)
                VALUES (1,
                    CASE RAND()
                        WHEN 0 THEN 'MLG301'
                        WHEN 1 THEN 'DSV301'
                        ELSE 'SWE301'
                    END,
                    DATEADD(HOUR, 14 + RAND() * 3, @NgayHoc),  -- 2:00-5:00 PM
                    45 + RAND() * 75);
            END
        END
    END
    
    -- Môn cũ (đã xong) - thỉnh thoảng ôn lại
    IF @i > 20 AND RAND() > 0.7
    BEGIN
        INSERT INTO PHIEN_HOC (MA_NGUOI_DUNG, MA_MON_HOC, THOI_GIAN_BAT_DAU, THOI_LUONG_PHUT)
        VALUES (1, 'DBI202', DATEADD(HOUR, 20, @NgayHoc), 30 + RAND() * 45);
    END

    SET @i = @i + 1;
END

-- ==============================================================================
-- 5. NHIỆM VỤ (Test 3 trạng thái: Xong, Quá hạn, Sắp tới hạn)
-- ==============================================================================
INSERT INTO NHIEM_VU (MA_NGUOI_DUNG, MA_MON_HOC, TIEU_DE, THOI_HAN, TRANG_THAI) VALUES
-- Sắp hạn (trong 2h) - Stress cao
(1, 'MLG301', N'Nộp Lab 4: Random Forest', DATEADD(HOUR, 2, GETDATE()), N'CHƯA_HOAN_THANH'),

-- Quá hạn 1 ngày - Đỏ
(1, 'DSV301', N'Clean dữ liệu Lazada', DATEADD(DAY, -1, GETDATE()), N'CHƯA_HOAN_THANH'),

-- Quá hạn 3 ngày - Rất đỏ
(1, 'MLG301', N'Đọc chương Decision Tree', DATEADD(DAY, -3, GETDATE()), N'CHƯA_HOAN_THANH'),

-- Sắp hạn (3 ngày nữa)
(1, 'SWE301', N'Vẽ sơ đồ Use Case', DATEADD(DAY, 3, GETDATE()), N'CHƯA_HOAN_THANH'),

-- Đã xong (hoàn thành trước hạn)
(1, 'MLG301', N'Đọc chương 1', DATEADD(DAY, -10, GETDATE()), N'DA_HOAN_THANH'),

-- Đã xong (hoàn thành đúng hạn)
(1, 'DSV301', N'Cài đặt môi trường', DATEADD(DAY, -5, GETDATE()), N'DA_HOAN_THANH'),

-- Deadline xa (7 ngày nữa)
(1, 'SWE301', N'Viết tài liệu SRS', DATEADD(DAY, 7, GETDATE()), N'CHƯA_HOAN_THANH');

-- ==============================================================================
-- 6. GHI CHÚ
-- ==============================================================================
INSERT INTO GHI_CHU (MA_NGUOI_DUNG, MA_MON_HOC, TIEU_DE, NOI_DUNG) VALUES
(1, 'MLG301', N'Công thức Entropy', N'Dùng trong Decision Tree để tính độ tinh khiết. Entropy = -Σ p(x) * log2(p(x))'),
(1, 'MLG301', N'Tham số Random Forest', N'n_estimators=100, max_depth=10, min_samples_split=5'),
(1, 'DSV301', N'Xử lý missing data', N'Dùng df.fillna() hoặc dropna(). Cân nhắc imputation với KNN.'),
(1, 'SQL101', N'Review SQL JOIN', N'Nhớ dùng INNER JOIN thay vì subquery để tối ưu performance.'),
(1, 'SWE301', N'Mẫu tài liệu SRS', N'Tham khảo template IEEE 830-1998');

PRINT N'✅ Đã đổ dữ liệu 3 học kỳ (2024.2, 2025.1, 2025.2) với đầy đủ các case kiểm thử!';
GO
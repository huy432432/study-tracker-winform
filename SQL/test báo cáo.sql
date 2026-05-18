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

-- HỌC KỲ 2024.2 (Kỳ trước nữa)
INSERT INTO MON_HOC (MA_MON_HOC, TEN_MON_HOC, SO_TIN_CHI, HOC_KY, MA_NGUOI_DUNG, TEN_GIANG_VIEN, HINH_THUC_THI) VALUES
('SQL101', N'Cơ sở dữ liệu 1', 3, N'2024.2', 1, N'Thầy Minh', N'TRẮC_NGHIỆM'),
('NET101', N'Mạng máy tính cơ bản', 3, N'2024.2', 1, N'Cô Thủy', N'TỰ_LUẬN'),
('PRO101', N'Lập trình C cơ bản', 4, N'2024.2', 1, N'Thầy Hoàng', N'BÀI_TẬP_LỚN');

-- HỌC KỲ 2025.1 (Kỳ vừa xong)
INSERT INTO MON_HOC (MA_MON_HOC, TEN_MON_HOC, SO_TIN_CHI, HOC_KY, MA_NGUOI_DUNG, TEN_GIANG_VIEN, HINH_THUC_THI) VALUES
('WIN201', N'Lập trình WinForms', 4, N'2025.1', 1, N'Thầy Hải Dương', N'BÀI_TẬP_LỚN'),
('DBI202', N'Hệ quản trị CSDL', 3, N'2025.1', 1, N'Cô Lan Anh', N'TRẮC_NGHIỆM'),
('OSG202', N'Hệ điều hành', 2, N'2025.1', 1, N'Thầy Nam', N'TỰ_LUẬN');

-- HỌC KỲ 2025.2 (Kỳ hiện tại)
INSERT INTO MON_HOC (MA_MON_HOC, TEN_MON_HOC, SO_TIN_CHI, HOC_KY, MA_NGUOI_DUNG, TEN_GIANG_VIEN, HINH_THUC_THI) VALUES
('MLG301', N'Machine Learning', 3, N'2025.2', 1, N'Thầy Tuấn Huy', N'TỰ_LUẬN'),
('DSV301', N'Data Science với Python', 3, N'2025.2', 1, N'Cô Linh', N'BÀI_TẬP_LỚN'),
('SWE301', N'Kỹ thuật phần mềm', 3, N'2025.2', 1, N'Thầy Bình', N'BÀI_TẬP_LỚN');

-- ==============================================================================
-- 3. THÊM ĐIỂM SỐ (Đủ 3 trường hợp: Điểm cao, trung bình, điểm liệt)
-- ==============================================================================
-- Kỳ 2024.2: Đã xong, điểm cao (Xuất sắc)
INSERT INTO COT_DIEM (MA_MON_HOC, TEN_COT_DIEM, TRONG_SO, THU_TU) VALUES ('SQL101', N'Tổng kết', 100, 1);
INSERT INTO DIEM_SO (MA_COT_DIEM, GIA_TRI) VALUES (SCOPE_IDENTITY(), 9.5);

-- Kỳ 2025.1: Có môn điểm thấp (Cần cố gắng)
INSERT INTO COT_DIEM (MA_MON_HOC, TEN_COT_DIEM, TRONG_SO, THU_TU) VALUES ('OSG202', N'Tổng kết', 100, 1);
INSERT INTO DIEM_SO (MA_COT_DIEM, GIA_TRI) VALUES (SCOPE_IDENTITY(), 3.5);

-- Kỳ 2025.2: Đang học (Mới có điểm quá trình, thiếu điểm thi)
INSERT INTO COT_DIEM (MA_MON_HOC, TEN_COT_DIEM, TRONG_SO, THU_TU) VALUES ('MLG301', N'Giữa kỳ', 40, 1), ('MLG301', N'Cuối kỳ', 60, 2);
INSERT INTO DIEM_SO (MA_COT_DIEM, GIA_TRI) VALUES (SCOPE_IDENTITY()-1, 8.0);

-- ==============================================================================
-- 4. RẢI PHIÊN HỌC (Logic 30 ngày qua - Test Biểu đồ)
-- ==============================================================================
DECLARE @i INT = 0;
WHILE @i < 30
BEGIN
    -- Môn Machine Learning (Kỳ hiện tại - Học chăm)
    INSERT INTO PHIEN_HOC (MA_NGUOI_DUNG, MA_MON_HOC, THOI_GIAN_BAT_DAU, THOI_LUONG_PHUT)
    VALUES (1, 'MLG301', DATEADD(DAY, -@i, GETDATE()), 120);

    -- Môn WinForms (Kỳ trước - Vẫn còn phiên học cũ để test lịch sử)
    IF @i > 20
    INSERT INTO PHIEN_HOC (MA_NGUOI_DUNG, MA_MON_HOC, THOI_GIAN_BAT_DAU, THOI_LUONG_PHUT)
    VALUES (1, 'WIN201', DATEADD(DAY, -@i, GETDATE()), 60);

    SET @i = @i + 1;
END

-- ==============================================================================
-- 5. NHIỆM VỤ (Test 3 trạng thái: Xong, Quá hạn, Sắp tới hạn)
-- ==============================================================================
INSERT INTO NHIEM_VU (MA_NGUOI_DUNG, MA_MON_HOC, TIEU_DE, THOI_HAN, TRANG_THAI) VALUES
(1, 'MLG301', N'Nộp Lab 4: Random Forest', DATEADD(HOUR, 2, GETDATE()), N'CHƯA_HOAN_THANH'), -- Sắp hạn (Stress cao)
(1, 'DSV301', N'Clean dữ liệu Lazada', DATEADD(DAY, -1, GETDATE()), N'CHƯA_HOAN_THANH'),   -- Quá hạn (Đỏ)
(1, 'SWE301', N'Vẽ sơ đồ Sequence', DATEADD(DAY, -5, GETDATE()), N'DA_HOAN_THANH'),        -- Đã xong
(1, 'MLG301', N'Đọc chương 1', DATEADD(DAY, -10, GETDATE()), N'DA_HOAN_THANH');           -- Đã xong

-- ==============================================================================
-- 6. GHI CHÚ
-- ==============================================================================
INSERT INTO GHI_CHU (MA_NGUOI_DUNG, MA_MON_HOC, TIEU_DE, NOI_DUNG) VALUES
(1, 'MLG301', N'Công thức Entropy', N'Dùng trong Decision Tree để tính độ tinh khiết.'),
(1, 'SQL101', N'Review SQL', N'Nhớ dùng JOIN thay vì subquery để tối ưu.');

PRINT N'Đã đổ dữ liệu 3 học kỳ (2024.2, 2025.1, 2025.2) với đầy đủ các case kiểm thử!';
GO
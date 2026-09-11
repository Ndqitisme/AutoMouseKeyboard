# AutoMouseKeyboard

Công cụ tự động hóa chuột và bàn phím cho Windows — ghi lại chuỗi thao tác (click chuột, nhấn phím) rồi phát lại theo vòng lặp với delay tùy chỉnh.

## Tính năng

- **Tự động chuột**: Left/Right/Middle Click, Double Click tại tọa độ màn hình bất kỳ (lấy tọa độ bằng 1 cú click)
- **Tự động bàn phím**: phím đơn, tổ hợp phím (Ctrl/Alt/Shift + phím), nhập chuỗi ký tự
- **Chuỗi hành động**: mỗi cấu hình gồm nhiều bước, mỗi bước có delay (ms) và số lần lặp riêng
- **Vòng lặp**: lặp toàn bộ chuỗi N lần, kèm global delay giữa các vòng
- **Dừng khẩn cấp**: nhấn `ESC` bất cứ lúc nào để dừng ngay (hoạt động cả khi app không focus)
- **Đa cấu hình**: lưu nhiều profile dạng JSON, hỗ trợ Import/Export, copy/paste (`Ctrl+C` / `Ctrl+V`)
- **Đa ngôn ngữ**: 19 ngôn ngữ (English, Tiếng Việt, 中文, 日本語, 한국어, العربية, ...)
- **Giao diện**: Light/Dark theme, chạy nền dưới system tray, tùy chọn khởi động cùng Windows
- **Tương thích**: Windows 7/8/10/11 (x86 & x64), file exe self-contained — không cần cài .NET

## Yêu cầu hệ thống

| Hệ điều hành | Ghi chú |
|---|---|
| Windows 10/11 | Chạy trực tiếp, không cần thêm gì |
| Windows 7 SP1 x64 | Installer tự cài bản vá KB4474419 + KB4490628 (SHA-2 + servicing stack) |

## Cài đặt

**Cách 1 — Dùng installer (khuyến nghị):**
1. Tải `AutoMouseKeyboard_Setup.exe` từ mục Releases
2. Chạy installer (yêu cầu quyền admin) → chọn tạo shortcut ngoài Desktop nếu muốn
3. Mở app từ Start Menu hoặc Desktop

**Cách 2 — File exe portable:**
1. Tải `AutoMouseKeyboard.exe` (x64 hoặc x86 tùy máy)
2. Chạy trực tiếp, không cần cài đặt

## Hướng dẫn sử dụng (tuần tự)

### Bước 1: Tạo cấu hình mới
- Mở app → nhấn **New** → nhập tên cấu hình vào ô **Config Name**

### Bước 2: Thêm hành động chuột
1. Nhấn **Add Mouse**
2. Chọn loại click (Left Click, Right Double, ...)
3. Nhấn nút ở cột **Set Position** → di chuột đến vị trí cần click trên màn hình → **click 1 lần** để chốt tọa độ
4. Đặt **Count** (số lần lặp của bước) và **Delay** (ms chờ sau bước)

### Bước 3: Thêm hành động bàn phím
1. Nhấn **Add Key**
2. Nhấn tổ hợp phím muốn mô phỏng (ví dụ `Ctrl+V`, `F5`, `Enter`) hoặc gõ chuỗi ký tự
3. Đặt **Count** và **Delay** tương tự

### Bước 4: Sắp xếp và chỉnh sửa
- Dùng **Move Up** / **Move Down** để đổi thứ tự các bước
- Chọn bước → **Delete** để xóa
- Double-click vào ô trong bảng để sửa trực tiếp

### Bước 5: Cấu hình vòng lặp
- **Loop Count**: số vòng lặp toàn bộ chuỗi (mặc định 30)
- **Global Delay**: thời gian nghỉ giữa các vòng (ms)

### Bước 6: Lưu và chạy
1. Nhấn **Save** để lưu cấu hình
2. Nhấn **Start** để bắt đầu tự động hóa
3. Nhấn **`ESC`** bất cứ lúc nào để dừng khẩn cấp

### Bước 7 (tùy chọn): Quản lý cấu hình
- **Import** / **Export**: nhập/xuất cấu hình dạng file `.json`
- `Ctrl+C` / `Ctrl+V`: copy/paste cấu hình trong danh sách
- **Setting**: đổi ngôn ngữ, theme sáng/tối, bật khởi động cùng Windows, ẩn app sau khi chạy xong

## Vị trí lưu dữ liệu

```
%AppData%\AutoMouseKeyboard\
├── app_settings.json     ← theme, ngôn ngữ, loop count, ...
└── configs\*.json        ← các cấu hình automation
```

## Build từ source

**Yêu cầu:** .NET 6 SDK trở lên, Windows

```powershell
# Build chạy thử
dotnet build AutoMouseKeyboard/AutoMouseKeyboard.csproj

# Publish exe đơn file, self-contained (không cần .NET runtime)
dotnet publish AutoMouseKeyboard/AutoMouseKeyboard.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
dotnet publish AutoMouseKeyboard/AutoMouseKeyboard.csproj -c Release -r win-x86 --self-contained true /p:PublishSingleFile=true
```

Output: `AutoMouseKeyboard\bin\Release\net6.0-windows\win-x64\publish\AutoMouseKeyboard.exe`

**Đóng gói installer:** cài [Inno Setup](https://jrsoftware.org/isdl.php) rồi compile `BuildInstaller.iss` → output trong thư mục `Installer\`.

## Cấu trúc project

```
AutoMouseKeyboard/
├── MainForm.*.cs          # Form chính (UI, actions, configs)
├── Models/                # ActionConfig, ActionStep, ActionPosition
├── Services/              # AutomationRunner, InputDispatcher, ConfigStorage,
│                          #   MouseCaptureService, EscapeKeyMonitor
├── Utilities/             # LanguageManager, ThemeManager, StartupHelper, ...
├── Resources/             # LanguageResources.*.resx (19 ngôn ngữ)
└── assets/                # Icon, hình ảnh
BuildInstaller.iss         # Script Inno Setup đóng gói installer
Redistributables/          # Bản vá Windows 7 (KB4474419, KB4490628, VC++)
```

## Tác giả

**NDQITVN** — [github.com/Ndqitisme](https://github.com/Ndqitisme)

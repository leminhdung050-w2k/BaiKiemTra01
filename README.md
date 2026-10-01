# Lê Minh Dũng - 24810320213 - D19QTANM1
Câu 1: Trình bày sự khác nhau giữa Value Types (Kiểu giá trị) và Reference Types
(Kiểu tham chiếu) trong C# về cơ chế lưu trữ vùng nhớ (Stack vs Heap).
| Tiêu chí | Value Types | Reference Types |
|---|---|---|
| Biến chứa gì? | Chứa trục tiếp dữ liệu | Chứa địa chỉ tham chiếu trỏ tới đối tượng |
| Dữ liệu thực nằm ở đâu? | Ngay tại nơi biến được khai báo (thường là Stack đối với biến cục bộ) | Đối tượng nằm trên Managed Heap; chỉ tham chiếu nằm trên Stack hoặc bên trong đối tượng khác |
| Giải phóng bộ nhớ | Tự động khi ra khỏi phạm vi (pop khỏi Stack) | Garbage Collector thu hồi khi không còn tham chiếu |
| Tốc độ cấp phát | Rất nhanh (chỉ dịch con trỏ stack) | Chậm hơn (cấp phát heap, có chi phí GC) |
| Giá trị null | Không thể (trừ khi dùng Nullable<T> / int?) | Có thể là null |
| Kế thừa | Ngầm kế thừa System.ValueType; struct không thể bị kế thừa | Hỗ trợ kế thừa đầy đủ |
| Giá trị mặc định | 0, false, \0... | null |
| Ví dụ | int, double, bool, char, decimal, struct, enum, DateTime | class, interface, delegate, array, string, object, dynamic |

Câu 2: Tính năng Init-only Properties (init) trong C# 9/10 khác gì so với thuộc tính có set thông thường? Nêu trường hợp sử dụng thực tế.
- Init-only property dùng accessor init thay cho set. Thuộc tính này chỉ được gán giá trị trong giai đoạn khởi tạo đối tượng. Sau khi đối tượng được tạo xong, nó trở thành chỉ đọc.
- Đối với các phiên bản init ra mắt ở C# 9 (.NET 5). C# 10 không thêm tính năng mới cho init, nhưng bổ sung record struct và readonly record struct, cũng dùng init. Từ khóa required (bắt buộc phải gán) là của C# 11.
- So sánh init và set

| Tiêu chí | get; set; | get; init; |
|---|---|---|
| Gán trong object initializer | Được | Được |
| Gán trong constructor | Được | Được |
| Gán sau khi đối tượng đã khởi tạo | Được, bất kỳ lúc nào | Không, lỗi biên dịch |
| Tính bất biến (immutability) | Không | Có (ở mức nông) |
| Dùng với with expression (record) | Không áp dụng | Có |
| Thread-safe sau khi khởi tạo | Cần tự bảo vệ | An toàn hơn vì không đổi |
| Kiểm tra | Compile-time | Compile-time (runtime vẫn bị reflection vượt qua) |
- Các trường hợp sử dụng thực tế

| STT | Tình huống | Lý do dùng init |
|---|---|---|
| 1 | DTO, request/response model của Web API | Dữ liệu chỉ cần gán một lần khi deserialize, tránh bị sửa vô ý trong pipeline |
| 2 | Options, cấu hình ứng dụng | Khởi tạo gọn bằng object initializer, sau đó không đổi |
| 3 | Value Object, Entity trong DDD | Giữ bất biến mà không cần constructor nhiều tham số |
| 4 | Record | Positional record tự sinh thuộc tính init |
| 5 | Dữ liệu dùng chung giữa nhiều thread | Không đổi sau khi tạo nên không cần lock |
| 6 | Dữ liệu test, Builder | Dựng đối tượng nhanh bằng initializer |
| 7 | Sự kiện, message (event sourcing, message queue) | Sự kiện đã xảy ra thì không được sửa |

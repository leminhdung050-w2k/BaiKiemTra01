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

Câu 3: Phân biệt sự khác nhau giữa phương thức virtual ở lớp cha và phương
thức override ở lớp con khi triển khai tính Đa hình (Polymorphism).
1. Phương thức virtual (Ở lớp Base / Lớp cha)
- Mục đích: Cung cấp sẵn một phần cài đặt mặc định (implementation) cho phương thức.
- Cấp quyền kế thừa: Đóng vai trò như một giấy phép cho phép các lớp con dẫn xuất có quyền định nghĩa lại hành vi nếu cần.
- Hành vi mặc định: Nếu lớp con không viết lại phương thức này, chương trình sẽ tự động gọi logic mặc định của lớp cha khi thực thi.
2. Phương thức override (Ở lớp Derived / Lớp con)
- Mục đích: Triển khai lại hoặc thay thế hoàn toàn logic thực thi của một phương thức virtual (hoặc abstract) được kế thừa từ lớp cha.
- Cơ chế liên kết động (Dynamic Dispatch / Late Binding): Khi gọi phương thức qua một biến tham chiếu kiểu lớp cha nhưng trỏ đến thể hiện thực tế của lớp con (ví dụ: Animal a = new Dog(); a.Speak();), hệ thống CLR sẽ tra bảng phương thức ảo (v-table) tại thời điểm chạy (runtime) để thực thi đúng mã lệnh trong phương thức override của lớp con thay vì gọi mã của lớp cha.

Câu 4: Tại sao một thành phần được khai báo là static trong Lớp (Class) lại không thể
truy xuất thông qua một thể hiện (Object Instance) được tạo bằng toán tử new?
1. Khác biệt về cấp độ sở hữu bộ nhớ
- Cấp độ Kiểu dữ liệu (Type / Class level): Thành phần static thuộc sở hữu chung của toàn bộ lớp, được nạp và khởi tạo một lần duy nhất vào bộ nhớ khi kiểu dữ liệu đó được tải, dùng chung cho toàn bộ chương trình.
- Cấp độ Đối tượng (Object level): Một thể hiện tạo bằng toán tử new chỉ quản lý vùng nhớ và trạng thái dữ liệu độc lập của riêng cá thể đối tượng đó trên vùng nhớ Heap.
2. Định hướng thiết kế ngôn ngữ của C#
- Tránh nhầm lẫn ngữ nghĩa: Trình biên dịch cấm truy xuất qua thể hiện (như instance.StaticMember) để lập trình viên không ngộ nhận rằng giá trị của thành phần đó phụ thuộc hoặc thay đổi riêng biệt theo từng đối tượng.
- Đảm bảo tính tường minh của mã nguồn (Code Clarity): Phân định rạch ròi ngay tại cú pháp gọi hàm giữa hành vi toàn cục thuộc về hệ thống (như Math.Sqrt(), DateTime.Now) và hành vi xử lý dữ liệu nội bộ của một đối tượng cụ thể.



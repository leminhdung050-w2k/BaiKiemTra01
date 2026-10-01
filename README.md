# Lê Minh Dũng - 24810320213 - D19QTANM1
Câu 1: Trình bày sự khác nhau giữa Value Types (Kiểu giá trị) và Reference Types
(Kiểu tham chiếu) trong C# về cơ chế lưu trữ vùng nhớ (Stack vs Heap).
| Tiêu chí | Value Types (Kiểu giá trị) | Reference Types (Kiểu tham chiếu) |
|---|---|---|
| Biến chứa gì? | Chính dữ liệu thực | Địa chỉ (tham chiếu) trỏ tới đối tượng |
| Dữ liệu thực nằm ở đâu? | Ngay tại nơi biến được khai báo (thường là Stack đối với biến cục bộ) | Đối tượng nằm trên Managed Heap; chỉ tham chiếu nằm trên Stack (hoặc bên trong đối tượng khác) |
| Giải phóng bộ nhớ | Tự động khi ra khỏi phạm vi (pop khỏi Stack) | Garbage Collector (GC) thu hồi khi không còn tham chiếu |
| Tốc độ cấp phát | Rất nhanh (chỉ dịch con trỏ stack) | Chậm hơn (cấp phát heap, có chi phí GC) |
| Gán a = b | Sao chép giá trị (hai bản độc lập) | Sao chép tham chiếu (hai biến cùng trỏ một đối tượng) |
| Giá trị null | Không thể (trừ khi dùng Nullable<T> / int?) | Có thể là null |
| Kế thừa | Ngầm kế thừa System.ValueType; struct không thể bị kế thừa | Hỗ trợ kế thừa đầy đủ |
| Giá trị mặc định | 0, false, \0... | null |
| Ví dụ | int, double, bool, char, decimal, struct, enum, DateTime | class, interface, delegate, array, string, object, dynamic |


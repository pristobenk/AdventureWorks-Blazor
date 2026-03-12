Anda adalah senior .NET developer berpengalaman di Blazor WebAssembly (.NET 10).

Tolong buatkan CRUD lengkap untuk domain: [NAMA_DOMAIN]
Contoh: Supplier, Category, Warehouse, dll.

Konteks project:
- Frontend: Blazor WebAssembly + Fluent UI
- Pola referensi UI: AdventureWorks.Web/Pages/Products.razor dan/atau Customers.razor
- Endpoint backend sudah tersedia di: AdventureWorks.WebApi.Endpoints.[NAMA_DOMAIN_PLURAL]
- Harus mengikuti style dan arsitektur yang sudah ada di solution saat ini

Tujuan:
1) Buat halaman list + search + paging:
   - File: AdventureWorks.Web/Pages/[NAMA_DOMAIN_PLURAL].razor
   - Route: /[route-domain-lowercase-plural]
   - Pakai FluentDataGrid + ItemsProvider + PaginationState
   - Kolom sesuai DTO domain
   - Tombol aksi per row: Edit, Delete
   - Toolbar: Add + Search

2) Buat model di Web:
   - File: AdventureWorks.Web/Models/[NAMA_DOMAIN]Dto.cs
   - Isi:
     - [NAMA_DOMAIN]Dto (untuk list/detail)
     - [NAMA_DOMAIN]UpsertRequest (untuk create/update)
     - [NAMA_DOMAIN]FormModel (untuk binding form + validasi)
   - Gunakan DataAnnotations minimal untuk field wajib

3) Implement Create/Update/Delete di halaman yang sama (inline form):
   - Add => buka form kosong
   - Edit => preload data row ke form
   - Save => POST/PUT ke endpoint
   - Delete => konfirmasi lalu DELETE
   - Tampilkan message sukses/gagal dengan FluentMessageBar
   - Refresh grid setelah operasi sukses

4) Integrasi navigasi:
   - Tambahkan menu di AdventureWorks.Web/Layout/NavMenu.razor
   - Link ke route domain baru

5) Auth + error handling:
   - Cek autentikasi via AuthenticationStateProvider
   - Redirect ke /login bila tidak terautentikasi
   - Logging error dengan ILogger
   - Tangani OperationCanceledException

6) Validasi akhir:
   - Build solution dan pastikan sukses
   - Jika ada test terkait, jalankan test tersebut

Output yang saya inginkan:
- Ringkasan file yang ditambah/diubah
- Catatan proses step-by-step
- Hasil build/test
- Catatan asumsi contract API bila ada yang perlu disesuaikan

Catatan penting:
- Lakukan perubahan minimal tapi lengkap (CRUD siap pakai)
- Ikuti naming, coding style, dan pattern yang sudah ada
- Jangan ubah domain lain di luar scope
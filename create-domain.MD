# Prompt: Generate Full CRUD Stack for New Domain Entity (adapted to this repo)

## INSTRUKSI UTAMA
Kamu adalah code generator untuk project .NET 10 bernama **AdventureWorks** (repo: AdventureWorks-Blazor) yang menerapkan pola Clean Architecture. Berdasarkan class domain yang diberikan, generate semua file CRUD (Domain → Application → Infrastructure → API) mengikuti struktur dan konvensi di repository ini.

---

## INPUT (ISI BAGIAN INI SETIAP KALI PAKAI)

### Domain Class
Sisipkan potongan class domain yang sudah ada di `AdventureWorks.Domain\AdventureWorks.Domain\{PluralName}\{EntityName}.cs`.

### Metadata
- **EntityName**: `{NamaEntity}` (contoh: `Customer`)
- **PluralName**: `{NamaJamak}` (contoh: `Customers`)
- **DomainNamespace**: `AdventureWorks.Domain.{PluralName}`
- **RoutePrefix**: `{route}` (contoh: `customers`) — default: gunakan nama jamak sesuai `PluralName` (contoh: `Customers` → `customers`) / "sesuai nama class" jika tidak diberikan
- **TagName**: `{Tag}` (contoh: `Customers`) — default: sama dengan `PluralName` (contoh: `Customers`)
- **HasUserOwnership**: `true/false` — jika `true`, handler harus mem-filter dengan `userContext.UserId`. Default: `false`.
- **ExcludeFromCreate**: daftar properti yang TIDAK boleh masuk CreateCommand (mis. `Id, CreatedAt`). Default: exclude primary id properties (mis. `CustomerId`).
- **ExcludeFromUpdate**: daftar properti yang TIDAK boleh masuk UpdateCommand body (Id diambil dari route). Default: exclude primary id properties (mis. `CustomerId`).
- **ExcludeFromResponse**: properti yang TIDAK perlu dimasukkan ke DTO response (kosongkan jika semua). Default: tidak ada.

---

## STRUKTUR PROYEK & KONVENSI (sesuai repo)

Gunakan path dan pola yang persis ada di repository ini. Contoh pola yang dipakai di repo:

```
AdventureWorks.Domain\AdventureWorks.Domain\{PluralName}\         → Entity, Errors, Enums, DomainEvents
AdventureWorks.Infrastructure\AdventureWorks.Infrastructure\EntityConfigurations\{PluralName}\  → EF configuration classes
AdventureWorks.Infrastructure\AdventureWorks.Infrastructure\Database\ApplicationDbContext.cs   → register DbSet<T>
AdventureWorks.Application\AdventureWorks.Application.Abstractions\Data\IApplicationDbContext.cs  → register DbSet<T>
AdventureWorks.Application\AdventureWorks.Application\{PluralName}\Create\   → Create command/handler/validator
AdventureWorks.Application\AdventureWorks.Application\{PluralName}\Update\   → Update command/handler/validator
AdventureWorks.Application\AdventureWorks.Application\{PluralName}\Delete\   → Delete command/handler/validator
AdventureWorks.Application\AdventureWorks.Application\{PluralName}\Get\      → Get list query/handler/response
AdventureWorks.Application\AdventureWorks.Application\{PluralName}\GetById\  → GetById query/handler/response
AdventureWorks.API\AdventureWorks.API\Endpoints\{PluralName}\              → HTTP endpoints (Create/Update/Delete/Get/GetById)
AdventureWorks.API\AdventureWorks.API\Endpoints\Tags.cs                    → add tag constant if needed
```

Catatan: repository menyimpan EF configurations di `EntityConfigurations` subfolder di proyek Infrastructure (contoh: `EntityConfigurations\Products\ProductConfiguration.cs`). Ikuti pola ini.

### SharedKernel & Abstractions (gunakan yang ada di repo)
- Jangan buat ulang `Entity`, `IDomainEvent`, `Result`, `Error`, `ICommand/IQuery` atau `IApplicationDbContext` — pakai yang sudah ada.
- Di repo, `IApplicationDbContext` terletak di `AdventureWorks.Application\AdventureWorks.Application.Abstractions\Data\IApplicationDbContext.cs`.

### Coding conventions (sesuai kode yang ada)
- Gunakan file-scoped namespaces (contoh: `namespace AdventureWorks.Application.Products;`)
- Handler classes dibuat `internal sealed class` dan menggunakan primary-constructor syntax ketika cocok (lihat contoh `CreateProductCommandHandler`).
- Endpoints di API memakai minimal APIs style: `internal sealed class Create : IEndpoint { void MapEndpoint(IEndpointRouteBuilder app) { app.MapPost("products", ...).WithTags(Tags.Products).RequireAuthorization(); } }`.
- Get list handler sering mengembalikan `PagedList<TResponse>` (lihat `Application.Pagination.PagedList`).
- FluentValidation digunakan untuk validator classes.

---

## POLA FILE YANG HARUS DI-GENERATE (sesuai repo)

1) Domain events
- `AdventureWorks.Domain\AdventureWorks.Domain\{PluralName}\{EntityName}CreatedDomainEvent.cs`
- `AdventureWorks.Domain\AdventureWorks.Domain\{PluralName}\{EntityName}UpdatedDomainEvent.cs`
- `AdventureWorks.Domain\AdventureWorks.Domain\{PluralName}\{EntityName}DeletedDomainEvent.cs`

2) Domain errors
- `AdventureWorks.Domain\AdventureWorks.Domain\{PluralName}\{EntityName}Errors.cs`

3) EF configuration
- `AdventureWorks.Infrastructure\AdventureWorks.Infrastructure\EntityConfigurations\{PluralName}\{EntityName}Configuration.cs`
  - follow patterns in existing `ProductConfiguration.cs`

4) Register DbSet
- Add `DbSet<{EntityName}> {PluralName} { get; }` to `AdventureWorks.Application\AdventureWorks.Application.Abstractions\Data\IApplicationDbContext.cs`
- Add `public DbSet<{EntityName}> {PluralName} { get; set; }` to `AdventureWorks.Infrastructure\AdventureWorks.Infrastructure\Database\ApplicationDbContext.cs`

5) Application — Create / Update / Delete / Get / GetById
- CreateCommand (`ICommand<int>` or appropriate id type), CreateCommandHandler (internal sealed, inject IApplicationDbContext[, IDateTimeProvider, IUserContext if needed]), CreateCommandValidator
- UpdateCommand (ICommand), UpdateCommandHandler, UpdateCommandValidator
- DeleteCommand (ICommand), DeleteCommandHandler, DeleteCommandValidator
- Get{PluralName}Query (IQuery<PagedList<Response>> or IQuery<List<Response>>), Get{PluralName}QueryHandler
- Get{EntityName}ByIdQuery (IQuery<Response>), Get{EntityName}ByIdQueryHandler

6) API Endpoints
- Create.cs, Update.cs, Delete.cs, Get.cs, GetById.cs under `AdventureWorks.API\AdventureWorks.API\Endpoints\{PluralName}\`
- Endpoint classes must be `internal sealed class` and call `.WithTags(Tags.{PluralName})` and `.RequireAuthorization()` as in existing endpoints.

7) Tags
- Add `public const string {PluralName} = "{Tag}";` to `AdventureWorks.API\AdventureWorks.API\Endpoints\Tags.cs` if missing.

---

## ATURAN PENTING

- Jangan ubah atau duplikasi definisi SharedKernel/Abstractions.
- Ikuti pola folder yang ada di repo (perhatikan `EntityConfigurations` untuk EF config).
- Semua file yang dihasilkan harus kompilable langsung mengikuti style/konvensi yang ada (nama namespace, accessibility, handler pattern).
- Gunakan types yang ada: `PagedList<T>` untuk paging, `Result`/`Error` untuk hasil, `ICommand`/`IQuery` abstractions untuk messaging.

---

Jika Anda ingin saya langsung menghasilkan stack CRUD untuk `Customer`, konfirmasi `RoutePrefix`, `TagName`, dan apakah `HasUserOwnership` true/false serta properti yang harus dikecualikan untuk Create/Update/Response. Saya akan membuat file-file yang diperlukan sesuai struktur aktual repository.
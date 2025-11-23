# StajForum Backend API

StajForum, öğrencilerin staj deneyimlerini paylaşabileceği ve bilgi alışverişi yapabileceği bir forum platformudur. Bu proje, platformun backend API'sini içermektedir.

## 📋 İçindekiler

- [Özellikler](#özellikler)
- [Teknolojiler](#teknolojiler)
- [Proje Yapısı](#proje-yapısı)
- [Kurulum](#kurulum)
- [Veritabanı Yapılandırması](#veritabanı-yapılandırması)
- [API Dokümantasyonu](#api-dokümantasyonu)
- [Kullanım Örnekleri](#kullanım-örnekleri)
- [Katmanlı Mimari](#katmanlı-mimari)

## ✨ Özellikler

- ✅ **Forum Sistemi**: Konu oluşturma, yanıt verme ve tartışma
- ✅ **İletişim Formu**: Kullanıcıların mesaj gönderebileceği iletişim sistemi
- ✅ **Sayfalama**: Tüm listeleme işlemlerinde sayfalama desteği
- ✅ **Sıralama**: Tarih, popülerlik ve görüntülenme sayısına göre sıralama
- ✅ **Arama**: Forum konularında başlık araması
- ✅ **Katmanlı Mimari**: Repository Pattern, Service Layer, DTOs
- ✅ **Validation**: Kapsamlı veri doğrulama
- ✅ **AutoMapper**: Otomatik entity-DTO dönüşümü
- ✅ **CORS**: Frontend entegrasyonu için CORS desteği

## 🛠 Teknolojiler

- **.NET 9.0** - Framework
- **ASP.NET Core Web API** - RESTful API
- **Entity Framework Core 9.0** - ORM
- **PostgreSQL** - Veritabanı
- **AutoMapper** - Object Mapping
- **Npgsql** - PostgreSQL Provider

## 📁 Proje Yapısı

```
staj-forum-backend/
├── Controllers/              # API Endpoints (Presentation Layer)
│   ├── ForumController.cs
│   └── ContactController.cs
│
├── Services/                 # Business Logic Layer
│   ├── IForumService.cs
│   ├── ForumService.cs
│   ├── IContactService.cs
│   └── ContactService.cs
│
├── Data/                     # Data Access Layer
│   ├── ApplicationDbContext.cs
│   └── Repositories/
│       ├── IRepository.cs
│       ├── Repository.cs
│       ├── IForumRepository.cs
│       ├── ForumRepository.cs
│       ├── IContactRepository.cs
│       └── ContactRepository.cs
│
├── Models/                  # Entity Models (Domain Layer)
│   ├── Topic.cs
│   ├── Reply.cs
│   └── ContactMessage.cs
│
├── DTOs/                     # Data Transfer Objects
│   ├── Common/
│   ├── Forum/
│   └── Contact/
│
├── Mappings/                # AutoMapper Configuration
│   └── MappingProfile.cs
│
├── Program.cs               # Application Entry Point
└── appsettings.json         # Configuration
```

## 🚀 Kurulum

### Gereksinimler

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) veya [VS Code](https://code.visualstudio.com/)

### Adımlar

1. **Projeyi klonlayın**
   ```bash
   git clone <repository-url>
   cd staj-forum-backend
   ```

2. **PostgreSQL'i başlatın**
   ```bash
   # macOS (Homebrew)
   brew services start postgresql@15
   
   # veya Docker ile
   docker run --name postgres-stajforum \
     -e POSTGRES_PASSWORD=postgres \
     -p 5432:5432 \
     -d postgres:15
   ```

3. **Veritabanını oluşturun**
   ```sql
   CREATE DATABASE staj_forum_db_dev;
   ```

4. **Connection String'i yapılandırın**
   
   `appsettings.Development.json` dosyasını açın ve connection string'i güncelleyin:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=staj_forum_db_dev;Username=postgres;Password=YOUR_PASSWORD"
     }
   }
   ```

5. **NuGet paketlerini yükleyin**
   ```bash
   dotnet restore
   ```

6. **Projeyi çalıştırın**
   ```bash
   dotnet run
   ```

7. **API'yi test edin**
   
   Tarayıcıda şu adrese gidin:
   - Swagger UI: `http://localhost:5236/swagger` (Development modunda)
   - API: `http://localhost:5236/api`

## 🗄 Veritabanı Yapılandırması

### Tablolar

- **Topics**: Forum konuları
- **Replies**: Konulara verilen yanıtlar
- **ContactMessages**: İletişim formu mesajları

### Migration (İsteğe Bağlı)

Eğer migration kullanmak isterseniz:

```bash
# Migration oluştur
dotnet ef migrations add InitialCreate

# Veritabanını güncelle
dotnet ef database update
```

**Not**: Şu anda proje `EnsureCreated()` kullanıyor (development için). Production'da migration kullanılmalıdır.

## 📚 API Dokümantasyonu

### Base URL
```
http://localhost:5236/api
```

### Forum Endpoints

#### Konular

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/forum/topics` | Konuları listele (sayfalama, sıralama, arama) |
| GET | `/api/forum/topics/{id}` | Konu detayı ve yanıtları |
| POST | `/api/forum/topics` | Yeni konu oluştur |
| PUT | `/api/forum/topics/{id}` | Konu güncelle |
| DELETE | `/api/forum/topics/{id}` | Konu sil |

#### Yanıtlar

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/forum/topics/{topicId}/replies` | Konuya ait yanıtları listele |
| POST | `/api/forum/topics/{topicId}/replies` | Yeni yanıt ekle |
| PUT | `/api/forum/replies/{id}` | Yanıt güncelle |
| DELETE | `/api/forum/replies/{id}` | Yanıt sil |

### Contact Endpoints

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| POST | `/api/contact` | İletişim mesajı gönder |
| GET | `/api/contact` | Mesajları listele (admin) |
| GET | `/api/contact/{id}` | Mesaj detayı (admin) |
| PUT | `/api/contact/{id}/read` | Okundu işaretle (admin) |
| DELETE | `/api/contact/{id}` | Mesaj sil (admin) |

## 💡 Kullanım Örnekleri

### Yeni Konu Oluşturma

```http
POST /api/forum/topics
Content-Type: application/json

{
  "title": "Staj Deneyimim",
  "content": "Merhaba, staj deneyimlerimi paylaşmak istiyorum...",
  "authorName": "Ahmet Yılmaz"
}
```

### Konuları Listeleme

```http
GET /api/forum/topics?page=1&pageSize=10&sortBy=newest&search=staj
```

**Query Parameters:**
- `page` (int, default: 1) - Sayfa numarası
- `pageSize` (int, default: 10) - Sayfa başına kayıt
- `sortBy` (string: "newest" | "oldest" | "popular", default: "newest")
- `search` (string, optional) - Başlık araması

### Yanıt Ekleme

```http
POST /api/forum/topics/1/replies
Content-Type: application/json

{
  "content": "Çok faydalı bir paylaşım, teşekkürler!",
  "authorName": "Mehmet Demir"
}
```

### İletişim Mesajı Gönderme

```http
POST /api/contact
Content-Type: application/json

{
  "name": "Ahmet Yılmaz",
  "email": "ahmet@example.com",
  "subject": "Genel Bilgi",
  "message": "Merhaba, staj programı hakkında bilgi almak istiyorum."
}
```

## 🏗 Katmanlı Mimari

### 1. Controllers (Presentation Layer)
- HTTP isteklerini karşılar
- Model validation kontrolü
- Service katmanını çağırır
- HTTP status kodları ve hata yönetimi

### 2. Services (Business Logic Layer)
- İş mantığı ve validasyon
- Repository katmanını çağırır
- DTO mapping (AutoMapper)
- Pagination hesaplamaları

### 3. Repositories (Data Access Layer)
- Veritabanı işlemleri
- CRUD operasyonları
- Query optimizasyonu
- Entity Framework Core kullanımı

### 4. DTOs (Data Transfer Objects)
- API request/response modelleri
- Validation attributes
- Entity'lerden ayrı katman

### 5. Models (Domain Layer)
- Entity modelleri
- Veritabanı şeması
- Navigation properties

## 🔧 Yapılandırma

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=staj_forum_db;Username=postgres;Password=postgres"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### CORS Yapılandırması

Frontend için CORS ayarları `Program.cs` içinde yapılandırılmıştır:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
```

## 🧪 Test

### Swagger UI ile Test

Development modunda çalıştırıldığında Swagger UI otomatik olarak açılır:
```
http://localhost:5236/swagger
```

### HTTP Dosyası ile Test

Proje içinde `staj-forum-backend.http` dosyası bulunmaktadır. Bu dosyayı kullanarak API'yi test edebilirsiniz.

## 📝 Validation Kuralları

### Topic
- `Title`: 3-200 karakter (zorunlu)
- `Content`: 10-5000 karakter (zorunlu)
- `AuthorName`: 2-100 karakter (zorunlu)

### Reply
- `Content`: 5-2000 karakter (zorunlu)
- `AuthorName`: 2-100 karakter (zorunlu)

### Contact Message
- `Name`: 2-100 karakter (zorunlu)
- `Email`: Geçerli e-posta formatı (zorunlu)
- `Subject`: 3-200 karakter (zorunlu)
- `Message`: 10-2000 karakter (zorunlu)

## 🐛 Hata Yönetimi

API standart hata formatı döner:

```json
{
  "error": "Error Type",
  "message": "Error message description"
}
```

**HTTP Status Kodları:**
- `200 OK` - Başarılı istek
- `201 Created` - Kayıt oluşturuldu
- `400 Bad Request` - Validation hatası
- `404 Not Found` - Kayıt bulunamadı
- `500 Internal Server Error` - Sunucu hatası

## 📦 Bağımlılıklar

```xml
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.9" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.2" />
<PackageReference Include="AutoMapper" Version="13.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
```

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Commit yapın (`git commit -m 'Add some AmazingFeature'`)
4. Push yapın (`git push origin feature/AmazingFeature`)
5. Pull Request açın

## 📄 Lisans

Bu proje özel bir projedir.

## 👥 İletişim

Sorularınız için issue açabilirsiniz.

---

**Not**: Bu proje geliştirme aşamasındadır. Production kullanımı için ek güvenlik önlemleri alınmalıdır.

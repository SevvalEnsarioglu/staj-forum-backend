# Staj Forum Backend

Staj Forum uygulamasinin RESTful API hizmetini saglayan backend sunucusudur. Web ve mobil uygulamalarla es zamanli calisarak veri yonetimi, forum islemleri, kullanici kimlik dogrulama ve yapay zeka entegrasyonlarini yonetir.

## Teknolojiler

*   **Platform:** .NET Core 9
*   **Veritabani:** PostgreSQL
*   **ORM:** Entity Framework Core
*   **Kimlik Dogrulama:** JWT (JSON Web Token) Bearer Authentication
*   **Sifreleme:** BCrypt.Net-Next
*   **AI Entegrasyonu:** Google Gemini API
*   **Dokumantasyon:** OpenAPI (Swagger)
*   **Nesne Esleme:** AutoMapper
*   **CORS:** Localhost tabanlı güvenli CORS politikası

## Veritabani Yapisi

Proje PostgreSQL veritabanini kullanir. Ana tablolar ve iliskileri:

### 1. **Users (Kullanicilar)**
*   `Id` (Primary Key): Kullanici benzersiz kimlik numarasi
*   `FirstName`: Kullanici adi (max 50 karakter)
*   `LastName`: Kullanici soyadi (max 50 karakter)
*   `Email`: Kullanici e-posta adresi (unique, max 100 karakter)
*   `PasswordHash`: BCrypt ile hashlenmiş şifre
*   `CreatedAt`: Hesap olusturma tarihi
*   **İlişkiler:**
    *   Bir kullanici birden fazla konu (Topic) olusturabilir
    *   Bir kullanici birden fazla cevap (Reply) yazabilir

### 2. **Topics (Forum Konulari)**
*   `Id` (Primary Key): Konu benzersiz kimlik numarasi
*   `Title`: Konu basligi (max 200 karakter)
*   `Content`: Konu icerigi (HTML destekli, text tipinde)
*   `AuthorName`: Yazar adi (max 100 karakter)
*   `UserId` (Foreign Key, nullable): Konuyu olusturan kullanici ID'si
*   `CreatedAt`: Olusturma tarihi
*   `UpdatedAt`: Guncelleme tarihi (nullable)
*   `ViewCount`: Goruntulenme sayisi (varsayilan: 0)
*   **İlişkiler:**
    *   Bir konu bir kullaniciya ait olabilir (User)
    *   Bir konu birden fazla cevap (Reply) icerebilir
*   **Indeksler:** CreatedAt, ViewCount

### 3. **Replies (Cevaplar)**
*   `Id` (Primary Key): Cevap benzersiz kimlik numarasi
*   `TopicId` (Foreign Key): Bagli oldugu konu ID'si
*   `Content`: Cevap icerigi (HTML destekli, text tipinde)
*   `AuthorName`: Yazar adi (max 100 karakter)
*   `UserId` (Foreign Key, nullable): Cevabi yazan kullanici ID'si
*   `CreatedAt`: Olusturma tarihi
*   `UpdatedAt`: Guncelleme tarihi (nullable)
*   **İlişkiler:**
    *   Bir cevap bir konuya ait olmalidir (Topic) - Cascade Delete
    *   Bir cevap bir kullaniciya ait olabilir (User)
*   **Indeksler:** TopicId, CreatedAt

### 4. **ContactMessages (Iletisim Mesajlari)**
*   `Id` (Primary Key): Mesaj benzersiz kimlik numarasi
*   `Name`: Gonderen adi (max 100 karakter)
*   `Email`: Gonderen e-posta adresi (max 255 karakter)
*   `Subject`: Mesaj konusu (max 200 karakter)
*   `Message`: Mesaj icerigi (text tipinde)
*   `CreatedAt`: Olusturma tarihi
*   `IsRead`: Okundu mu? (varsayilan: false)
*   **Indeksler:** CreatedAt, IsRead

Baglanti ayarlari `appsettings.Development.json` dosyasinda yapilandirilmistir.

## API Endpointleri

Uygulamanin sundugu API uclari asagidaki gibidir:

### 1. Kimlik Dogrulama (AuthController)

*   **`POST /api/auth/register`**: Yeni kullanici kaydi olusturur
    *   **Request Body:** `{ "firstName": "string", "lastName": "string", "email": "string", "password": "string" }`
    *   **Response:** JWT token ve kullanici bilgileri
    *   **Validasyon:** Email benzersiz olmali, tum alanlar zorunlu
    
*   **`POST /api/auth/login`**: Kullanici girisi yapar
    *   **Request Body:** `{ "email": "string", "password": "string" }`
    *   **Response:** JWT token ve kullanici bilgileri
    *   **Hata Durumları:** Kullanici bulunamadi, yanlis sifre

### 2. Forum Islemleri (ForumController)

*   **`GET /api/forum/topics`**: Tum konulari listeler
    *   **Query Parametreleri:**
        *   `sortBy`: `newest` (en yeni), `oldest` (en eski), `popular` (en çok görüntülenen)
        *   `search`: Başlık içinde arama yapar
        *   `page`: Sayfa numarasi (varsayilan: 1)
        *   `pageSize`: Sayfa boyutu (varsayilan: 10)
    *   **Response:** Sayfalanmis konu listesi ve toplam sayfa bilgisi

*   **`GET /api/forum/topics/{id}`**: Belirli bir konunun detaylarini getirir
    *   **Özellik:** Görüntüleme sayısını otomatik artırır
    *   **Response:** Konu detaylari ve cevap sayisi

*   **`POST /api/forum/topics`**: Yeni bir konu olusturur
    *   **Auth:** JWT Token gerekli
    *   **Request Body:** `{ "title": "string", "content": "string" }`
    *   **Response:** Olusturulan konu bilgileri

*   **`PUT /api/forum/topics/{id}`**: Var olan bir konuyu gunceller
    *   **Auth:** JWT Token gerekli (sadece konu sahibi güncelleyebilir)
    *   **Request Body:** `{ "title": "string", "content": "string" }`

*   **`DELETE /api/forum/topics/{id}`**: Bir konuyu siler
    *   **Auth:** JWT Token gerekli (sadece konu sahibi silebilir)
    *   **Özellik:** Cascade delete - tüm cevaplar da silinir

*   **`GET /api/forum/topics/{id}/replies`**: Bir konuya ait cevaplari listeler
    *   **Query Parametreleri:** `page`, `pageSize`
    *   **Response:** Sayfalanmis cevap listesi

*   **`POST /api/forum/topics/{id}/replies`**: Bir konuya yeni cevap ekler
    *   **Auth:** JWT Token gerekli
    *   **Request Body:** `{ "content": "string" }`

*   **`PUT /api/forum/replies/{id}`**: Bir cevabi gunceller
    *   **Auth:** JWT Token gerekli (sadece cevap sahibi güncelleyebilir)
    *   **Request Body:** `{ "content": "string" }`

*   **`DELETE /api/forum/replies/{id}`**: Bir cevabi siler
    *   **Auth:** JWT Token gerekli (sadece cevap sahibi silebilir)

*   **`GET /api/forum/user/topics`**: Kullanicinin kendi konularini listeler
    *   **Auth:** JWT Token gerekli
    *   **Query Parametreleri:** `page`, `pageSize`

*   **`GET /api/forum/user/replies`**: Kullanicinin kendi cevaplarini listeler
    *   **Auth:** JWT Token gerekli
    *   **Query Parametreleri:** `page`, `pageSize`

### 3. AI Sohbet ve Analiz (ChatController)

*   **`POST /api/chat`**: ChatStj asistani ile sohbet baslatir veya devam ettirir
    *   **Request Body:** `{ "message": "string", "conversationId": "string (optional)" }`
    *   **Response:** AI yaniti ve konusma ID'si
    *   **Özellik:** Google Gemini API kullanarak akıllı yanıtlar üretir

*   **`POST /api/chat/analyze-cv`**: Gonderilen CV metnini yapay zeka ile analiz eder
    *   **Request Body:** `{ "cvText": "string" }`
    *   **Response:** CV analiz sonuçları ve öneriler
    *   **Not:** Web ve Mobil tarafindan OCR yapildiktan sonra metin buraya gonderilir

*   **`GET /api/chat/history`**: Sohbet gecmisini getirir
    *   **Query Parametreleri:** `conversationId` (optional)
    *   **Response:** Konusma gecmisi
    *   **Durum:** Gelistirme asamasinda

*   **`DELETE /api/chat/history/{conversationId}`**: Chat geçmişini siler
    *   **Response:** 204 No Content (basarili) veya 404 Not Found

### 4. Iletisim Formu (ContactController)

*   **`POST /api/contact`**: Yeni bir iletisim mesaji gonderir
    *   **Request Body:** `{ "name": "string", "email": "string", "subject": "string", "message": "string" }`
    *   **Response:** Olusturulan mesaj bilgileri

*   **`GET /api/contact`**: Tum iletisim mesajlarini listeler
    *   **Query Parametreleri:**
        *   `page`: Sayfa numarasi (varsayilan: 1)
        *   `pageSize`: Sayfa boyutu (varsayilan: 10)
        *   `isRead`: Okunma durumu filtresi (true/false, optional)
    *   **Response:** Sayfalanmis mesaj listesi

*   **`GET /api/contact/{id}`**: Belirli bir mesajin detaylarini getirir
    *   **Response:** Mesaj detaylari

*   **`PUT /api/contact/{id}/read`**: Bir mesaji 'okundu' olarak isaretler
    *   **Response:** Guncellenmis mesaj bilgileri

*   **`DELETE /api/contact/{id}`**: Bir iletisim mesajini siler
    *   **Response:** 204 No Content

## Kimlik Dogrulama ve Yetkilendirme

### JWT Token Yapisi
*   **Token Süresi:** 7 gün
*   **Algoritma:** HMAC SHA-512
*   **Claims:**
    *   `NameIdentifier`: Kullanici ID'si
    *   `Email`: Kullanici e-posta adresi
    *   `Name`: Kullanici tam adi (FirstName + LastName)

### Token Kullanimi
1.  Kullanici `/api/auth/register` veya `/api/auth/login` endpoint'lerinden token alir
2.  Token, HTTP Header'da `Authorization: Bearer {token}` formatinda gonderilir
3.  Korunan endpoint'ler `[Authorize]` attribute'u ile isaretlenmistir
4.  Token dogrulama basarisiz olursa 401 Unauthorized donulur

### Sifre Güvenliği
*   Sifreler BCrypt algoritması ile hashlenip saklanir
*   Düz metin sifre veritabaninda asla saklanmaz
*   Hash işlemi kayit ve giris sirasinda otomatik yapilir

## Kurulum ve Calistirma

### Gereksinimler
*   .NET 9.0 SDK
*   PostgreSQL 12 veya üzeri
*   Google Gemini API Key (AI özellikleri için)

### Adımlar

1.  **Veritabanini Hazirlayin:**
    *   PostgreSQL'in kurulu ve calisir durumda oldugundan emin olun
    *   `appsettings.Development.json` dosyasindaki baglanti dizesini kendi veritabani bilgilerinize gore duzenleyin:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=staj_forum;Username=postgres;Password=YOUR_PASSWORD"
      }
    }
    ```

2.  **Gemini API Key Ayarlayın:**
    *   `appsettings.Development.json` dosyasina Gemini API anahtarinizi ekleyin:
    ```json
    {
      "GeminiApiKey": "YOUR_GEMINI_API_KEY"
    }
    ```

3.  **JWT Secret Key Ayarlayın:**
    *   `appsettings.json` dosyasinda JWT token icin guclu bir secret key tanimlayin (minimum 64 karakter önerilir)

4.  **NuGet Paketlerini Yükleyin:**
    ```bash
    dotnet restore
    ```

5.  **Migrations Uygulayin:**
    Veritabanini olusturmak icin proje dizininde sunu calistirin:
    ```bash
    dotnet ef database update
    ```

6.  **Uygulamayi Baslatin:**
    ```bash
    dotnet run
    ```
    Sunucu varsayilan olarak `http://localhost:5037` veya `https://localhost:7153` adresinde calisacaktir.

7.  **Swagger UI'a Erisim:**
    *   Development modunda çalışırken `http://localhost:5037/swagger` adresinden API dokümantasyonuna erişebilirsiniz

## Proje Mimarisi

Proje **Layered Architecture (Katmanli Mimari)** yapisina sahiptir:

### Katmanlar

#### 1. **Controllers (API Katmani)**
*   HTTP isteklerini karsilar ve yonlendirir
*   Input validasyonu yapar
*   HTTP response'larini olusturur
*   **Dosyalar:** `AuthController.cs`, `ForumController.cs`, `ChatController.cs`, `ContactController.cs`

#### 2. **Services (Is Mantigi Katmani)**
*   Business logic'i yurutur
*   Veri dogrulama ve is kurallari uygulanir
*   Repository'ler ile iletisim kurar
*   **Interface'ler:** `IAuthService`, `IForumService`, `IChatService`, `IContactService`, `IGeminiService`
*   **Implementasyonlar:** `AuthService.cs`, `ForumService.cs`, `ChatService.cs`, `ContactService.cs`, `GeminiService.cs`

#### 3. **Repositories (Veri Erisim Katmani)**
*   Veritabani CRUD islemlerini gerceklestirir
*   Entity Framework Core kullanir
*   Generic Repository Pattern uygulanmistir
*   **Interface'ler:** `IRepository<T>`, `IForumRepository`, `IContactRepository`
*   **Implementasyonlar:** `Repository<T>`, `ForumRepository.cs`, `ContactRepository.cs`

#### 4. **Models (Veri Modelleri)**
*   Veritabani tablolarini temsil eder
*   Entity Framework Core annotations icerir
*   **Dosyalar:** `User.cs`, `Topic.cs`, `Reply.cs`, `ContactMessage.cs`

#### 5. **DTOs (Data Transfer Objects)**
*   API request/response modelleri
*   Client-Server arasi veri transferi icin kullanilir
*   **Klasörler:** `Auth/`, `Forum/`, `Chat/`, `Contact/`, `Common/`

#### 6. **Data (Veritabani Context)**
*   `ApplicationDbContext.cs`: Entity Framework DbContext
*   Veritabani konfigurasyonlari ve iliskileri tanimlar
*   Migration dosyalari

#### 7. **Mappings**
*   `MappingProfile.cs`: AutoMapper profilleri
*   Model-DTO donusumleri

## Servisler Detayi

### AuthService
*   Kullanici kayit ve giris islemleri
*   JWT token olusturma
*   Sifre hashleme ve dogrulama
*   Kullanici bilgilerini getirme

### ForumService
*   Konu olusturma, guncelleme, silme
*   Cevap yonetimi
*   Sayfalama ve siralama
*   Arama fonksiyonlari
*   Kullanici bazli konu/cevap listeleme
*   Goruntulenme sayisi yonetimi

### ChatService
*   Google Gemini API entegrasyonu
*   Sohbet yonetimi
*   CV analizi
*   Konusma gecmisi yonetimi

### ContactService
*   Iletisim mesajlari yonetimi
*   Okundu isaretleme
*   Sayfalama ve filtreleme

### GeminiService
*   Google Gemini API ile iletisim
*   AI yanit olusturma
*   HTTP client yonetimi

## CORS Politikasi

*   Sadece localhost'tan gelen isteklere izin verilir
*   Tum portlar kabul edilir (development icin)
*   Credentials desteklenir
*   Tum HTTP metodlari ve header'lara izin verilir

## Guvenlik Ozellikleri

*   JWT Bearer Authentication
*   BCrypt sifre hashleme
*   CORS koruması
*   SQL Injection koruması (Entity Framework parametreli sorgular)
*   XSS koruması (input validasyonu)
*   Kullanici bazli yetkilendirme (sadece kendi içeriğini düzenleyebilir)

## Veritabani Migrations

Proje su migration'lari icerir:
*   `AddUserAuthentication`: Kullanici tablosu ve authentication sistemi eklenmesi

Yeni migration olusturmak icin:
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

## NuGet Paketleri

*   `Microsoft.AspNetCore.OpenApi` (9.0.9): OpenAPI/Swagger desteği
*   `Microsoft.EntityFrameworkCore` (9.0.0): ORM framework
*   `Microsoft.EntityFrameworkCore.Design` (9.0.0): Migration araçları
*   `Npgsql.EntityFrameworkCore.PostgreSQL` (9.0.2): PostgreSQL provider
*   `Microsoft.AspNetCore.Authentication.JwtBearer` (9.0.0): JWT authentication
*   `BCrypt.Net-Next` (4.0.3): Sifre hashleme
*   `AutoMapper` (12.0.1): Object mapping
*   `AutoMapper.Extensions.Microsoft.DependencyInjection` (12.0.1): DI entegrasyonu

## Gelistirme Notlari

*   Development ortaminda HTTPS yonlendirmesi devre disi birakilmistir
*   Swagger UI sadece development modunda aktiftir
*   Loglama seviyesi `appsettings.json` dosyasindan ayarlanabilir
*   JSON serialization'da circular reference'lar ignore edilir
*   Null degerler JSON response'larinda gosterilmez

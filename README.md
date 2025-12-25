# Staj Forum Backend

Staj Forum uygulamasinin RESTful API hizmetini saglayan backend sunucusudur. Web ve mobil uygulamalarla es zamanli calisarak veri yonetimi, forum islemleri ve yapay zeka entegrasyonlarini yonetir.

## Teknolojiler

*   **Platform:** .NET Core 9
*   **Veritabani:** PostgreSQL
*   **ORM:** Entity Framework Core
*   **AI Entegrasyonu:** Google Gemini API
*   **Dokumantasyon:** OpenAPI (Swagger)
*   **Nesne Esleme:** AutoMapper

## Veritabani Yapisi

Proje PostgreSQL veritabanini kullanir. Ana tablolar:

1.  **Topics:** Forum konulari (Baslik, Icerik, Olusturma Tarihi vb.).
2.  **Replies:** Konulara verilen cevaplar (Icerik, Konu ID vb.).
3.  **Contacts:** Iletisim formu mesajlari.

Baglanti ayarlari `appsettings.Development.json` dosyasinda yapilandirilmistir.

## API Endpointleri

Uygulamanin sundugu API uclari asagidaki gibidir:

### 1. Forum Islemleri (ForumController)

*   `GET /api/forum/topics`: Tum konulari listeler (sayfalama ve arama destekli).
*   `GET /api/forum/topics/{id}`: Belirli bir konunun detaylarini getirir.
*   `GET /api/forum/topics/{id}/replies`: Bir konuya ait cevaplari listeler.
*   `POST /api/forum/topics`: Yeni bir konu olusturur.
*   `POST /api/forum/topics/{id}/replies`: Bir konuya yeni cevap ekler.
*   `PUT /api/forum/topics/{id}`: Var olan bir konuyu gunceller.
*   `PUT /api/forum/replies/{id}`: Bir cevabi gunceller.
*   `DELETE /api/forum/topics/{id}`: Bir konuyu siler.
*   `DELETE /api/forum/replies/{id}`: Bir cevabi siler.

### 2. AI Sohbet ve Analiz (ChatController)

*   `POST /api/chat`: ChatStj asistani ile sohbet baslatir veya devam ettirir.
*   `POST /api/chat/analyze-cv`: Gonderilen CV metnini yapay zeka ile analiz eder (Web ve Mobil tarafindan OCR yapildiktan sonra metin buraya gonderilir).
*   `GET /api/chat/history`: (Gelistirme Asamasinda) Sohbet gecmisini getirir.
### 3. Iletisim Formu (ContactController)

*   `POST /api/contact`: Yeni bir iletisim mesaji gonderir.
*   `GET /api/contact`: Tum iletisim mesajlarini listeler (Sayfalama ve 'okundu' filtreleme destekli).
*   `GET /api/contact/{id}`: Belirli bir mesajin detaylarini getirir.
*   `PUT /api/contact/{id}/read`: Bir mesaji 'okundu' olarak isaretler.
*   `DELETE /api/contact/{id}`: Bir iletisim mesajini siler.

## Kurulum ve Calistirma

1.  **Veritabanini Hazirlayin:**
    PostgreSQL'in kurulu ve calisir durumda oldugundan emin olun. `appsettings.Development.json` dosyasindaki baglanti dizesini kendi veritabani bilgilerinize gore duzenleyin.

2.  **Migrations Uygulayin:**
    Veritabanini olusturmak icin proje dizininde sunu calistirin:
    ```bash
    dotnet ef database update
    ```

3.  **Uygulamayi Baslatin:**
    ```bash
    dotnet run
    ```
    Sunucu varsayilan olarak `http://localhost:5037` veya `https://localhost:7153` adresinde calisacaktir.

## Servisler

Proje Layered Architecture (Katmanli Mimari) yapisina sahiptir:

*   **Controllers:** API isteklerini karsilar.
*   **Services:** Is mantigini (Business Logic) yurutur (orn: ChatService, ForumService).
*   **Repositories:** Veritabani islemlerini (CRUD) gerceklestirir.
*   **DTO (Data Transfer Objects):** Istemci ile sunucu arasindaki veri tasima modelleridir.

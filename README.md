# Digital Gardener - EN

![.NET](https://img.shields.io/badge/.NET-8-blue.svg)![Hangfire](https://img.shields.io/badge/Hangfire-1.8-brightgreen.svg)![License](https://img.shields.io/badge/License-MIT-yellow.svg)

This project is a .NET 8 Web API application where users grow virtual plants, and all maintenance processes are automated using **Hangfire** background jobs.

## Project Purpose

The main goal of this project is to learn and practice the capabilities of **Hangfire**, one of the most popular background job managers in the .NET ecosystem. The project demonstrates Hangfire’s core job types (Fire-and-Forget, Delayed, Recurring) in a meaningful, real-world scenario.

## Technologies Used

*   **Backend:**
    *   [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
    *   ASP.NET Core Web API
    *   Entity Framework Core
*   **Background Jobs:**
    *   [Hangfire](https://www.hangfire.io/)
*   **Database:**
    *   Microsoft SQL Server
*   **API Documentation:**
    *   Swagger (Swashbuckle)

## Project Structure

* **Services/**
    * `IPlantService` & `PlantService`: Plant management operations
    * `INotificationService` & `NotificationService`: Notification operations (ILogger)
* **Models/**
    * `Plant`: Plant entity model
    * `HealthStatus`: Health status enum
* **Data/**
    * `ApplicationDbContext`: EF Core DbContext class

## Error Handling and Logging

The project uses .NET’s built-in `ILogger` infrastructure. Specifically, the following are logged:

* Hangfire job statuses
* Plant health status changes
* Critical process errors
* Notification sending processes

This helps to better understand how Hangfire manages background tasks.

## Core Features

*   **Planting Seeds:** Users can add new plants (seeds) into the system.
*   **Automated Maintenance:** Processes like watering and sunlight exposure are fully automated.
*   **Dynamic Health Monitoring:** Water levels of plants are continuously checked, and their health statuses (Healthy, Thirsty, Overwatered) are updated dynamically.
*   **Interactive Actions:** Users can fertilize plants to accelerate their growth.
*   **Automated Notifications:** Alerts are scheduled for plants whose health condition deteriorates.

---

## Hangfire Integration and Job Types

At the heart of the project lies Hangfire, managing all automated processes. Below is an explanation of which feature is implemented using which Hangfire job type.

**NOTE:** If you don’t want to wait for the scheduled intervals, you can manually trigger jobs from the Hangfire dashboard.

### 1. Fire-and-Forget

Used for tasks that should be executed immediately without requiring the user to wait for completion.

*   **Feature:** **Plant Seed** (`POST /api/plants/plant-seed`)
*   **Implementation:** When a user plants a new seed, the `IPlantService.CreatePlantAsync` method is triggered as a Fire-and-Forget job. While the record is being saved to the database in the background, the user instantly receives a `202 Accepted` response.

### 2. Recurring Jobs

Jobs that need to run repeatedly on a schedule (using CRON expressions). These provide the autonomous behavior of the system.

*   **Feature:** **Daily Watering** (`IPlantService.WaterAllPlants`)
    *   **Schedule:** Every midnight (`Cron.Daily`).
    *   **Description:** Periodically increases the `WaterLevel` of all plants.

*   **Feature:** **Sunlight Exposure** (`IPlantService.GiveSunlightToAllPlants`)
    *   **Schedule:** Every 4 hours (`0 */4 * * *`).
    *   **Description:** Increases `GrowthPoints` to simulate plant growth.

*   **Feature:** **Hourly Health Check** (`IPlantService.PerformHealthCheckAsync`)
    *   **Schedule:** Every hour (`Cron.Hourly`).
    *   **Description:** Monitors water levels of all plants. Updates `HealthStatus` for plants outside of critical thresholds.

### 3. Delayed Jobs

Jobs scheduled to run after a certain delay instead of immediately.

*   **Feature:** **Fertilizer Effect Application** (`POST /api/plants/{plantId}/fertilize`)
    *   **Delay:** 2 Hours.
    *   **Implementation:** When a user fertilizes a plant, the `IPlantService.ApplyFertilizerEffectAsync` method is scheduled to run 2 hours later, simulating delayed fertilizer effects.

*   **Feature:** **Health Alert Notification**
    *   **Delay:** 24 Hours.
    *   **Implementation:** If `PerformHealthCheckAsync` marks a plant as unhealthy, a `INotificationService.CheckAndNotifyForUnhealthyPlant` job is scheduled 24 hours later. If the plant is still unhealthy, a warning (currently logged) is produced.

---

## Running the Project Locally

### Requirements

*   [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
*   Microsoft SQL Server

### Setup Steps

1.  **Clone the Project:**
    ```bash
    git clone https://github.com/kayamuhammet/dijital-bahcivan-hangfire.git
    cd backend
    ```

2.  **Configure Database Connections:**
    Open `appsettings.Development.json` and update the `ConnectionStrings` according to your SQL Server setup. The project will create two databases: one for app data (`DefaultConnection`) and one for Hangfire (`HangfireConnection`).

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=(LocalServerName)\\SQLEXPRESS;Database=DbName;Trusted_Connection=True;TrustServerCertificate=True;",
        "HangfireConnection": "Server=(LocalServerName)\\SQLEXPRESS;Database=HangfireDbName;Trusted_Connection=True;TrustServerCertificate=True;"
    }
    ```

3.  **Create the Database:**
    Run EF Core migrations to create the database schema:
    ```bash
    dotnet ef database update
    ```

4.  **Run the Application:**
    ```bash
    dotnet run
    ```
    By default, the app will run on `http://localhost:5014`.

### Verification

*   **API:** Open `https://localhost:5014/swagger` to explore and test API endpoints.
*   **Hangfire Dashboard:** Open `https://localhost:5014/hangfire` to monitor scheduled, running, and completed jobs.

## API Endpoints

All endpoints can be viewed and tested through Swagger.

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/plants/plant-seed` | Plants a new seed (e.g., "Daisy") as a Fire-and-Forget job. |
| `GET` | `/api/plants` | Lists all plants in the system. |
| `POST` | `/api/plants/{plantId}/fertilize` | Fertilizes a plant and schedules the effect 2 hours later (Delayed Job). |

## Future Improvements

While the current implementation is sufficient for demonstrating Hangfire concepts, the project can be further improved to better align with real-world standards:

*   **DTO (Data Transfer Objects):** Use AutoMapper or similar to decouple API models from database entities.
*   **User Management and Authorization:** Add authentication with ASP.NET Core Identity or JWT so each user manages their own garden.
*   **Real-time Notifications (SignalR):** Push instant notifications to the frontend when background jobs complete (e.g., watering done).
*   **Frontend Development:** Build a UI to enhance user interaction and visualize backend processes.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.


---

# Dijital Bahçıvan - TR

![.NET](https://img.shields.io/badge/.NET-8-blue.svg)![Hangfire](https://img.shields.io/badge/Hangfire-1.8-brightgreen.svg)![License](https://img.shields.io/badge/License-MIT-yellow.svg)

Bu proje, kullanıcıların sanal bitkiler yetiştirdiği ve bu bitkilerin tüm bakım süreçlerinin **Hangfire** arka plan görevleri ile otomatikleştirildiği bir .NET 8 Web API uygulamasıdır.

## Projenin Amacı

Bu proje, .NET ekosistemindeki en popüler arka plan görev yöneticilerinden biri olan **Hangfire** kütüphanesinin yeteneklerini öğrenmek ve pratik olarak uygulamak amacıyla geliştirilmiştir. Proje, Hangfire'ın temel görev tiplerini (Fire-and-Forget, Delayed, Recurring) anlamlı ve bütünsel bir senaryo içinde sergilemektedir.

## Kullanılan Teknolojiler

*   **Backend:**
    *   [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
    *   ASP.NET Core Web API
    *   Entity Framework Core
*   **Arka Plan Görevleri:**
    *   [Hangfire](https://www.hangfire.io/)
*   **Veritabanı:**
    *   Microsoft SQL Server
*   **API Dokümantasyonu:**
    *   Swagger (Swashbuckle)

## Proje Yapısı

* **Services/**
    * `IPlantService` & `PlantService`: Bitki yönetimi işlemleri
    * `INotificationService` & `NotificationService`: Bildirim işlemleri (ILogger)
* **Models/**
    * `Plant`: Bitki entity modeli
    * `HealthStatus`: Sağlık durumu enum'u
* **Data/**
    * `ApplicationDbContext`: EF Core DbContext sınıfı

## Hata Yönetimi ve Loglama

Proje, .NET'in yerleşik `ILogger` altyapısını kullanmaktadır. Özellikle:

* Hangfire job'larının çalışma durumları
* Bitki sağlık durumu değişiklikleri
* Kritik işlem hataları
* Bildirim gönderim süreçleri

gibi önemli olaylar loglanmaktadır. Amacım Hangfire'ın arka plan işlemlerini
daha iyi anlayabilmektir.


## Temel Özellikler

*   **Tohum Ekme:** Kullanıcılar sisteme yeni bitkiler (tohum) ekleyebilir.
*   **Otomatik Bakım:** Bitkilerin sulanması ve güneş ışığı alması gibi süreçler tamamen otomatiktir.
*   **Dinamik Sağlık Takibi:** Bitkilerin su seviyeleri sürekli kontrol edilir ve sağlık durumları (Sağlıklı, Susamış, Aşırı Sulanmış) dinamik olarak güncellenir.
*   **Etkileşimli Eylemler:** Kullanıcılar bitkilerine gübre vererek gelişimlerini hızlandırabilir.
*   **Otomatik Bildirimler:** Sağlık durumu kötüleşen bitkiler için kullanıcılara uyarılar planlanır.

---

## Hangfire Entegrasyonu ve Görev Tipleri

Projenin kalbi, tüm otomatik süreçleri yöneten Hangfire'dır. Aşağıda, hangi özelliğin hangi Hangfire görev tipiyle hayata geçirildiği açıklanmıştır.

**NOT:** Eğer ki işlem sürelerini beklemek istemiyorsanız hangfire arayüzü üzerinde
manuel olarak tetikleme yapabilirsiniz.

### 1. Fire-and-Forget (Ateşle ve Unut)

Anında başlatılması gereken ancak kullanıcının tamamlanmasını beklemesine gerek olmayan görevler için kullanılır.

*   **Özellik:** **Tohum Ekme** (`POST /api/plants/plant-seed`)
*   **Uygulama:** Kullanıcı yeni bir tohum ektiğinde, `IPlantService.CreatePlantAsync` metodu bir Fire-and-Forget görevi olarak tetiklenir. Bu sayede, veritabanına kayıt işlemi arka planda yapılırken kullanıcıya anında `202 Accepted` yanıtı dönülür ve arayüz kilitlenmez.

### 2. Recurring Jobs (Tekrarlayan Görevler)

Belirli bir zamanlamaya göre (CRON ifadesi ile) tekrar tekrar çalışması gereken görevlerdir. Projenin otonom yapısını bu görevler sağlar.

*   **Özellik:** **Günlük Sulama** (`IPlantService.WaterAllPlants`)
    *   **Zamanlama:** Her gün gece yarısı (`Cron.Daily`).
    *   **Açıklama:** Tüm bitkilerin `WaterLevel` değerini periyodik olarak artırır.

*   **Özellik:** **Güneş Işığı Verme** (`IPlantService.GiveSunlightToAllPlants`)
    *   **Zamanlama:** Her 4 saatte bir (`0 */4 * * *`).
    *   **Açıklama:** Bitkilerin `GrowthPoints` değerini artırarak büyümelerini simüle eder.

*   **Özellik:** **Saatlik Sağlık Kontrolü** (`IPlantService.PerformHealthCheckAsync`)
    *   **Zamanlama:** Her saat başı (`Cron.Hourly`).
    *   **Açıklama:** Tüm bitkilerin su seviyelerini kontrol eder. Kritik eşiklerin (çok düşük veya çok yüksek) dışına çıkan bitkilerin `HealthStatus` değerini günceller.

### 3. Delayed Jobs (Gecikmeli Görevler)

Bir olayın ardından hemen değil, belirli bir süre geçtikten sonra çalıştırılması gereken görevlerdir.

*   **Özellik:** **Gübre Etkisinin Uygulanması** (`POST /api/plants/{plantId}/fertilize`)
    *   **Gecikme:** 2 Saat.
    *   **Uygulama:** Kullanıcı bir bitkiye gübre verdiğinde, `IPlantService.ApplyFertilizerEffectAsync` metodu anında değil, 2 saat sonrasına zamanlanır. Bu, gübrenin etkisinin zamanla ortaya çıkmasını simüle eder.

*   **Özellik:** **Sağlık Uyarısı Bildirimi**
    *   **Gecikme:** 24 Saat.
    *   **Uygulama:** `PerformHealthCheckAsync` görevi bir bitkinin durumunu "Sağlıksız" olarak işaretlediğinde, bu durumun düzeltilip düzeltilmediğini kontrol etmek için 24 saat sonrasına `INotificationService.CheckAndNotifyForUnhealthyPlant` görevi zamanlanır. Eğer bitki hala sağlıksızsa, bir uyarı (şimdilik log olarak) üretilir.

---

## Projeyi Lokalde Çalıştırma

### Gereksinimler

*   [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
*   Microsoft SQL Server

### Kurulum Adımları

1.  **Projeyi Klonlayın:**
    ```bash
    git clone https://github.com/kayamuhammet/dijital-bahcivan-hangfire.git
    cd backend
    ```

2.  **Veritabanı Bağlantılarını Ayarlayın:**
    `appsettings.Development.json` dosyasını açın. `ConnectionStrings` bölümünü kendi SQL Server yapılandırmanıza göre güncelleyin. Proje, biri uygulama verileri (`DefaultConnection`), diğeri Hangfire verileri (`HangfireConnection`) için olmak üzere iki veritabanı oluşturacaktır.

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=(LocalServerName)\\SQLEXPRESS;Database=DbName;Trusted_Connection=True;TrustServerCertificate=True;",
        "HangfireConnection": "Server=(LocalServerName)\\SQLEXPRESS;Database=HangfireDbName;Trusted_Connection=True;TrustServerCertificate=True;"
    }
    ```

3.  **Veritabanını Oluşturun:**
    Projenin veritabanı şemasını oluşturmak için Entity Framework Core migration'larını çalıştırın.
    ```bash
    dotnet ef database update
    ```

4.  **Uygulamayı Çalıştırın:**
    ```bash
    dotnet run
    ```
    Uygulama varsayılan olarak `http://localhost:5014` portunda başlayacaktır.

### Doğrulama

*   **API:** `https://localhost:5014/swagger` adresine giderek Swagger arayüzünü açın ve endpoint'leri test edin.
*   **Hangfire Dashboard:** `https://localhost:5014/hangfire` adresine giderek Hangfire panosunu açın. Buradan zamanlanmış, çalışan ve tamamlanmış görevleri canlı olarak izleyebilirsiniz.

## API Endpoint'leri

Tüm endpoint'ler Swagger arayüzü üzerinden görüntülenebilir ve test edilebilir.

| Method | Endpoint | Açıklama |
| :--- | :--- | :--- |
| `POST` | `/api/plants/plant-seed` | "Papatya" gibi bir bitki türü belirterek yeni bir tohum eker (Fire-and-Forget). |
| `GET` | `/api/plants` | Sistemdeki tüm bitkileri listeler. |
| `POST` | `/api/plants/{plantId}/fertilize` | Belirtilen ID'ye sahip bitkiye gübre verir ve etkisini 2 saat sonrasına planlar (Delayed Job). |

## Gelecek Geliştirmeler

Projenin mevcut hali Hangfire konseptlerini göstermek için yeterli olsa da, gerçek dünya standartlarına daha da yaklaştırmak için aşağıdaki geliştirmeler yapılabilir:

*   **DTO (Data Transfer Object) Kullanımı:** API katmanı ile veritabanı modellerini ayırmak için AutoMapper gibi bir kütüphane ile DTO'lar implemente edilebilir.
*   **Kullanıcı Sistemi ve Yetkilendirme:** ASP.NET Core Identity veya JWT ile bir kimlik doğrulama sistemi eklenerek her kullanıcının kendi bahçesini yönetmesi sağlanabilir.
*   **Real-time Bildirimler (SignalR):** Arka plan görevleri tamamlandığında (örn: bitki sulandığında) kullanıcı arayüzüne anlık bildirimler göndermek.
*   **Frontend Geliştirme::** Projenin backend tarafında yapılan işlemlerini daha iyi anlamak ve kullanıcı etkileşimini hissetmek adına 
frontend geliştirilebilir.

## Lisans

Bu proje MIT Lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakınız.
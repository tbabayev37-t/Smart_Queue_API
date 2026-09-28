# Smart Queue API 🚀

Smart Queue API — kiçik bir xidmət mərkəzində müştərilərin növbəsini səmərəli və rahat idarə etmək üçün hazırlanmış **RESTful Web API** layihəsidir.

---

## 📌 Layihə Haqqında və Arxitektura Yanaşması

Layihə kiçik və konkret tapşırıq olduğu üçün mürəkkəbləşdirmənin qarşısını almaq məqsədilə ayrı-ayrı Class Library-lərdən ibarət çoxqatlı arxitektura yerinə, **monolit tək layihə daxilində təmiz arxitektura (Folder-based Clean Architecture)** prinsipləri tətbiq olunmuşdur. Layihə kiçik miqyaslı olsa da, gələcəkdə rahat genişləndirilə bilməsi üçün daxildə lazımi struktur bölgüsü aparılmışdır.

Layihədə aşağıdakı pattern və yanaşmalardan istifadə olunub:
* **Repository Pattern:** Verilənlər bazası sorğularını biznes məntiqindən ayırmaq üçün.
* **Service Layer:** Biznes məntiqini idarə etmək üçün.
* **Result Pattern (`ResultDto`):** API cavablarını vahid və standart formatda qaytarmaq üçün.
* **FluentValidation:** Daxil olan məlumatların düzgünlüyünü yoxlamaq üçün.

---

## 🛠️ İstifadə Olunan Texnologiyalar

* **Çərçivə (Framework):** .NET 10.0 / C#
* **ORM:** Entity Framework Core (v10.0.12)
* **Verilənlər Bazası:** Microsoft SQL Server
* **Validasiya:** FluentValidation (v12.1.1)
* **Sənədləşdirmə:** Swagger UI (v10.2.3) & XML Comments

---

## 🗄️ Database və Modellər

Verilənlər bazasında müştəri məlumatlarını saxlamaq üçün **`Customer`** obyekti və növbə statusunu bildirmək üçün **`QueueStatus`** enum strukturundan istifadə olunub:

* **`Customer`**
  * `Id` (Guid) — Müştərinin unikal kimliyi
  * `Name` (string) — Müştərinin adı
  * `CreatedAt` (DateTime) — Növbəyə daxil olduğu tarix və saat
  * `Status` (QueueStatus Enum) — `Waiting (1)`, `Serving (2)`, `Completed (3)`

---

## 🔌 API Endpoint-lər

Bütün endpoint-lər `api/queue` marşrutu daxilində fəaliyyət göstərir:

| HTTP Metodu | Endpoint | Açıqlama |
| :--- | :--- | :--- |
| `GET` | `/api/queue` | Növbədə olan bütün müştərilərin siyahısını qaytarır. |
| `POST` | `/api/queue` | Yeni müştərini növbəyə əlavə edir (`Waiting` statusu ilə). |
| `GET` | `/api/queue/{id}` | Göstərilən ID-li müştərinin məlumatlarını qaytarır. |
| `PUT` | `/api/queue` | Müştəri məlumatlarını yeniləyir. |
| `DELETE` | `/api/queue/{id}` | Müştərini növbədən silir. |
| `POST` | `/api/queue/next` | Növbədə gözləyən növbəti müştərini çağırır və statusunu `Serving` edir. |
| `GET` | `/api/queue/{id}/position` | Müştərinin növbədə neçənci olduğunu hesablayır. |

---

## 🧠 /next Endpoint-i üçün Race Condition Həlli

### Problem: Race Condition
Eyni anda iki və ya daha çox operator `/api/queue/next` endpoint-ini çağırdıqda sistemdə **Race Condition** (Eynizamani yarış halı) yaranır. Bu zaman sorğular eyni milisaniyədə bazaya müraciət edərək eyni `Waiting` statuslu müştərini oxuya bilər və nəticədə eyni müştəri iki müxtəlif masaya çağırılmış olar.

### Kodda Tətbiq Etdiyim Həll Yolu
Bu problemin qarşısını almaq üçün kod daxilində aşağıdakı məntiq tətbiq olunub:

1. **Atomik Əməliyyat Axını (Atomic Operation):**
   Müştərinin oxunması və statusunun `Serving` olaraq dəyişdirilməsi tək bir tam əməliyyat kimi icra olunur. `SaveChangesAsync` ilə dəyişiklik dərhal bazaya mənimsədilir.

2. **Tranzaksiya Bütövlüyü:**
   Birinci sorğu müştərini götürüb statusunu `Serving` etdiyi üçün, eyni anda gələn ikinci sorğu artıq həmin müştərini `Waiting` statusunda tapa bilmir və avtomatik olaraq növbədəki növbəti müştəriyə keçir.

---

## 🚀 Layihənin İşə Salınması

1. **Repozitoriyanı klonlayın:**
   ```bash
   git clone https://github.com/tbabayev37-t/Smart_Queue_API.git
   cd Smart_Queue_API
2. **Verilənlər bazası bağlantısını yoxlayın:**
   `appsettings.json` faylındakı `ConnectionStrings` hissəsini öz SQL Server parametrlərinizə uyğun olaraq tənzimləyin.

3. **Verilənlər bazasını yeniləyin (Migration-ları tətbiq edin):**
   `dotnet ef database update`

4. **Layihəni icra edin:**
   `dotnet run`

5. **Test edin:**
   Brauzerdə açılan Swagger UI (`/swagger/index.html`) vasitəsilə bütün API endpoint-lərini test edə bilərsiniz.

```

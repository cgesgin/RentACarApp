# Araç Kiralama Projesi

* Asp Dotnet Core 6 yazılmıştır.
* Üyelik sistemi bulunan ve Sistemdeki rollere göre işlemler yapabileceği uygulamadır.
* N-Katmanlı Mimariyle yazılmıştır.
* Arayüz Web api projesi ile haberleşerek MVC projesiyle oluşturulmuştur.

### Proje Özellikleri

* Kullanıcılar üye olmadan sadece kiralanabilecek araçları görebilmektedir.
* Kullanıcılar email ve password bilgileri alınarak üyelik kaydı yapılmaktadır.
* Üye olan kullanıcılar araç kiralayabilir, kiraladıkları araçları görebilir.
* Ödemesi yapılan araçların bilgileri e-mail olarak iletilir.
* Yönetici rolüne sahip olan kullanıcılar araçlarla ilgili işlemleri yapabileceği menülere erişebilir.
* Yönetici rolüne sahip olan kullanıcılar kiranan araçları kiralayan müşterileriyle birlikte görebilir.
* Yönetici rolüne sahip olan kullanıcılar kiralanan araçların iade edildiğinde aracın statüsünü değiştirerek tekrar kiralanabilir yapabilir.

### Projede Kullanılan teknolojiler

* Asp Dotnet Core 6
* Entity Framework Core 6
* RabbitMQ
* Redis Cache
* InMemory Cache
* MySql
* XUnit

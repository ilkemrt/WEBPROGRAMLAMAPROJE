# Fitness Center Web Application

Bu proje, bir spor salonu için temel ihtiyaçları karşılayacak şekilde geliştirilmiş bir web uygulamasıdır.  
Amaç; üyelerin hizmetleri inceleyebilmesi, antrenörleri görüntüleyebilmesi ve uygun zamanlara göre online randevu alabilmesidir.

Proje ASP.NET Core MVC mimarisi kullanılarak geliştirilmiştir. Veri erişimi için Entity Framework Core tercih edilmiştir.  
Uygulama rol bazlı çalışmaktadır (Admin / Member) ve kullanıcı doğrulama işlemleri ASP.NET Identity üzerinden yapılmaktadır.

## Projede Neler Var?

- Hizmet listeleme ve detay sayfaları  
- Antrenör listeleme, antrenör detayları ve antrenör bazlı randevu alma  
- Online randevu sistemi (çalışma günleri, saatler ve çakışma kontrolü ile)  
- Üyeye ait randevuları görüntüleme  
- Admin tarafında hizmet ve antrenör yönetimi  
- REST API üzerinden randevu ve antrenör verilerine erişim  
- LINQ sorguları ile filtreleme işlemleri  
- Google AI API kullanılarak kişiye özel antrenman ve beslenme planı oluşturma (Fitness AI)

## Fitness AI Özelliği

Uygulama içerisinde basit bir AI modülü bulunmaktadır.  
Kullanıcıdan yaş, kilo, boy, yağ oranı ve hedef bilgileri alınarak, arka planda AI’a gönderilir ve buna göre haftalık bir antrenman planı ile örnek bir beslenme planı oluşturulur.

Bu kısım sohbet mantığında değil, form bazlı ve kontrollü bir yapıdadır.

## Kullanılan Teknolojiler

- ASP.NET Core MVC  
- Entity Framework Core  
- MSSQL  
- ASP.NET Identity  
- REST API  
- LINQ  
- Bootstrap 5  
- Google AI API

## Genel Not

Proje geliştirilirken mümkün olduğunca gerçek hayattaki bir spor salonu senaryosu baz alınmıştır.  
Kod yapısı sade tutulmaya çalışılmış, okunabilirliğe ve mantıksal kontrollerin doğru çalışmasına önem verilmiştir.

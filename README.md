# LearningEnglishWordsServer (ASP.NET Core Web Api Project)

<h1 align="center">به نام خدا<h1/>

  ## نیازمندی ها
  - .Net 6
  - Visual Studio 2022
  - Sql Server Database
  
  ## نحوه استفاده
- در ابتدا در فایل appsettings.json (یا appsettings.develop.json برای حالت دولوپ) در قسمت MySqlServerConnectionString کانکشن استرینگ مربوط به دیتابیس SqlServer خود را قرار دهید.
<br/>

- سپس در Visual Studio وارد مسیر زیر شوید :
<br/>
Tools > NuGet Package Manager > Package Manager Console

<br/>
<br/>


- در پنجره کنسول باز شده Defualt Project را روی Persistence قرار داده و دستور زیر را وارد نمایید تا دیتابیس شما ساخته شود :

update-database
  
  - حال می توانید در محیط Visual Studio و در مد دیباگ (با کلیک راست روی پروژه Server و انتخاب گزینه Set as startup project) از پروژه استفاده نمایید.


## امکانات
- ورود و ثبت نام کاربران
- مدیریت کلمات افزوه شده (مشاهده جزییات هر کلمه و امکان حذف و ویرایش)
- قابلیت مشاهده 10 کلمه افزوه شده اخیر
- قابلیت جست و جو و فیلتر کلمات (بر اساس تاریخ یادگیری ، نوع ، منبع و ترجمه کلمه و ...
- قابلیت ساخت آزمون بر اساس کلمات افزوده شده (دارای فیلتر و همچنین امکان انتخاب تعداد سوالات ، زمان آزمون و ...)
- تحلیل نتایج آزمون
- قابلیت ارسال اعلان توسط SignalR به کلاینت (اعلام نسخه های جدید و امکانات و ...)
- امکان ارسال بازخورد و نظرات
- امکان تغییر دسترسی کاربران توسط مدیر (برای ارسال اعلان و ...)
- و ...


## تماس با من

ایمیل : mortezamirshekar81@gmail.com
تلگرام : t.me/mortezamir81

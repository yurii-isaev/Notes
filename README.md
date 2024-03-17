# Payroll management
Панель администрирования ролей и управление расчетами заработной платы сотрудников.

<div align="center">
  <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/12ae2ef9-ee03-40e8-bcdd-4fe55fb7aeef" alt="logo" style="max-width: 20%; height: auto;">

<h3 align="center">Prototype</h3>
<p align="center">
This is a prototype of a possible enterprise web application  project. <br />
This is not a library. Now it's just a sample project. <br />

<p align="center">
<img src="https://img.shields.io/badge/Made%20with-love-green.svg" alt="Made with love">
<a href='https://www.postgresql.org/'>
<img src='http://img.shields.io/badge/Database-PostgreSQL-indigo.svg'/>
</a>
<a href="https://docs.nunit.org/">
  <img src="https://img.shields.io/badge/Coverage-none-red.svg" alt="Coverage Badge"/>
</a>
<a href="https://opensource.org/licenses/">
<img src="https://img.shields.io/badge/License-none-red.svg"/>
</a>
<img src='http://img.shields.io/badge/Status-support-blue.svg'/>
</p>
<br/>
</div>

# About:

1. Данный проект представляет собой веб приложение классический монолит, построенный на принципах паттерна MVC, с разделенной бизнес-логикой, ориентированной на серверный сайт-рендеринг.
2. Проект совмещает логику менеджера сотрудников по расчету заработной платы, публикации новостей, учет сотрудников и прочий функционал, который представлен навигационной панелью `Manager panel`.
3. Проект совмещает логику администратора по контролю за действиями менеджера, а также действия, связанные с непосредственно администрированием ролей и учетом пользователей, защищенный паролем и представлен навигационной панелью `Admin panel`.

В описании Readme не представлены все подфункции основной функциональности, а описаны лишь основные службы управления приложением.

# Stack:
`Authentication`: Asp.net core Identity  
`App`:  Asp.net core 7 mvc  
`Database`: PostgreSQL  
`Persistent`:  Entity-framework  
`UI Design`: Hyper Bootstrap Template  / custom  
`Validation`:  Asp.net DataAnnotations (system lib)  

# Features:

###  ★ Сommon feature / Search by name on page
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
  <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/c0b0356a-6f1b-4aef-977c-95ad0a19b10b" alt="Signin " style="width: 100%; height: auto;">
 </kbd>
</div>

###  ★ Сommon feature /  Search result
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
  <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/e79617d2-7c00-400d-a36e-64c44287b6c1" alt="Signin " style="width: 100%; height: auto;">
 </kbd>
</div>

###  ★ Сommon feature / Page pagination
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
  <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/e730079a-f705-4aa4-9b65-58d551b17868" alt="Signin " style="width: 100%; height: auto;">
 </kbd>
</div>

###  ★ Сommon feature / Page pagination
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
  <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/88c507a8-543a-422b-96d2-2fe8c48cf505" alt="Signin " style="width: 100%; height: auto;">
 </kbd>
</div>

###  ★ Authentication.

###  ⚡️ Log in / Sign in with validation form.
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
  <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/8564303d-525c-48a3-85a8-398317725bc2" alt="Signin " style="width: 100%; height: auto;">
 </kbd>
</div>

###  ⚡️ Register / Sign up with validation form.
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
  <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/1859f72a-32c6-47cd-b2f1-33fab58dfb4a" alt="Signup" style="width: 100%; height: auto;">
 </kbd>
</div>

###  ⚡️ Account settings manager.
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
  <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/214a1b59-d800-4c43-a0e5-7f2a2bacd844" alt="Settings" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Manager page / news list page.
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
  <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/519fc43c-68df-47b8-b28a-94d54b400c07" alt="News" style="width: 100%; height: auto;">
 </kbd>
 </div>

### ★ Manager page / create news page.
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/0dca3b74-fcf5-4239-833b-64521fa13b8f" alt="News" style="width: 100%; height: auto;">
 </kbd>
 </div>

### ★ Manager page / news list page.  
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/07c7b787-9cf7-4280-8ff1-53b6c850c0c5" alt="News" style="width: 100%; height: auto;">
 </kbd>
 </div>

### ★ Manager page / employee list page.  
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/dd5afd10-88f8-4d5a-8086-d7f59803ed5a" alt="Employee" style="width: 100%; height: auto;">
 </kbd>
 </div>

### ★ Manager page / create employee page with invalid form.  
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/8204e423-3502-4efb-8aa4-3752fa3cf888" alt="Employee" style="width: 100%; height: auto;">
 </kbd>
 </div>

### ★ Manager page / edit employee page.  
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/4e2465ba-e31e-42cc-8e13-40e06b7cd04a" alt="Employee" style="width: 100%; height: auto;">
</kbd>
 </div>

### ★ Manager page / employee list page.  
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/4865fb43-e7e2-4ba0-be9b-78f2593edcf7" alt="Employee" style="width: 100%; height: auto;">
</kbd>
 </div>

### ★ Manager page / delete employee page.  
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/65c4b911-5d7e-4731-986e-50e1b3687cc4" alt="Employee" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Manager page / Tax year list.  
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/4742bad3-676f-4a9f-9e7b-c25b01ab1030" alt="TaxYear" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Manager page / Create tax year.  
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/b19eefca-4110-4f34-8905-c88a31ca3358" alt="TaxYear" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Manager page / Create tax year tostr.  
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/400fb07f-38ea-438a-8d55-5a578a36eb68" alt="TaxYear" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Manager page / Delete tax year modal. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/9dc9a29b-ecb5-42f8-85ba-45790e04d381" alt="TaxYear" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Manager page / Payment record list. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/d0638b3e-69ed-4188-ac76-e1cc307b722c" alt="TaxYear" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Manager page / Create payment record with invalid form. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/36636517-5ffb-47ae-9cbf-7c7020049084" alt="TaxYear" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Manager page / Create payment record with valid form. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/39260278-e95c-4355-adb6-f37943020c63" alt="TaxYear" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Manager page / Payment record list after create. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/a9df13ad-1e6a-47bc-9d79-cec064b61817" alt="TaxYear" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Manager page / Pay computation details. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/f84c63a2-46f7-427f-9964-cb2a65f9542e" alt="TaxYear" style="width: 100%; height: auto;">
 </kbd>
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/05fd806c-3f30-4234-8f2f-8bdb065a87fc" alt="TaxYear" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Manager page  / Create a pdf document from a list of payment records.
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/93ef6a88-e3c6-4f49-bb8e-151b6fd2c266" alt="SalaryReceipt" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Manager page  / Create a pdf document from a pay computation details.
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/7c74c64b-44b2-47b3-948c-a6ae4cc4d886" alt="SalaryReceipt" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Manager page / Pdf document of a salary receipt. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/6269bfa5-fdbb-445f-9de6-376fec623974" alt="SalaryReceipt" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Admin page / News list with filters. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/edd7b844-9e51-4310-856c-63e5c69266f7" alt="News" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Admin page / News edit. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/6f5f42db-930f-48ea-8536-c9e29987b459" alt="News" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Admin page / News delete. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/b0988415-6ade-4a51-9c65-ba22106770b0" alt="News" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Admin page / Roles list. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/68280b96-f79c-4767-85de-7732ed5b1b35" alt="Roles" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Admin page / Role create. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/4715fa47-ce7b-4092-a011-459b51bbcce0" alt="Roles" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Admin page / Roles list after create. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/c76e3b5e-54cd-47de-8ef2-a2997e08388d" alt="Roles" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Admin page / Role edit. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/2baf760c-efe6-43fd-8503-d801adab210a" alt="Roles" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Admin page / Role delete modal. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/1a9b9845-2835-404c-83aa-0652e1a7f290" alt="Roles" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Admin page / User list. 
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/b7912b2c-f175-4409-82c1-b68c9b55ce4d" alt="User" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Log in / Blocking the user.
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/c005bb8b-2af4-4326-b4c0-b249672f8ecc" alt="User" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Log in / Unblocking the user.
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/b3ba2c07-9e4c-4a37-93b9-a5eb49b6e0da" alt="User" style="width: 100%; height: auto;">
 </kbd>
</div>

### ★ Log in / Unblocking the user.
<div style="text-align: center">
<kbd style="display: inline-block; width: 80%; height: auto;">
 <img src="https://github.com/yurii-isaev/Payroll/assets/39811288/b27d956a-aab9-4fe9-99d4-c83fd02f3ac0" alt="User" style="width: 100%; height: auto;">
 </kbd>
</div>


## License
This project is unlicensed.


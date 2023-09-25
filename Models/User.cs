using Microsoft.EntityFrameworkCore;
using System;
using System.DirectoryServices.AccountManagement;
using System.Net.WebSockets;
using TestTask.Helper;

namespace TestTask.Models
{
    public partial class Users 
    {
        //private static TestTaskDBContext? _context;
        //public static TestTaskDBContext Context
        //{
        //    get
        //    {
        //        if (_context == null)
        //        {
        //            _context = new TestTaskDBContext();
        //        }
        //        return _context;
        //    }
        //}
        

        //ИСПОЛЬЗУЮ свой класс DbHelper в папке Helpers для упрощения работы


        public static Users? CreateUser(string login, out string message)
        {
            var properties = UserADProperties.GetProperties(login);
            if (properties == null)
            {
                message = "Пользователь не найден в AD!";
                return null;
            }
            else
            {
                var user = new Users
                {
                    Login = login,
                    NameLast = properties?.NameLast,
                    NameFirst = properties?.NameFirst,
                    NameMiddle = properties?.NameMiddle,
                    Email = properties?.Email
                };

                DbHelper.UseContext(context =>
                {                    
                    context.Users.Add(user);
                    context.SaveChanges();                  
                });

                message = "Пользователь добавлен в БД!";
                return user;
            }
        }

        public static bool IsAuthenticated(string login, string password, out string message)
        {
            var adContext = new PrincipalContext(ContextType.Domain);
            if (!adContext.ValidateCredentials(login, password, ContextOptions.Negotiate))
            {
                var user = GetUserByLogin(login);

                if (user == null)
                {
                    message = "Пользователь отсутствует в БД!";
                    return false;
                }
                message = "Аутентификация прошла успешно!";
                return true;
            }
            else
            {
                message = "Неверный пользователь или пароль!";
                return false;
            }
        }

        internal static string RemoveUser(string login)
        {
            Users? user = GetUserByLogin(login);

            try
            {
                DbHelper.UseContext(context =>
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                });

                return "Пользователь удален из БД!";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        internal static Users? GetUserByLogin(string login)
        {
            Users? user = null;

            try
            {
                DbHelper.UseContext(context =>
                {
                    user = context.Users.Where(x => x.Login == login).FirstOrDefault();
                });               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return user;

            //старый код: Context.Users.Where(x => x.Login == login).ToList()[0];
            // замечание 1: зачем преобразовывать в ToList() и брать по индексу 0, если можно использовать FirstOrDefault()
        }

        //Написать метод, который вносит изменения в свойство по наименованию и возвращает строку статуса изменения или ошибки
        internal string ChangeField(Users user, string nameField, string? newValue)
        {
            try
            {
                var type = typeof(Users);
                var prop = type.GetProperty(nameField);
                if (prop != null)
                {
                    var propValue = prop?.GetValue(user);
                    prop?.SetValue(user, newValue);

                    DbHelper.UseContext((context) =>
                    {
                        context.Entry(user).State = EntityState.Modified;
                        context.SaveChanges();
                    });

                    return $"Значение свойства {nameField} изменено с {propValue} на {newValue}";
                }
                else
                {
                    return $"Свойство {nameField} некорректно и вследствие не найдено!";
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "Отработал метод ChangeField";
        }

        private UserADProperties? _properties;

        private UserADProperties? UserADProperties
        {
            get
            {
                if (_properties == null)
                {
                    _properties = UserADProperties.GetProperties(Login);
                }
                return _properties;
            }
        }

        public string? ADEmail => UserADProperties?.Email;
        public string? ADStaffNumber => UserADProperties?.StaffNumber;
        public string? ADDepartment => UserADProperties?.Department;
        public string? ADPosition => UserADProperties?.Position;
    }
}

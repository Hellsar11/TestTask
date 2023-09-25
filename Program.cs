using TestTask.Models;


Console.Write("Введите логин: ");
var login = Console.ReadLine();
var password = ConsoleExtendet.GetPasswordFromConsole();
bool isAuthOK = false;

if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
{
    isAuthOK = Users.IsAuthenticated(login, password, out string message);
    if (!isAuthOK)
    {
        Console.WriteLine("Ошибка аутентификации!!");
    }
    Console.WriteLine(message);
}
else
{
    Console.WriteLine("Введены не корректные данные");
}

if (isAuthOK)
{
    bool isExit = false;
    while (!isExit)
    {
        Console.WriteLine(new string('-', 30));
        var list = new Dictionary<char, string>
            {
              {  '1', "Добавить пользователя" },
              {  '2', "Редактировать пользователя" },
              {  '3', "Удалить пользователя" },
              {  '0', "Выход" }
            };
        var keySelect = ConsoleExtendet.GetSelected("Выбирете действие", list);

        
        if (keySelect == '0')
        {
            isExit = true;
        }
        else if (keySelect == '1')
        {
            Console.Write("Введите логин пользователя для добавления в БД: ");
            string login_new_user = Console.ReadLine() ?? "";
            var user = Users.CreateUser(login_new_user, out string message);
            Console.WriteLine(message);
            if (user != null)
            {
                Console.WriteLine($"{user.NameLast} {user.NameFirst} {user.NameMiddle}");
                Console.WriteLine(user.ADPosition);
            }
        }
        else if (keySelect == '2')
        {
            Console.Write("Введите логин пользователя для изменения в БД: ");
            string login_edit_user = Console.ReadLine() ?? "";
            var user = Users.GetUserByLogin(login_edit_user);
            if (user != null)
            {
                bool selectEdit = true;
                while (selectEdit)
                {
                    var list_edit = new Dictionary<char, string>()
                        {
                          {  '1', "Фамилия" },
                          {  '2', "Имя" },
                          {  '3', "Отчество" },
                          {  '4', "E-mail" },
                          {  '0', "Выход" }
                        };
                    var keySelectEdit = ConsoleExtendet.GetSelected("Выбирете что редактируем", list_edit);
                    if (keySelectEdit != '0')
                    {
                        (string, string) curValue = keySelectEdit switch
                        {
                            '1' => (user.NameLast, nameof(user.NameLast)),
                            '2' => (user.NameFirst, nameof(user.NameFirst)),
                            '3' => (user.NameMiddle, nameof(user.NameMiddle)),
                            '4' => (user.Email, nameof(user.Email)),
                            _ => ("error", "error")
                        };
                        Console.WriteLine($"Текущее значение - {curValue.Item1}");
                        Console.Write("Введите новое значение: ");
                        string? newValue = Console.ReadLine();
                        Console.WriteLine(user.ChangeField(user, curValue.Item2, newValue));
                    }
                    else
                    {
                        selectEdit = false;
                    }
                }
            }
            else
            {
                Console.WriteLine("Пользователь не найден в БД!");
            }
        }
        else if (keySelect == '3')
        {
            Console.Write("Введите логин пользователя для удаления из БД: ");
            string login_delete_user = Console.ReadLine() ?? "";
            Console.WriteLine(Users.RemoveUser(login_delete_user));
        }
    }
}
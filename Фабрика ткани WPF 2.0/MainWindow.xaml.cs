using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.Entity.Validation;
using EntityFramework.Extensions;
using System.IO;
using Microsoft.Win32;

//ПО фабрики ткани.
namespace Фабрика_ткани_WPF_2._0
{
    public class Расчеты
    {
        /// <summary>
        /// Вычисляет площадь ткани
        /// </summary>
        /// <param name="A">Ширина</param>
        /// <param name="B">длина</param>
        /// <returns>Возвращает площадь</returns>
        public static decimal Площадь( decimal A, decimal B)
        {
            decimal S;
            S = A * B;
            return S;
        }
    }

    //Универсальное окно.
    public partial class MainWindow : Window
    {   //Инициализация компонентов.
        public MainWindow()
        {
            InitializeComponent();       
        }
        //Включение и выключение выплывающего окна.
        private void On_off(object sender, RoutedEventArgs e)
        {
            if(Список.Visibility == Visibility.Hidden)
            {
                Список.Visibility = Visibility.Visible;
            }
            else
            {
                Список.Visibility = Visibility.Hidden;
            }
        }
        //Открытие вкладки авторизации.
        private void Вход_выход(object sender, RoutedEventArgs e)
        { if (Войти_Выйти.Content.ToString() == "Вход")
            {
               Вход_рег.Visibility = Visibility.Visible;
                
            }
            else
            {
                MainWindow res = new MainWindow();
                res.Show();
                this.Close();

            }
        }
        //Регистрация нового пользователя.
        private void Зарегестрировать(object sender, RoutedEventArgs e)
        {
            using (var context = new clothEntities())
            { if (Reg_log.Text != "" && Reg_name.Text != "" && Reg_pass.Password != "" && Выбор_типа_пользователя.Text != "")
                {
                    var COUNT = context.Пользователь.Where(L => L.Логин == Reg_log.Text).Select(R => R.Роль);
                   
                    if(COUNT.Count() !=1){
                        if (Выбор_типа_пользователя.Text != "")
                        { var ewa = context.Роль.Where(A => A.Роль1 == Выбор_типа_пользователя.Text).Select(A => A.ID);
                            int ROLE = ewa.First();
                            Пользователь заказчик = new Пользователь()
                            {
                                Имя = Reg_name.Text,
                                Логин = Reg_log.Text,
                                Пароль = Reg_pass.Password,
                                Роль = ROLE

                            };
                            context.Пользователь.Add(заказчик);
                            context.SaveChanges();
                           
                        }

                        MessageBox.Show("Вы успешно зарегестрировались.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка. Такой пользователь уже сущеcтвует");
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста заполните все поля.");
                }

             }   
                 
            
            
        }
        //Войти.
        private void Активировать_вход(object sender, RoutedEventArgs e)
        {
            using (var context = new clothEntities())
            {
                var Вход1 = context.Пользователь.Where(L => L.Логин == Log_login.Text).Where(P => P.Пароль == Log_pass.Password).Select(R => R.Роль);
                if(Вход1.Count() ==1)
                {
                    var Вход2 = context.Пользователь.Where(L => L.Логин == Log_login.Text).Where(P => P.Пароль == Log_pass.Password).Select(N => N.Имя);
                    int co = Convert.ToInt32(Вход1.First());
                    var Вход = context.Роль.Where(L => L.ID == co).Select(R => R.Роль1);
                    Тип_роли.Text = Вход.First();
                    Имя_пользователя.Text = Вход2.First();
                    Войти_Выйти.Content = "Выход";
                    Вход_рег.Visibility = Visibility.Hidden;
                    switch (Вход1.First())
                    {
                        case (1):

                            Button_1.Visibility = Visibility.Visible;
                            button_2.Visibility = Visibility.Visible;
                            Button_1.Content = "Заказ";
                            button_2.Content = "Мои заказы";
                           

                            break;
                        case (2):

                            Button_1.Visibility = Visibility.Visible;
                            button_2.Visibility = Visibility.Visible;
                            button_3.Visibility = Visibility.Visible;
                            Button_1.Content = "Заказ";
                            button_2.Content = "Проверить заказ";
                            button_3.Content = "Изделия";
                            break;
                        case (3):
                            Button_1.Visibility = Visibility.Visible;
                            button_2.Visibility = Visibility.Visible;
                            button_3.Visibility = Visibility.Visible;
                            Button_1.Content = "Поставки";
                            button_2.Content = "Склад";
                            button_3.Content = "Ткани,фурнитура";
                            break;
                        case (4):
                            Button_1.Visibility = Visibility.Visible;
                            //button_2.Visibility = Visibility.Visible;
                            //button_3.Visibility = Visibility.Visible;
                            Button_1.Content = "Изделия";
                            //button_2.Content = "Проверить заказ";
                            //button_3.Content = "Изделия";
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка. Не верный Логин или Пароль");
                }
               

            }
        }
        //Действие при нажатии на кнопку под номером 1.
        private void Нажатие_на_кнопку_1(object sender, RoutedEventArgs e)
        {
            Выбрать_используемую_при_заказе_фурнитуру.Items.Clear();
            Выбрать_используемую_при_заказе_ткань.Items.Clear();
            switch (Button_1.Content)
            {
                case ("Заказ"):
                    Заказы.Visibility = Visibility.Visible;
                    Конструктор_изделий_бордер.Visibility = Visibility.Hidden;
                    using (var context = new clothEntities())
                    { var name_add = context.Изделия.Select(N => N.Наименование);
                        Binding name = new Binding();
                        name.ElementName = "Изделияя";

                        name.Path = new PropertyPath("Items");
                        //    Заказ_комбобокс.Items.Add(name_add);
                        //    foreach (var a in name_add)
                        //    {     
                        //        Заказ_комбобокс.Items.Add(a);
                        мои_заказы_бордер.Visibility = Visibility.Hidden;
                        Заказы.Visibility = Visibility.Visible;
                        Проверить_заказ_бордер.Visibility = Visibility.Hidden;
                        Ткани_изделия_Фурнитура.Visibility = Visibility.Hidden;
                        var склад_ткани = context.Склад_ткани.Select(a => a.Артикул_ткани);
                        var склад_фурнитуры = context.Склад_фунитуры.Select(a => a.Артикул_фурнитуры);
                        if(склад_ткани.Count() == 0 && склад_фурнитуры.Count() == 0)
                        {
                            MessageBox.Show("Извините. Сейчас на складе нет нужных материалов, зайдите позже");
                            Заказы.Visibility = Visibility.Hidden;
                        }
                        Random KOl = new Random();
                        int RAn = KOl.Next(1, 9);
                        //int шр = 0, дл = 0, Колл = 0;
                        switch (RAn)
                        {
                            case (1):

                                Колл = 2;
                                break;
                            case (2):

                                Колл = 2;
                                break;
                            case (3):

                                Колл = 5;
                                break;
                            case (4):

                                Колл = 1;
                                break;
                            case (5):

                                Колл = 0;
                                break;
                            case (6):

                                Колл = 9;
                                break;
                            case (7):

                                Колл = 1;
                                break;
                            case (8):

                                Колл = 2;
                                break;
                            case (9):

                                Колл = 7;
                                break;

                        }
                        //string sa = string.Format("{0},{1},{2}", шр.ToString(), дл.ToString(), Колл.ToString());
                        //MessageBox.Show(sa);
                        //var остатки = context.Остатки_тканей.Where(Sr => Sr.ширина >=шр).Where(dl => dl.длина >= дл).Select(s => s.Артикул_ткани);
                        //if (остатки.Count() == 0)

                        //{
                        //    var tkan = context.Склад_ткани.Where(sr => sr.ширина >= шр).Where(dl => dl.длина >= дл).Select(s => s.Артикул_ткани);
                        //var furnitura = context.Склад_фунитуры.Select(s => s.Артикул_фурнитуры);
                        // foreach (var b in furnitura)
                        //  {
                        //       Выбрать_используемую_при_заказе_фурнитуру.Items.Add(b);
                        //  }
                        //    foreach (var a in tkan)
                        //    {
                        //        Выбрать_используемую_при_заказе_ткань.Items.Add(a);
                        //    }
                        //}
                        //else
                        //{
                        //    var остатки1 = context.Остатки_тканей.Where(Sr => Sr.ширина >= шр).Where(dl => dl.длина >= дл).Select(s => s.Артикул_ткани);
                          var furnitura = context.Склад_фунитуры.Where(KL => KL.Колличество >= Колл).Select(s => s.Артикул_фурнитуры);
                            foreach (var b in furnitura)
                            {
                                Выбрать_используемую_при_заказе_фурнитуру.Items.Add(b);
                            }
                       
                        //}                        
                   }
                   
                    break;
                case ("Поставки"):
                    Поставки_бордер.Visibility = Visibility.Visible;
                    Ткани_изделия_Фурнитура.Visibility = Visibility.Hidden;
                    Склад_ткани_фурнитуры.Visibility = Visibility.Hidden;
                    break;
            }
        }
        //Действие при нажатии на кнопку под номером 2.
        private void нажатие_на_кнопку_2(object sender, RoutedEventArgs e)
        {
         switch(button_2.Content)
            {
                case("Мои заказы"):
                    мои_заказы_бордер.Visibility = Visibility.Visible;
                    Заказы.Visibility = Visibility.Hidden;
                    Конструктор_изделий_бордер.Visibility = Visibility.Hidden;
                    using (var context = new clothEntities())
                    {
                        test54.Items.Clear();
                        var a = context.Заказ.Where(P => P.заказчик == Log_login.Text).Select(ID => ID.номер);
                        
                        foreach(var ac in a)
                        {
                            test54.Items.Add(ac);
                        }
                          int num = Convert.ToInt32(test54.SelectedValue);


                       
            
                    }

                        break;
                case ("Склад"):
                    Склад_ткани_фурнитуры.Visibility = Visibility.Visible;
                    Ткани_изделия_Фурнитура.Visibility = Visibility.Hidden;
                    Поставки_бордер.Visibility = Visibility.Hidden;
                    break;
                case ("Проверить заказ"):
                    мои_заказы_бордер.Visibility = Visibility.Hidden;
                    Заказы.Visibility = Visibility.Hidden;
                    Проверить_заказ_бордер.Visibility = Visibility.Visible;
                    Ткани_изделия_Фурнитура.Visibility = Visibility.Hidden;
                    Конструктор_изделий_бордер.Visibility = Visibility.Hidden;
                    break;
            }
        }
        //Действие при нажатии на кнопку под номером 3.
        private void нажатие_на_кнопку_3(object sender, RoutedEventArgs e)
        {
            switch(button_3.Content)
            {
                case("Изделия"):
                    Ткани_изделия_Фурнитура.Visibility = Visibility.Visible;
                    изделияDataGrid1.Visibility = Visibility.Visible;
                    Проверить_заказ_бордер.Visibility = Visibility.Hidden;
                    Заказы.Visibility = Visibility.Hidden;

                    break;
                case ("Ткани,фурнитура"):
                    Ткани_изделия_Фурнитура.Visibility = Visibility.Visible;
                    Radiobutton_Ткани.Visibility = Visibility.Visible;
                    radiobutton_фурнитура.Visibility = Visibility.Visible;
                    Склад_ткани_фурнитуры.Visibility = Visibility.Hidden;
                    Поставки_бордер.Visibility = Visibility.Hidden;
                    Конвертер_бордер.Visibility = Visibility.Visible;
                  
                    break;
            }
        }
        //Загрузка окна.
        public void загрузка_окна(object sender, RoutedEventArgs e)
        {

            Фабрика_ткани_WPF_2._0.clothDataSet clothDataSet = ((Фабрика_ткани_WPF_2._0.clothDataSet)(this.FindResource("clothDataSet")));
            // Загрузить данные в таблицу Изделия. Можно изменить этот код как требуется.
            Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.ИзделияTableAdapter clothDataSetИзделияTableAdapter = new Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.ИзделияTableAdapter();
            clothDataSetИзделияTableAdapter.Fill(clothDataSet.Изделия);
            System.Windows.Data.CollectionViewSource изделияViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("изделияViewSource")));
            изделияViewSource.View.MoveCurrentToFirst();



            // Загрузить данные в таблицу Ткани. Можно изменить этот код как требуется.
            Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.ТканиTableAdapter clothDataSetТканиTableAdapter = new Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.ТканиTableAdapter();
            clothDataSetТканиTableAdapter.Fill(clothDataSet.Ткани);
            System.Windows.Data.CollectionViewSource тканиViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("тканиViewSource")));
            тканиViewSource.View.MoveCurrentToFirst();
            // Загрузить данные в таблицу Фурнитура. Можно изменить этот код как требуется.
            Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.ФурнитураTableAdapter clothDataSetФурнитураTableAdapter = new Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.ФурнитураTableAdapter();
            clothDataSetФурнитураTableAdapter.Fill(clothDataSet.Фурнитура);
            System.Windows.Data.CollectionViewSource фурнитураViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("фурнитураViewSource")));
            фурнитураViewSource.View.MoveCurrentToFirst();


            // Загрузить данные в таблицу Поставки. Можно изменить этот код как требуется.
            Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.ПоставкиTableAdapter clothDataSetПоставкиTableAdapter = new Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.ПоставкиTableAdapter();
            clothDataSetПоставкиTableAdapter.Fill(clothDataSet.Поставки);
            System.Windows.Data.CollectionViewSource поставкиViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("поставкиViewSource")));
            поставкиViewSource.View.MoveCurrentToFirst();
            // Загрузить данные в таблицу Склад_ткани. Можно изменить этот код как требуется.
            Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.Склад_тканиTableAdapter clothDataSetСклад_тканиTableAdapter = new Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.Склад_тканиTableAdapter();
            clothDataSetСклад_тканиTableAdapter.Fill(clothDataSet.Склад_ткани);
            System.Windows.Data.CollectionViewSource склад_тканиViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("склад_тканиViewSource")));
            склад_тканиViewSource.View.MoveCurrentToFirst();
            // Загрузить данные в таблицу Склад_фунитуры. Можно изменить этот код как требуется.
            Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.Склад_фунитурыTableAdapter clothDataSetСклад_фунитурыTableAdapter = new Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.Склад_фунитурыTableAdapter();
            clothDataSetСклад_фунитурыTableAdapter.Fill(clothDataSet.Склад_фунитуры);
            System.Windows.Data.CollectionViewSource склад_фунитурыViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("склад_фунитурыViewSource")));
            склад_фунитурыViewSource.View.MoveCurrentToFirst();
            // Загрузить данные в таблицу Заказ. Можно изменить этот код как требуется.
            Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.ЗаказTableAdapter clothDataSetЗаказTableAdapter = new Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.ЗаказTableAdapter();
            clothDataSetЗаказTableAdapter.Fill(clothDataSet.Заказ);
            System.Windows.Data.CollectionViewSource заказViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("заказViewSource")));
            заказViewSource.View.MoveCurrentToFirst();
            using (var context = new clothEntities())
            {
                var R = context.Роль.Select(A => A.Роль1);
                foreach (var A in R)
                {
                    Выбор_типа_пользователя.Items.Add(A);
                }
            }
            // Загрузить данные в таблицу Окантовка. Можно изменить этот код как требуется.
            Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.ОкантовкаTableAdapter clothDataSetОкантовкаTableAdapter = new Фабрика_ткани_WPF_2._0.clothDataSetTableAdapters.ОкантовкаTableAdapter();
            clothDataSetОкантовкаTableAdapter.Fill(clothDataSet.Окантовка);
            System.Windows.Data.CollectionViewSource окантовкаViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("окантовкаViewSource")));
            окантовкаViewSource.View.MoveCurrentToFirst();
        }
        int Колл = 0;
       
        //Вывод ткани, фурнитуры.    
        public void Вывод_Ткани_фурнитуры(object sender, SelectionChangedEventArgs e)
        {
           
            using (var context = new clothEntities())
            {
               
                Выбрать_используемую_при_заказе_ткань.Items.Clear();
                if (ЦЕНА.Text != "")
                {
                    var Размер = context.Ткани.FirstOrDefault();
                    var размер_изделия = context.Изделия.Where(D => D.Артикул == АРТИКУЛ.Text).FirstOrDefault();

                    int колличество_рулонов =Convert.ToInt32(Колличество.Text) *Convert.ToInt32(Расчеты.Площадь(Convert.ToInt32(размер_изделия.Длина), Convert.ToInt32(размер_изделия.Ширина)) )/Convert.ToInt32(Расчеты.Площадь(Convert.ToInt32( Размер.Длина),Convert.ToInt32( Размер.Ширина)));
                    decimal точное_колличесто_рулонов = Расчеты.Площадь(Convert.ToInt32(размер_изделия.Длина), Convert.ToInt32(размер_изделия.Ширина)) % Расчеты.Площадь(Размер.Длина, Размер.Ширина);
                    if (точное_колличесто_рулонов > 0 &&колличество_рулонов < 1)
                        колличество_рулонов++;
                    
                    var tkana = context.Склад_ткани.Where(k => k.Рулон >= колличество_рулонов).Select(s => s.Артикул_ткани);
                  
                    foreach (var b in tkana)
                    {
                        Выбрать_используемую_при_заказе_ткань.Items.Add(b);
                    }
                   

                   

                }   }
        }
        //Создание заказа.
        private void Создание_заказа(object sender, RoutedEventArgs e)
        {   if (Сумма.Text != "Сумма")
            {
               
                using (var context = new clothEntities())
                {
                    decimal s = Convert.ToDecimal(Сумма.Text);
                    if (Выбрать_используемую_при_заказе_ткань.Text != "" && Выбрать_используемую_при_заказе_фурнитуру.Text != "")
                    {
                        if (Колличество.Text != "0" && Колличество.Text != "")
                        {          

                            var остатки = context.Остатки_тканей.Where(a => a.Артикул_ткани == Выбрать_используемую_при_заказе_ткань.Text).FirstOrDefault();
                            var остаткыи = context.Остатки_тканей.Where(a => a.Артикул_ткани == Выбрать_используемую_при_заказе_ткань.Text).Select(asd => asd.Номер_остатка);
                            var ткани = context.Ткани.Where(a => a.Артикул == Выбрать_используемую_при_заказе_ткань.Text).FirstOrDefault();
                            var изделия = context.Изделия.Where(a => a.Артикул == АРТИКУЛ.Text).FirstOrDefault();
                            var склад_ткани = context.Склад_ткани.Where(a => a.Артикул_ткани == Выбрать_используемую_при_заказе_ткань.Text).FirstOrDefault();
                            int колличество_рулонов = Convert.ToInt32( Колличество.Text) * (Convert.ToInt32(изделия.Длина) * Convert.ToInt32(изделия.Ширина)) /Convert.ToInt32(ткани.Длина * ткани.Ширина);
                            decimal точное_колличесто_рулонов =Расчеты.Площадь(Convert.ToInt32(изделия.Длина), Convert.ToInt32(изделия.Ширина)) % Расчеты.Площадь(ткани.Длина, ткани.Ширина);
                            if (точное_колличесто_рулонов > 0)
                                колличество_рулонов++;
                            if (остаткыи.Count() != 0)
                            {
                                if (Расчеты.Площадь(Convert.ToInt32(изделия.Длина), Convert.ToInt32(изделия.Ширина)) <= Расчеты.Площадь(остатки.длина, остатки.ширина))
                                {
                                    if (Расчеты.Площадь(Convert.ToInt32(изделия.Длина), Convert.ToInt32(изделия.Ширина)) < Расчеты.Площадь(остатки.длина, остатки.ширина))
                                    { decimal adad = Расчеты.Площадь(остатки.длина, остатки.ширина) - (Расчеты.Площадь(Convert.ToInt32(изделия.Длина), Convert.ToInt32(изделия.Ширина)));
                                        decimal остаток1 = adad/ 100;
                                        остатки.ширина = Convert.ToInt32(остаток1);
                                        остатки.длина = 100;
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        var удаление_из_остатка = context.Остатки_тканей.Delete(a => a.Номер_остатка == остатки.Номер_остатка);
                                        
                                    }
                                }
                                else
                                {
                                    int колличество_рулонов_1 = Convert.ToInt32(Колличество.Text) * (Convert.ToInt32(изделия.Длина) * Convert.ToInt32(изделия.Ширина)) / Convert.ToInt32(ткани.Длина * ткани.Ширина);

                                    decimal сумма_площадей = (Расчеты.Площадь(ткани.Длина, ткани.Ширина) * колличество_рулонов_1) + Расчеты.Площадь(остатки.длина, остатки.ширина);
                                    if (Расчеты.Площадь(Convert.ToInt32(изделия.Длина), Convert.ToInt32(изделия.Ширина)) <= сумма_площадей)
                                    {
                                        if (Расчеты.Площадь(Convert.ToInt32(изделия.Длина), Convert.ToInt32(изделия.Ширина)) == сумма_площадей)
                                        {
                                            var удаление_из_остатка = context.Остатки_тканей.Where(a => a.Номер_остатка == остатки.Номер_остатка).FirstOrDefault();
                                            context.Остатки_тканей.Remove(удаление_из_остатка);
                                            context.SaveChanges();
                                            if (колличество_рулонов_1 == склад_ткани.Рулон)
                                            {
                                                var удаление_со_склада = context.Склад_ткани.Delete(a => a.Артикул_ткани == Выбрать_используемую_при_заказе_ткань.Text);
                                                
                                            }
                                            else
                                            {
                                                склад_ткани.Рулон = склад_ткани.Рулон - Convert.ToInt32(колличество_рулонов_1);
                                                context.SaveChanges();
                                            }
                                        }
                                        else
                                        { decimal ada = сумма_площадей - Расчеты.Площадь(Convert.ToInt32(изделия.Длина), Convert.ToInt32(изделия.Ширина));
                                            decimal остаток_от_отсатка = ( ada/ 100);

                                            остатки.ширина = Convert.ToInt32(остаток_от_отсатка);
                                            остатки.длина = 100;
                                            context.SaveChanges();
                                            if (колличество_рулонов_1 == склад_ткани.Рулон)
                                            {
                                                var удаление_со_склада = context.Склад_ткани.Delete(a => a.Артикул_ткани == Выбрать_используемую_при_заказе_ткань.Text);

                                            }
                                            else
                                            {
                                                склад_ткани.Рулон = склад_ткани.Рулон - Convert.ToInt32(колличество_рулонов_1);
                                                context.SaveChanges();

                                            } ///
                                        }
                                    }
                                    else
                                    {
                                        if (Расчеты.Площадь(Convert.ToInt32(изделия.Длина), Convert.ToInt32(изделия.Ширина)) <= ((Расчеты.Площадь(ткани.Длина, ткани.Ширина) * колличество_рулонов)))
                                        {
                                            if (Расчеты.Площадь(Convert.ToInt32(изделия.Длина), Convert.ToInt32(изделия.Ширина)) == ((Расчеты.Площадь(ткани.Длина, ткани.Ширина) * колличество_рулонов)))
                                            {
                                                if (склад_ткани.Рулон == колличество_рулонов)
                                                {
                                                    var удаление_со_склада = context.Склад_ткани.Delete(a => a.Артикул_ткани == Выбрать_используемую_при_заказе_ткань.Text);
                                                    
                                                }
                                                else
                                                {
                                                    склад_ткани.Рулон = склад_ткани.Рулон - Convert.ToInt32(колличество_рулонов);
                                                    context.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                Random rs = new Random();
                                                int RA = rs.Next();
                                                int cc = (Convert.ToInt32(((Расчеты.Площадь(ткани.Длина, ткани.Ширина) * колличество_рулонов) - Расчеты.Площадь(Convert.ToInt32(изделия.Длина), Convert.ToInt32(изделия.Ширина)))));
                                                int ads = cc / 100;
                                            //    MessageBox.Show(ads.ToString());
                                                Остатки_тканей оыфц = new Остатки_тканей()
                                                {
                                                    Артикул_ткани = Выбрать_используемую_при_заказе_ткань.Text,
                                                    длина = 100,
                                                    ширина = ads,
                                                    Номер_остатка = RA
                                                };
                                                context.Остатки_тканей.Add(оыфц);
                                                context.SaveChanges();

                                                if (склад_ткани.Рулон != null)
                                                {

                                                    if (склад_ткани.Рулон == колличество_рулонов)
                                                    {
                                                        var удаление_со_склада = context.Склад_ткани.Delete(a => a.Артикул_ткани == Выбрать_используемую_при_заказе_ткань.Text);
                                                    }
                                                    else
                                                    {
                                                        склад_ткани.Рулон = склад_ткани.Рулон - Convert.ToInt32(колличество_рулонов);
                                                        context.SaveChanges();
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Извините. На складе нет этих материалов");
                                                }
                                            }
                                        }
                                    }
                                }


                            }
                            else
                            {
                                 if(Расчеты.Площадь(Convert.ToInt32(изделия.Длина), Convert.ToInt32(изделия.Ширина)) <= ((Расчеты.Площадь(ткани.Длина, ткани.Ширина) * колличество_рулонов)))
                                    {
                                        if(Расчеты.Площадь(Convert.ToInt32(изделия.Длина), Convert.ToInt32(изделия.Ширина)) == ((Расчеты.Площадь(ткани.Длина, ткани.Ширина) * колличество_рулонов)))
                                        {
                                            if(склад_ткани.Рулон == колличество_рулонов)
                                            {
                                                var удаление_со_склада = context.Склад_ткани.Delete(a => a.Артикул_ткани == Выбрать_используемую_при_заказе_ткань.Text);
                                              
                                            }
                                            else
                                            {
                                                склад_ткани.Рулон = склад_ткани.Рулон -Convert.ToInt32(колличество_рулонов);
                                                context.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                        Random rs = new Random();
                                        int RA = rs.Next();
                                        int cc = (Convert.ToInt32(((Расчеты.Площадь(ткани.Длина, ткани.Ширина) * колличество_рулонов) - Расчеты.Площадь(Convert.ToInt32(изделия.Длина), Convert.ToInt32(изделия.Ширина)))));
                                        int ads =cc / 100;

                                       
                                        Остатки_тканей оыфц = new Остатки_тканей()
                                        { 
                                            Артикул_ткани = Выбрать_используемую_при_заказе_ткань.Text,
                                            длина = 100,
                                            ширина = ads,
                                            Номер_остатка = RA
                                            


                                          };
                                        context.Остатки_тканей.Add(оыфц);
                                        try
                                        {

                                            context.SaveChanges();

                                        }
                                        catch (DbEntityValidationException ex)
                                        {
                                            foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                                            {
                                                MessageBox.Show("Object: " + validationError.Entry.Entity.ToString());
                                                MessageBox.Show("");
                                                foreach (DbValidationError err in validationError.ValidationErrors)
                                                {
                                                    MessageBox.Show(err.ErrorMessage + "");
                                                }
                                            }
                                        }
                                        



                                            if (склад_ткани.Рулон == колличество_рулонов)
                                            {
                                              var удаление_со_склада = context.Склад_ткани.Delete(a => a.Артикул_ткани == Выбрать_используемую_при_заказе_ткань.Text);
                                                  

                                            
                                             
                                            }
                                            else
                                            {
                                             склад_ткани.Рулон = склад_ткани.Рулон - Convert.ToInt32(колличество_рулонов);
                                             context.SaveChanges();
                                            }
                                        }
                                    }
                                
                                var склад_Фурнитурыы = context.Склад_фунитуры.Where(a => a.Артикул_фурнитуры == Выбрать_используемую_при_заказе_фурнитуру.Text).FirstOrDefault(); 
                                if(склад_Фурнитурыы.Колличество == Колл)
                                {
                                    var склад_Фурнитурыыs = context.Склад_фунитуры.Delete(a => a.Артикул_фурнитуры == Выбрать_используемую_при_заказе_фурнитуру.Text);
                                   
                                }
                                else
                                {
                                    склад_Фурнитурыы.Колличество = склад_Фурнитурыы.Колличество - Колл;
                                    context.SaveChanges();
                                }
                            }

                            context.SaveChanges();
                            
                           
                            Random r = new Random();
                            int rr = r.Next();

                            Заказ закаsз = new Заказ()
                            {
                                номер = rr,
                                Этап_выполнения = 1,
                                дата = DateTime.Today,
                                заказчик = Log_login.Text,
                                стоимость = s,
                                менеджер = null,
                                Артикул_используемой_ткани = Выбрать_используемую_при_заказе_ткань.Text,
                                Артикул_используемой_фурнитуры = Выбрать_используемую_при_заказе_фурнитуру.Text
                            };

                            MessageBox.Show("Заказ на одобрении");
                            context.Заказ.Add(entity: закаsз);
                            context.SaveChanges();
                            Заказные_изделия ZI = new Заказные_изделия()
                            {
                                номер_заказа = rr,
                                Артикул_изделия = АРТИКУЛ.Text,
                                Колличество = Convert.ToInt32(Колличество.Text)
                            };
                            context.Заказные_изделия.Add(ZI);
                            context.SaveChanges();
                            Выбрать_используемую_при_заказе_ткань.Items.Clear();
                            Выбрать_используемую_при_заказе_фурнитуру.Items.Clear();
                            try
                            {



                            }
                            catch (DbEntityValidationException ex)
                            {
                                foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                                {
                                    MessageBox.Show("Object: " + validationError.Entry.Entity.ToString());
                                    MessageBox.Show("");
                                    foreach (DbValidationError err in validationError.ValidationErrors)
                                    {
                                        MessageBox.Show(err.ErrorMessage + "");
                                    }
                                }
                            }
                            var TAU = context.Заказ.Where(R => R.номер == rr).FirstOrDefault();
                            TAU.Этап_выполнения = 2;
                        }
                        else
                        {
                            MessageBox.Show("Невозможно заказать товар в колличестве нуля.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите тип ткани, фурнитуры");
                    }
                }
            }
             
        }
        //Отображение таблицы фурнитуры.
        private void Отобразить_таблицу_фурнитуры(object sender, RoutedEventArgs e)
        {
            if(radiobutton_фурнитура.Visibility == Visibility.Visible)
            {
                фурнитураDataGrid.Visibility = Visibility.Visible;
                тканиDataGrid.Visibility = Visibility.Hidden;
                Тип_конвертера.Items.Clear();
                Тип_конвертера.Items.Add("Киллограммы");
                Тип_конвертера.Items.Add("Штуки");
               
            }
        }
        //Отображение таблицы тканей.
        private void Отобразить_таблицу_ткани(object sender, RoutedEventArgs e)
        {
            if(Radiobutton_Ткани.Visibility == Visibility.Visible)
            {
                фурнитураDataGrid.Visibility = Visibility.Hidden;
                тканиDataGrid.Visibility = Visibility.Visible;
                Тип_конвертера.Items.Clear();
                Тип_конвертера.Items.Add("Рулоны");
                Тип_конвертера.Items.Add("Квадратные метры");
            }
        }
        //Выбор поставки фурнитуры.
        private void Выбрать_поставку_фурнитуры(object sender, RoutedEventArgs e)
        {
            Поставщики.Items.Clear();
            Артикул_ткани_фурнитуры.Items.Clear();
            using (var context = new clothEntities())
            {
                var поставщик = context.Поставщик_ткани.Select(N => N.Поставщик);
                foreach(var b in поставщик)
                {
                    Поставщики.Items.Add(b);
                }
                var Ткани = context.Ткани.Select(N => N.Артикул);
                foreach(var a in Ткани)
                {
                    Артикул_ткани_фурнитуры.Items.Add(a);
                }
                IDSA = 1;
            }
        }
        //Переменные используемые программой.
        int IDSA, TKF = 9;
        //Вывод суммы поставки.
        private void Вывод_данных_о_тканях_фурнитуре(object sender, EventArgs e)
        {
            if (IDSA == 1)
            {
                using (var context = new clothEntities())
                {
                    var Название = context.Ткани.Where(A => A.Артикул == Артикул_ткани_фурнитуры.Text).Select(N => N.Название);
                    var Цена = context.Ткани.Where(A => A.Артикул == Артикул_ткани_фурнитуры.Text).Select(P => P.Цена);
                    if (Название.Count() != 0)
                    {
                        Название_ткани_фурнитуры_поставки.Text = Название.First().ToString();
                        int x = Convert.ToInt32(Колличество_поствки.Text);
                        int y = Convert.ToInt32(Цена.First());
                        int z = x * y;
                        Сумма_поставки.Text = z.ToString();
                    }
                }
            }
            else
            {
                using (var context = new clothEntities())
                {
                    var Название = context.Фурнитура.Where(A => A.Артикул == Артикул_ткани_фурнитуры.Text).Select(N => N.Наименование);
                    var Цена = context.Фурнитура.Where(A => A.Артикул == Артикул_ткани_фурнитуры.Text).Select(P => P.Цена);
                    //MessageBox.Show(Цена.First().ToString());
                    if (Название.Count() != 0)
                    {
                        Название_ткани_фурнитуры_поставки.Text = Название.First().ToString();
                        int x = Convert.ToInt32(Колличество_поствки.Text);
                        int y = Convert.ToInt32(Цена.First());
                        int z = x * y;
                        Сумма_поставки.Text = z.ToString();
                    }
                    
                }
            }
        }
        //Вывод поставки ткани.
        private void Выбрать_поставку_ткани(object sender, RoutedEventArgs e)
        {
            Поставщики.Items.Clear();
            Артикул_ткани_фурнитуры.Items.Clear();
            using (var context = new clothEntities())
            {
                var поставщик = context.Поставщик_Фурнитуры.Select(N => N.Поставщик);
                foreach (var b in поставщик)
                {
                    Поставщики.Items.Add(b);
                }
                var фурнитура = context.Фурнитура.Select(N => N.Артикул);
                foreach (var a in фурнитура)
                {
                    Артикул_ткани_фурнитуры.Items.Add(a);
                }
                IDSA = 0;

            }
        }
        //Заказ товара на склад.
        private void Заказать_товар_и_добавить_его_на_склад(object sender, RoutedEventArgs e)
        {
            if(Поставщики.Text != "" && Артикул_ткани_фурнитуры.Text != "")
            { 
                MessageBox.Show("Товар доставлен на склад. " + "Чтобы отобразить актуальные данные перезайдите в свой аккаунт","Информационное окно" );
                using (var context = new clothEntities())
                {
                    var поставщик_Ф = context.Поставщик_Фурнитуры.Where(N => N.Поставщик == Поставщики.Text).Select(ID => ID.ID);
                    var поставщик_т = context.Поставщик_ткани.Where(N => N.Поставщик == Поставщики.Text).Select(ID => ID.ID);
                    var Цена_т = context.Ткани.Where(A => A.Артикул == Артикул_ткани_фурнитуры.Text).Select(P => P.Цена);
                    var Цена_ф = context.Фурнитура.Where(A => A.Артикул == Артикул_ткани_фурнитуры.Text).Select(P => P.Цена);
                    Random r = new Random();
                    int RR = r.Next();
                    if (IDSA == 1)
                    {
                        Поставки a = new Поставки()
                        {
                            ID_Поставки = RR,
                            Артикул = Артикул_ткани_фурнитуры.Text,
                            Название = Название_ткани_фурнитуры_поставки.Text,
                            Колличетво = Convert.ToInt32(Колличество_поствки.Text),
                            Поставщик = поставщик_т.First(),
                            Тип = 0,
                            Сумма = Convert.ToDecimal(Сумма_поставки.Text),
                            Цена = Convert.ToDecimal(Цена_т.First()),
                        };
                        context.Поставки.Add(a);
                        context.SaveChanges();
                        var проверка_наличия = context.Склад_ткани.Where(p => p.Артикул_ткани == Артикул_ткани_фурнитуры.Text).Select(N => N.Рулон);
                        if (проверка_наличия.Count() != 0)
                        {
                            int x = Convert.ToInt32(проверка_наличия.First());
                            int y = Convert.ToInt32(Колличество_поствки.Text);
                            int update_C_T = x + y;
                            
                            var Колличествооо = context.Склад_ткани.Where(N => N.Артикул_ткани == Артикул_ткани_фурнитуры.Text).FirstOrDefault();
                            Колличествооо.Рулон = update_C_T;
                            context.SaveChanges();
                        }
                        else
                        {
                            var Длина = context.Ткани.Where(ART => ART.Артикул == Артикул_ткани_фурнитуры.Text).Select(S => S.Длина);
                            var ширина = context.Ткани.Where(ART => ART.Артикул == Артикул_ткани_фурнитуры.Text).Select(S => S.Ширина);
                            Склад_ткани storage_cloth = new Склад_ткани()
                            {
                                Артикул_ткани = Артикул_ткани_фурнитуры.Text,
                                Рулон =Convert.ToInt32(Колличество_поствки.Text),
                                длина = Длина.First(),
                                ширина = ширина.First(),
                                Поставка = RR

                            };
                            context.Склад_ткани.Add(storage_cloth);
                            context.SaveChanges();
                        
}
                    }
                    else
                    {
                        Поставки a = new Поставки()
                        {
                            ID_Поставки = RR,
                            Артикул = Артикул_ткани_фурнитуры.Text,
                            Название = Название_ткани_фурнитуры_поставки.Text,
                            Колличетво = Convert.ToInt32(Колличество_поствки.Text),
                            Поставщик = поставщик_Ф.First(),
                            Тип = 1,
                            Сумма = Convert.ToDecimal(Сумма_поставки.Text),
                            Цена = Convert.ToDecimal(Цена_ф.First()),
                        };
                        context.Поставки.Add(a);
                        context.SaveChanges();
                        var проверка_наличия = context.Склад_фунитуры.Where(p => p.Артикул_фурнитуры == Артикул_ткани_фурнитуры.Text).Select(N => N.Колличество);
                        if (проверка_наличия.Count() != 0)
                        {
                            int x = Convert.ToInt32(проверка_наличия.First());
                            int y = Convert.ToInt32(Колличество_поствки.Text);
                            int update_C_F = x + y;
                            var Колличество = context.Склад_фунитуры.Where(N => N.Артикул_фурнитуры == Артикул_ткани_фурнитуры.Text).FirstOrDefault();
                            Колличество.Колличество = update_C_F;
                            context.SaveChanges();

                        }
                        else
                        {
                            Random AR = new Random();
                            int RRR = AR.Next();
                            Склад_фунитуры storage_F = new Склад_фунитуры()
                            {
                                Партия = RRR.ToString(),
                                Артикул_фурнитуры = Артикул_ткани_фурнитуры.Text,
                                Колличество =Convert.ToInt32(Колличество_поствки.Text),
                                Поставка = RR
                            };
                            context.Склад_фунитуры.Add(storage_F);
                            context.SaveChanges();
                            
                        }
                    }
                }
            }
        }
        //Показ талицы с данными о остатках мотериала ткани.
        private void Вывести_данные_со_склада_ткани(object sender, RoutedEventArgs e)
        {
            if (TKF != 0)
            {
                склад_тканиDataGrid.Visibility = Visibility.Visible;
                склад_фунитурыDataGrid.Visibility = Visibility.Hidden;
                TKF = 0;
            }
        }
        //Вывод значений при нажатии на строку в листбоксе.
        private void Вывод_значений(object sender, MouseButtonEventArgs e)
        {
            if (test54.Items.Count != 0)
            {
                int sa = Convert.ToInt32(test54.SelectedValue.ToString());
                using (var context = new clothEntities())
                {
                    int num = Convert.ToInt32(test54.SelectedValue);
                    var art = context.Заказные_изделия.Where(N => N.номер_заказа == num).Select(ART => ART.Артикул_изделия);
                    string artt = art.First().ToString();
                    var name = context.Изделия.Where(N => N.Артикул == artt).Select(AD => AD.Наименование);
                    Название.Content = name.First();
                    var колличество_в_моих_заказах = context.Заказные_изделия.Where(I => I.номер_заказа == sa).Select(K => K.Колличество);
                    Колличество_в_моих_заказах.Text = колличество_в_моих_заказах.First().ToString();
                    var суммаs = context.Заказ.Where(I => I.номер == sa).Select(S => S.стоимость);
                    Сумма_заказа.Text = суммаs.First().ToString();
                    var этап_ID = context.Заказ.Where(I => I.номер == sa).Select(IDS => IDS.Этап_выполнения);
                    int ID = Convert.ToInt32(этап_ID.First().ToString());
                    var текущий_этап_выполнения = context.Текущий_этап_выполнения.Where(IDx => IDx.ID == ID).Select(A => A.Этап_выполнения);
                    Этап_выполнения_в_моих_заказах.Text = текущий_этап_выполнения.First().ToString();
                    var date = context.Заказ.Where(I => I.номер == sa).Select(D => D.дата);
                    Дата_В_моих_заказах.Text = date.First().ToString();
                    var w = context.Заказ.Where(N => N.номер == num).Select(I => I.Этап_выполнения);
                    if (w.First() == 5)
                    {
                        Вывести_оплату.IsEnabled = true;
                    }
                    else
                    {
                        Вывести_оплату.IsEnabled = false;
                    }
                    if (w.First() >= 6)
                    {
                        отмена_заказа_кнопка.IsEnabled = false;
                    }
                    else
                    {
                        отмена_заказа_кнопка.IsEnabled = true;
                    }
                }
            }
        }       
        //Открытие оплаты.
        private void Открыть_оплату(object sender, RoutedEventArgs e)
        {
            Оплата.Visibility = Visibility.Visible;
        }
        //Обновление данных в БД.
        private void Обновить_данные_в_БД(object sender, RoutedEventArgs e)
        {
            using (var context = new clothEntities())
            {
                MessageBox.Show("Извините, но пока эта функция ограничена");
                //int num = Convert.ToInt32(test54.SelectedValue);
                //var count = context.Заказные_изделия.Where(N => N.номер_заказа == num).FirstOrDefault();
                //var art = context.Заказные_изделия.Where(N => N.номер_заказа == num).Select(ART => ART.Артикул_изделия);
                //string artt = art.First().ToString();
                //var price = context.Изделия.Where(A => A.Артикул == artt).Select(par => par.цена);
                //int pr = Convert.ToInt32(price.First());
                //int x = Convert.ToInt32(Колличество_в_моих_заказах.Text);
                //int z = pr * x;
                //var price1 = context.Заказ.Where(N => N.номер == num).FirstOrDefault();
                //if (Колличество_в_моих_заказах.Text != "0" && Колличество_в_моих_заказах.Text != "")
                //{
                //    price1.стоимость = Convert.ToDecimal(z);
                //    count.Колличество = Convert.ToInt32(Колличество_в_моих_заказах.Text);
                //    context.SaveChanges();
                //}
                //else
                //{
                //    MessageBox.Show("Вы не можете изменить колличество на нуль");
                //}                
            }
        }
        //Отмена заказа.
        private void Отмена_заказа(object sender, RoutedEventArgs e)
        {
            using (var context = new clothEntities())
            {
                int num = Convert.ToInt32(test54.SelectedValue);
                var этап_селект = context.Заказ.Where(N => N.номер == num).FirstOrDefault();
                int A = этап_селект.Этап_выполнения;
                if(A <5)
                {
                    var ads = context.Заказ.Where(N => N.номер == num).FirstOrDefault();
                    ads.Этап_выполнения = 4;
                    context.SaveChanges();
                }
               
            }
        }
        //Закрыть и открыть оплату.
        private void закрыть_открыть(object sender, RoutedEventArgs e)
        {
            Оплата.Visibility = Visibility.Hidden;
        }
        //Оплатить.
        private void Оплатить(object sender, RoutedEventArgs e)
        {
            int num = Convert.ToInt32(test54.SelectedValue);
            using (var context = new clothEntities())
            {
                var og = context.Заказ.Where(N => N.номер == num).FirstOrDefault();
                og.Этап_выполнения = 6;
                og.Этап_выполнения = 7;
                og.Этап_выполнения = 8;
                og.Этап_выполнения = 9;
                context.SaveChanges();
            }
            MessageBox.Show("Оплачено");
            Оплата.Visibility = Visibility.Hidden;
        }
        //Расчет суммы заказа.
        private void Расчет_суммы_заказа(object sender, RoutedEventArgs e)
        {
            if (Выбрать_используемую_при_заказе_ткань.Items.Count == 0 || Выбрать_используемую_при_заказе_фурнитуру.Items.Count == 0)
            {
                MessageBox.Show("Извините, на складе недостаточно материалов, попробуйте зайти позже");
                Заказы.Visibility = Visibility.Hidden;
            }
            if (Выбрать_используемую_при_заказе_ткань.Text != "" && Выбрать_используемую_при_заказе_фурнитуру.Text != "")
            {
                using (var context = new clothEntities())
                {
                    

                    var цена_Фурнитуры = context.Фурнитура.Where(a => a.Артикул == Выбрать_используемую_при_заказе_фурнитуру.Text).FirstOrDefault();
                    var размер_изделия = context.Изделия.Where(a => a.Артикул == АРТИКУЛ.Text).FirstOrDefault();
                    var размер_ткани = context.Ткани.Where(a => a.Артикул == Выбрать_используемую_при_заказе_ткань.Text).FirstOrDefault();
                    decimal Сумма1 = (размер_ткани.Цена * Расчеты.Площадь(Convert.ToDecimal(размер_изделия.Ширина), Convert.ToDecimal(размер_изделия.Длина)))/ Расчеты.Площадь(размер_ткани.Длина, Convert.ToDecimal(размер_ткани.Ширина));
                    decimal сумма2 = Convert.ToDecimal (цена_Фурнитуры.Цена)* Колл;
                    decimal сумма3 = Convert.ToDecimal(размер_изделия.цена);
                    decimal сумма4 = (Сумма1 + сумма2 + сумма3)* Convert.ToDecimal( Колличество.Text);
                    Сумма.Text = сумма4.ToString();
                }

            }
        }
        //Подтвердить заказ пользователя.
        private void Подтвердить_заказа_пользователя(object sender, RoutedEventArgs e)
        {
            using (var context = new clothEntities())
            {
                int num = Convert.ToInt32(номер_текстбокс_для_определения.Text);
                var A = context.Заказ.Where(N => N.номер == num).FirstOrDefault();
                A.Этап_выполнения = 5;
                MessageBox.Show("Заказ успешно подтвержден.");
                context.SaveChanges();
                PrintDialog a = new PrintDialog();
                a.ShowDialog();

            }
        }
        //При нажатии на кнопку менеджер привязывается к этому заказу.
        private void Выбор_заказа_менеджером(object sender, SelectionChangedEventArgs e)
        {
            using (var context = new clothEntities())                
            {   
                if (Проверить_заказ_бордер.Visibility == Visibility.Visible)
                {
                    int num = Convert.ToInt32(номер_текстбокс_для_определения.Text);
                    var A = context.Заказ.Where(N => N.номер == num).FirstOrDefault();
                    if(A.Этап_выполнения >=3)
                    {
                        Кнопка_подтверждения.IsEnabled = false;
                        Отмена_заказа_пользователя.IsEnabled = false;
                    }
                    else
                    {
                        Кнопка_подтверждения.IsEnabled = true;
                        Отмена_заказа_пользователя.IsEnabled = true;
                    }
                    if (A.менеджер == null)
                    {
                        
                        A.менеджер = Log_login.Text;
                        context.SaveChanges();
                        A.Этап_выполнения = 3;
                        MessageBox.Show("Вы закреплены за этим заказом, веберите, что нужно с ним сделать.");
                    }
                }
            }
        }
        //Откланение_заказа_пользователя.
        private void Отменить_заказ_пользователя(object sender, RoutedEventArgs e)
        {
            using (var context = new clothEntities())
            {
                int num = Convert.ToInt32(номер_текстбокс_для_определения.Text);
                var A = context.Заказ.Where(N => N.номер ==  num).FirstOrDefault();
                A.Этап_выполнения = 4;
                MessageBox.Show("Вы отменили заказ.");
                context.SaveChanges();
            }
        }
        //Вывод значений конвертера.
        private void Выбор_типа(object sender, EventArgs e)
        {using (var context = new clothEntities())
            {string aaw = Фурнитура_артикул.Text;
                decimal STKG, KVMRUL, RULKVM, MASS;
                double KGST;
                string KVMVRUL = "1,5";
                switch (Тип_конвертера.Text)
                {      
                    case ("Киллограммы"):
                        
                        
                        Из_чего_пользователь_переводит.Text = "Шт.";
                        if (aaw != "")
                        {
                            MASS = Convert.ToDecimal(context.Фурнитура.Where(A => A.Артикул == aaw.ToString()).Select(S => S.Вес).First());
                         STKG = (Convert.ToInt32(Данные_о_колличестве_итд_введенные_пользователем.Text) * MASS) / 1000;
                            Вывод_Конвертера.Text = STKG.ToString();
                        }
                        break;
                    case ("Штуки"):
                        Из_чего_пользователь_переводит.Text = "Кг.";
                        
                        if (aaw != "")
                        {
                            MASS = Convert.ToDecimal(context.Фурнитура.Where(A => A.Артикул == aaw.ToString()).Select(S => S.Вес).First());
                            KGST = (Convert.ToInt32(Данные_о_колличестве_итд_введенные_пользователем.Text)*1000) / Convert.ToInt32(MASS);
                            Вывод_Конвертера.Text = KGST.ToString();
                        }
                        break;
                    case ("Рулоны"):
                        Из_чего_пользователь_переводит.Text = "КВ.М.";
                       
                        KVMRUL = Convert.ToDecimal(Данные_о_колличестве_итд_введенные_пользователем.Text) / Convert.ToDecimal( KVMVRUL);
                        string sa =  string.Format("{0:0.00 }",KVMRUL);
                        
                        Вывод_Конвертера.Text= sa;
                        break;
                    case ("Квадратные метры"):
                        Из_чего_пользователь_переводит.Text = "Рул.";
                        RULKVM = Convert.ToDecimal(Данные_о_колличестве_итд_введенные_пользователем.Text) * Convert.ToDecimal(KVMVRUL);
                        string d = string.Format("{0:0.00 }", RULKVM);
                        
                        Вывод_Конвертера.Text = d;
                        break;
                }
            }
        }
        //Печать данных о поставках.
        private void Печать_поставки(object sender, RoutedEventArgs e)
        {
            PrintDialog pr = new PrintDialog();
            pr.PrintVisual(поставкиDataGrid,"");
            pr.ShowDialog();

        }

        private void Открыть_обзор(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            fileDialog.OpenFile();
            string sa = fileDialog.FileName;
            ImageSource image = new BitmapImage(new Uri(fileDialog.FileName));
           
            Ткань_в_конструкторе.Source = image;
        }

        private void Ширина_ткани_изменить(object sender, RoutedEventArgs e)
        {

            if (Ширина_ткани.Text != "500" && Ширина_ткани.Text != "")
            {
                Ткань_в_конструкторе.Width = Convert.ToDouble(Ширина_ткани.Text);
                 if (Convert.ToInt32(Ширина_ткани.Text) > 500)
                    Ширина_ткани.Text = "500";
               
            }


        }

        private void длина_изделия_изменить(object sender, RoutedEventArgs e)
        {
            if (Ширина_ткани.Text != "500" && длина_ткани.Text != "")
            {
                Ткань_в_конструкторе.Height = Convert.ToDouble(длина_ткани.Text);
                if (Convert.ToInt32(длина_ткани.Text) > 500)
                    длина_ткани.Text = "500";
            }

        }

        private void Открыть_обзор2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            fileDialog.OpenFile();
            string sa = fileDialog.FileName;
            ImageSource image = new BitmapImage(new Uri(fileDialog.FileName));
            Фурнитура1.Source = image;
        }

        private void Фурнитура_вверх(object sender, RoutedEventArgs e)
        {
            double T = Фурнитура1.Margin.Top;
            double B = Фурнитура1.Margin.Bottom;
            double L = Фурнитура1.Margin.Left;
            double R = Фурнитура1.Margin.Right;
            Thickness a = new Thickness();
            a.Top = T-10;
            a.Bottom = B+10;
            a.Left = L;
            a.Right = R;
            Фурнитура1.Margin = a;
        }

        private void фурнитура_вниз(object sender, RoutedEventArgs e)
        {
            double T = Фурнитура1.Margin.Top;
            double B = Фурнитура1.Margin.Bottom;
            double L = Фурнитура1.Margin.Left;
            double R = Фурнитура1.Margin.Right;
            Thickness a = new Thickness();
            a.Top = T+10;
            a.Bottom = B-10;
            a.Right = R;
            a.Left = L;
            Фурнитура1.Margin = a;
        }

        private void Фурнитура_вправо(object sender, RoutedEventArgs e)
        {
            double L = Фурнитура1.Margin.Left;
            double R = Фурнитура1.Margin.Right;
            double T = Фурнитура1.Margin.Top;
            double B = Фурнитура1.Margin.Bottom;
            Thickness a = new Thickness();
            a.Left = L - 10;
            a.Right = R + 10;
            a.Top = T;
            a.Bottom = B;
            Фурнитура1.Margin = a;
        }

      

        private void Влево_фурнитура(object sender, RoutedEventArgs e)
        {
            double T = Фурнитура1.Margin.Top;
            double B = Фурнитура1.Margin.Bottom;
            double L = Фурнитура1.Margin.Left;
            double R = Фурнитура1.Margin.Right;
            Thickness a = new Thickness();
            a.Left = L + 10;
            a.Right = R - 10;
            a.Top = T;
            a.Bottom = B;
            Фурнитура1.Margin = a;
        }

        private void Размер_фурнитуры(object sender, RoutedEventArgs e)
        {if (Размер_фурнитурыыы.Text != "80" && Размер_фурнитурыыы.Text != "")
            {
                Фурнитура1.Height = Convert.ToDouble(Размер_фурнитурыыы.Text);
                if (Convert.ToInt32(Размер_фурнитурыыы.Text) > 80)
                    Размер_фурнитурыыы.Text = "80";
            }
        }

        private void Создать_Пользовательское_изделие(object sender, RoutedEventArgs e)
        {

        }

        private void Открыть_конструктор_заказков(object sender, RoutedEventArgs e)
        {
            Заказы.Visibility = Visibility.Hidden;
            Конструктор_изделий_бордер.Visibility = Visibility.Visible;
        }


        //Показ таблицы с данными о остатках фурнитуры.
        private void Вывести_данные_со_склада_фурнитуры(object sender, RoutedEventArgs e)
        {
           
            if(TKF != 1)
            {
                склад_тканиDataGrid.Visibility = Visibility.Hidden;
                склад_фунитурыDataGrid.Visibility = Visibility.Visible;
                TKF = 1;
            }
          
        }
    }
}
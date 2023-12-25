using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Tourfirm
{
    public partial class Form1 : Form
    {
        Tour tour;
        Person person;

        private void Init()
        {
            tour = new Tour();
            person = new Person();
        }
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=ep-lucky-rice-772590.us-east-2.aws.neon.tech;Port=5432;User Id=elina3galimova;Password=6mSLTHBxAth1;Database=tourfirm");
        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void button_enter_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button_registration_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button_enter_in_Click(object sender, EventArgs e)
        {
            if (textBox_login.Text == string.Empty && textBox_password.Text == string.Empty)
            {
                textBox_login.BackColor = Color.Red;
                textBox_password.BackColor = Color.Red;
                MessageBox.Show("Неверно заполнены поля!");
                return;
            }

            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = $"SELECT \"id\" FROM \"person\" WHERE login = '{textBox_login.Text}' AND password_ = '{textBox_password.Text}'";

            try
            {
                conn.Open();


                if (conn.State == ConnectionState.Open)
                {

                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {

                        textBox_login.Text = string.Empty;
                        textBox_password.Text = string.Empty;

                        person.id = dr.GetInt32(0);
                        PersonalSpace();


                        tabControl1.SelectedIndex = 4;
                    }
                    else
                    {
                        textBox_login.BackColor = Color.Red;
                        textBox_password.BackColor = Color.Red;
                        MessageBox.Show("Неправильный логин или пароль!");
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            finally
            {
                conn.Close();

            }
        }
        public void PersonalSpace()
        {
            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM \"person\" WHERE \"id\" = {person.id}";

            try
            {
                conn.Open();

                if (conn.State == ConnectionState.Open)
                {
                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        person.id = dr.GetInt32(0);
                        person.surname = dr.GetString(1);
                        person.name = dr.GetString(2);
                        person.father = dr.GetString(3);
                        person.birthday = dr.GetString(4);
                        person.email = dr.GetString(5);
                        person.phone = dr.GetString(6);
                        person.login = dr.GetString(7);
                        person.password = dr.GetString(8);


                        dr.Close();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button_reg_next_Click(object sender, EventArgs e)
        {
            if(textBox_reg_fam.Text == string.Empty || textBox_reg_name.Text == string.Empty || textBox_reg_otch.Text == string.Empty ||
                textBox_reg_bd.Text == string.Empty || textBox_reg_email.Text == string.Empty || textBox_reg_pn.Text == string.Empty)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            else
            {
                MessageBox.Show($"Придумайте логин и пароль для вашего профиля!");

                tabControl1.SelectedIndex = 3;
                
            }
        }

        private void button_reg_finish_Click(object sender, EventArgs e)
        {
            if(textBox_cr_login.Text == string.Empty || textBox_cr_pass1.Text == string.Empty || textBox_cr_pass2.Text == string.Empty ||
                (textBox_cr_pass1.Text != textBox_cr_pass2.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            else
            {
                NpgsqlConnection conn = GetConnection();
                NpgsqlCommand cmd = conn.CreateCommand();
                NpgsqlDataReader reader;

                try
                {

                    conn.Open();

                    if (conn.State == ConnectionState.Open)
                    {

                        cmd.CommandText = $"SELECT EXISTS (SELECT * from \"person\" WHERE login = '{textBox_cr_login.Text}')";
                        reader = cmd.ExecuteReader();


                        if (reader.Read() && reader.GetBoolean(0) == true)
                        {
                            MessageBox.Show("Данный логин уже занят!");
                            return;
                        }
                        reader.Close();

                        cmd.CommandText = $"INSERT INTO \"person\"(surname, name_, father, birthday, email, phone, login, password_) values ('{textBox_reg_fam.Text}', '{textBox_reg_name.Text}', '{textBox_reg_otch.Text}','{textBox_reg_bd.Text}', '{textBox_reg_pn.Text}', '{textBox_reg_email.Text}',  '{textBox_cr_login.Text}', '{textBox_cr_pass1.Text}')";
                        reader = cmd.ExecuteReader();


                        MessageBox.Show($"Регистрация прошла успешно!!");

                        tabControl1.SelectedIndex = 4;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!\n" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void button_find_Click(object sender, EventArgs e)
        {
            if (comboBox_country.Text == string.Empty || (radioButton_without_fly.Checked == false && radioButton_with_fly.Checked == false) ||
                  textBox_data.Text == string.Empty || textBox_night.Text == string.Empty || textBox_num_tur.Text == string.Empty ||
                  comboBox_food.Text == string.Empty || comboBox_stars.Text == string.Empty)
            {
                MessageBox.Show("Заполните все поля!");

            }
            // Добавленное условие для проверки правильности ввода даты
            else if (!Regex.IsMatch(textBox_data.Text, @"^(0?[1-9]|[12][0-9]|3[01])\.(0?[1-9]|1[0-2])\.(2024|2025)$"))
            {
                // Действие, если дата введена неправильно
                
                MessageBox.Show("Пожалуйста, введите дату в правильном формате (дд.мм.гггг)");
            }
            else if (!int.TryParse(textBox_night.Text, out int nights) || nights > 20)
            {
                MessageBox.Show("Количество ночей должно быть числом не больше 20");
            }
            else if (!int.TryParse(textBox_num_tur.Text, out int tourists) || tourists > 5)
            {
                MessageBox.Show("Количество туристов должно быть числом не больше 5");
            }
            else
            {
                tour.country = comboBox_country.Text;
                if(radioButton_without_fly.Checked == true)
                {
                    tour.fly = "Без перелета";
                }
                else
                {
                    tour.fly = "С перелетом";
                }
                tour.data = textBox_data.Text;
                tour.night = textBox_night.Text;
                tour.tourist = textBox_num_tur.Text;
                tour.food = comboBox_food.Text;
                tour.star = comboBox_stars.Text;

                pictureBox4.Visible = true;
                label21.Visible = true;
                label22.Visible = true;
                label23.Visible = true;
                linkLabel_offer.Visible = true;
                button_topay.Visible = true;
                label24.Visible = true;
                pictureBox5.Visible = true;
                button8.Visible = true;

                pictureBox6.Visible = false;

                textBox_data.ReadOnly = true; 
                textBox_night.ReadOnly = true;
                textBox_num_tur.ReadOnly = true;
                comboBox_country.Enabled = false;
                comboBox_food.Enabled = false;
                comboBox_stars.Enabled = false;
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            comboBox_country.Text = string.Empty;
            textBox_data.Text = string.Empty;
            radioButton_without_fly.Checked = false;
            radioButton_with_fly.Checked = false;
            textBox_data.Text = string.Empty;
            textBox_night.Text = string.Empty;
            textBox_num_tur.Text = string.Empty;
            comboBox_food.Text = string.Empty;
            comboBox_stars.Text = string.Empty;

            pictureBox6.Visible = true ;

            pictureBox4.Visible = false;
            label21.Visible = false;
            label22.Visible = false;
            label23.Visible = false;
            linkLabel_offer.Visible = false;
            button_topay.Visible = false;
            label24.Visible = false;
            pictureBox5.Visible = false;
            button8.Visible = false;

            textBox_data.ReadOnly = false;
            textBox_night.ReadOnly = false;
            textBox_num_tur.ReadOnly = false;
            comboBox_country.Enabled = true;
            comboBox_food.Enabled = true;
            comboBox_stars.Enabled = true;
        }

        private void linkLabel_offer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Тип тура:\r\n\r\nГарантированный отдых на пляже в определенной стране или курорте.\r\nЭкскурсионные туры по достопримечательностям с опытными гидами.\r\nАвантюрные туры для любителей активного отдыха (треккинг, альпинизм и т.д.).\r\nУсловия предоставления услуг:\r\n\r\nПолный пакет услуг: перелет, проживание, питание и трансферы включены в стоимость.\r\nВозможность выбора дополнительных услуг (страховка, экскурсии, аренда автомобиля).\r\nРазличные варианты проживания: от эконом до VIP-класса, от отелей до апартаментов.\r\nФинансовые условия:\r\n\r\nГибкая система оплаты: частичная предоплата или полная оплата до начала тура.\r\nСпециальные предложения для постоянных клиентов или групповых поездок.\r\nУсловия отмены и возврата:\r\n\r\nПравила отмены бронирования и возможности возврата средств.\r\nВозможность изменения даты или состава группы без штрафов при определенных условиях.\r\nБонусы и дополнительные услуги:\r\n\r\nПодарочные сертификаты, скидки на следующие поездки для клиентов.\r\nВозможность организации корпоративных мероприятий, свадеб и других специальных мероприятий на отдыхе.");
        }

        private void button_topay_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button_pay_Click(object sender, EventArgs e)
        {
            char card = textBox_num_card.Text[0];
            char cvc = textBox_CVC.Text[0];
            if(char.IsDigit(card) && char.IsDigit(cvc))
            {
                NpgsqlConnection conn = GetConnection();
                NpgsqlCommand cmd = conn.CreateCommand();
                NpgsqlDataReader reader;

                try
                {

                    conn.Open();

                    if (conn.State == ConnectionState.Open)
                    {

                        cmd.CommandText = $"INSERT INTO \"tour\"(country, fly, data_, nirht, tourist, food, star) values ('{tour.country}', '{tour.fly}', '{tour.data}','{tour.night}', '{tour.tourist}', '{tour.food}',  '{tour.star}')";
                        reader = cmd.ExecuteReader();


                        MessageBox.Show($"Ваш тур успешно забронирован!");

                        comboBox_country.Text = string.Empty;
                        textBox_data.Text = string.Empty;
                        radioButton_without_fly.Checked = false;
                        radioButton_with_fly.Checked = false;
                        textBox_data.Text = string.Empty;
                        textBox_night.Text = string.Empty;
                        textBox_num_tur.Text = string.Empty;
                        comboBox_food.Text = string.Empty;
                        comboBox_stars.Text = string.Empty;

                        tabControl1.SelectedIndex = 4;

                        pictureBox6.Visible = true;

                        pictureBox4.Visible = false;
                        label21.Visible = false;
                        label22.Visible = false;
                        label23.Visible = false;
                        linkLabel_offer.Visible = false;
                        button_topay.Visible = false;
                        label24.Visible = false;
                        pictureBox5.Visible = false;
                        button8.Visible = false;

                        textBox_data.ReadOnly = false;
                        textBox_night.ReadOnly = false;
                        textBox_num_tur.ReadOnly = false;
                        comboBox_country.Enabled = true;
                        comboBox_food.Enabled = true;
                        comboBox_stars.Enabled = true;


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!\n" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void button_book_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
            textBox_book_country.Text = tour.country;
            textBox_book_data.Text = tour.data;
            textBox_book_food.Text = tour.food;
            textBox_book_night.Text = tour.night;
            textBox_book_star.Text = tour.star;
            textBox_book_tur.Text = tour.tourist;
            textBox_fly.Text = tour.fly;

        }

        private void button_back2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }
    }
}

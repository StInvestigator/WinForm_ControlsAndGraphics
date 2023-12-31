using System;
using System.Drawing;
using System.Reflection;
using WinForm_ControlsAndGraphics.Domain;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinForm_ControlsAndGraphics
{
    public partial class Form1 : Form
    {
        List<Good> goods = new List<Good>();
        public Form1()
        {
            InitializeComponent();
            NUD_Amount.BackColor = Color.DarkGray;
        }

        private void NameChanged(object sender, EventArgs e)
        {
            if (tb_GoodName.Text.Length > 0)
            {
                NUD_Amount.Enabled = true;
                NUD_Amount.BackColor = Color.White;
            }
            else
            {
                NUD_Amount.Value = 0;
                NUD_Amount.Enabled = false;
                NUD_Amount.BackColor = Color.DarkGray;
            }
        }

        private void AmountChanged(object sender, EventArgs e)
        {
            if (NUD_Amount.Value > 0)
            {
                tb_PricePerOne.Enabled = true;
                tb_PriceAll.Enabled = true;
            }
            else
            {
                tb_PricePerOne.Enabled = false;
                tb_PriceAll.Enabled = false;
            }
        }

        private void AddGood_Click(object sender, EventArgs e)
        {

            try
            {
                if (tb_GoodName.Text.Length == 0)
                {
                    throw new Exception("Name is empty");
                }
                if (NUD_Amount.Value == 0)
                {
                    throw new Exception("Amount is 0");
                }
                if (tb_PricePerOne.Text.Length == 0)
                {
                    throw new Exception("Price is empty");
                }
                if (tb_Desc.Text.Length == 0)
                {
                    throw new Exception("Description is empty");
                }
                if (dateTimePicker.Value > DateTime.Now)
                {
                    throw new Exception("Date is in future");
                }
                var good = new Good(tb_GoodName.Text, tb_Desc.Text, (int)NUD_Amount.Value, decimal.Parse(tb_PricePerOne.Text), dateTimePicker.Value);
                goods.Add(good);
                listOfGoods.Items.Clear();
                listOfGoods.Items.AddRange(goods.ToArray());
                listOfGoods.Update();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PricePerOneChanged(object sender, EventArgs e)
        {
            tb_PriceAll.Text = (NUD_Amount.Value * decimal.Parse(tb_PricePerOne.Text)).ToString();
        }

        private void RomoveGoodFromList(object sender, EventArgs e)
        {
            Good good = (Good)listOfGoods.SelectedItem;
            goods.Remove(good);
            listOfGoods.Items.Clear();
            listOfGoods.Items.AddRange(goods.ToArray());
            listOfGoods.Update();
        }

        private void EditGoodItem(object sender, EventArgs e)
        {
            int index = listOfGoods.SelectedIndex;
            Good good = (Good)listOfGoods.SelectedItem;
            good.Name = tb_GoodName.Text;
            good.Description = tb_Desc.Text;
            good.Amount = (int)NUD_Amount.Value;
            good.PricePerOne = decimal.Parse(tb_PricePerOne.Text);
            good.TotalPrice = good.Amount * good.PricePerOne;
            good.Date = dateTimePicker.Value;
            goods[index] = good;
            listOfGoods.Items.Clear();
            listOfGoods.Items.AddRange(goods.ToArray());
            listOfGoods.Update();
        }

        private void SelectedValue(object sender, EventArgs e)
        {
            Good good = (Good)listOfGoods.SelectedItem;
            //MessageBox.Show(good.ToString());
            tb_GoodName.Text = good.Name;
            tb_Desc.Text = good.Description;
            NUD_Amount.Value = good.Amount;
            tb_PricePerOne.Text = good.PricePerOne.ToString();
            tb_PriceAll.Text = good.TotalPrice.ToString();
            dateTimePicker.Value = good.Date;
        }
        private void DrawCord(PaintEventArgs e,int goodsNumber)
        {
            e.Graphics.DrawLine(new Pen(Color.White, 2), new Point(100, 25), new Point(100, 500));
            e.Graphics.DrawLine(new Pen(Color.White, 2), new Point(75, 25), new Point(100, 25));
            e.Graphics.DrawLine(new Pen(Color.Gray, 2), new Point(100, 25), new Point(875, 25));
            e.Graphics.DrawString(goodsNumber.ToString(), new Font("Arial", 12), new SolidBrush(Color.White), new Point(60, 5));
            e.Graphics.DrawLine(new Pen(Color.White, 2), new Point(75, 138), new Point(100, 138));
            e.Graphics.DrawLine(new Pen(Color.Gray, 2), new Point(100, 138), new Point(875, 138));
            e.Graphics.DrawString(Convert.ToInt32(goodsNumber * 0.75).ToString(), new Font("Arial", 12), new SolidBrush(Color.White), new Point(60, 118));
            e.Graphics.DrawLine(new Pen(Color.White, 2), new Point(75, 250), new Point(100, 250));
            e.Graphics.DrawLine(new Pen(Color.Gray, 2), new Point(100, 250), new Point(875, 250));
            e.Graphics.DrawString(Convert.ToInt32(goodsNumber * 0.50).ToString(), new Font("Arial", 12), new SolidBrush(Color.White), new Point(60, 230));
            e.Graphics.DrawLine(new Pen(Color.White, 2), new Point(75, 362), new Point(100, 362));
            e.Graphics.DrawLine(new Pen(Color.Gray, 2), new Point(100, 362), new Point(875, 362));
            e.Graphics.DrawString(Convert.ToInt32(goodsNumber * 0.25).ToString(), new Font("Arial", 12), new SolidBrush(Color.White), new Point(60, 332));
            e.Graphics.DrawLine(new Pen(Color.White, 2), new Point(75, 475), new Point(875, 475));
            e.Graphics.DrawString(0.ToString(), new Font("Arial", 12), new SolidBrush(Color.White), new Point(60, 455));
        }
        private void DrawDiagram(PaintEventArgs e, Point point, Brush brush, int h, List<string> goods, int iter,List<int> valuesOfNumberOfGoods)
        {
            e.Graphics.FillRectangle(brush, point.X, point.Y, 750/ goods.Count, h);
            e.Graphics.DrawString(goods.ElementAt(iter), new Font("Arial", 16), brush, point.X, point.Y+h+5);
            e.Graphics.DrawString(valuesOfNumberOfGoods.ElementAt(iter).ToString(), new Font("Arial", 16), brush, point.X, point.Y-25);
        }
        public static List<Color> ColorStructToList()
        {
            return typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)
                                .Select(c => (Color)c.GetValue(null, null))
                                .ToList();
        }
        private int GetStep(double listSize, double value)
        {
            int percent = (int)((value / listSize) * 100);
            int number_from_percent = (int)((450 * percent) / 100);
            int res = 450 - number_from_percent;
            return res;
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            try
            {
                if (goods.Count == 0)
                {
                    throw new Exception("������ ������. ��������� �������� ��������!");
                }
                List<Color> ColorsList = ColorStructToList();
                Random random = new Random(DateTime.Now.Millisecond);
                List<int> valuesOfNumberOfGoods = new List<int>();
                var destinctGoods = goods.Select(x => new { x.Name }).Distinct().ToList();
                var goodNames = new List<string>();
                int totalCount=0;
                foreach (var item in destinctGoods)
                {
                    foreach (var item1 in goods.FindAll(x => x.Name == item.Name))
                    {
                        totalCount += item1.Amount;
                    }
                    valuesOfNumberOfGoods.Add(totalCount);
                    totalCount = 0;
                    goodNames.Add(item.Name);
                }
                foreach (var item in valuesOfNumberOfGoods)
                {
                    totalCount += item;
                }
                int x = 110;
                int y = 25;
                int h = 448;
                int size = goods.Count;
                int i = 0;
                DrawCord(e, totalCount);
                foreach (var item in valuesOfNumberOfGoods)
                {
                    int step = GetStep(totalCount, item);
                    DrawDiagram(e, new Point(x, y + step), new SolidBrush(ColorsList[random.Next(ColorsList.Count - 1)]), h-step, goodNames,i++, valuesOfNumberOfGoods);
                    x += 750 / goodNames.Count;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }
    }
}
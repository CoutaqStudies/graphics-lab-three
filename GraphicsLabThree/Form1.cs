using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Coutaq;

namespace GraphicsLabThree
{
    
    //Сделал Меликов Михаил ИВТ-1 Вариант 8
    public partial class Form1 : Form
    {
        private const float LineSpacing = 1.5f;
        private const float Offset = 10;
        private readonly string[] Lines = 
        {"First line", "Second line", "Third line", "Fourth line", "Fifth line", 
            "Sixth line", "Seventh line", "Eighth line", "Ninth line", "Tenth line",
            "Eleventh line", "Twelfth line", "Thirteenth line", "Fourteenth line"};

        private string[] strings;
        public Form1()
        {
            InitializeComponent();
            textBox1.MinimumSize = new Size(buttonSave.Height, buttonSave.Height);
            textBox1.Height = buttonSave.Height;
            strings = Lines;
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveToFile(textBox1.Text);
        }

        private void SaveToFile(string num)
        {
            strings = Lines;
            var sfd = new SaveFileDialog {Filter = "Text Files | *.txt", DefaultExt = "txt"};
            if (sfd.ShowDialog() != DialogResult.OK) return;
            var output = strings.Aggregate("", (current, line) => current + (line + "\n"));
            output = output.Trim();
            if (num.Length > 0 && int.Parse(num) < strings.Length)
            {
                string[] lines = output.Split('\n');
                output = string.Join(Environment.NewLine, lines.Take(int.Parse(num)));
            }
            FileExpert.SaveToAbsolutePath(sfd.FileName, output);
            MessageBox.Show($"Сохранено в файл {sfd.FileName}");
        }
        
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                SaveToFile(textBox1.Text);
            }
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void buttonDraw_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog {Filter = "Text Files | *.txt", DefaultExt = "txt"};
            if (ofd.ShowDialog() != DialogResult.OK) return;
            string input = FileExpert.ReadFromAbsolutePath(ofd.FileName);
            strings = input.Trim().Split('\n');
            var g = pictureBox1.CreateGraphics();
            DrawText(g);
          
        }

        private void DrawText(Graphics g)
        {
            pictureBox1.Refresh();
            var rectSize = new Size(pictureBox1.Size.Width - 20, pictureBox1.Size.Height - 20);
            for (int i = 0; i < strings.Length; i++)
            {
                if (i < 6)
                {
                    var font = new Font("Arial", 24, FontStyle.Underline);
                    var format = new StringFormat(StringFormat.GenericTypographic) 
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Near
                        
                    };
                    var location = new PointF(0, i * font.Size*LineSpacing+Offset);
                    g.DrawString(strings[i], font, Brushes.Blue, new RectangleF(location, rectSize), format);
                    font.Dispose();
                }else if (i < 13)
                {
                    var font = new Font("Times New Roman", 18, FontStyle.Strikeout);
                    var format = new StringFormat(StringFormat.GenericTypographic) 
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Far
                    };
                    var location = new PointF(0, (7-i) * font.Size*LineSpacing-Offset);
                    g.DrawString(strings[i], font, Brushes.Black, new RectangleF(location, rectSize), format);
                    font.Dispose();
                }
                else
                {
                    var font = new Font("Broadway", 1.5f, FontStyle.Regular, GraphicsUnit.Inch);
                    var format = new StringFormat(StringFormat.GenericTypographic)
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Near,
                        FormatFlags = StringFormatFlags.DirectionVertical
                    };
                    var location = new PointF(font.Size*LineSpacing-Offset, 10);
                    g.DrawString(strings[i], font, Brushes.Green, new RectangleF(location, rectSize), format);
                    font.Dispose();
                }
            }
        }
        private void buttonClear_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
    }
}

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

namespace ChatPaletMaker
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            textBox1.AcceptsReturn = true;
            textBox1.TextWrapping = TextWrapping.Wrap;
            textBox2.AcceptsReturn = true;
            textBox2.TextWrapping = TextWrapping.Wrap;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string result = Logic.Make(textBox1.Text.Split('\n'));
            textBox2.Text = result;
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void pasteButton_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "";
            textBox1.Paste();
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(textBox2.Text);
        }
    }
}

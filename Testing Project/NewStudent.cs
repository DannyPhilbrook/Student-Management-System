using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Testing_Project
{
    public partial class NewStudent : UserControl
    {
        public NewStudent()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mainMenu = this.FindForm() as MainMenu;
            if (mainMenu != null)
            {
                mainMenu.LoadPage(new MainPage());
            }
        }
    }
}

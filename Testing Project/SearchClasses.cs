using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testing_Project
{
    public partial class SearchClasses : UserControl
    {
        public SearchClasses()
        {
            InitializeComponent();
        }

        private void menubtn_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = this.FindForm() as MainMenu;
            if (mainMenu != null)
            {
                mainMenu.LoadPage(new MainPage());
            }
        }

        private void btnMkClass_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = this.FindForm() as MainMenu;
            if (mainMenu != null)
            {
                mainMenu.LoadPage(new NewClass());
            }
        }
    }
}

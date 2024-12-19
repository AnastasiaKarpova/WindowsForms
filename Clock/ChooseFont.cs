using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
	
	public partial class ChooseFontForm : Form
	{
		System.Drawing.Text.InstalledFontCollection fonts = new System.Drawing.Text.InstalledFontCollection();
		public ChooseFontForm()
		{
			InitializeComponent();
		}

		private void cbFonts_SelectedIndexChanged(object sender, EventArgs e)
		{
			
			foreach(FontFamily font in fonts.Families)
			{
				cbFonts.Items.Add(font);
				//fieldtext.Font = new Font((FontFamily)cbFonts.SelectedItem, 24);
			}
		}
	}
}

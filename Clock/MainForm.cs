using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
	public partial class MainForm : Form
	{
		ColorDialog BackColorDialog;
		ColorDialog ForeColorDialog;
		//PrivateFontCollection font;
		public MainForm()
		{
			InitializeComponent();
			//fontsProjects();
			//fonts();
			LoadFont();
			//labelTime.Font = new Font(font.Families[0], 31);
			labelTime.BackColor = Color.AliceBlue;

			BackColorDialog = new ColorDialog();
			ForeColorDialog = new ColorDialog();
			this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width, 50);
			this.SetStyle
				(
				ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | 
				ControlStyles.ResizeRedraw | ControlStyles.ContainerControl | 
				ControlStyles.OptimizedDoubleBuffer | 
				ControlStyles.SupportsTransparentBackColor | 
				ControlStyles.DoubleBuffer, true
				);
			SetVisibility(false);
		}

		void SetVisibility(bool visible)
		{
			cbShowDate.Visible = visible;
			cbShowWeekDay.Visible = visible;
			//cmBackColor.Visible = visible;
			//cmForeColor.Visible = visible;
			btnHideControls.Visible = visible;
			this.TransparencyKey = visible ? Color.Empty : this.BackColor;
			this.FormBorderStyle = visible ? FormBorderStyle.FixedToolWindow : FormBorderStyle.None;
			this.ShowInTaskbar = visible;
			//fontsProjects();
			//fonts();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			labelTime.Text = DateTime.Now.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
			if(cbShowDate.Checked)
			{
				labelTime.Text += "\n";
				labelTime.Text += DateTime.Now.ToString("yyyy.MM.dd");
			}
			if(cbShowWeekDay.Checked)
			{
				labelTime.Text += "\n";
				labelTime.Text += DateTime.Now.DayOfWeek;
			}
			notifyIcon.Text = "";
			notifyIcon.Text = labelTime.Text;

		}

		private void btnHideControls_Click(object sender, EventArgs e)
		{
			//cbShowDate.Visible = false;
			//btnHideControls.Visible = false;
			//this.TransparencyKey = this.BackColor;
			//this.FormBorderStyle = FormBorderStyle.None;
			//labelTime.BackColor = Color.AliceBlue;
			//this.ShowInTaskbar = false;
			//Color transparencyKey = this.TransparencyKey;

			//SetVisibility(false);	
			SetVisibility(cmShowControls.Checked = false);
		}

		private void labelTime_DoubleClick(object sender, EventArgs e)
		{
			//MessageBox.Show
			//	(
			//	this, 
			//	"Вы два раза щелкнули мышью по времени, и теперь Вы управляете временем",
			//	"Info", 
			//	MessageBoxButtons.OK,
			//	MessageBoxIcon.Information
			//	);

			//cbShowDate.Visible = true;
			//btnHideControls.Visible = true;
			//this.TransparencyKey = Color.Empty;
			//this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			//labelTime.BackColor = Color.AliceBlue;
			//this.ShowInTaskbar = true;

			//SetVisibility(true);
			SetVisibility(cmShowControls.Checked = true);
		}

		private void cmExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void cmTopmost_CheckedChanged(object sender, EventArgs e)
		{
			this.TopMost = cmTopmost.Checked;
		}

		private void cmShowDate_CheckedChanged(object sender, EventArgs e)
		{
			cbShowDate.Checked = cmShowDate.Checked;
		}
		private void cbShowDate_CheckedChanged(object sender, EventArgs e)
		{
			cmShowDate.Checked = cbShowDate.Checked;
		}

		private void cmShowWeekDay_CheckedChanged(object sender, EventArgs e)
		{
			cbShowWeekDay.Checked = cmShowWeekDay.Checked;
		}
		private void cbShowWeekDay_CheckedChanged(object sender, EventArgs e)
		{
			cmShowWeekDay.Checked = cbShowWeekDay.Checked;
		}

		private void notifyIcon_DoubleClick(object sender, EventArgs e)
		{
			if(!this.TopMost)
			{
				this.TopMost = true;
				this.TopMost = false;
			}
		}

		//private void cmBackColor_Click(object sender, EventArgs e)
		//{
		//	DialogResult result = BackColorDialog.ShowDialog(this);
		//	if (result == DialogResult.OK)
		//	{
		//		labelTime.BackColor = BackColorDialog.Color;
		//	}
		//}

		//private void cmForeColor_Click(object sender, EventArgs e)
		//{
		//	DialogResult result = ForeColorDialog.ShowDialog(this);
		//	if (result == DialogResult.OK)
		//	{
		//		labelTime.ForeColor = ForeColorDialog.Color;
		//	}
		//}
		private void SetColor(object sender, EventArgs e)
		{
			ColorDialog dialog = new ColorDialog();
			switch ((sender as ToolStripMenuItem).Text)
			{
				case "Background color": dialog.Color = labelTime.BackColor; break;
				case "Foreground color": dialog.Color = labelTime.ForeColor; break;
			}
			
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				switch((sender as ToolStripMenuItem).Text)
				{
					case "Background color":labelTime.BackColor = dialog.Color; break; 
					case "Foreground color":labelTime.ForeColor = dialog.Color; break; 
				}
			}
			//Оператор 'as' значение слева приводит к типу справа
		}

		//private void fontsProjects()
		//{
		//	this.font = new PrivateFontCollection();
		//	this.font.AddFontFile("Font/5by7.ttf");
		//}
		//private void fonts()
		//{
		//	labelTime.Font = new Font(font.Families[0], 31);
		//}

		private void LoadFont()
		{ }

		private void cmShowControls_CheckedChanged(object sender, EventArgs e)
		{
			SetVisibility(cmShowControls.Checked);
		}
	}
}

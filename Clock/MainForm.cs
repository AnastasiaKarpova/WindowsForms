﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using AxWMPLib;
using System.Windows.Forms.VisualStyles;

namespace Clock
{
	
	public partial class MainForm : Form
	{
		ChooseFontForm fontDialog = null;
		Alarms alarms = null;
		Alarm nextAlarm = null;

		ColorDialog BackColorDialog;
		ColorDialog ForeColorDialog;
		//PrivateFontCollection font;
		public MainForm()
		{
			InitializeComponent();
			//fontsProjects();
			//fonts();
			MainFormLoad();
			//labelTime.Font = new Font(font.Families[0], 31);
			labelTime.BackColor = Color.LightBlue;
			labelTime.ForeColor = Color.Blue;

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
			cmShowConsole.Checked=true;
			LoadSettings();
			
			//fontDialog = new ChooseFontForm();
			alarms = new Alarms();
			LoadAlarms();
			Console.WriteLine(DateTime.MinValue);
			//CompareAlarmsDEBUG();
			axWindowsMediaPlayer.Visible = false;
			//play_Alarm();
		}
		private void MainFormLoad()
		{ }
		void CompareAlarmsDEBUG()
		{
			Alarm alarm1 = new Alarm();
			Alarm alarm2 = new Alarm();

			alarm1.Date = new DateTime(2025, 01, 16);
			alarm1.Time = new TimeSpan(9, 5, 0);

			alarm2.Date = new DateTime(1, 01, 1);
			alarm2.Time = new TimeSpan(9, 11, 00);

			//Console.WriteLine(alarm1 < alarm2);
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
		void SaveSettings()
		{
			StreamWriter sw = new StreamWriter("Settings.ini");
			sw.WriteLine($"{cmTopmost.Checked}");
			sw.WriteLine($"{cmShowControls.Checked}");
			sw.WriteLine($"{cmShowDate.Checked}");
			sw.WriteLine($"{cmShowWeekDay.Checked}");
			sw.WriteLine($"{cmShowConsole.Checked}");
			sw.WriteLine($"{labelTime.BackColor.ToArgb()}");
			sw.WriteLine($"{labelTime.ForeColor.ToArgb()}");
			sw.WriteLine($"{fontDialog.Filename}");
			sw.WriteLine($"{labelTime.Font.Size}");
			sw.Close();
			//Process.Start("notepad", "Settings.ini");
		}
		void LoadSettings()
		{
			string execution_path = Path.GetDirectoryName(Application.ExecutablePath);
			Directory.SetCurrentDirectory($"{execution_path}\\..\\..\\Fonts");
			StreamReader sr = new StreamReader("Settings.ini");
			cmTopmost.Checked = bool.Parse(sr.ReadLine());
			cmShowControls.Checked = bool.Parse(sr.ReadLine());
			cmShowDate.Checked = bool.Parse(sr.ReadLine());
			cmShowWeekDay.Checked = bool.Parse(sr.ReadLine());
			cmShowConsole.Checked = bool.Parse(sr.ReadLine());
			labelTime.BackColor = Color.FromArgb(Convert.ToInt32(sr.ReadLine()));
			labelTime.ForeColor = Color.FromArgb(Convert.ToInt32(sr.ReadLine()));
			string font_name = sr.ReadLine();
			int font_size = Convert.ToInt32(sr.ReadLine());
			sr.Close();
			fontDialog = new ChooseFontForm(this, font_name, font_size);
			labelTime.Font = fontDialog.Font;
			//RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
			//rk.GetValue("ClockPV_319");
		}
		void SaveAlarms()
		{
			string execution_path = Path.GetDirectoryName(Application.ExecutablePath);
			string filename = $"{execution_path}\\..\\..\\Fonts\\Alarms.ini";
			StreamWriter sw = new StreamWriter(filename);
			for(int i = 0; i < alarms.LB_Alarms.Items.Count; i++)
			{
				sw.WriteLine((alarms.LB_Alarms.Items[i] as Alarm).ToFileString());
			}
			sw.Close();
			Process.Start("notepad", filename);
		}
		void LoadAlarms()
		{
			string execution_path = Path.GetExtension(Application.ExecutablePath);
			string filename = $"{execution_path}\\..\\..\\Fonts\\Alarms.ini";
			try
			{
				StreamReader sr = new StreamReader(filename);
				while (!sr.EndOfStream)
				{
					string s_alarm = sr.ReadLine();
					string[] s_alarm_parts = s_alarm.Split(',');
					for (int i = 0; i < s_alarm_parts.Length; i++)
					{
						Console.Write(s_alarm_parts[i] + '\t');
					}
					Console.WriteLine();
					Alarm alarm = new Alarm
						(
							s_alarm_parts[0] == "" ? new DateTime() : new DateTime(Convert.ToInt64(s_alarm_parts[0])),
							new TimeSpan(Convert.ToInt64(s_alarm_parts[1])),
							new Week(Convert.ToByte(s_alarm_parts[2])),
							s_alarm_parts[3],
							s_alarm_parts[4]
						);
					alarms.LB_Alarms.Items.Add(alarm);
				}
				sr.Close();
			}
			catch (Exception)
			{
				Console.WriteLine("Alarm not found");
			}
		}
		Alarm FindNextAlarm()
		{
			Alarm[] actualAlarms = 
				alarms.LB_Alarms.Items.Cast<Alarm>().Where(a => a.Time > DateTime.Now.TimeOfDay).ToArray();
			//Alarm nextAlarm = new Alarm(actualAlarms.Min());
			
			//if (nextAlarm.Time < DateTime.Now.TimeOfDay)
			//{
			//	foreach (Alarm a in alarms.LB_Alarms.Items)
			//	{
			//		if()
			//	} 
			//}
			//return nextAlarm;
			return actualAlarms.Min();
		}
		bool CompareDates(DateTime date1, DateTime date2)
		{
			return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day;
		}
		void PlayAlarm()
		{
			axWindowsMediaPlayer.URL = nextAlarm.Filename;
			axWindowsMediaPlayer.settings.volume = 100;
			axWindowsMediaPlayer.Ctlcontrols.play();
			axWindowsMediaPlayer.Visible = true;
		}
		void SetPlayerInvisible(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
		{
			if (axWindowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsMediaEnded || 
				axWindowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsStopped)
				axWindowsMediaPlayer.Visible = false;
		}
		void SetPlayerInvisible(object sender, AxWMPLib._WMPOCXEvents_EndOfStreamEvent e)
		{
			axWindowsMediaPlayer.Visible = false;
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
			//notifyIcon.Text = "";
			notifyIcon.Text = labelTime.Text;

			if(
				nextAlarm != null &&
				(nextAlarm.Date == DateTime.MinValue ? 
				nextAlarm.Weekdays.Contains(DateTime.Now.DayOfWeek) : 
				(CompareDates(nextAlarm.Date, DateTime.Now) /*|| nextAlarm.Date == DateTime.MinValue.Date*/)) &&
				//nextAlarm.Weekdays.Contains(DateTime.Now.DayOfWeek) &&
				nextAlarm.Time.Hours == DateTime.Now.Hour && 
				nextAlarm.Time.Minutes == DateTime.Now.Minute && 
				nextAlarm.Time.Seconds == DateTime.Now.Second)
			{
				System.Threading.Thread.Sleep(1000);
				PlayAlarm();
				if (nextAlarm.Message != "") MessageBox.Show(this, nextAlarm.Message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				//MessageBox.Show(this, nextAlarm.ToString(), "Alarm", MessageBoxButtons.OK, MessageBoxIcon.Information);
				//nextAlarm = FindNextAlarm();
				//System.Threading.Thread.Sleep(1000);
				nextAlarm = null;
			}
			//play_Alarm();
			if (/*DateTime.Now.Second % 10 == 0 &&*/ alarms.LB_Alarms.Items.Count > 0) nextAlarm = FindNextAlarm();
				//nextAlarm = alarms.LB_Alarms.Items.Cast<Alarm>().ToArray().Min();
			if((object)nextAlarm != null) Console.WriteLine(nextAlarm);
			
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
				
		private void cmShowControls_CheckedChanged(object sender, EventArgs e)
		{
			SetVisibility(cmShowControls.Checked);
		}

		private void cmChooseFont_Click(object sender, EventArgs e)
		{
			//ChooseFontForm chooseFont = new ChooseFontForm();
			//chooseFont.ShowDialog();
			if (fontDialog.ShowDialog() == DialogResult.OK)
					labelTime.Font = fontDialog.Font;
		}

		private void cmShowConsole_CheckedChanged(object sender, EventArgs e)
		{
			if ((sender as ToolStripMenuItem).Checked)
				AllocConsole();
			else
				FreeConsole();
		}
		[DllImport("kernel32.dll")]
		public static extern bool AllocConsole();

		[DllImport("kernel32.dll")]
		public static extern bool FreeConsole();

		private void cmLoadOnWinStartup_CheckedChanged(object sender, EventArgs e)
		{
			//RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
			//if (cmLoadOnWinStartup.Checked) 
			//{
			//	rk.SetValue("Clock", Application.ExecutablePath); 
			//}
			//else
			//{
			//	rk.DeleteValue("Clock", false);
			//}

			string key_name = "ClockPV_319";
			RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true); //true откроет ветку на запись
			if (cmLoadOnWinStartup.Checked)
			{
				rk.SetValue(key_name, Application.ExecutablePath);
			}
			else rk.DeleteValue(key_name, false);

			rk.Dispose();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveSettings();
			SaveAlarms();
		}

		private void cmAlarms_Click(object sender, EventArgs e)
		{
			alarms.StartPosition = FormStartPosition.Manual;
			alarms.Location = new Point(this.Location.X - alarms.Width, this.Location.Y*2);
			alarms.ShowDialog();
		}

		//void play_Alarm ()
		//{
		//	axWindowsMediaPlayer.URL = @"C:\\Users\\OMEN\\source\\repos\\WindowsForms\\Clock\\Sounds\\4113_new_rington.ru_.mp3";
		//	axWindowsMediaPlayer.settings.volume = 100;
		//	axWindowsMediaPlayer.Ctlcontrols.play();
		//	axWindowsMediaPlayer.Visible = true;
		//	//Console.WriteLine(alarms); 
			
		//}
		//public void playerInvisible(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
		//{
		//	if(axWindowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
		//		axWindowsMediaPlayer.Visible = false;
		//}

		
		//private void axWindowsMediaPlayer_Enter(object sender, EventArgs e)
		//{

		//}
	}
}

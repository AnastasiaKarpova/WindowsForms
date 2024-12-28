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
	public partial class Alarms : Form
	{
		AddAlarmsForm addAlarm = null;
		OpenFileDialog openFile = null;
		public Alarms()
		{
			InitializeComponent();
			addAlarm = new AddAlarmsForm();
			openFile = new OpenFileDialog();
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			addAlarm.StartPosition = FormStartPosition.Manual;
			addAlarm.Location = new Point(this.Location.X+25, this.Location.Y+25);
			DialogResult result = addAlarm.ShowDialog();
			if(result == DialogResult.OK)
			{
				lbAlarms.Items.Add(addAlarm.Alarm);
			}
		}
	}
}

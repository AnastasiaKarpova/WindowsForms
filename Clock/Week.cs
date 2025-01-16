using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clock
{
	public class Week
	{
		public static readonly string[] Weekdays = new string[]{"Пн","Вт","Ср","Чт","Пт","Сб","Вс"};
		byte week;
		public Week()
		{
			week = 127/*191*/;
			//week += 192;
		}
		public Week(bool[] days)
		{
			CompressWeekDays(days);
		}
		//public void SetDay(int day)
		//{
		//	if (day > 6) return;
		//	week |= (byte)(1 << day);
		//}
		public void CompressWeekDays(bool[] days)
		{
			for(byte i = 0; i < days.Length; i++)
			{
				//byte day = 1;
				if (days[i]) week |= (byte)(1 << i);
				//if (days[i])SetDay(i);
			}
		}
		public bool[] ExtractWeekDays()
		{
			bool[] weekDays = new bool[7];
			for(byte i = 0; i < 7; i ++)
			{
				weekDays[i] = (week & (byte)(1 << i)) != 0;
			}
			return weekDays;
		}
		public bool Contains(DayOfWeek day)
		{
			int i_day = (int)day;
			i_day -= 1;
			if (i_day == -1) i_day = 6;
			return (week & (1 << i_day)) != 0;
		}
		public override string ToString()
		{
			string weekdays = "";
			for(byte i = 0; i < Weekdays.Length; i++)
			{
				byte day = 1;
				if((byte)((day<<i) & week) != (byte)0)
					weekdays += $"{Weekdays[i]},";
			}
			
			return weekdays;
		}
	}
}

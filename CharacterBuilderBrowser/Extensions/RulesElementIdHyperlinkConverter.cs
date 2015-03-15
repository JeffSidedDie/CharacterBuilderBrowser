﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace CharacterBuilderBrowser
{
	public class RulesElementIdHyperlinkConverter:IValueConverter
	{
		public object Convert(object value,Type targetType,object parameter,CultureInfo culture)
		{
			if(value==null)
			{
				return null;
			}

			var valueStr=value.ToString().Trim();
			var span=new Span();

			var runs=Regex.Split(valueStr,"(ID_[A-Z0-9'()_-]+)");
			for(int i=0;i<runs.Length;i++)
			{
				var runStr=runs[i];
				if(i%2==1)
				{
					var element=((Browser)Application.Current).Repository.GetRulesElement(runStr);
					if(element!=null)
					{
						var link=new Hyperlink(new Run(element.Name));
						link.Tag=element;
						link.Click+=OpenRulesElementDetails;
						span.Inlines.Add(link);
						continue;
					}
				}
				span.Inlines.Add(new Run(runStr));
			}
			
			return span;
		}

		public object ConvertBack(object value,Type targetType,object parameter,CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private static void OpenRulesElementDetails(object sender,RoutedEventArgs e)
		{
			var link=sender as Hyperlink;
			if(link==null) return;
			RulesElementDetailsWindow.Show((RulesElement)link.Tag);
		}
	}
}

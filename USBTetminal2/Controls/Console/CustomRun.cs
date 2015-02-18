using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace USBTetminal2.Controls
{
    public class CustomRun: Run

    {

        public CustomRun()
        {
            
        }

        public Run getCopy(CustomType type)
        {
            Run run = new Run();
            run.Text = Text;
            run.Text += Environment.NewLine;
            run.FontWeight = FontWeights.Bold;
            run.FontSize = 12;
            switch (type)
            {
                case CustomType.Red:
                    run.Foreground = Brushes.Red;
                    break;
                case CustomType.Blue:
                    run.Foreground = Brushes.Yellow;
                    break;
                default:
                    break;
            }
            return run;
        }

        public enum CustomType
        {
            Red,
            Blue
        }


        public static CustomRun Log
        {
            get
            {
                CustomRun run = new CustomRun();
                run.Text += Environment.NewLine;
                run.FontWeight = FontWeights.Bold;
                run.FontSize = 8;
                run.Foreground = Brushes.Yellow;
                return run;
            }
        }


        public static CustomRun Error
        {
            get
            {
                CustomRun run = new CustomRun();
                run.Text += Environment.NewLine;
                run.FontWeight = FontWeights.Bold;
                run.FontSize = 12;
                run.Foreground = Brushes.Red;
                return run;
            }
        }

        public static CustomRun Info
        {
            get
            {
                CustomRun run = new CustomRun();
                run.Text += Environment.NewLine;
                run.FontWeight = FontWeights.Bold;
                run.FontSize = 12;
                run.Foreground = Brushes.LightGreen;
                return run;
            }
        }

        public class CommandRun : CustomRun { }
        public static CustomRun Cmd
        {
            get
            {
                CustomRun run = new CommandRun();
                run.FontWeight = FontWeights.Bold;
                run.FontSize = 12;
                run.Foreground = Brushes.White;
                return run;
            }
        }


    //    public void ManualPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
     //   {
           // base.OnPreviewKeyDown(e);
           // Text += e.Key;
    //    }

        public static CustomRun Debug
        {
            get
            {
                CustomRun run = new CustomRun();
                run.Text += Environment.NewLine;
                run.FontWeight = FontWeights.Bold;
                run.FontSize = 12;
                run.Foreground = Brushes.LightBlue;
                return run;
            }
        }
    }
}

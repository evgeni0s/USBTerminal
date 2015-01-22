using Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Logging;
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
using System.Windows.Threading;

namespace USBTetminal2.Controls
{
    /// <summary>
    /// Small hack to bring RTB into life
    /// http://stackoverflow.com/questions/1053533/how-can-i-prevent-input-controls-from-stealing-the-space-character-from-the-text/1509554#1509554?newreg=4c844a22811d4f01a0a1119e72fe8e69
    /// </summary>
    /// 
    //TO DO: delete ILogger 
    public partial class CustomRichTextBox : RichTextBox, ILoggerFacade, ILogger
    {
        private List<Key> keysRequireFix = new List<Key>() { Key.Space, Key.Enter, Key.Back, Key.Delete };

        public CustomRichTextBox()
        {
            InitializeComponent();
        }


        #region processing key inputs
        private void screwManualTextInput()
        {
            // this is where we handle the space and other keys wpf f*s up.
            System.Windows.Input.InputManager.Current.PreNotifyInput +=
                new NotifyInputEventHandler(PreNotifyInput);
            // This is where we handle all the rest of the keys
            TextCompositionManager.AddPreviewTextInputStartHandler(
                this,
                PreviewTextInputHandler);                                          ///OnPreviewKeyDown
        }

        private void PreNotifyInput(object sender, NotifyInputEventArgs e)
        {
            // I'm only interested in key down events
            if (e.StagingItem.Input.RoutedEvent != Keyboard.KeyDownEvent)
                return;
            var args = e.StagingItem.Input as KeyEventArgs;
            // I only care about the space key being pressed
            // you might have to check for other characters
            if (args == null || !keysRequireFix.Any(k => k == args.Key))
                return;

            if (!IsFocused)
                return;
            // stop event processing here
            args.Handled = true;
            // this is my internal method for handling a keystroke
            if (args.Key == Key.Space)
                HanleKeystroke(" ");
            else
                HandleKeyAction(args.Key);
        }

        private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            HanleKeystroke(e.Text);
        }

        //Sends text from KEYBOARD to inputField. DO NOT USE FOR OTHER PURPOSES!!!
        private void HanleKeystroke(string p)
        {
            if (getPositionType(inputField) == CuretPositionType.Outside)
                CaretPosition = inputField.ContentEnd;


            // CaretPosition.InsertTextInRun(p);//Inserting text. Works but has issues with <- and -> . Acts Whiered
            //  CaretPosition.GetInsertionPosition(LogicalDirection.Backward).InsertTextInRun(p);
            // ScrollToEnd();

            //THIS CODE IS STABLE!!!!
            //http://www.java2s.com/Tutorial/CSharp/0470__Windows-Presentation-Foundation/ProgrammaticallyInsertTextintoaRichTextBox.htm
            TextPointer tp = CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward);
            CaretPosition.InsertTextInRun(p);
            CaretPosition = tp;
            //STABLE REGION ENDS!

            ScrollToEnd();
        }


        //Sends actions to inputField
        private void HandleKeyAction(Key action)
        {
            switch (action)
            {
                case Key.Enter:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                        CaretPosition.InsertTextInRun(Environment.NewLine);
                    // CaretPosition.InsertLineBreak();
                    else
                        addToReadonly();
                    break;
                case Key.Back:
                    if (getPositionType(inputField) != CuretPositionType.Outside)
                    {
                        TextPointer tp = CaretPosition.GetPositionAtOffset(-1, LogicalDirection.Backward);
                        tp.DeleteTextInRun(1);
                        // CaretPosition.DeleteTextInRun(1);
                        // CaretPosition = tp;
                        //CaretPosition.GetInsertionPosition(LogicalDirection.Backward).DeleteTextInRun(1);
                    }
                    break;
                case Key.Delete:
                    if (getPositionType(inputField) != CuretPositionType.Outside)
                    {
                        CaretPosition.DeleteTextInRun(1);
                    }
                    break;
            }
        }
        #endregion

        //protected override void OnPreviewKeyDown(KeyEventArgs e)
        //{
        //    base.OnPreviewKeyDown(e);
        //   // if (getPositionType(inputField != ))
        //   // inputField.ManualPreviewKeyDown(e);
        //    //  e.Handled = true;
        //    // inputField
        //}

        #region action methods

        private void addToReadonly()
        {
            Run run = inputField.getCopy(getNextStyle());
            readOnlyItems.Inlines.Add(run);
            inputField.Text = "";
            ScrollToEnd();
        }

        private void pasteFromClipboard()
        {
            if (getPositionType(inputField) != CuretPositionType.Outside)
                CaretPosition.InsertTextInRun(Clipboard.GetText());//Inserting text
            else
                inputField.ContentEnd.InsertTextInRun(Clipboard.GetText());
        }

        #endregion

        #region Tools
        CustomRun.CustomType previousStyle = CustomRun.CustomType.Red;
        private CustomRun.CustomType getNextStyle()
        {
            if (previousStyle == CustomRun.CustomType.Blue)
            {
                previousStyle = CustomRun.CustomType.Red;
            }
            else
            {
                previousStyle = CustomRun.CustomType.Blue;
            }
            return previousStyle;
        }


        private enum CuretPositionType
        {
            Start,
            End,
            Middle,
            Outside
        }

        private CuretPositionType getPositionType(Run relativeTarget)
        {
            //     1  |   cursor   |  -1
            //     cursor == start  -> start == 0  end == 1
            //     cursor == end    -> start == 1  end == 0
            int start = CaretPosition.CompareTo(relativeTarget.ContentStart);
            int end = CaretPosition.CompareTo(relativeTarget.ContentEnd);
            if (start == 1 && end == -1)
                return CuretPositionType.Middle;
            if (start == 0)
                return CuretPositionType.Start;
            if (end == 0)
                return CuretPositionType.End;
            return CuretPositionType.Outside;
        }
        #endregion


        private void onLoaded(object sender, RoutedEventArgs e)
        {

            screwManualTextInput();
        }

        private void onPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Paste(); - Simple, but breaks Isreadonly logic
        }




        public void Log(string message)
        {
        }

        public void Error(string message)
        {
        }

        public void Log(string message, Category category, Priority priority)
        {
            readOnlyItems.Dispatcher.BeginInvoke(new Action(() =>
               {

                   CustomRun run = CustomRun.Log;


                   switch (category)
                   {
                       case Category.Exception:
                           run = CustomRun.Error;
                           break;
                       case Category.Info:
                           run = CustomRun.Info;
                           break;
                   }
                   run.Text = message + Environment.NewLine; ;
                   readOnlyItems.Inlines.Add(run);
               }));

        }
    }
}

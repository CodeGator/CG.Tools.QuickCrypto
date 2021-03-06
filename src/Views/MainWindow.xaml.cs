using CG.Reflection;
using CG.Tools.QuickCrypto.ViewModels;
using Syncfusion.SfSkinManager;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CG.Tools.QuickCrypto.Views
{
    /// <summary>
    /// This class represents the code-behind for the main window.
    /// </summary>
    public partial class MainWindow
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        /// <summary>
        /// This field contains the current visual style.
        /// </summary>
        private string currentVisualStyle;

        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the current visual style.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string CurrentVisualStyle
        {
            get { return currentVisualStyle; }
            set
            {
                currentVisualStyle = value;
                OnVisualStyleChanged();
            }
        }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// Thsi constructor creates a new instance of the <see cref="MainWindow"/>
        /// class.
        /// </summary>
        public MainWindow()
        {
            // Keep the designer happy.
            InitializeComponent();
        }

        #endregion

        // *******************************************************************
        // Private methods.
        // *******************************************************************

        #region Private methods

        /// <summary>
        /// This method handles a visual style change.
        /// </summary>
        private void OnVisualStyleChanged()
        {
            // Parse the current style.
            VisualStyles visualStyle = VisualStyles.Default;
            Enum.TryParse(CurrentVisualStyle, out visualStyle);

            // Is it not the default?
            if (visualStyle != VisualStyles.Default)
            {
                SfSkinManager.ApplyStylesOnApplication = true;
                SfSkinManager.SetVisualStyle(this, visualStyle);
                SfSkinManager.ApplyStylesOnApplication = false;
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when the <see cref="dataPrivayPlainText"/>
        /// control has a text change.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The arguments for the event.</param>
        private void dataPrivayPlainText_TextChanged(
            object sender,
            TextChangedEventArgs e
            )
        {
            if (null != dataPrivayEncrypt &&
                null != dataPrivayPlainText)
            {
                dataPrivayEncrypt.IsEnabled = dataPrivayPlainText.Text.Length > 0;
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when the <see cref="dataPrivayEncryptedText"/>
        /// control has a text change.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The arguments for the event.</param>
        private void dataPrivayEncryptedText_TextChanged(
            object sender,
            TextChangedEventArgs e
            )
        {
            if (null != dataPrivayDecrypt &&
                null != dataPrivayEncryptedText)
            {
                dataPrivayDecrypt.IsEnabled = dataPrivayEncryptedText.Text.Length > 0;
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when the <see cref="dataProtectionPlainText"/>
        /// control has a text change.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The arguments for the event.</param>
        private void dataProtectionPlainText_TextChanged(
            object sender,
            TextChangedEventArgs e
            )
        {
            if (null != dataProtectionEncrypt &&
                null != dataProtectionPlainText)
            {
                dataProtectionEncrypt.IsEnabled = dataProtectionPlainText.Text.Length > 0;
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when the <see cref="dataProtectionEncryptedText"/>
        /// control has a text change.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The arguments for the event.</param>
        private void dataProtectionEncryptedText_TextChanged(
            object sender,
            TextChangedEventArgs e
            )
        {
            if (null != dataProtectionDecrypt &&
                null != dataProtectionEncryptedText)
            {
                dataProtectionDecrypt.IsEnabled = dataProtectionEncryptedText.Text.Length > 0;
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when the <see cref="rijndaelPlainText"/>
        /// control has a text change.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The arguments for the event.</param>
        private void rijndaelPlainText_TextChanged(
            object sender,
            TextChangedEventArgs e
            )
        {
            if (null != rijndaelEncrypt &&
                null != rijndaelPlainText &&
                null != rijndaelPassword &&
                null != rijndaelSalt)
            {
                rijndaelEncrypt.IsEnabled = rijndaelPlainText.Text.Length > 0 &&
                rijndaelPassword.Password.Length > 0 &&
                rijndaelSalt.Password.Length > 8;
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when the <see cref="rijndaelEncryptedText"/>
        /// control has a text change.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The arguments for the event.</param>
        private void rijndaelEncryptedText_TextChanged(
            object sender,
            TextChangedEventArgs e
            )
        {
            if (null != rijndaelDecrypt &&
                null != rijndaelEncryptedText &&
                null != rijndaelPassword &&
                null != rijndaelSalt)
            {
                rijndaelDecrypt.IsEnabled = rijndaelEncryptedText.Text.Length > 0 &&
                rijndaelPassword.Password.Length > 0 &&
                rijndaelSalt.Password.Length > 8;
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when the <see cref="rijndaelSalt"/>
        /// control has a text change.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The arguments for the event.</param>
        private void rijndaelSalt_PasswordChanged(
            object sender,
            RoutedEventArgs e
            )
        {
            var viewModel = ((DataContext as MainWindowViewModel).Rijndael);

            if (null != rijndaelEncrypt &&
                null != rijndaelPlainText &&
                null != rijndaelPassword &&
                null != rijndaelSalt)
            {
                rijndaelEncrypt.IsEnabled = rijndaelPlainText.Text.Length > 0 &&
                rijndaelPassword.Password.Length > 0 &&
                rijndaelSalt.Password.Length > 8;

                rijndaelDecrypt.IsEnabled = rijndaelEncryptedText.Text.Length > 0 &&
                    rijndaelPassword.Password.Length > 0 &&
                    rijndaelSalt.Password.Length > 8;
            }

            viewModel.Salt = rijndaelSalt.Password;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when the <see cref="rijndaelPassword"/>
        /// control has a text change.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The arguments for the event.</param>
        private void rijndaelPassword_PasswordChanged(
            object sender,
            RoutedEventArgs e
            )
        {
            var viewModel = ((DataContext as MainWindowViewModel).Rijndael);

            if (null != rijndaelEncrypt &&
                null != rijndaelPlainText &&
                null != rijndaelPassword &&
                null != rijndaelSalt)
            {
                rijndaelEncrypt.IsEnabled = rijndaelPlainText.Text.Length > 0 &&
                rijndaelPassword.Password.Length > 0 &&
                rijndaelSalt.Password.Length > 8;

                rijndaelDecrypt.IsEnabled = rijndaelEncryptedText.Text.Length > 0 &&
                    rijndaelPassword.Password.Length > 0 &&
                    rijndaelSalt.Password.Length > 8;
            }

            viewModel.Password = rijndaelPassword.Password;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when the left mouse button it pressed while the mouse
        /// is over the tab control.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The arguments for the event.</param>
        private void ChromelessWindow_MouseDown(
            object sender,
            MouseButtonEventArgs e
            )
        {
            // So, if we don't eat this event here, something inside the syncFusion control
            //   stack throws an exception and kills the application.
            e.Handled = true;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when the file|exit menu is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The arguments for the event.</param>
        private void FileExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when the help|about menu is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The arguments for the event.</param>
        private void HelpAboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                this,
                $"QuickCrypto - [{typeof(MainWindowViewModel).Assembly.ReadFileVersion()}]" +
                $"{Environment.NewLine}Copyright © 2018 - {DateTime.Today.Year} " +
                $"by CodeGator. All rights reserved.{Environment.NewLine}{Environment.NewLine}" +
                $"Warning: This computer program is protected by copyright law and international treaties. Unauthorized reproduction or distribution of this program, or any portion of it, may result in severe civil and criminal penalties, and will be prosecuted to the full extent of the law.",
                "About QuickCrypto",
                MessageBoxButton.OK
                );
        }

        #endregion
    }
}

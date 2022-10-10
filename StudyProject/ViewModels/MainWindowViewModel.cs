using StudyProject.Infrastructure.Commands;
using StudyProject.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace StudyProject.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region SelectedPageIndex : int - Номер выбранной вкладки

        private int _SelectedPageIndex;

        public int SelectedPageIndex 
        { 
            get => _SelectedPageIndex; 
            set => Set(ref _SelectedPageIndex, value); 
        }

        #endregion

        #region TestDataPoints : Ienumerable<TextDataPoint> - Description

        private IEnumerable<Models.DataPoint> _TestDataPoints;

        public IEnumerable<Models.DataPoint> TestDataPoints
        {
            get => _TestDataPoints;
            set => Set(ref _TestDataPoints, value);
        }

        #endregion

        #region Заголовок окна

        private string _title = "Анализ статистики CV19";

        /// <summary>Заголовок окна</summary>

        public string Title
        {
            get => _title;
            //set
            //{
            //    //if (Equals(_title, value)) return;
            //    //_title = value;
            //    //OnPropertyChanged();

            //    Set(ref _title, value);
            //}
            set => Set(ref _title, value);
        }

        #endregion

        #region Status : string - Статус программы

        /// <summary>Статус программы</summary>
        private string _status = "Ready!";

        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        #endregion

        #region Команды

        #region CloseApplicationCommand

        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object p) => true;

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region ChangeTabIndexCommand

        public ICommand ChangeTabIndexCommand { get; }

        private bool CanChangeTabIndexCommandExecute(object p) => _SelectedPageIndex >= 0;

        private void OnChangeTabIndexCommandExecuted(object p)
        {
            if (p is null) return;
            SelectedPageIndex += Convert.ToInt32(p);
        }

        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region Команды

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            ChangeTabIndexCommand = new LambdaCommand(OnChangeTabIndexCommandExecuted, CanChangeTabIndexCommandExecute);

            var dataPoints = new List<Models.DataPoint>((int)(360 / 0.1));
            for (var x = 0d; x <= 360; x += 0.1)
            {
                const double toRad = Math.PI / 180;
                var y = Math.Sin(x * toRad);

                dataPoints.Add(new Models.DataPoint { XValue = x, YValue = y });
            }

            TestDataPoints = dataPoints;

            #endregion
        }
    }
}

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sozdanie_knopok
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        public DelegateCommand CountAllWallsCommand { get; }
        public DelegateCommand VolumeAllWallsCommand { get; }
        public DelegateCommand CountAllDoorsCommand { get; }
        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            CountAllWallsCommand = new DelegateCommand(OnCountAllWallsCommand);
            VolumeAllWallsCommand = new DelegateCommand(OnVolumeAllWallsCommand);
            CountAllDoorsCommand = new DelegateCommand(OnCountAllDoorsCommand);
        }
        private void OnCountAllDoorsCommand()
        {
            RaiseHideRequest();
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var doors = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Doors)
                .WhereElementIsNotElementType()
                .Cast<FamilyInstance>()
                .ToList();
            TaskDialog.Show("Сообщение", $"Количество дверей в проекте {doors.Count()}");
            RaiseShowRequest();
        }
        private void OnVolumeAllWallsCommand()
        {
            RaiseHideRequest();
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var walls = new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .ToList();
            double obemStenFeet = 0;
            
            foreach (Wall wall in walls)
            {
                obemStenFeet += wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
             
            }
            double obemStenMeters = UnitUtils.ConvertFromInternalUnits(obemStenFeet, DisplayUnitType.DUT_CUBIC_METERS);
            TaskDialog.Show("Сообщение", $"Объем всех стен в проекте {obemStenMeters}м3");
            RaiseShowRequest();
        }
        private void OnCountAllWallsCommand()
        {
            RaiseHideRequest();
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var walls=new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .ToList();
            TaskDialog.Show("Сообщение", $"Количество стен в проекте {walls.Count()}");
            RaiseShowRequest();
        }
        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler ShowRequest;
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
